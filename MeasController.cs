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
using OpenTK.Graphics.OpenGL;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;


namespace KTL_Magnet2
{
    public delegate void LoadSetupDelegate(ExpSetup setup);
    delegate string ErrLine(int x);
    public enum PlotDataType {In, Out_AD, Out_Visa, Readout};


    public delegate void ListUpdatedDelegate();
    public delegate List<String> RequestDataNamesDelegate();
    public delegate void PlotFormClosedDelegate(PlotForm plotForm);
    public delegate void NewDataDelegate(String name);

    public class MeasController
    {
        public ExpSetup expSetup = new ExpSetup();
        private Measurer msr = new Measurer();
        private Thread m_thread;
        public bool terminated;
        private bool running = false;
        private bool dt_ready = false;
        private bool tables_ready = false;
        private bool calibration_ok = false;
        private InterpolationTable table_setpoint_v1 = new InterpolationTable();
        private InterpolationTable table_setpoint_v2 = new InterpolationTable();
        private InterpolationTable table_readout = new InterpolationTable();

        public WriteReadoutValueDelegate writeReadout;
        public WriteOutpuValueDelegate writeOutput;
        private Action<string> WriteStatus;
        private Action<int> MarkErrorLine;
        private Action<int> MarkCompleteLine;
        private Action ResetTable;



        public AD_settings ad_Settings;
        public String[] VisaRes = new String[0];
        public VisaMeasurement[] vm;

        public BindingList<InputLine> inputLines = new BindingList<InputLine>();
        private double[,] OutputValues;
        private List<double> ReadoutValues = new List<double>();


        private List<String> PlotNames = new List<String>();
        private List<PlotDataType> PlotTypes = new List<PlotDataType>();
        private List<int> PlotIndices = new List<int>();

        public ListUpdatedDelegate ListUpdated = () => { };
        public NewDataDelegate NewData = (string name) => { };


        public MeasController(MainForm form)
        {
            WriteStatus = form.WriteStatus;
            MarkErrorLine = form.MarkErrorLine;
            MarkCompleteLine = form.MarkCompleteLine;
            ResetTable = form.ResetTableColors;
        }

        public void PlotFormClosed (PlotForm plotForm)
        {
            NewData -= plotForm.NewData;
            ListUpdated -= plotForm.ListUpdated;
        }

        public List<double> RequestData(string name)
        {
            int data_id = PlotNames.IndexOf(name);
            if (data_id == -1) throw new Exception("bad_index");
            if (PlotTypes[data_id] == PlotDataType.In)
            {
                List<double> result = new List<double>();
                for (int i = 0; i < inputLines.Count; i++)
                {
                    if (name == "V1") result.Add(inputLines[i].V1);
                    if (name == "V2") result.Add(inputLines[i].V2);
                    if (name == "Sign") result.Add(inputLines[i].Sign);
                    if (name == "B_setpoint") result.Add(inputLines[i].B_Setpoint);
                }
                return result;
            }
            if (PlotTypes[data_id] == PlotDataType.Readout) return ReadoutValues;
            if (PlotTypes[data_id] == PlotDataType.Out_AD)
            {
                int index = -1;
                for (int i = 0; index < expSetup.ad_Measurements.Count; i++)
                {
                    if (expSetup.ad_Measurements[i].ch_num == PlotIndices[data_id])
                    {
                        index = i; break;
                    }
                }
                List<double> result = new List<double>();
                for (int i = 0; i < inputLines.Count; i++)
                {
                    result.Add(OutputValues[i, index]);
                }
                return result;
            }
            if (PlotTypes[data_id] == PlotDataType.Out_Visa)
            {
                int first_column = expSetup.ad_Measurements.Count;
                List<double> result = new List<double>();
                for (int i =0; i<inputLines.Count;i++)
                {
                    result.Add(OutputValues[i, PlotIndices[data_id]]);
                }
                return result;
            }

            throw new Exception("bad_index2");
            //return null;
        }

        private void BuildValueNamesList()
        {
            PlotNames.Clear();
            PlotTypes.Clear();
            PlotIndices.Clear();

            List<String> names = new List<String>();
            if (expSetup.UseSetpointCalibrationTables)
            {
                PlotNames.Add("B_setpoint");
                PlotTypes.Add(PlotDataType.In);
                PlotIndices.Add(0);

            }
            if (expSetup.UseReadoutCalibrationTables)
            {
                PlotNames.Add("B_readout");
                PlotTypes.Add(PlotDataType.Readout);
                PlotIndices.Add(0);
            }

            PlotNames.Add("V1");
            PlotTypes.Add(PlotDataType.In);
            PlotIndices.Add(1);
            PlotNames.Add("V2");
            PlotTypes.Add(PlotDataType.In);
            PlotIndices.Add(2);
            PlotNames.Add("Sign");
            PlotTypes.Add(PlotDataType.In);
            PlotIndices.Add(3);

            for (int i = 0; i < expSetup.ad_Measurements.Count; i++)
            {
                PlotNames.Add("AD_"+expSetup.ad_Measurements[i].ch_num.ToString());
                PlotTypes.Add(PlotDataType.Out_AD);
                PlotIndices.Add(i);
            }
            for (int i = 0; i<expSetup.visa_Measurements.Count; i++)
            {
                PlotNames.Add ("VISA_" + expSetup.visa_Measurements[i].Type.ToString() + "_" + i.ToString());
                PlotTypes.Add(PlotDataType.Out_Visa);
                PlotIndices.Add(i+expSetup.ad_Measurements.Count);
            }
            ListUpdated();
        }

        public List<string> GetValueNames()
        { return PlotNames; }   

        public void LoadSetup(ExpSetup setup)
        {

            try
            {
                bool from_state = this.expSetup.UseSetpointCalibrationTables;
                bool to_state = setup.UseSetpointCalibrationTables;
                if (!from_state && to_state)  inputLines.Clear(); 
                this.expSetup = setup;
                if (expSetup.UseSetpointCalibrationTables) LoadTables();
                BuildValueNamesList();

            }
            catch (Exception ex){ MessageBox.Show(ex.Message); }
            
        }

        public bool CheckAvaibleVisaDevices()

        { 
            bool result = true;

            for (int i = 0; i < expSetup.visa_Measurements.Count; i++) 
            {
                result &= VisaRes.Contains(expSetup.visa_Measurements[i].DeviceName);
            }
            //return result;
            return true;
        }

        public void Start()
        {
            

            ScanDevices();
            ResetTable();


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

            int columns = expSetup.ad_Measurements.Count + expSetup.visa_Measurements.Count;
            int rows = inputLines.Count;
            OutputValues = new double[rows,columns];
            ReadoutValues.Clear();

            if (msr.Initialized)
            {
                m_thread = new Thread(this.Work);
                m_thread.Start();
                running = true;
                terminated = false;
            }

        }

        private void Work()
        {
            int i = 0;
            for (; i < inputLines.Count; i++) 
            {
                if (terminated) break;

                WriteStatus("Выполнение эксперимента " + (i+1).ToString() + "/" + inputLines.Count.ToString());
                int first_visa_column = expSetup.ad_Measurements.Count;
                bool need_smooth_transition = false;
                if (i != 0) 
                {
                    need_smooth_transition = (inputLines[i].Sign != inputLines[i - 1].Sign);
                    need_smooth_transition |= Math.Abs(inputLines[i].V1 - inputLines[i - 1].V1) > expSetup.MaxV1Step;
                    need_smooth_transition |= Math.Abs(inputLines[i].V2 - inputLines[i - 1].V2) > expSetup.MaxV2Step;
                    need_smooth_transition &= expSetup.UseSmoothZeroCrossing;
                }
                if (need_smooth_transition) SmoothTransition(i-1);


                msr.SetOutputVolatages(inputLines[i].V1, inputLines[i].V2, inputLines[i].Sign);
                NewData("V1");
                NewData("V2");
                NewData("Sign");
                if (expSetup.UseSetpointCalibrationTables) NewData("B_setpoint");
                
                for (int j = 0;j<expSetup.ad_Measurements.Count;j++)
                {
                    double measured_value = msr.PerformADMeasurement(expSetup.ad_Measurements[j]);
                    OutputValues[i, j] = measured_value;
                    writeOutput(i, j, measured_value);
                    NewData("AD_" + expSetup.ad_Measurements[j].ch_num.ToString());
                }
                  
                for (int j = 0; j < expSetup.visa_Measurements.Count; j++)
                {
                    double measured_value = msr.PerformVisaMeasurement(expSetup.visa_Measurements[j]);
                    OutputValues[i, j + first_visa_column] = measured_value;
                    writeOutput(i, j + first_visa_column, measured_value);
                    NewData("VISA_" + expSetup.visa_Measurements[j].Type.ToString() + "_" + j.ToString());
                }
                if (expSetup.UseReadoutCalibrationTables)
                {
                    double b_readout = CalculateReadoutValue(i);
                    writeReadout(i,b_readout);
                    ReadoutValues.Add(b_readout);
                    NewData("B_readout");
                }
                MarkCompleteLine(i);
            }
            string final_text = "Измерения остановлены";
            if (i >= inputLines.Count)
            {
                final_text = "Измерения завершены";
                i = inputLines.Count - 1;
            }
            SmoothShutdown(inputLines[i].V1, inputLines[i].V2, inputLines[i].Sign);
            WriteStatus(final_text);
        }

        private void SmoothTransition(int line)
        {
            const double tol = 1e-8;
            Func<double, double, bool> less_or_equal = (a, b) =>
            {
                bool eq = Math.Abs(a - b) < tol;
                return (eq || (a < b - tol));
            };
            Func<double,double, bool> greater_or_equal = (a, b) =>
            {
                bool eq = Math.Abs(a - b) < tol;
                return (eq || (a > b + tol));
            };
            Func<double, double, bool> less = (a, b) =>
            {
                return (a < b-tol);
            };
            Func<double, double, bool> greater = (a, b) =>
            {
                return (a > b + tol);
            };
            Func<double, double, bool> equal = (a, b) =>
            {
                return Math.Abs(a - b) < tol;
            };


            if (line <0) return;
            if (line > inputLines.Count - 1) return;
            bool zero_crossing = ((inputLines[line].Sign != inputLines[line + 1].Sign));
            double v1_start = inputLines[line].V1;
            double v2_start = inputLines[line].V2;
            double v1_final = inputLines[line+1].V1;
            double v2_final = inputLines[line+1].V2;
            double v1 = v1_start;
            double v2 = v2_start;

            int StartSign = inputLines[line].Sign;
            int EndSign = inputLines[line+1].Sign;

            msr.SetOutputVolatages(v1, v2, StartSign);
            WriteStatus("Выполняется плавный переходный процесс");


            if (zero_crossing)
            {

                while (greater(v1, 0) && greater(v2, 0))
                {
                    Thread.Sleep(expSetup.SmoothDelay);
                    v1 -= expSetup.SmoothStep; if (v1 < 0) v1 = 0;
                    v2 -= expSetup.SmoothStep; if (v2 < 0) v2 = 0;
                    msr.SetOutputVolatages(v1, v2, StartSign);
                }

                Thread.Sleep(expSetup.ZeroCrossingDelay);
                msr.SetOutputVolatages(0, 0, EndSign);

                while (less(v1, v1_final)
                        && less(v2, v2_final))
                {
                    Thread.Sleep(expSetup.SmoothDelay);
                    v1 += expSetup.SmoothStep; if (v1 > v1_final) v1 = v1_final;
                    v2 += expSetup.SmoothStep; if (v2 > v2_final) v2 = v1_final;
                    msr.SetOutputVolatages(v1, v2, EndSign);
                }
            }
            else
            {
                int v1_sign = Math.Sign(v1_final - v1_start);
                int v2_sign = Math.Sign(v2_final - v2_start);

                while (!(equal(v1, v1_final) && equal(v2, v2_final)))        
                {
                    Thread.Sleep(expSetup.SmoothDelay);
                    v1 += v1_sign * expSetup.SmoothStep; if (v1_final - v1 < v1_sign * tol) v1 = v1_final;
                    v2 += v2_sign * expSetup.SmoothStep; if (v2_final - v2 < v2_sign * tol) v2 = v2_final;
                    msr.SetOutputVolatages(v1, v2, StartSign);
                }
            }





        }

        private void SmoothShutdown(double curr_v1, double curr_v2, int curr_sign)
        {
            double vstep = 0.1;
            int delay = 100;
            double v1 = curr_v1;
            double v2 = curr_v2;

            WriteStatus("Завершение эксперимента...");
            if (!expSetup.UseSmoothZeroCrossing)
            {
                vstep = expSetup.SmoothStep;
                delay = expSetup.SmoothDelay;
            }
            while (v1 > 1e-8 && v2 > 1e-8)
            {
                Thread.Sleep(delay);
                v1 -= vstep; if (v1 < 0) v1 = 0;
                v2 -= vstep; if (v2 < 0) v2 = 0;
                msr.SetOutputVolatages(v1, v2, curr_sign);
            }
            msr.SetOutputVolatages(0, 0, 1);

        }

        private double CalculateReadoutValue(int row)
        {
            double result = double.NaN;
            if (!expSetup.UseReadoutCalibrationTables) return result;
            double input_value = double.NaN;
            if (expSetup.readoutSourceType == ReadoutSourceType.AD)
            {
                int ad_index=-1;
                for (int i = 0; i < expSetup.ad_Measurements.Count; i++)
                {
                    if (expSetup.ad_Measurements[i].ch_num == expSetup.readoutSourceId)
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
                int first_visa_column = expSetup.ad_Measurements.Count;
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
                line.V1 = Math.Abs( temp_V1);
                line.V2 = Math.Abs( temp_V2);
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
                bool skip_value = false;
                if (expSetup.UseSetpointCalibrationTables)
                {
                    skip_value = Math.Abs(inputLines[i].B_Setpoint) < 0.01 || Math.Abs(inputLines[i + 1].B_Setpoint) < 0.01;
                    skip_value |= inputLines[i].Sign != inputLines[i+1].Sign;
                    skip_value &= expSetup.UseSmoothZeroCrossing;
                }
                if (skip_value) continue;


                if (Math.Abs(inputLines[i+1].V1 - inputLines[i].V1) > expSetup.MaxV1Step)
                        throw new Exception(error_message + "V1" + errline(i));
                if (Math.Abs(inputLines[i + 1].V2 - inputLines[i].V2) > expSetup.MaxV2Step)
                        throw new Exception(error_message + "V2" + errline(i));
                if (Math.Abs(inputLines[i + 1].B_Setpoint - inputLines[i].B_Setpoint) > expSetup.MaxBStep)
                    throw new Exception(error_message + "B_setpoint" + errline(i));
            }
            error_message = "Превышена скорость нарастания переменной ";
            double steptime = (double)StepDuration() / 1000.0;
            for (int i = 0; i < inputLines.Count - 1; i++)
            {
                bool skip_value = false;
                if (expSetup.UseSetpointCalibrationTables)
                {
                    skip_value = Math.Abs(inputLines[i].B_Setpoint) < 0.01 || Math.Abs(inputLines[i + 1].B_Setpoint) < 0.01;
                    skip_value |= inputLines[i].Sign != inputLines[i + 1].Sign;
                    skip_value &= expSetup.UseSmoothZeroCrossing;
                }
                if (skip_value) continue;

                if (Math.Abs(inputLines[i + 1].V1 - inputLines[i].V1)/steptime > expSetup.MaxV1SlewRate)
                        throw new Exception(error_message + "V1" + errline(i));
                if (Math.Abs(inputLines[i + 1].V2 - inputLines[i].V2) / steptime  > expSetup.MaxV2SlewRate)
                        throw new Exception(error_message + "V2" + errline(i));
                if (Math.Abs(inputLines[i + 1].B_Setpoint - inputLines[i].B_Setpoint) / steptime  > expSetup.MaxBSlewrate)
                    throw new Exception(error_message + "B_setpoint" + errline(i));
            }
        }

        private int StepDuration()
        {
            int result = 0;

            for (int i = 0; i < expSetup.ad_Measurements.Count; i++) 
            {
                if (expSetup.ad_Measurements[i].Delay != -1)
                result += expSetup.ad_Measurements[i].Delay;
            }
            for (int i = 0; i < expSetup.visa_Measurements.Count; i++)
            {
                if (expSetup.visa_Measurements[i].Delay != -1)
                    result += expSetup.visa_Measurements[i].Delay;
            }
            return result;
        }

        public void ScanDevices()
        {
            try
            {     
                DeviceMgr dev_mgr = DeviceMgr.Get();
                String[] OLdevices = dev_mgr.GetDeviceNames();
                dt_ready = (OLdevices.Length == 1);


                //dt_ready = true;
            }
            catch (Exception e) { MessageBox.Show(e.Message); }

            try
            {
                ResourceManager rm;
                rm = ResourceManager.GetLocalManager();
                VisaRes = rm.FindResources("(USB)?*");
            }
            catch { WriteStatus("Не удалось загрузить менеджер ресурсов VISA"); }

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
                try { File.Delete("settings/" + filename); }
                catch { }
                JsonSerializer.Serialize(fs,expSetup);
            }
        }

        public void LoadSettings(string filename)
        {
            using (FileStream fs = new FileStream("settings/" + filename, FileMode.OpenOrCreate))
            { 
                expSetup = JsonSerializer.Deserialize<ExpSetup>(fs);
            }
            BuildValueNamesList();
          }

        public void SaveTableAsCSV(string filename)
        {
            string[] lines = new string[inputLines.Count+1];

            var nfi = new NumberFormatInfo
            {
                NumberDecimalSeparator = ","
            };

            lines[0] = "V1,V2,Sign";
            for (int i = 0; i < inputLines.Count; i++)
            {
                lines[i+1] = inputLines[i].V1.ToString(nfi) + ',' +
                    inputLines[i].V2.ToString(nfi) + ',' +
                    inputLines[i].Sign.ToString();
            }
            if (expSetup.UseSetpointCalibrationTables)
            { 
                lines[0] += ",B_setpoint";
                for (int i = 0; i < inputLines.Count; i++)
                {
                    lines[i + 1] += "," + inputLines[i].B_Setpoint.ToString(nfi);
                } 
            }
            if (!expSetup.UseReadoutCalibrationTables)
            {
                lines[0] += ",B_readout";
                for (int i = 0; i < inputLines.Count; i++)
                {
                    lines[i + 1] += ","+ReadoutValues[i].ToString(nfi);
                }
            }
            for (int i = 0; i < expSetup.ad_Measurements.Count; i++) 
            {
                lines[0] += ",AD_" + expSetup.ad_Measurements[i].ch_num.ToString(nfi);
            }
            for (int i = 0; i < expSetup.visa_Measurements.Count; i++)
            {
                lines[0] += ",VISA_"
                    + expSetup.visa_Measurements[i].Type.ToString()
                    + "_" + i.ToString(nfi);
            }
            for (int i = 0; i < OutputValues.GetLength(0); i++)
            {
                string line = "";
                for (int j = 0; j < OutputValues.GetLength(1); j++)
                {
                    line += "," + OutputValues[i, j].ToString(nfi);
                }
                lines[i + 1] += line;
            }

            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (string line in lines)
                {
                    writer.WriteLine(line); 
                }
            }

            




        }


        public void SaveTableAsTxt(string filename)
        {
            string[] lines = new string[inputLines.Count + 1];

            lines[0] = "V1\tV2\tSign";
            for (int i = 0; i < inputLines.Count; i++)
            {
                lines[i + 1] = inputLines[i].V1.ToString() + '\t' +
                    inputLines[i].V2.ToString() + '\t' +
                    inputLines[i].Sign.ToString();
            }
            if (expSetup.UseSetpointCalibrationTables)
            {
                lines[0] += "\tB_setpoint";
                for (int i = 0; i < inputLines.Count; i++)
                {
                    lines[i + 1] += "\t" + inputLines[i].B_Setpoint.ToString();
                }
            }
            if (expSetup.UseReadoutCalibrationTables)
            {
                lines[0] += "\tB_readout";
                for (int i = 0; i < inputLines.Count; i++)
                {
                    lines[i + 1] += "\t" + ReadoutValues[i].ToString();
                }
            }
            for (int i = 0; i < expSetup.ad_Measurements.Count; i++)
            {
                lines[0] += "\tAD_" + expSetup.ad_Measurements[i].ch_num.ToString();
            }
            for (int i = 0; i < expSetup.visa_Measurements.Count; i++)
            {
                lines[0] += "\tVISA_"
                    + expSetup.visa_Measurements[i].Type.ToString()
                    + "_" + i.ToString();
            }
            for (int i = 0; i < OutputValues.GetLength(0); i++)
            {
                string line = "";
                for (int j = 0; j < OutputValues.GetLength(1); j++)
                {
                    line += "\t" + OutputValues[i, j].ToString();
                }
                lines[i + 1] += line;
            }

            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                }
            }





        }




    }
}
