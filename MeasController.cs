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


namespace KTL_Magnet2
{
    public delegate void LoadSetupDelegate(ExpSetup setup);
    delegate string ErrLine(int x);
    
    public class MeasController
    {
        public ExpSetup expSetup;
        private Measurer msr;
        private Thread m_thread;
        private bool running = false;
        private bool dt_ready = false;
        




        public AD_settings ad_Settings;
        public String[] VisaRes = new String[0];
        public VisaMeasurment[] vm;

        public BindingList<InputLine> inputLines = new BindingList<InputLine>();


        public void LoadSetup(ExpSetup setup)
        {

            //todo - add cal.tables check and load
            this.expSetup = setup;
        }

        private void CheckExperimentalPlan()
        {
            String error_message = "Превышено начальное значение переменной ";

            ErrLine errline = (int x) => { return ", строка " + x.ToString(); };

            if (inputLines[0].V1 > expSetup.MaxV1Step) throw new Exception(error_message + "V1");
            if (inputLines[0].V2 > expSetup.MaxV1Step) throw new Exception(error_message + "V2");
            if (inputLines[0].B_Setpoint > expSetup.MaxV1Step) throw new Exception(error_message + "B_setpoint");

            error_message = "Превышен шаг переменной ";
            for (int i = 0; i < inputLines.Count - 1; i++)
            {
                if (Math.Abs(inputLines[i+1].V1 - inputLines[1].V1) > expSetup.MaxV1Step & (expSetup.MaxV1Step != double.NaN))
                     throw new Exception(error_message + "V1" + errline(i));
                if (Math.Abs(inputLines[i + 1].V2 - inputLines[1].V2) > expSetup.MaxV2Step)
                    throw new Exception(error_message + "V2" + errline(i));
                if (Math.Abs(inputLines[i + 1].B_Setpoint - inputLines[1].B_Setpoint) > expSetup.MaxBStep)
                    throw new Exception(error_message + "B_setpoint" + errline(i));
            }
            error_message = "Превышена шаг переменной ";
            double steptime = StepDuration() / 1000;
            for (int i = 0; i < inputLines.Count - 1; i++)
            {
                if ((inputLines[i + 1].V1 - inputLines[1].V1) > expSetup.MaxV1Step)
                    throw new Exception(error_message + "V1" + errline(i));
                if ((inputLines[i + 1].V2 - inputLines[1].V2) > expSetup.MaxV2Step)
                    throw new Exception(error_message + "V2" + errline(i));
                if ((inputLines[i + 1].B_Setpoint - inputLines[1].B_Setpoint) > expSetup.MaxBStep)
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
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize(fs,expSetup);
            }
        }

        public void LoadSettings(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            { 
                expSetup = JsonSerializer.Deserialize<ExpSetup>(fs);
            }

          }




    }
}
