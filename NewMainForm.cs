using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KTL_Magnet2.DialogForms;

namespace KTL_Magnet2
{
    public partial class NewMainForm : Form
    {
        private enum BMode : int {FromTo = 0, List = 1, Steady = 2 }

        BMode mode = (BMode)Settings.GetValue<int>("mode", 0);

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


        }

        private void FillControls()
        {

            textBox_filename.Text = Settings.GetValue<string>("lastFilename", "new_experiment");
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
            form.Show();
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
    }
}
