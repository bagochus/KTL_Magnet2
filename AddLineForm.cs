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
    public partial class AddLineForm : Form
    {
        public double v1_start;
        public double v1_step;
        public double v2_start;
        public double v2_step;
        public int steps;
        public int sign;
        public bool input_valid = false;

        public AddLineForm()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            signBox.SelectedIndex = 0;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            
            input_valid = Double.TryParse(v1startBox.Text.ToString(),out v1_start);
            input_valid &= Double.TryParse(v1stepBox.Text.ToString(), out v1_step);
            input_valid &= Double.TryParse(v2startBox.Text.ToString(), out v2_start);
            input_valid &= Double.TryParse(v2stepBox.Text.ToString(), out v2_step);
            input_valid &= Int32.TryParse(stepsBox.Text.ToString(), out steps);

            switch (signBox.SelectedIndex)
            {
                case 0: sign = 1; break;
                case 1: sign = -1; break;
            }

            if (!input_valid) MessageBox.Show("Некорректные значения!");
            else this.Close();

        }


    }
}
