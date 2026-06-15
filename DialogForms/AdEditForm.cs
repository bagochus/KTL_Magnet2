using KTL_Magnet2.Measurements;
using KTL_Magnet2.Measurments;
using ScottPlot.PlotStyles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace KTL_Magnet2.DialogForms
{
    public partial class AdEditForm : Form
    {
        IFormatProvider frmt = new NumberFormatInfo { NumberDecimalSeparator = "." };

        public int ch_num  = 0;
        public int Avg = 1;
        public int Delay  = 0;
        public int Gain  = 1;
        public string _Name = "";

        AD_Measurement parentObject;

        private List<string> exisitngNames = new List<string>();

        public AdEditForm(string[] exisitngNames, ExperimentStep parent)
        {
            parentObject = (parent as AD_Measurement);
            if (exisitngNames?.Length > 0)
                this.exisitngNames = exisitngNames?.ToList();
            InitializeComponent();
            FillDefaults();
        }

        private void FillDefaults()
        {
            ch_num = parentObject.ch_num;
            Avg = parentObject.Avg;
            Gain = parentObject.Gain;
            Delay = parentObject.Delay;
            _Name = parentObject.Name;


            IADController ad = MagnetController.GetController();
            if (ad.AvaiableGains is null) return;
            foreach (double d in ad.AvaiableGains)
                comboBox_gain.Items.Add(d.ToString(CultureInfo.InvariantCulture));

            if (ch_num >= 0)
            textBox_channel.Text = ch_num.ToString();
            comboBox_gain.Text = Gain.ToString();
            textBox_delay.Text = Delay.ToString();
            textBox_avg.Text = Avg.ToString();
            textBox_name.Text = _Name;
        }

        private bool ReadForm()
        {



            bool inputValid = true;

            inputValid &= ParseBoxInt(textBox_channel, ref ch_num);
            inputValid &= ParseBoxInt(comboBox_gain, ref Gain, false);
            inputValid &= ParseBoxInt(textBox_delay, ref Delay);
            inputValid &= ParseBoxInt(textBox_avg, ref Avg, false);

            if (ExperimentStep.IsValidVariableNameManual(textBox_name.Text)
                && (!exisitngNames.Contains(textBox_name.Text) || textBox_name.Text == _Name))
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
