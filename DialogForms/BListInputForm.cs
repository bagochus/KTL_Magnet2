using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTL_Magnet2.DialogForms
{
    public partial class BListInputForm : Form
    {
        private static IFormatProvider frmt = new NumberFormatInfo { NumberDecimalSeparator = "." };

        public List<double> values = new List<double>();
        public BListInputForm(IList<double> inputValues = null)
        {
            InitializeComponent();
            Text = "Введите список целевых значений индукции";
            if (inputValues?.Count > 0)
            {
                richTextBox1.Clear();
                for (int i = 0; i < inputValues.Count; i++)
                    richTextBox1.Text += inputValues[i].ToString(frmt) + "\n";
            }

            DialogResult = DialogResult.Cancel;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            values = ParseList(richTextBox1.Text);
            if (values?.Count > 0)
            {
                DialogResult = DialogResult.Yes;
                Close();
            }
        }

        private static List<double> ParseList(string s)
        {
            char[] delimiters = new char[] { ' ', '\t', '\n', '\r' };
            string[] splitStrings = s.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            List<double> result = new List<double>();
            for (int i = 0; i < splitStrings.Length; i++)
                if (Double.TryParse(splitStrings[i], NumberStyles.Any, frmt, out double val))
                    result.Add(val);
            return result;
        }
    }
}
