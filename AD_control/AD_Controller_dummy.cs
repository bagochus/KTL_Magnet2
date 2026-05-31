
using System.Windows.Forms;
using System;
using System.Linq;

namespace KTL_Magnet2
{
    public class AD_Controller_dummy : IADController
    {
        private AD_controller_dummy_form form;

        public int AvaiableChannels { get; private set; }
        public double[] AvaiableGains { get; private set; }

        private double v1, v2, b;
        private byte digiOut;
        private double[] vouts = new double[5];

        public void Init()
        {
            form = new AD_controller_dummy_form();
            form.Show();

            AvaiableChannels = 5;
            AvaiableGains = new double [] {1,10,100,1000 };

        }

        public void SetVoltage(double value, int channel)
        {
            if (channel == 1) v1 = value; 
            if (channel == 2) v2 = value;
            Recalc();

            if (!(form == null || form.IsDisposed))
            {
                form.Invoke((MethodInvoker)(() => form.SetVoltage(value, channel)));
            }
        }

        public double GetVoltage(int chanell, int gain)
        {
            return vouts[chanell];
        }

        public void SetDigitalOutput(byte value)
        {
            digiOut = value;
            if (!(form == null || form.IsDisposed))
            {
                form.Invoke((MethodInvoker)(() => form.SetDigitalOutput(value)));
            }
        }

        private void Recalc()
        {
            int sign = 1;
            if (digiOut == 3) sign = -1;
            Random random = new Random();

            if (v1 < 5) b = 0;
            else
                b = sign * System.Math.Pow(v1 - 5, 2) / 25.0;
            vouts[0] = 0;
            vouts[1] = b * 0.6;
            vouts[2] = b * 0.01;
            vouts[3] = Math.Round(random.NextDouble() * 10, 2);

            form.Invoke((MethodInvoker)(() => form.SetB(b.ToString())));

            var vis = vouts.Select((x)=>x.ToString()).ToArray();
            var viss = String.Join(",", vis);

            form.Invoke((MethodInvoker)(() => form.SetVi(viss)));

        }



        public void Dispose()
        {
            form.Close();
            form.Dispose();
            form = null;
        }

    }
}
