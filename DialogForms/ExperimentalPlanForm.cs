using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KTL_Magnet2.Measurements;
using KTL_Magnet2.Measurments;

namespace KTL_Magnet2.DialogForms
{
    public partial class ExperimentalPlanForm : Form
    {
        public BindingList<ExperimentStep> experiments = new BindingList<ExperimentStep>();

        public ExperimentalPlanForm()
        {
            experiments = new BindingList<ExperimentStep>(MagnetController.Experiments) ?? 
                new BindingList<ExperimentStep>(); 

            InitializeComponent();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = experiments;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Имя",
                DataPropertyName = "Name",
                Width = 100
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Описание",
                DataPropertyName = "Description",
                Width = 500
            });
            //dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void AddItem()
        {
            MeasurmentSelect form = new MeasurmentSelect();
            form.ShowDialog();
            if (form.result is null) return;
            if ((form.result as ExperimentStep).EditForm(GetExistingNames()))
            {
                experiments.Add((form.result as ExperimentStep));
                MagnetController.experimentalPlanChanged = true;
            }

        }

        private void EditItem()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var es = dataGridView1.SelectedRows[0].DataBoundItem as ExperimentStep;
                if (es.EditForm(GetExistingNames()))
                    MagnetController.experimentalPlanChanged = true;
            }
                
        }

        private void DeleteItems()
        {
            List<ExperimentStep> expToDelete = new List<ExperimentStep>();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    expToDelete.Add(experiments[dataGridView1.SelectedRows[i].Index]);
                foreach (var e in expToDelete)
                    experiments.Remove(e);
            }
        }

        private void SaveProfile()
        {
            List<string> names = ExperimentsDB.GetProfiles().Select(x => x.Item1).ToList();
            Func<string, bool> validate = (s) =>
            {
                if (String.IsNullOrEmpty(s)) return false;
                if (names.Contains(s)) return false;
                if (names.Contains(s.Trim())) return false;
                return true;
            };
            TextInput form = new TextInput(validate);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.Yes)
            {
                MagnetController.lastUsedProfileId = ExperimentsDB.SaveExperimentSet(experiments, form.InputText);
                MagnetController.experimentalPlanChanged = false;
            }
               
        }


        private string[] GetExistingNames()
        {
            return experiments?.Select(x => x.Name)?.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddItem();
        }

        private void button_edit_Click(object sender, EventArgs e)
        {
            EditItem();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeleteItems();
        }

        private void button_up_Click(object sender, EventArgs e)
        {
            if (experiments.Count < 1) return;
            if (dataGridView1.SelectedRows.Count != 1) return;
            int selected = dataGridView1.SelectedRows[0].Index;
            if (selected < 1) return;
            var temp = experiments[selected - 1];
            experiments[selected - 1] = experiments[selected];
            experiments[selected] = temp;
            dataGridView1.ClearSelection();
            dataGridView1.Rows[selected -1].Selected = true;
        }

        private void button_down_Click(object sender, EventArgs e)
        {
            if (experiments.Count < 1) return;
            if (dataGridView1.SelectedRows.Count != 1) return;
            int selected = dataGridView1.SelectedRows[0].Index;
            if (selected > experiments.Count - 2) return;
            var temp = experiments[selected + 1];
            experiments[selected + 1] = experiments[selected];
            experiments[selected] = temp;
            dataGridView1.ClearSelection();
            dataGridView1.Rows[selected+1].Selected = true;
        }

        private void ExperimentalPlanForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProfileSelectForm form = new ProfileSelectForm();
            form.ShowDialog();
            if (form.DialogResult == DialogResult.Yes)
            {
                experiments = new BindingList<ExperimentStep>(MagnetController.Experiments);
                dataGridView1.DataSource = experiments;
                dataGridView1.Invalidate();
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProfile();
        }

        private void button_deleteAll_Click(object sender, EventArgs e)
        {

        }
    }
}
