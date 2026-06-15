using KTL_Magnet2.Measurements;
using KTL_Magnet2.Measurments;
using NCalc;
using ScottPlot.Colormaps;
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
using System.Xml.Linq;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace KTL_Magnet2.DialogForms
{
    public partial class CalculateEditForm : Form
    {
        IFormatProvider frmt = new NumberFormatInfo { NumberDecimalSeparator = "." };

        public string _Name;
        public string Formula {  get; set; }



        Calculation parentObject;

        private List<string> exisitngNames = new List<string>();

        public CalculateEditForm(string[] exisitngNames, Calculation parent)
        {
            parentObject = parent as Calculation;
            if (exisitngNames?.Length > 0) this.exisitngNames = exisitngNames.ToList();
            InitializeComponent();
            FillDefaults();
        }

        private void FillDefaults()
        {
            textBox_name.Text = parentObject.Name;
            textBox_formula.Text = parentObject.Formula;

            foreach (string s in exisitngNames)
                textBox_check.Text += $"{s}=1 ";
        }

        private bool ReadForm()
        {
            bool inputValid = true;

            Formula = textBox_formula.Text;

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

        public void Check()
        {
            try
            {
                var list = ParseKeyValuePairs(textBox_check.Text);
                Expression expression = new Expression(textBox_formula.Text);
                foreach ((string s, double d) in list)
                {
                    expression.Parameters[s] = d;
                }
                double result = (double)expression.Evaluate();

                MessageBox.Show($"Результат: {result}");

            } catch (Exception ex)

            { 
                MessageBox.Show($"Error: {ex.Message}"); 
            }
        }


        public static List<(string Name, double Value)> ParseKeyValuePairs(string input)
        {
            var result = new List<(string, double)>();

            if (string.IsNullOrWhiteSpace(input))
                return result;

            // Разделяем по пробелам
            string[] pairs = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string pair in pairs)
            {
                // Ищем первое вхождение '=' (имя может содержать '=', но ключом будет всё до первого)
                int equalIndex = pair.IndexOf('=');

                if (equalIndex <= 0 || equalIndex == pair.Length - 1)
                {
                    // Пропускаем некорректные пары: нет '=', имя пустое или нет значения
                    continue;
                }

                string name = pair.Substring(0, equalIndex);
                string valueStr = pair.Substring(equalIndex + 1);

                // Проверяем, что имя непустое
                if (string.IsNullOrEmpty(name))
                    continue;

                // Парсим double (с учётом invariant культуры для точки)
                if (double.TryParse(valueStr,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out double value))
                {
                    result.Add((name, value));
                }
                // Если хотите выбрасывать исключение при ошибке парсинга числа:
                 else throw new FormatException($"Не удалось распарсить число: {valueStr}");
            }

            return result;
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

        private void button1_Click(object sender, EventArgs e)
        {
            Check();
        }
    }
}
