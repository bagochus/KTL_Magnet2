using KTL_Magnet2.DialogForms;
using NationalInstruments.VisaNS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTL_Magnet2.Measurments
{
    public class VisaVoltageMeasurment : ExperimentStep
    {
        public static new string GetMeasurmentName()
        {
            return "Напряжение DC (USB устройство)";
        }

        public override string Description 
        {
            get => GetDescription(); 
        }


        IFormatProvider frmt = new NumberFormatInfo { NumberDecimalSeparator = "." };
        private string _deviceName;
        public String DeviceName { get; set; }
        public double PLC_time { get; set; } = -1;
        public double Limit { get; set; } = -1;
        public int Channel { get; set; } = -1;
        public int Delay { get; set; } = 0;
        public int Avg { get; set; } = 1;





        private UsbSession uss;

        double result = double.NaN;

        public override bool EditForm(string[] exisitngNames)
        {
            VisaVoltageEditForm form = new VisaVoltageEditForm(exisitngNames, this);
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
            {
                Name = form._Name;
                PLC_time = form.PLC_time;
                Limit = form.Limit;
                Channel = form.Channel;
                Delay = form.Delay;
                Avg = form.Avg;
                DeviceName = form.DeviceName;
                return true;
            }
            else return false;
        }

        private string GetDescription()
        {
            string result = "Напр. DC (мультиметр). ";
            if (Limit > 0) result += $"Предел={Limit} ";
            if (PLC_time > 0) result += $"PLC={PLC_time} ";
            if (Channel > 0) result += $"Канал{Channel}";
            return result;
        }


        public override void Init()
        {
            var rm = ResourceManager.GetLocalManager();
            if (rm is null) throw new Exception("Не удалось загрузить менеджер устройств VISA");
            var resources = rm.FindResources("USB:");
            if (!(resources?.Length > 0)) throw new Exception("Не найдено ни одного устройства USB");
            if (!(_deviceName?.Length > 0))
            {
                _deviceName = resources[0];
            }
            else
            {
                if (!resources.Contains(DeviceName))
                    throw new Exception("Указанное устройство недоступно");
                _deviceName = DeviceName;
            }

            
        }

        public override void Execute()
        {
            uss = new UsbSession(_deviceName);

            uss.Timeout = 2000;
            if (Channel > 0)
            {
                uss.Write("ROUT:CLOS " + Channel.ToString());
            }
            if (Limit > 0)
            {
                uss.Write("SENS:VOLT:DC:RANG " + Limit);
            }
            if (PLC_time > 0)
            {
                uss.Write("SENS:VOLT:DC:NPLC " + PLC_time);
            }
            if (Delay > 0)
            {
                Thread.Sleep(Delay);
            }



            if (Avg > 1)
            {
                result = 0;
                for (int i = 0; i < Avg; i++)
                {
                    double res_temp = 0;
                    uss.Write("MEAS:VOLT:DC?");
                    Double.TryParse(uss.ReadString(), NumberStyles.Any, frmt, out res_temp);
                    result += res_temp;
                }
                result /= Avg;
            }
            else
            { 
                uss.Write("MEAS:VOLT:DC?");
                result = double.NaN;
                Double.TryParse(uss.ReadString(), NumberStyles.Any, frmt, out result);
            }


            if (Channel > 0)
            {
                uss.Write("ROUT:OPEN");
            }

            uss.Dispose();
        }

        public override void Finish()
        {
           
        }

        public override double GetValue()
        {
            return result;
        }


    }
}
