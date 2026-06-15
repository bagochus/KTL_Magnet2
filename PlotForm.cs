using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;



namespace KTL_Magnet2
{

    public partial class PlotForm : Form
    {
        
        
        
        
        //readonly FormsPlot FormsPlot1 = new FormsPlot() { Dock = DockStyle.Fill };

        private List<String> ValueNames = new List<string>();
        //private RequestDataDelegate RequestData;
        //public PlotFormClosedDelegate plotFormClosed;
        //private RequestDataNamesDelegate RequestDataNames;
        //public bool AutoUpdate { get; set; }
        //private bool new_x = false;
        //private bool new_y = false;
        private String x_name = "";
        private String y_name = "";
        private List<double> x_values;
        private List<double> y_values;
        private bool dataUpdated = false;
        private DataTable bindingTable;


        private  List<double> RequestData(string name)
        {

            if (!MagnetController.table.Columns.Contains(name))
                return null;
            List<double> result = new List<double>();

            lock (MagnetController.table)
            {
                for (int i = 0; i < MagnetController.table.Rows.Count; i++)
                {
                    var value = MagnetController.table.Rows[i][name];

                    if (value != DBNull.Value)
                    {
                        result.Add(Convert.ToDouble(value));
                    }
                }
            }

            return result;
        }

        private void Plot_OnPaint(object sender, EventArgs args)
        {
            if (dataUpdated)
            {
                formsPlot1.Plot.Clear();
                formsPlot1.Plot.Add.Scatter(x_values, y_values);
                formsPlot1.Refresh();
                dataUpdated = false;
            }
        }

        private async Task UpdateData()
        {



            if (!MagnetController.table.Columns.Contains(x_name) ||
                !MagnetController.table.Columns.Contains(y_name))
                return;
            List<double> result = new List<double>();
            await Task.Run(() =>
            {
                lock (MagnetController.table)
                {
                    for (int i = x_values.Count; i < MagnetController.table.Rows.Count; i++)
                    {
                        var x_value = MagnetController.table.Rows[i][x_name];
                        var y_value = MagnetController.table.Rows[i][y_name];

                        if (x_value != DBNull.Value && y_value != DBNull.Value)
                        {
                            x_values.Add(Convert.ToDouble(x_value));
                            y_values.Add(Convert.ToDouble(y_value));
                        }
                    }
                }
            });
        }

        public PlotForm()
        {
            InitializeComponent();
            MagnetController.DataUpdated += this.ListUpdated;


            bindingTable = MagnetController.table;
            if (bindingTable is null) return;

            for (int i = 0; i < MagnetController.table.Columns.Count; i++)
            {
                ValueNames.Add(MagnetController.table.Columns[i].ColumnName);
            }
            
            comboBox_xname.Items.AddRange(ValueNames.ToArray());
            comboBox_yname.Items.AddRange(ValueNames.ToArray());
            formsPlot1.Paint += Plot_OnPaint;

        }

        private async void ListUpdated(object sender, EventArgs e)
        {
            if (x_name == "" || y_name == "") return;
            if (MagnetController.table != bindingTable) return;
            await UpdateData();
            dataUpdated = true;
            formsPlot1?.Invoke(new Action(()=> { formsPlot1?.Invalidate(); }));
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
            if (x_name == "" || y_name == "") return;
            Replot();
            MagnetController.DataUpdated += ListUpdated;
            button_Plot.Enabled = false;
        }


        private void PlotForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MagnetController.DataUpdated -= ListUpdated;
        }

        private void PlotForm_Resize(object sender, EventArgs e)
        {
            formsPlot1.Width = this.Width - 50;
            formsPlot1.Height = this.Height - 120;
            formsPlot1.Refresh();
        }




    }
}
