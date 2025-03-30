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


namespace KTL_Magnet2
{
    public delegate void LoadSetupDelegate(ExpSetup setup);
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
            this.expSetup = setup;
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
