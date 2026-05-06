using NationalInstruments.VisaNS;
using OpenLayers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using KTL_Magnet2.Measurement;
using ScottPlot;

namespace KTL_Magnet2
{
    public class AD_settings
    {
        public int[] ch_num;
        public int[] Range;
        public int[] Avg;
        public int[] Delay;
        public int[] Gain;
        public int count;
        public AD_settings(int n_ch)
        {
            count = n_ch;
            //const int n_ch = 5;
            ch_num = new int[n_ch];
            Range = new int[n_ch];
            Avg = new int[n_ch];
            Delay = new int[n_ch];
            Gain = new int[n_ch];
            for (int i = 0; i < n_ch; i++)
            {
                ch_num[i] = i;
                Range[i] = 0;
                Avg[i] = 1;
                Delay[i] = 0;
                Gain[i] = 1;
            }
        }
    }

    public enum MeasurementType { r, v_dc, i_dc, v_ac, i_ac, f, c, t }

    public class VisaMeasurement
    {
       public String DeviceName;
       public MeasurementType Type;
       public double PLC_time = -1;
       public double Limit = -1;
       public int Channel = -1;
       public int Delay = -1;
        public VisaMeasurement() { }
    }



    public class VisaOutput
    {

    }


    public class Measurer
    {
        IFormatProvider frmt = new NumberFormatInfo { NumberDecimalSeparator = "." };
        private DeviceMgr deviceMgr1;
        private Device dev1;
        private string[] devlist;
        AnalogInputSubsystem ainp_ss;
        AnalogOutputSubsystem aotp_ss;
        DigitalOutputSubsystem dotp_ss;
        public AD_settings ads;
        public bool Terminated;
        public bool Initialized;
        public double[] volt1;
        public double[] volt2;
        public int[] v_sign;
        public double[,] v_readout;
        public double[,] visa_readout;
        public int n_meas;
        public int n_current;
        public VisaMeasurement[] visaMeasurements;
        private AD_controller ad_controller = new AD_controller();


        public Measurer()
        {
            Terminated = true;
        }


        public void Initialize()
        {
            try
            {

                ad_controller.Init();
                Initialized = true;
            }
            catch (Exception ex)
            {
            }

        }

        public void Dispose_handlers()
        {
            ainp_ss.Dispose();
            aotp_ss.Dispose();
            dev1.Dispose();
        }

       


        public void SetOutputVolatages(double V1, double V2, int Sign)
        {
            if (Sign == -1) ad_controller.SetDigitalOutput(0x3);
            if (Sign == 1) ad_controller.SetDigitalOutput(0x1);

            ad_controller.SetVoltage(V1, 0);
            ad_controller.SetVoltage(V2, 1);
        }

        public double PerformADMeasurement(AD_Measurement ad)
        {
            double result = 0;
            Thread.Sleep(ad.Delay);
            double tempsum = 0;
            for (int k = 0; k < ad.Avg; k++)
            {
                tempsum += ad_controller.GetVoltage(ad.ch_num, ad.Gain);
            }
            result = tempsum / ad.Avg;
            return result;
        }

        public double PerformVisaMeasurement(VISA_Measurement vm)
        {
            if (vm.DeviceName == "") return double.NaN;
            UsbSession uss = new UsbSession(vm.DeviceName);
            if (vm.Channel > 0)
            {
                uss.Write("ROUT:CLOS " + vm.Channel.ToString());
            }
            if (vm.Limit > 0)
            {
                uss.Write("SENS:" + MeasID(vm.Type) + ":RANG " + vm.Limit);
            }
            if (vm.PLC_time > 0)
            {
                uss.Write("SENS:" + MeasID(vm.Type) + ":NPLC " + vm.PLC_time);
            }
            if (vm.Delay > 0)
            {
                Thread.Sleep(vm.Delay);
            }
            uss.Write("MEAS:" + MeasID(vm.Type) + "?");

            double result = double.NaN;
            Double.TryParse(uss.ReadString(), NumberStyles.Any, frmt, out result);
            

            if (vm.Channel > 0)
            {
                uss.Write("ROUT:OPEN");
            }

            return result;
        }

        public void FinalActions()
        {
            aotp_ss.SetSingleValueAsVolts(0, 0);
            aotp_ss.SetSingleValueAsVolts(0, 0);
        }

        private int MeasureVisa(ref double[,] results, int line)
        {
            for (int i = 0; i < visaMeasurements.Count(); i++)
            {
                if (visaMeasurements[i] == null || visaMeasurements[i].DeviceName == "") continue;
                UsbSession uss = new UsbSession(visaMeasurements[i].DeviceName);
                if (visaMeasurements[i].Channel > 0)
                {
                    uss.Write("ROUT:CLOS " + visaMeasurements[i].Channel.ToString());
                }
                if (visaMeasurements[i].Limit > 0)
                {
                    uss.Write("SENS:" + MeasID(visaMeasurements[i].Type) + ":RANG " + visaMeasurements[i].Limit);
                }
                if (visaMeasurements[i].PLC_time > 0)
                {
                    uss.Write("SENS:" + MeasID(visaMeasurements[i].Type) + ":NPLC " + visaMeasurements[i].PLC_time);
                }
                if (visaMeasurements[i].Delay > 0)
                {
                    Thread.Sleep(visaMeasurements[i].Delay);
                }
                uss.Write("MEAS:" + MeasID(visaMeasurements[i].Type)+"?");

                results[line,i] = double.Parse(uss.ReadString(), frmt);

                if (visaMeasurements[0].Channel > 0)
                {
                    uss.Write("ROUT:OPEN");
                }
            }

            return 0;
        }
       
        private string MeasID(MeasurementType type)
        {
            switch (type)
            {
                case MeasurementType.r: return "RES";
                case MeasurementType.v_dc: return "VOLT:DC";
                case MeasurementType.i_dc: return "CURR:DC";
                case MeasurementType.v_ac: return "VOLT:AC";
                case MeasurementType.i_ac: return "CURR:AC";
                case MeasurementType.f: return "FREQ";
                case MeasurementType.c: return "CAP";
                case MeasurementType.t: return "TC";
            }
            return "";
        }





    }
}
