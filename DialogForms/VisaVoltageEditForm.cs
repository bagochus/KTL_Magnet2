using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antlr.Runtime;
using KTL_Magnet2.Measurments;
using NationalInstruments.VisaNS;

namespace KTL_Magnet2.DialogForms
{
    public partial class VisaVoltageEditForm : Form
    {

        IFormatProvider frmt = new NumberFormatInfo { NumberDecimalSeparator = "." };

        //public DialogResult dialogResult = DialogResult.None;

        private VisaVoltageMeasurment parentObject;

        public string DeviceName = "";
        public double PLC_time = -1;
        public double Limit = -1;
        public int Channel = -1;
        public int Delay = -1;
        public int Avg = 1;

        public string _Name {  get; set; }

        private List<string> exisitngNames = new List<string>();
        private List<string> deviceList = new List<string>();

        public VisaVoltageEditForm(string[] exisitngNames, ExperimentStep parent)
        {
            parentObject = (parent as VisaVoltageMeasurment);
            if (exisitngNames?.Length > 0)
            this.exisitngNames = exisitngNames?.ToList();
            InitializeComponent();
            //GetDeviceList();
            FillDefaults();
        }




        private void FillDefaults()
        {
            DeviceName = parentObject.DeviceName;
            PLC_time = parentObject.PLC_time;
            Limit = parentObject.Limit;
            Channel = parentObject.Channel;
            Delay = parentObject.Delay;
            Avg = parentObject.Avg;
            _Name = parentObject.Name;

            comboBox_device.Items.Add("По умолчанию");

            if (string.IsNullOrEmpty(DeviceName))
                comboBox_device.SelectedIndex = 0;
            else if (deviceList.Contains(DeviceName))
                comboBox_device.SelectedIndex = deviceList.IndexOf(DeviceName) + 1;
            else comboBox_device.Text = $"@{DeviceName}";

            comboBox_chanel.Items.Add("Не использовать");
            if (Channel == -1)
                comboBox_chanel.SelectedIndex = 0;
            else comboBox_chanel.Text = Channel.ToString();

            comboBox_limit.Items.Add("Авто");
            if (Limit == -1)
                comboBox_limit.SelectedIndex = 0;
            else 
                comboBox_limit.Text = Limit.ToString(CultureInfo.InvariantCulture);

            comboBox_plc.Items.Add("Авто");
            if (PLC_time == -1)
            comboBox_plc.SelectedIndex = 0;
            else comboBox_plc.Text = PLC_time.ToString(CultureInfo.InvariantCulture);

            textBox_delay.Text = Delay.ToString();
            textBox_avg.Text = Avg.ToString();
            textBox_name.Text = parentObject.Name;
        }


        private void GetDeviceList()
        {
            var rm = ResourceManager.GetLocalManager();
            if (rm is null)
            { 
                label_device_status.Text = "Не удалось загрузить менеджер устройств VISA";
                return;
            }
            deviceList = rm.FindResources("USB:").ToList<string>();
            if (!(deviceList?.Count > 0))
            {
                label_device_status.Text = "Не найдено ни одного устройства USB";
                return;
            }
            for (int i = 0; i < deviceList?.Count; i++)
            {
                UsbSession usbSession = new UsbSession(deviceList[i]);
                usbSession.Write("*IDN?");
                string result = "";
                try
                {
                    result = usbSession.ReadString();
                }
                catch
                {
                    result = deviceList[i];
                }
                comboBox_device.Items.Add(result);
            }
        }

        private bool ReadForm()
        {

            bool inputValid = true;
            if (comboBox_device.SelectedIndex > 0) DeviceName = deviceList[comboBox_device.SelectedIndex + 1];

            inputValid &= ParseBoxDouble(comboBox_plc, ref PLC_time);
            inputValid &= ParseBoxDouble(comboBox_limit, ref Limit);
            inputValid &= ParseBoxInt(comboBox_chanel, ref Channel);
            inputValid &= ParseBoxInt(textBox_delay, ref Delay);
            inputValid &= ParseBoxInt(textBox_avg, ref Avg, false);

            if (ExperimentStep.IsValidVariableNameManual(textBox_name.Text)
                && (!exisitngNames.Contains(textBox_name.Text) || textBox_name.Text == _Name) )
                _Name = textBox_name.Text;
            else
            { 
                HighlightControl(textBox_name); 
                return false;
            }

            return inputValid;
        }


        private void HighlightControl(Control control)
        {
            Task.Run(() => 
            {
                control.Invoke(new Action(() => { control.BackColor = Color.MistyRose; }));    
                Thread.Sleep(1000);
                control.Invoke(new Action(() => { control.BackColor = SystemColors.Window; }));
            });
        }

        private bool ParseBoxDouble(Control control, ref double value, bool allowZero = true)
        {
            if (control is ComboBox &&
                (control as ComboBox).SelectedIndex == 0)
                return true;

            bool parseOk = false;

            double result;
            parseOk = double.TryParse(control.Text, NumberStyles.Any, frmt, out result);
            if (allowZero)
                parseOk &= result >= 0;
            else
                parseOk &= result > 0;

            if (parseOk)
            {
                value = result;
                return true;
            }
            else
            {
                HighlightControl(control);
                return false;
            }
        }

        private bool ParseBoxInt(Control control, ref int value, bool allowZero = true)
        {
            if (control is ComboBox &&
                (control as ComboBox).SelectedIndex == 0)
                return true;

            bool parseOk = false;
            int result;
            parseOk = int.TryParse(control.Text, out result);
            if (allowZero)
                parseOk &= result >= 0;
            else
                parseOk &= result > 0;

            if (parseOk)
            {
                value = result;
                return true;
            }
            else
            {
                HighlightControl(control);
                return false;
            }
        }




        private void button_ok_Click(object sender, EventArgs e)
        {
            if (ReadForm()) 
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
