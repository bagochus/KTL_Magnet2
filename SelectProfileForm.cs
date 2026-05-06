using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTL_Magnet2
{
    public partial class SelectProfileForm : Form
    {
        public string selected_name = null;

        public SelectProfileForm()
        {
            InitializeComponent();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(DB_Manager.GetProfilesNames().ToArray());

        }

        private void SelectProfileForm_Load(object sender, EventArgs e)
        {
            selected_name = listBox1.SelectedItem.ToString();
            Close();
        }
    }
}
