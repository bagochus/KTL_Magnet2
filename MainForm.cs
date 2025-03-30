using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenLayers.Base;
using System.Threading;
using NationalInstruments.VisaNS;
using System.IO;
using System.Globalization;



namespace KTL_Magnet2
{
    public partial class MainForm : Form
    {   
        MeasController measController = new MeasController();
        Measurer msr;
        AD_settings ads3;
        Thread m_thread;
        bool running = false;
        bool dt_ready = false;
        String[] VisaRes;
        VisaMeasurment[] vm;



        public MainForm()
        {
            InitializeComponent();
            msr = new Measurer();
            //msr.Initialize();
            running = false;
            ScanDevices();

            try { measController.LoadSettings("last.json"); }
            catch { MessageBox.Show("Не удалось загрузить предыдущие настройки"); }
        }



        public void SetupUpdated()
        {
            try { BuildTable(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }



        private void BuildTable()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = measController.inputLines;
            
            if (measController.expSetup.UseSetpointCalibrationTables)
            {
                DataGridViewColumn column = new DataGridViewColumn();
                column.CellTemplate = new DataGridViewTextBoxCell();
                column.DataPropertyName = "B_Setpoint";
                column.Name = "B setpoint";
                dataGridView1.Columns.Add(column);
            }
            else
            {
                DataGridViewColumn column_v1 = new DataGridViewColumn(); 
                column_v1.CellTemplate = new DataGridViewTextBoxCell();
                column_v1.Name = "V1";
                column_v1.DataPropertyName = "V1";
                dataGridView1.Columns.Add(column_v1);
                DataGridViewColumn column_v2 = new DataGridViewColumn();
                column_v2.CellTemplate = new DataGridViewTextBoxCell();
                column_v2.Name = "V2";
                column_v2.DataPropertyName = "V2";
                dataGridView1.Columns.Add(column_v2);
            }

            for (int i = 0; i < measController.expSetup.ad_Measurments.Count; i++)
            {
                DataGridViewColumn column = new DataGridViewColumn();
                column.CellTemplate = new DataGridViewTextBoxCell();
                column.Name = "AD_" + measController.expSetup.ad_Measurments[i].ch_num.ToString();
                dataGridView1.Columns.Add(column);
            }

            for (int i = 0; i < measController.expSetup.visa_Measurments.Count; i++) 
            {
                DataGridViewColumn column = new DataGridViewColumn();
                column.CellTemplate = new DataGridViewTextBoxCell();
                column.Name = "VISA_" + measController.expSetup.visa_Measurments[i].Type.ToString();
                dataGridView1.Columns.Add(column);
            }
            

        }






      
        public void Update_Interface()  // обновление таблицы и статуса в ходе измерений, обновляется в потоке
        {
            int my_n = -1;
            while (!msr.Terminated || my_n < msr.n_current)
            {
                if (my_n<msr.n_current)
                {
                    my_n++;
                    //statusStrip1.Text = "Executing line " + msr.n_current.ToString() + "out of " + msr.n_meas.ToString();
                    toolStripStatusLabel1.Text = "Executing line " + (msr.n_current+1).ToString() + " out of " + msr.n_meas.ToString();
                    
                    for (int i =0; i < msr.ads.count; i++) 
                    {
                        dataGridView1.Rows[my_n].Cells[i+3].Value = msr.v_readout[my_n,i];
                    }
                    for (int i = 0;i < vm.Count(); i++)
                    {
                        dataGridView1.Rows[my_n].Cells[i + 3+ msr.ads.count].Value = msr.visa_readout[my_n, i];
                    }
                    
                }

            }
            toolStripStatusLabel1.Text = "Finished";

        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Output.csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = dataGridView1.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dataGridView1.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += dataGridView1.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < dataGridView1.Rows.Count-1; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += dataGridView1.Rows[i - 1].Cells[j].Value.ToString() + ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, System.Text.Encoding.UTF8);
                            MessageBox.Show("Data Exported Successfully !!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        } // сохранение таблицы

        private void button3_Click(object sender, EventArgs e)
        {
            Start();           
        } // конпка старт

        public int Start()  // действия при нажатии на старт
        {
            ScanDevices();

            if (!dt_ready)
            {
                MessageBox.Show("Ошибка свзяи с блоком ЦПА/АЦП");
                return 1;
            }
            if (ads3 == null) ads3 = new AD_settings(0);
            msr.ads = ads3;
            msr.Initialize();
            if (!msr.Initialized) { return 2; }
            msr.visaMeasurments = vm;

            if (msr!=null && msr.Initialized && msr.Terminated && dt_ready)
            {

                int line = 0;

                if (CheckTable(out line))
                {
                    LoadTable();
                    m_thread = new Thread(msr.Work);
                    m_thread.Start();
                    running = true;
                    Thread t_thread = new Thread(this.Update_Interface);
                    t_thread.Start();
                }
                else MessageBox.Show("Error at line " + (line + 1).ToString());
            }

            return 0;
        }  


        public void UpdateTable()
        {
            int count = dataGridView1.ColumnCount;
            for (int iCol = 3;iCol < count; iCol++) 
            {
                dataGridView1.Columns.RemoveAt(3);
            }
            for (int i = 0; i < ads3.count;i++)
            {
                dataGridView1.Columns.Add("col" + i.ToString(), "V" + ads3.ch_num[i].ToString());
            }
            for (int i = 0; i < vm.Count(); i++)
            {
                dataGridView1.Columns.Add("vcol" + i.ToString(), vm[i].Type.ToString() + "_" + i.ToString() );
            }


        }

        public bool CheckTable(out int line)
        {
            line = 0;
            bool result = true;
            for(int iRow = 0;iRow < dataGridView1.Rows.Count-1;iRow++)
            {
                int sign = 0;

                result = Int32.TryParse(dataGridView1.Rows[iRow].Cells[0].Value.ToString(), out sign);
                result = ((sign==-1)||(sign==1));
                if (!result)
                {
                    line = iRow;
                    break;
                }
                double v = 0;
                result = Double.TryParse(dataGridView1.Rows[iRow].Cells[1].Value.ToString(), out v);
                if (result) result = Double.TryParse(dataGridView1.Rows[iRow].Cells[2].Value.ToString(), out v);
                if (!result)
                {
                    line = iRow;
                    break;
                }
            }
            return result; 
        }

        public void LoadTable()

        {
            msr.Prepare(dataGridView1.RowCount - 1);
            for (int iRow = 0; iRow < dataGridView1.Rows.Count-1; iRow++) 
            {
                msr.v_sign[iRow] = int.Parse(dataGridView1.Rows[iRow].Cells[0].Value.ToString());
                msr.volt1[iRow] = Double.Parse(dataGridView1.Rows[iRow].Cells[1].Value.ToString());
                msr.volt2[iRow] = Double.Parse(dataGridView1.Rows[iRow].Cells[2].Value.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!msr.Terminated) { msr.Terminated = true; }
        }



        private void ScanDevices()
        {
            

            try
            {
                ResourceManager rm;
                rm = ResourceManager.GetLocalManager();
                VisaRes = rm.FindResources("(USB)?*");
                DeviceMgr dev_mgr = DeviceMgr.Get();
                String[] OLdevices = dev_mgr.GetDeviceNames();
                dt_ready = (OLdevices.Length == 1);
            }
            catch (Exception e ) { MessageBox.Show(e.Message); }




        }

        private void plotButton_Click(object sender, EventArgs e)
        {
            PlotForm plotForm = new PlotForm();
            plotForm.Show();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddLineForm frm4 = new AddLineForm();
            frm4.ShowDialog();
            if (frm4.input_valid)
            {
                int i_start = dataGridView1.Rows.Count - 1;
                dataGridView1.Rows.Add(frm4.steps);

                for (int i = 0; i < frm4.steps; i++)
                {
                    dataGridView1.Rows[i_start+i].Cells[0].Value = frm4.sign;
                    dataGridView1.Rows[i_start + i].Cells[1].Value = frm4.v1_start + i * frm4.v1_step;
                    dataGridView1.Rows[i_start + i].Cells[2].Value = frm4.v2_start + i * frm4.v2_step;
                }




            }



        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }


        private void добавитьСтрокиToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void настройкиПрограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MeasSetupForm msf = new MeasSetupForm(measController, this);
            msf.Show();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            measController.SaveSettings("last.json");
        }
    }


}
