using KTL_Magnet2.Measurments;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTL_Magnet2.DialogForms
{
    public partial class ArbitraryVisaEdit : Form
    {
        IFormatProvider frmt = new NumberFormatInfo { NumberDecimalSeparator = "." };

        private ArbitraryVisaMesurement parentObject;

        public String DeviceName { get; set; } = String.Empty;

        public List<string> InitStrings { get; set; } = new List<string>();

        public List<string> MeasureStrings { get; set; } = new List<string>();

        public List<string> FinishStrings { get; set; } = new List<string>();

        public string _Name { get; set; }

        private List<string> exisitngNames = new List<string>();

        public ArbitraryVisaEdit(string[] exisitngNames, ArbitraryVisaMesurement parent)
        {
            parentObject = (parent as ArbitraryVisaMesurement);

            if (exisitngNames?.Length > 0)
                this.exisitngNames = exisitngNames?.ToList();

            InitializeComponent();
            FillDefaults();
        }



        private void FillDefaults() 
        {
            InitStrings = parentObject.InitStrings;
            MeasureStrings = parentObject.MeasureStrings;
            FinishStrings = parentObject.FinishStrings;
            _Name = parentObject.Name;

            foreach (var l in MeasureStrings)
                richTextBox_cycle.AppendText(l + Environment.NewLine);

            foreach (var l in InitStrings)
                richTextBox_init.AppendText(l + Environment.NewLine);

            foreach (var l in FinishStrings)
                richTextBox_final.AppendText(l + Environment.NewLine);

            textBox_name.Text = parentObject.Name ?? "";
            textBox_device.Text = parentObject.DeviceName ?? "";

        }


        private bool ReadForm()
        {
            bool inputValid = true;

            try
            {
                string[] linesArray = richTextBox_cycle.Text.Split(new[] { '\n' }, StringSplitOptions.None);
                MeasureStrings = linesArray.Where(l => l.Length > 0).ToList();

                linesArray = richTextBox_init.Text.Split(new[] { '\n' }, StringSplitOptions.None);
                InitStrings = linesArray.Where(l => l.Length > 0).ToList();

                linesArray = richTextBox_final.Text.Split(new[] { '\n' }, StringSplitOptions.None);
                FinishStrings = linesArray.Where(l => l.Length > 0).ToList();

                DeviceName = textBox_device.Text;
            }
            catch 
            {
                inputValid = false;
            }


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
