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
    public partial class PlotForm : Form
    {
        //readonly FormsPlot FormsPlot1 = new FormsPlot() { Dock = DockStyle.Fill };

        private List<String> ValueNames;



        public PlotForm(MeasController measController)
        {
            InitializeComponent();
            
        }

        private void PlotForm_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };

            formsPlot1.Plot.Add.Scatter(dataX, dataY);
            formsPlot1.Refresh();
        }
    }
}
