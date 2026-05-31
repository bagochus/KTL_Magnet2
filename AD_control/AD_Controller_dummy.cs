using System.Windows.Forms;

namespace KTL_Magnet2
{
    public class AD_Controller_dummy : IADController
    {
        private AD_controller_dummy_form form;

        public int AvaiableChannels { get; private set; }
        public double[] AvaiableGains { get; private set; }

        public void Init()
        {
            form = new AD_controller_dummy_form();
            form.Show();

            AvaiableChannels = 5;
            AvaiableGains = new double [] {1,10,100,1000 };

        }

        public void SetVoltage(double value, int channel)
        {
            if (!(form == null || form.IsDisposed))
            {
                form.Invoke((MethodInvoker)(() => form.SetVoltage(value, channel)));
            }
        }

        public double GetVoltage(int chanell, int gain)
        {
            if (!(form == null || form.IsDisposed))
            {
                return form.GetVoltage(chanell,gain);
            }
            return 0;
        }
        public void SetDigitalOutput(byte value)
        {
            if (!(form == null || form.IsDisposed))
            {
                form.Invoke((MethodInvoker)(() => form.SetDigitalOutput(value)));
            }
        }

        public void Dispose()
        {
            form.Close();
            form.Dispose();
            form = null;
        }

    }
}
