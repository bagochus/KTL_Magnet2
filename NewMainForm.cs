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
using KTL_Magnet2.BModes;
using KTL_Magnet2.DialogForms;
using KTL_Magnet2.Measurments;

namespace KTL_Magnet2
{
    public enum FieldState {None, Normal, Transitioning, Abnormal}

    public partial class NewMainForm : Form
    {
        IFormatProvider frmt = new NumberFormatInfo { NumberDecimalSeparator = "." };
        private enum BMode : int {FromTo = 0, List = 1, Steady = 2 }

        BMode mode = (BMode)Settings.GetValue<int>("mode", 0);

        private string externalVariableName;

        private double increment;



        public NewMainForm()
        {
            InitializeComponent();
            Width = 700;

            int bmode_panel_x = 12;
            int bmode_panel_y = panel_status.Location.Y + panel_status.Height + 12;   

            panel_list.Left = bmode_panel_x;
            panel_list.Top = bmode_panel_y;
            panel_list.Visible = false;

            panel_steady.Left = bmode_panel_x;
            panel_steady.Top = bmode_panel_y;
            panel_steady.Visible = false;

            panel_fromto.Left = bmode_panel_x;
            panel_fromto.Top = bmode_panel_y;
            panel_fromto.Visible = false;

            SetModeRadiobuttons();
            ChangePanelVisiblity();
            FillControls();

            MagnetController.dto.dataChanged += ((o, e) => UpdateDisplay(o as DisplayDTO));

            MagnetController.disableStartButton = () => 
            {
                this.Invoke(new Action(() =>
                {
                    button_start.Enabled = false;
                    contextMenuStrip1.Enabled = false;
                }));
            };

            MagnetController.enableStartButton = () => 
            {
                this.Invoke(new Action(() =>
                {
                    button_start.Enabled = true;
                    contextMenuStrip1.Enabled = true;
                }));
            };


            MagnetController.enableStopButton = () => 
            {
                this.Invoke(new Action(() =>
                {
                    button_stop.Enabled = true;
                }));
            };

            MagnetController.disableStopButton = () =>
            {
                this.Invoke(new Action(() =>
                {
                    button_stop.Enabled = false;
                }));
            };

        }

        public void UpdateDisplay(DisplayDTO ddto)
        {
            this.Invoke(new Action(() =>
            {
                label_bsetpoint.Text = $"B уст. = {ddto.bSetpoint.ToString("0.00")} Тл";
                label_breadout.Text = $"B факт = {ddto.bReadout.ToString("0.00")} Тл";
                label_v1.Text = $"U1 = {ddto.v1.ToString("0.00")}V(контроль напряжения)";
                label_v2.Text = $"U2 = {ddto.v2.ToString("0.00")}V(контроль тока)";
                string pol = ddto.Sign < 0 ? "-" : "+";
                label_pol.Text = "Полярность:" + pol;
                label_status.Text = ddto.displayString;
            }));
        }

        private void FillControls()
        {

            textBox_filename.Text = Settings.GetValue<string>("lastFilename", "new_experiment");
             
            textBox_b_start.Text = Settings.GetValue<double>("b_start", -1).ToString(CultureInfo.InvariantCulture);
            textBox_b_end.Text = Settings.GetValue<double>("b_end", 1).ToString(CultureInfo.InvariantCulture);
            textBox_b_step.Text = Settings.GetValue<double>("b_step", 0.1).ToString(CultureInfo.InvariantCulture);
            textBox_bset_delay.Text = Settings.GetValue<int>("bset_delay", 500).ToString(CultureInfo.InvariantCulture);
            checkBox_reverse.Checked = Settings.GetValue<bool>("reverse", false);
            checkBox_path_to_zero.Checked = Settings.GetValue<bool>("reverse", false);

            textBox_bset_delay_list.Text = Settings.GetValue<int>("bset_delay_list", 500).ToString(CultureInfo.InvariantCulture);




        }

        private void StartFromTo()
        {
            BModeFromTo mode = new BModeFromTo();
            bool input_valid = true;
            input_valid &= ParseBoxDouble(textBox_b_start, ref mode.BFrom);
            input_valid &= ParseBoxDouble(textBox_b_end, ref mode.BTo);
            input_valid &= ParseBoxDouble(textBox_b_step, ref mode.BStep);
            input_valid &= ParseBoxInt(textBox_bset_delay, ref mode.Delay);
            mode.Reverse = checkBox_reverse.Checked;
            mode.PathToZero = checkBox_path_to_zero.Checked;
            if (input_valid)
                MagnetController.Run(mode);
        }

        private void StartSteady()
        {
            BModeSteady mode = new BModeSteady();
            bool inputValid = true;
            inputValid &= ParseBoxDouble(textBox_b_level, ref mode.BLevel);
            inputValid &= ParseBoxDouble(textBox_outer_value, ref mode.ExternalVariable);
            inputValid &= ParseBoxInt(textBox_steady_delay, ref mode.Delay);
            mode.ExternalVarName = externalVariableName;
            mode.Continous = radioButton_steady_cont.Checked;

            if (inputValid)
                MagnetController.Run(mode);

        

        }


        private void SetModeRadiobuttons()
        {
            switch (mode)
            {
                case BMode.FromTo:
                    radioButton_fromto.Checked = true;
                    break;
                case BMode.List:
                    radioButton_list.Checked = true;
                    break;
                case BMode.Steady:
                    radioButton_steady.Checked = true;
                    break;
            }
        }

        private void ChangePanelVisiblity()
        {
            panel_list.Visible = false;
            panel_steady.Visible = false;
            panel_fromto.Visible = false;
            switch (mode)
            {
                case BMode.FromTo:
                    panel_fromto.Visible = true;
                    break;
                case BMode.List:
                    panel_list.Visible = true;
                    break;
                case BMode.Steady:
                    panel_steady.Visible = true;
                    break;
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void ShowSettingsForm()
        {
            SettingsForm form = new SettingsForm();
            form.ShowDialog();
        }

        private void ShowDataList()
        {
            if (MagnetController.table is null)
            {
                return; 
            }

            Form form = new Form();
            form.Text = "Данные " + MagnetController.GetFilename();
            DataGridView dataGridView = new DataGridView();
            dataGridView.DataSource = MagnetController.table;
            dataGridView.ReadOnly = true;
            form.Controls.Add(dataGridView);
            dataGridView.Dock = DockStyle.Fill;
            form.Width = 700;
            form.Height = 400;

            EventHandler refreshTable = new EventHandler((o, ea) =>
            {
                dataGridView.Invoke(new Action(() => {dataGridView.Invalidate(); }));
            });

            MagnetController.DataUpdated += refreshTable;

            //fuck my life
            form.FormClosed += (o, eaa) => 
            {
                MagnetController.DataUpdated -= refreshTable;
            };
            

            dataGridView.CellFormatting += (sender, e) =>
            {
            // Проверяем, что это не заголовок и значение не null
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.Value != null)
            {
                // Проверяем, что значение числовое
                if (e.Value is double || e.Value is decimal || e.Value is float)
                {
                    double value = Convert.ToDouble(e.Value);
                    // Форматируем с 5 знаками после запятой
                    e.Value = value.ToString("0.#####");
                    e.FormattingApplied = true;
                }
            }
            };



            form.Show();



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

        private bool ParseBoxDouble(Control control, ref double value, bool allowZero = true, bool allowNeg = true)
        {
            if (control is ComboBox &&
                (control as ComboBox).SelectedIndex == 0)
                return true;

            bool parseOk = false;

            double result;
            parseOk = double.TryParse(control.Text, NumberStyles.Any, frmt, out result);
            if (!allowZero)
                parseOk &= result != 0;
            if (!allowNeg)
                parseOk &= result >= 0;

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

        private bool ParseBoxInt(Control control, ref int value, bool allowZero = true, bool allowNeg = true)
        {
            if (control is ComboBox &&
                (control as ComboBox).SelectedIndex == 0)
                return true;

            bool parseOk = false;
            int result;
            parseOk = int.TryParse(control.Text, out result);
            if (!allowZero)
                parseOk &= result != 0;
            if (!allowNeg)
                parseOk &= result >= 0;

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




        private void radioButton_fromto_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_fromto.Checked)
            {
                mode = BMode.FromTo;
                ChangePanelVisiblity();
            }
        }

        private void radioButton_list_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_list.Checked)
            {
                mode = BMode.List;
                ChangePanelVisiblity();
            }
        }

        private void radioButton_steady_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_steady.Checked)
            {
                mode = BMode.Steady;
                ChangePanelVisiblity();
            }
        }

        private void button_settings_Click(object sender, EventArgs e)
        {
            ShowSettingsForm();
        }

        private void button_measurement_Click(object sender, EventArgs e)
        {
            ExperimentalPlanForm form = new ExperimentalPlanForm();
            form.ShowDialog();
        }

        private void NewMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MagnetController.OnClosing();
        }

        private void textBox_filename_Leave(object sender, EventArgs e)
        {
            MagnetController.SetFilename(textBox_filename.Text);
            textBox_filename.Text = MagnetController.GetFilename();
        }

        private void button_data_Click(object sender, EventArgs e)
        {
            ShowDataList();
        }

        private void button_plot_Click(object sender, EventArgs e)
        {
            if (MagnetController.table is null) return;
            PlotForm form = new PlotForm();
            form.Show();
        }

        private async void button_start_Click(object sender, EventArgs e)
        {
            MagnetController.SetFilename(textBox_filename.Text);
            textBox_filename.Text = MagnetController.GetFilename();
            if (mode == BMode.FromTo) await Task.Run(() => { StartFromTo(); });
            else if (mode == BMode.List) { }
            else if (mode == BMode.Steady) { }
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            MagnetController.Stop();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Func<string, bool> validate = (s) =>
             {
                 if (!ExperimentStep.IsValidVariableNameManual(s)) return false;
                 if (MagnetController.Experiments.Any((x) => x.Name == s)) return false;
                 return true;  
            };
            TextInput form = new TextInput(validate);
            form.Text = "Введите имя для внешней переменной";
            form.ShowDialog();
            if (form.DialogResult == DialogResult.Yes)
            {
                label_outer_value.Text = form.InputText;
                externalVariableName = form.InputText;
            }
        }
    }
}
