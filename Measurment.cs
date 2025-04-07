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
using KTL_Magnet2.Measurment;
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

    public enum MeasurmentType { r, v_dc, i_dc, v_ac, i_ac, f, c, t }

    public class VisaMeasurment
    {
       public String DeviceName;
       public MeasurmentType Type;
       public double PLC_time = -1;
       public double Limit = -1;
       public int Channel = -1;
       public int Delay = -1;
        public VisaMeasurment() { }
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
        public VisaMeasurment[] visaMeasurments;


        public Measurer()
        {
            Terminated = true;
            Initialized = false;
        }

        public void Prepare(int n)
        {
            n_meas = n;
            volt1 = new double[n];
            volt2 = new double[n];
            v_sign = new int[n];
            v_readout = new double[n, ads.count];
            visa_readout = new double[n, visaMeasurments.Count()];
            n_current = -1;
        }


        public void Initialize()
        {
            try
            {

                deviceMgr1 = DeviceMgr.Get();
                dev1 = deviceMgr1.GetDevice("DT9806(00)");

                ainp_ss = dev1.AnalogInputSubsystem(0);
                ainp_ss.DataFlow = DataFlow.SingleValue;
                ainp_ss.Config();

                // test
               /* for (int i = 0; i < ads.count; i++)
                {
                    ainp_ss.ChannelList.Add(i);
                    ainp_ss.ChannelList[i].Gain = ads.Gain[i];

                }
                ainp_ss.Config();
               */


                //test

                aotp_ss = dev1.AnalogOutputSubsystem(0);
                aotp_ss.Config();
                dotp_ss = dev1.DigitalOutputSubsystem(0);
                dotp_ss.Config();

                ainp_ss.GetSingleValueAsVolts(0, 1);
                Initialized = true;
            }
            catch (Exception ex) {MessageBox.Show(ex.Message); }

        }

        public void Dispose_handlers()
        {
            ainp_ss.Dispose();
            aotp_ss.Dispose();
            dev1.Dispose();
        }
        public void Work()
        {
            Terminated = false;

            for (int i = 0; i < n_meas; i++)
            {

                if (Terminated) break;
                if (v_sign[i] == -1) dotp_ss.SetSingleValue(0x3);
                if (v_sign[i] == 1) dotp_ss.SetSingleValue(0x1);

                aotp_ss.SetSingleValueAsVolts(0, volt1[i]);
                aotp_ss.SetSingleValueAsVolts(1, volt2[i]);

                for (int j = 0; j < ads.count; j++)
                {

                    ainp_ss.Config();
                    Thread.Sleep(ads.Delay[j]);
                    double tempsum = 0;
                    for (int k = 0; k < ads.Avg[j]; k++)
                    {
                        tempsum += ainp_ss.GetSingleValueAsVolts(ads.ch_num[j], 1);
                    }
                    v_readout[i, j] = tempsum / ads.Avg[j];
                }
                if (MeasureVisa(ref visa_readout, i) != 0) Terminated = true;

                n_current++;
            }
            aotp_ss.SetSingleValueAsVolts(0, 0);
            aotp_ss.SetSingleValueAsVolts(0, 0);


            Dispose_handlers();
            Terminated = true;
        }


        public void SetOutputVolatages(double V1, double V2, int Sign)
        {
            if (Sign == -1) dotp_ss.SetSingleValue(0x3);
            if (Sign == 1) dotp_ss.SetSingleValue(0x1);

            aotp_ss.SetSingleValueAsVolts(0, V1);
            aotp_ss.SetSingleValueAsVolts(1, V2);

        }

        public double PerformADMeasurment(AD_Measurment ad)
        {
            double result = 0;
            ainp_ss.Config();
            Thread.Sleep(ad.Delay);
            double tempsum = 0;
            for (int k = 0; k < ad.Avg; k++)
            {
                tempsum += ainp_ss.GetSingleValueAsVolts(ad.ch_num, 1);
            }
            result = tempsum / ad.Avg;
            return result;
        }

        public double PerformVisaMeasurment(VISA_Measurment vm)
        {
            if (vm.DeviceName == "") return double.NaN;
            UsbSession uss = new UsbSession(vm.DeviceName);
            if (vm.Channel > 0)
            {
                uss.Write("ROUT:CLOS " + vm.Channel.ToString());
            }
            if (vm.Limit > 0)
            {
                uss.Write("SENS:" + MeasID(vm.Type) + ":RANG " + visaMeasurments[i].Limit);
            }
            if (vm.PLC_time > 0)
            {
                uss.Write("SENS:" + MeasID(vm.Type) + ":NPLC " + visaMeasurments[i].PLC_time);
            }
            if (vm.Delay > 0)
            {
                Thread.Sleep(vm.Delay);
            }
            uss.Write("MEAS:" + MeasID(vm.Type) + "?");

            double result = double.NaN;
            Double.TryParse(uss.ReadString(), NumberStyles.Any, frmt, out result);
            

            if (visaMeasurments[0].Channel > 0)
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
            for (int i = 0; i < visaMeasurments.Count(); i++)
            {
                if (visaMeasurments[i] == null || visaMeasurments[i].DeviceName == "") continue;
                UsbSession uss = new UsbSession(visaMeasurments[i].DeviceName);
                if (visaMeasurments[i].Channel > 0)
                {
                    uss.Write("ROUT:CLOS " + visaMeasurments[i].Channel.ToString());
                }
                if (visaMeasurments[i].Limit > 0)
                {
                    uss.Write("SENS:" + MeasID(visaMeasurments[i].Type) + ":RANG " + visaMeasurments[i].Limit);
                }
                if (visaMeasurments[i].PLC_time > 0)
                {
                    uss.Write("SENS:" + MeasID(visaMeasurments[i].Type) + ":NPLC " + visaMeasurments[i].PLC_time);
                }
                if (visaMeasurments[i].Delay > 0)
                {
                    Thread.Sleep(visaMeasurments[i].Delay);
                }
                uss.Write("MEAS:" + MeasID(visaMeasurments[i].Type)+"?");

                results[line,i] = double.Parse(uss.ReadString(), frmt);

                if (visaMeasurments[0].Channel > 0)
                {
                    uss.Write("ROUT:OPEN");
                }
            }

            return 0;
        }
       
        private string MeasID(MeasurmentType type)
        {
            switch (type)
            {
                case MeasurmentType.r: return "RES";
                case MeasurmentType.v_dc: return "VOLT:DC";
                case MeasurmentType.i_dc: return "CURR:DC";
                case MeasurmentType.v_ac: return "VOLT:AC";
                case MeasurmentType.i_ac: return "CURR:AC";
                case MeasurmentType.f: return "FREQ";
                case MeasurmentType.c: return "CAP";
                case MeasurmentType.t: return "TC";
            }
            return "";
        }





    }
}
