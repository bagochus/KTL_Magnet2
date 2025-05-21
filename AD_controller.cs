using OpenLayers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using KTL_Magnet2.Measurment;
//using OpenLayers.DeviceCollection;

namespace KTL_Magnet2
{
    public class AD_controller: IDisposable
    {
        private DeviceMgr deviceMgr1;
        private Device dev1;
        private string[] devlist;
        AnalogInputSubsystem ainp_ss;
        AnalogOutputSubsystem aotp_ss;
        DigitalOutputSubsystem dotp_ss;
        bool initialized = false;
        bool _disposed = false;

        public AD_controller() 
        {



        }

        public void Init()
        {
            try
            {
                deviceMgr1 = DeviceMgr.Get();
                dev1 = deviceMgr1.GetDevice("DT9806(00)");

                ainp_ss = dev1.AnalogInputSubsystem(0);
                ainp_ss.DataFlow = DataFlow.SingleValue;
                ainp_ss.Config();

                aotp_ss = dev1.AnalogOutputSubsystem(0);
                aotp_ss.Config();
                dotp_ss = dev1.DigitalOutputSubsystem(0);
                dotp_ss.Config();

                //double test = ainp_ss.GetSingleValueAsVolts(0, 1);
                initialized = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                aotp_ss?.Dispose();
                ainp_ss?.Dispose();
                dotp_ss?.Dispose();

            }
        }

        public void SetVoltage(double voltage,int chanell)
        {
            if (!initialized) return;
            aotp_ss.SetSingleValueAsVolts(chanell, voltage);
        }

        public double GetVoltage(int chanell,int gain)
        {
            double result = double.NaN;
            if (!initialized) return result;
            if (!ainp_ss.SupportedGains.Contains(gain)) return result; 
            ainp_ss.GetSingleValueAsVolts(chanell, gain);
            return result;
        }

        public void SetDigitalOutput(byte value)
        {
            if (!initialized) return;
            dotp_ss.SetSingleValue(value);
        }
       
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                ainp_ss.Dispose();
                aotp_ss.Dispose();
                dotp_ss.Dispose();
                dev1.Dispose();
            }
            initialized = false;
            _disposed = true;
        }
        
        ~AD_controller()
        {
            Dispose(false);
        }
    }
}
