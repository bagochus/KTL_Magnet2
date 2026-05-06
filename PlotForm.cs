using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;



namespace KTL_Magnet2
{
    public delegate List<double> RequestDataDelegate(string name);


    public partial class PlotForm : Form
    {
        //readonly FormsPlot FormsPlot1 = new FormsPlot() { Dock = DockStyle.Fill };

        private List<String> ValueNames;
        private RequestDataDelegate RequestData;
        public PlotFormClosedDelegate plotFormClosed;
        private RequestDataNamesDelegate RequestDataNames;
        public bool AutoUpdate { get; set; }
        private bool new_x = false;
        private bool new_y = false;
        private String x_name = "";
        private String y_name = "";
        private List<double> x_values;
        private List<double> y_values;


        public PlotForm(MeasController measController)
        {
            InitializeComponent();
            measController.ListUpdated += this.ListUpdated;
            measController.NewData += this.NewData;
            RequestDataNames = measController.GetValueNames;
            this.plotFormClosed = measController.PlotFormClosed;
            ValueNames = measController.GetValueNames();
            RequestData = measController.RequestData;
            //checkBox_autoupdate.DataBindings.Add(new Binding("Checked", this, "AutoUpdate"));
            comboBox_xname.Items.AddRange(ValueNames.ToArray());
            comboBox_yname.Items.AddRange(ValueNames.ToArray());
        }

         public void ListUpdated()
         {
            if (AutoUpdate) ValueNames = RequestDataNames();
            comboBox_xname.Items.Clear();
            comboBox_yname.Items.Clear();
            comboBox_xname.Items.AddRange(ValueNames.ToArray());
            comboBox_yname.Items.AddRange(ValueNames.ToArray());
        }

        public void NewData(string name)
        {
            if(!AutoUpdate) return;
            new_x = (x_name == name);
            new_y = (y_name == name);
            if (new_x && new_y) 
            {
                Replot();
                new_x = false;
                new_y = false;
            }
        }

        private void Replot()
        {
            if (x_name == "" || y_name == "") return;
            try
            {
                x_values = RequestData(x_name);
                y_values = RequestData(y_name);
                formsPlot1.Plot.Clear();
                formsPlot1.Plot.Add.Scatter(x_values, y_values);
                formsPlot1.Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //double[] dataX = { 1, 2, 3, 4, 5 };
            //double[] dataY = { 1, 4, 9, 16, 25 };
            x_name = comboBox_xname.Text;
            y_name = comboBox_yname.Text;
            Replot();
        }


        private void PlotForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            plotFormClosed(this);
        }

        private void PlotForm_Resize(object sender, EventArgs e)
        {
            formsPlot1.Width = this.Width - 50;
            formsPlot1.Height = this.Height - 120;
            formsPlot1.Refresh();
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            Replot();
            ListUpdated();
        }

        private void checkBox_autoupdate_CheckedChanged(object sender, EventArgs e)
        {
            AutoUpdate = checkBox_autoupdate.Checked;
        }
    }
}
