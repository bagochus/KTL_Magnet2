using KTL_Magnet2.Measurments;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTL_Magnet2.DialogForms
{
    public partial class ProfileSelectForm : Form
    {

        private List<(string, int)> profiles = new List<(string, int)>();
        private bool showAutosave = false;


        public ProfileSelectForm()
        {
            DialogResult = DialogResult.Cancel;
            InitializeComponent();
            action += LoadProfile;
            BuildList();
        }

        private Action action = () => { };

        private void LoadProfile()
        {
            int selected = listBox_profiles.SelectedIndex;
            int failed = 0;
            List<ExperimentStep> exps = null;
            if (selected >= 0)
                exps = ExperimentsDB.GetExperiments(profiles[selected].Item2, out failed);
            else return;
            if (exps.Count == 0)
            {
                MessageBox.Show("Не удалось загрузить ни одного эксперимента из профиля");
                return;
            }
            if (failed > 0)
            {
                var dr = MessageBox.Show($"Не удалось згрузить из базы {failed} экспериментов. Все равно продолжить?",
                    "Ошибка",MessageBoxButtons.YesNo);
                if (dr != DialogResult.Yes) return;
            }
            MagnetController.Experiments = exps;
            MagnetController.lastUsedProfileId = profiles[selected].Item2;
            MagnetController.experimentalPlanChanged = false;
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void BuildList()
        {
            profiles = ExperimentsDB.GetProfiles(showAutosave);
            listBox_profiles.Items.Clear();
            for (int i = 0; i < profiles?.Count; i++)
            {
                listBox_profiles.Items.Add(profiles[i].Item1);
            }
        }


        private void button_action_Click(object sender, EventArgs e)
        {
            action?.Invoke();
        }

        private void listBox_profiles_DoubleClick(object sender, EventArgs e)
        {
            action?.Invoke();
        }

        private void ProfileSelectForm_Load(object sender, EventArgs e)
        {

        }

        private void checkBox_autosave_CheckedChanged(object sender, EventArgs e)
        {
            showAutosave = checkBox_autosave.Checked;
            BuildList();
        }
    }
}
