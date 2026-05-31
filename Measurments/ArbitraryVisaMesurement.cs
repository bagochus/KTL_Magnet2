using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KTL_Magnet2.DialogForms;
using NationalInstruments.VisaNS;

namespace KTL_Magnet2.Measurments
{
    public class ArbitraryVisaMesurement : ExperimentStep
    {
        IFormatProvider frmt = new NumberFormatInfo { NumberDecimalSeparator = "." };
        private string _deviceName;
        public String DeviceName { get; set; } = String.Empty;

        public List<string> InitStrings { get; set; } = new List<string>();

        public List<string> MeasureStrings { get; set; } = new List<string>();

        public List<string> FinishStrings { get; set; } = new List<string>();

        public override string Description { get { return GetDescription(); }}

        double result = double.NaN;

        public static new string GetMeasurmentName()
        {
            return "Произвольное измерение (USB устройство)";
        }

        private string GetDescription()
        {
            string com = MeasureStrings[0] ?? "";
            if (com.Length > 30) com = com.Substring(0, 30)+"...";
            string dev = "";
            if (string.IsNullOrEmpty(DeviceName)) dev = "default";
            else dev = DeviceName;

            return $"Device={dev} Command={com}";
        }


        public override bool EditForm(string[] exisitngNames)
        {
            ArbitraryVisaEdit form = new ArbitraryVisaEdit(exisitngNames, this);
            form.ShowDialog();
            if (form.DialogResult == System.Windows.Forms.DialogResult.OK)
            { 
                DeviceName = form.DeviceName;
                InitStrings = form.InitStrings;
                MeasureStrings = form.MeasureStrings;
                FinishStrings = form.FinishStrings;
                Name = form._Name;
                return true;
            }
            return false;
        }

        public override void Execute()
        {
            result = double.NaN;
            UsbSession uss = new UsbSession(_deviceName);
            for (int i = 0; i < MeasureStrings?.Count; i++)
            {
                if (MeasureStrings[i].EndsWith("?"))
                {
                    uss.Write(MeasureStrings[i]);
                    uss.Timeout = 2000;
                    double result = double.NaN;
                    Double.TryParse(uss.ReadString(), NumberStyles.Any, frmt, out result);
                }
                else 
                    uss.Write(InitStrings[i]);
            }
            uss.Dispose();
        }

        public override void Finish()
        {
            UsbSession uss = new UsbSession(_deviceName);
            for (int i = 0; i < InitStrings?.Count; i++) uss.Write(InitStrings[i]);
            uss.Dispose();
        }

        public override double GetValue()
        {
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

            UsbSession uss = new UsbSession(_deviceName);
            for (int i = 0; i < InitStrings?.Count; i++) uss.Write(InitStrings[i]);
            uss.Dispose();


        }
    }
}
