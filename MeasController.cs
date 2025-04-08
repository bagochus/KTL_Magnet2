using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NationalInstruments.VisaNS;
using OpenLayers.Base;
using System.Text.Json;
using System.IO;
using OpenTK.Graphics.ES20;
using System.Linq.Expressions;


namespace KTL_Magnet2
{
    public delegate void LoadSetupDelegate(ExpSetup setup);
    delegate string ErrLine(int x);
    
    public class MeasController
    {
        public ExpSetup expSetup = new ExpSetup();
        private Measurer msr;
        private Thread m_thread;
        private bool running = false;
        private bool dt_ready = false;
        private bool tables_ready = false;
        private bool calibration_ok = false;
        private InterpolationTable table_setpoint_v1 = new InterpolationTable();
        private InterpolationTable table_setpoint_v2 = new InterpolationTable();
        private InterpolationTable table_readout = new InterpolationTable();

        public WriteReadoutValueDelegate writeReadout;
        public WriteOutpuValueDelegate writeOutput;



        public AD_settings ad_Settings;
        public String[] VisaRes = new String[0];
        public VisaMeasurment[] vm;

        public BindingList<InputLine> inputLines = new BindingList<InputLine>();
        private double[,] OutputValues;
        private List<double> ReadoutValues = new List<double>();


        public void LoadSetup(ExpSetup setup)
        {

            try
            {
                bool from_state = this.expSetup.UseSetpointCalibrationTables;
                bool to_state = setup.UseSetpointCalibrationTables;
                if (!from_state && to_state)  inputLines.Clear(); 
                this.expSetup = setup;
                if (expSetup.UseSetpointCalibrationTables) LoadTables();

            }
            catch (Exception ex){ MessageBox.Show(ex.Message); }
            
        }


        public bool CheckAvaibleVisaDevices()

        { 
            bool result = true;

            for (int i = 0; i < vm.Count(); i++) 
            {
                result &= VisaRes.Contains(vm[i].DeviceName);
            }
            return result;
        }


        public void Start()
        {

            ScanDevices();

            if (!dt_ready)
            {
                MessageBox.Show("Ошибка свзяи с блоком ЦПА/АЦП");
                return ;
            }
            if (!CheckAvaibleVisaDevices())
            {
                MessageBox.Show("Одно из устройств недоступно");
                return;
            }

            try 
            {
                CheckLimits();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            
            if (expSetup.UseSetpointCalibrationTables)
            {
                if (!tables_ready)
                {
                    try { LoadTables(); }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
                ApplyAllLines();
            }


            try
            {
                msr.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            int columns = expSetup.ad_Measurments.Count + expSetup.visa_Measurments.Count;
            int rows = inputLines.Count;
            OutputValues = new double[rows,columns];
            ReadoutValues.Clear();

            if (msr.Initialized)
            {
                m_thread = new Thread(this.Work);
                m_thread.Start();
                running = true;
            }

        }

        private void Work()
        {
            for (int i = 0; i < inputLines.Count; i++) 
            {
                int first_visa_column = expSetup.ad_Measurments.Count;
                for (int j = 0;j<expSetup.ad_Measurments.Count;j++)
                {
                    double measured_value = msr.PerformADMeasurment(expSetup.ad_Measurments[j]);
                    OutputValues[i, j] = measured_value;
                    writeOutput(i, j, measured_value);
                }
                  
                for (int j = 0; j < expSetup.visa_Measurments.Count; j++)
                {
                    double measured_value = msr.PerformVisaMeasurment(expSetup.visa_Measurments[j]);
                    OutputValues[i, j + first_visa_column] = measured_value;
                    writeOutput(i, j + first_visa_column, measured_value);
                }
                if (expSetup.UseReadoutCalibrationTables)
                {
                    double b_readout = CalculateReadoutValue(i);
                    writeReadout(i,b_readout);
                    ReadoutValues.Add(b_readout);   
                }

            }


        }

        private double CalculateReadoutValue(int row)
        {
            double result = double.NaN;
            if (!expSetup.UseReadoutCalibrationTables) return result;
            double input_value = double.NaN;
            if (expSetup.readoutSourceType == ReadoutSourceType.AD)
            {
                int ad_index=-1;
                for (int i = 0; i < expSetup.ad_Measurments.Count; i++)
                {
                    if (expSetup.ad_Measurments[i].ch_num == expSetup.readoutSourceId)
                    {
                        ad_index = i;
                        break;
                    }
                }
                if (ad_index == -1) return result;
                input_value = OutputValues[row, ad_index];
            }
            if (expSetup.readoutSourceType == ReadoutSourceType.Visa)
            {
                int first_visa_column = expSetup.ad_Measurments.Count;
                input_value = OutputValues[row, expSetup.readoutSourceId + first_visa_column];
            }
            if (input_value != double.NaN)
            {
                result = table_readout.GetY(input_value);
            }
            return result;
        }
        



        private void LoadTables()
        {
            tables_ready = false;
            if (expSetup.UseReadoutCalibrationTables) 
            {
                if(! table_readout.ParseFile("tables/" + expSetup.Readout_filename))
                 throw new Exception("Не удалось прочитать калибровочную таблицу для измерения B"); 
            }
            if (expSetup.UseSetpointCalibrationTables)
            {
                bool load_succesful = true;
                load_succesful &= table_setpoint_v1.ParseFile("tables/" + expSetup.v1_filename);
                load_succesful &= table_setpoint_v2.ParseFile("tables/" + expSetup.v2_filename);
                if (!load_succesful) throw new Exception("Не удалось прочитать калибровочную таблицу для установки B");
            }
            tables_ready = true;
        }

        private void ApplySetpointTables(InputLine line)
        {
            if (!expSetup.UseSetpointCalibrationTables) throw new Exception("Ошибка режима калибровки");
            if (!tables_ready) throw new Exception("Ошибка режима калибровки");
            double temp_V1 = table_setpoint_v1.GetY(line.B_Setpoint);
            double temp_V2 = table_setpoint_v2.GetY(line.B_Setpoint);
            if (Math.Sign(temp_V1) == Math.Sign(temp_V2))
            {
                line.V1 = temp_V1;
                line.V2 = temp_V2;
                line.Sign = Math.Sign(temp_V1);
            }
            else throw new Exception("В результате преобразования получились управляющие напряжения разных знаков");
        }

        private void  ApplyAllLines()
        {
            calibration_ok = false;
            try 
            {
                for (int i = 0; i < inputLines.Count; i++) ApplySetpointTables(inputLines[i]);
                calibration_ok = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }


        private void CheckLimits()
        {
            String error_message = "Превышено начальное значение переменной ";

            ErrLine errline = (int x) => { return ", строка " + x.ToString(); };

            if (inputLines[0].V1 > expSetup.MaxV1Step) throw new Exception(error_message + "V1");
            if (inputLines[0].V2 > expSetup.MaxV1Step) throw new Exception(error_message + "V2");
            if (inputLines[0].B_Setpoint > expSetup.MaxV1Step) throw new Exception(error_message + "B_setpoint");

            error_message = "Превышен шаг переменной ";
            for (int i = 0; i < inputLines.Count - 1; i++)
            {
                if (Math.Abs(inputLines[i+1].V1 - inputLines[1].V1) > expSetup.MaxV1Step)
                     throw new Exception(error_message + "V1" + errline(i));
                if (Math.Abs(inputLines[i + 1].V2 - inputLines[1].V2) > expSetup.MaxV2Step)
                    throw new Exception(error_message + "V2" + errline(i));
                if (Math.Abs(inputLines[i + 1].B_Setpoint - inputLines[1].B_Setpoint) > expSetup.MaxBStep)
                    throw new Exception(error_message + "B_setpoint" + errline(i));
            }
            error_message = "Превышена скорость нарастания переменной ";
            double steptime = StepDuration() / 1000;
            for (int i = 0; i < inputLines.Count - 1; i++)
            {
                if (((inputLines[i + 1].V1 - inputLines[1].V1))/steptime > expSetup.MaxV1SlewRate)
                    throw new Exception(error_message + "V1" + errline(i));
                if (((inputLines[i + 1].V2 - inputLines[1].V2)) / steptime  > expSetup.MaxV2SlewRate)
                    throw new Exception(error_message + "V2" + errline(i));
                if (((inputLines[i + 1].B_Setpoint - inputLines[1].B_Setpoint)) / steptime  > expSetup.MaxBSlewrate)
                    throw new Exception(error_message + "B_setpoint" + errline(i));
            }
        }

        private int StepDuration()
        {
            int result = 0;

            for (int i = 0; i < expSetup.ad_Measurments.Count; i++) 
            {
                result += expSetup.ad_Measurments[i].Delay;
            }
            for (int i = 0; i < expSetup.visa_Measurments.Count; i++)
            {
                result += expSetup.visa_Measurments[i].Delay;
            }
            return result;
        }


        private void ScanDevices()
        {
            try
            {
                ResourceManager rm;
                rm = ResourceManager.GetLocalManager();
                VisaRes = rm.FindResources("(USB)?*");
                DeviceMgr dev_mgr = DeviceMgr.Get();
                String[] OLdevices = dev_mgr.GetDeviceNames();
                dt_ready = (OLdevices.Length == 1);
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

        public void AddExperiment(double B)
        {
            InputLine il = new InputLine();
            il.B_Setpoint = B;
            inputLines.Add(il);

        }

        public void AddExperiment (int sign, double V1, double V2)
        {
            InputLine il = new InputLine();
            il.V1 = V1;
            il.V2 = V2;
            il.Sign = sign;
            inputLines.Add(il);
        }

        public void SaveSettings (string filename)
        {
            using (FileStream fs = new FileStream("settings/"+filename, FileMode.OpenOrCreate))
            {
                //File.Delete("settings/" + filename);
                JsonSerializer.Serialize(fs,expSetup);
            }
        }

        public void LoadSettings(string filename)
        {
            using (FileStream fs = new FileStream("settings/" + filename, FileMode.OpenOrCreate))
            { 
                expSetup = JsonSerializer.Deserialize<ExpSetup>(fs);
            }

          }




    }
}
