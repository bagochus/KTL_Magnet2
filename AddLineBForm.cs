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
    public delegate void AddExpB(double b);

    public partial class AddLineBForm: Form
    {
        private double bstart;
        private double bstep;
        private int nsteps;
        AddExpB addExpB;

        public AddLineBForm(MeasController measController)
        {
            InitializeComponent();
            this.addExpB = measController.AddExperiment;
        }

        private void button_calcel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            try
            {
                bstart = Double.Parse(textBox_bstart.Text);
                bstep = Double.Parse(textBox_bstep.Text);
                nsteps = Int32.Parse(textBox_nsteps.Text);

                for (int i = 0; i < nsteps; i++)
                {
                    addExpB (bstart +  (i * bstep));    
                }
            
            }
            catch (Exception ex) {MessageBox.Show(ex.Message); }



        }
    }
}
