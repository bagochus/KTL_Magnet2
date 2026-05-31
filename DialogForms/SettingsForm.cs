using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTL_Magnet2.DialogForms
{
    public partial class SettingsForm : Form
    {



        private Dictionary<string, double> doubleValues = new Dictionary<string, double>();
        private Dictionary<string,int> intValues = new Dictionary<string, int>();
        private Dictionary<string,string> stringValues = new Dictionary<string, string>();
        private Dictionary<string,bool> boolValues = new Dictionary<string, bool>();
        private Dictionary<string,string> descriptions = new Dictionary<string,string>();

        private int yOffset = 20;
        private int xOffset = 20;
        private int spacing = 10;

        private List<TextBox> doubleBoxes = new List<TextBox>();
        private List<Label> doubleLabels = new List<Label>();

        private List<TextBox> intBoxes = new List<TextBox>();
        private List<Label> intLabels = new List<Label>();

        private List<TextBox> stringBoxes = new List<TextBox>();
        private List<Label> stringLabels = new List<Label>();

        private List<CheckBox> boolBoxes = new List<CheckBox>();



        public SettingsForm()
        {
            InitializeComponent();
            InitDictionaries();
            FillDictionaries();
            ConstructForm();
        }

        private void InitDictionaries()
        {

            doubleValues.Add("v1_max_step", 0.1);
            descriptions.Add("v1_max_step", "Макс. шаг управляющего напряжения V1 (напряжение магнита), В");
            doubleValues.Add("v2_max_step", 0.1);
            descriptions.Add("v2_max_step", "Макс. шаг управляющего напряжения V2 (ток магнита), В");
            doubleValues.Add("b_max_step", 0.1);
            descriptions.Add("b_max_step", "Макс. шаг индукции поля (ток магнита), Тл");

            doubleValues.Add("v1_slewrate", 1);
            descriptions.Add("v1_slewrate", "Макс. ск. нарастания напряжения V1 (напряжение магнита), В/с");
            doubleValues.Add("v2_slewrate", 1);
            descriptions.Add("v2_slewrate", "Макс. ск. нарастания управляющего напряжения V2 (ток магнита), В/с");
            doubleValues.Add("b_slewrate", 0.2);
            descriptions.Add("b_slewrate", "Макс. ск. нарастания индукции поля (ток магнита), Тл/с");

            doubleValues.Add("maxBMismatch", 0.2);
            descriptions.Add("maxBMismatch", "Макс. отклонение индукции от целевого значения Тл");

            boolValues.Add("bMismatchStop", false);
            descriptions.Add("bMismatchStop", "Останавливать эксперимент при отклонении индукции");

            stringValues.Add("ReadoutFormula", "");
            descriptions.Add("ReadoutFormula", "Формула для вычисления фактической индукции поля. " +
                "Используйте V1,V2... для обознаяения каналов АЦП. Пример 0.6*V1." +
                "Для преобразования по таблице используйте table(v1)");

            stringValues.Add("ReadoutGains", "");
            descriptions.Add("ReadoutGains", "Коэф.усиления для каналов при измерении индукции в формате g1=1 g2=10." +
                "Если не указать коэф. усиления, будет использовано 1");


        }

        private void FillDictionaries()
        {
            if (doubleValues?.Count > 0)
            {
                var keys = doubleValues.Keys.ToList();
                foreach (var k in keys)
                    doubleValues[k] = Settings.GetValue<double>(k, 0.0);
            }

            if (intValues?.Count > 0)
            {
                var keys = intValues.Keys.ToList();
                foreach (var k in keys)
                    intValues[k] = Settings.GetValue<int> (k, 0);
            }

            if (stringValues?.Count > 0)
            {
                var keys = stringValues.Keys.ToList();
                foreach (var k in keys)
                    stringValues[k] = Settings.GetValue<string>(k, "");
            }

            if (boolValues?.Count > 0)
            {
                var keys = boolValues.Keys.ToList();
                foreach (var k in keys)
                    boolValues[k] = Settings.GetValue<bool>(k, false);
            }
        }

        private void ConstructForm()
        {
            int y = yOffset;

            if (doubleValues?.Count > 0)
            {
                foreach (var v in doubleValues)
                { 
                    TextBox box = new TextBox();
                    box.Text = v.Value.ToString(CultureInfo.InvariantCulture);
                    box.Left = xOffset;
                    box.Top = y;
                    box.Width = 80;
                    box.Tag = v.Key;
                    doubleBoxes.Add(box);
                    Controls.Add(box);
                    box.TextAlign = HorizontalAlignment.Right;
                    box.Leave += OnDoubleBoxLeave;


                    Label label = new Label();
                    label.MaximumSize = new Size(500, 10000);
                    label.AutoSize = true;
                    label.TextAlign = ContentAlignment.TopLeft;
                    label.Left = box.Left + box.Width + spacing;
                    label.Top = y;
                    label.Text = descriptions[v.Key];
                    y += spacing + label.Height;

                    doubleLabels.Add(label);
                    Controls.Add(label);
                }
            }

            if (intValues?.Count > 0)
            {
                foreach (var v in intValues)
                {
                    TextBox box = new TextBox();
                    box.Text = v.Value.ToString(CultureInfo.InvariantCulture);
                    box.Left = xOffset;
                    box.Top = y;
                    box.Width = 80;
                    box.Tag = v.Key;
                    intBoxes.Add(box);
                    Controls.Add(box);
                    box.TextAlign = HorizontalAlignment.Right;
                    box.Leave += OnIntBoxLeave;

                    Label label = new Label();
                    label.MaximumSize = new Size(500, 10000);
                    label.AutoSize = true;
                    label.TextAlign = ContentAlignment.TopLeft;
                    label.Left = box.Left + box.Width + spacing;
                    label.Top = y;
                    label.Text = descriptions[v.Key];
                    y += spacing + label.Height;

                    intLabels.Add(label);
                    Controls.Add(label);
                }
            }

            if (stringValues?.Count > 0)
            {
                foreach (var v in stringValues)
                {
                    TextBox box = new TextBox();
                    box.Text = v.Value;
                    box.Left = xOffset;
                    box.Top = y;
                    box.Width = 200;
                    box.Tag = v.Key;
                    stringBoxes.Add(box);
                    Controls.Add(box);
                    box.TextAlign = HorizontalAlignment.Right;
                    box.Leave += OnStringBoxLeave;

                    Label label = new Label();
                    label.MaximumSize = new Size(500, 10000);
                    label.AutoSize = true;
                    label.TextAlign = ContentAlignment.TopLeft;
                    label.Left = box.Left + box.Width + spacing;
                    label.Top = y;
                    label.Text = descriptions[v.Key];
                    y += spacing + label.Height;

                    stringLabels.Add(label);
                    Controls.Add(label);
                }
            }

            if (boolValues?.Count > 0)
            {
                foreach (var v in boolValues)
                {
                    CheckBox box = new CheckBox();
                    box.Checked = v.Value;
                    box.Text = descriptions[v.Key];
                    box.Left = xOffset;
                    box.Top = y;
                    box.Width = 200;
                    box.Tag = v.Key;
                    box.TextAlign = ContentAlignment.TopLeft;
                    box.MaximumSize = new Size(800, 10000);
                    box.AutoSize = true;
                    boolBoxes.Add(box);
                    Controls.Add(box);
                    y += spacing + box.Height;
                    box.CheckStateChanged += OnBoolBoxChanged;

                }
            }

            Height = y + 120;
        }


        private void OnDoubleBoxLeave(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;
            box.Text.Replace(',', '.');
            double value;
            if (Double.TryParse(box.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                doubleValues[(string)box.Tag] = value;
            else
                box.Text = doubleValues[(string)box.Tag].ToString(CultureInfo.InvariantCulture);
        }
        private void OnIntBoxLeave(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;
            box.Text.Replace(',', '.');
            int value;
            if (Int32.TryParse(box.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                intValues[(string)box.Tag] = value;
            else
                box.Text = intValues[(string)box.Tag].ToString(CultureInfo.InvariantCulture);
        }

        private void OnStringBoxLeave(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;
            stringValues[(string)box.Tag] = box.Text;
        }

        private void OnBoolBoxChanged(object sender, EventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            boolValues[(string)box.Tag] = box.Checked;
        }


        private void SaveAllValues()
        { 
            foreach (var v in doubleValues)
                Settings.SetValue(v.Key,v.Value);
            foreach (var v in intValues)
                Settings.SetValue(v.Key, v.Value);
            foreach (var v in stringValues)
                Settings.SetValue(v.Key, v.Value);
            foreach (var v in boolValues)
                Settings.SetValue(v.Key, v.Value);
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            SaveAllValues();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
