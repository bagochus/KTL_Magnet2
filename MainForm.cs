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
using System.Xml.Serialization;



namespace KTL_Magnet2
{
    public delegate void WriteOutpuValueDelegate(int row,int column,double value);
    public delegate void WriteReadoutValueDelegate(int row, double value);

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

        private int InputColumns; 


        public MainForm()
        {
            InitializeComponent();
            //msr = new Measurer();
            //msr.Initialize();
            running = false;
            ScanDevices();
            measController.writeOutput = WriteNewOutputValue;
            measController.writeReadout = WriteBReadoutValue;

            try { measController.LoadSettings("last.json"); }
            catch {
                measController = new MeasController();
                
                MessageBox.Show("Не удалось загрузить предыдущие настройки"); }
            BuildTable();



        }



        public void SetupUpdated()
        {
            try { BuildTable(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void AddBindedFloatColumn(String name, string pr_name)
        {
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = pr_name;
            column.Name = name;
            column.DefaultCellStyle.Format = "0.###";
            dataGridView1.Columns.Add(column);
        }
        private void AddFloatColumn(String name)
        {
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.Name = name;
            column.DefaultCellStyle.Format = "0.###";
            dataGridView1.Columns.Add(column);
        }

        private void BuildTable()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = measController.inputLines;
            InputColumns = 0;
            
            if (measController.expSetup.UseSetpointCalibrationTables)
            {
                AddBindedFloatColumn("B_setpoint", "B_setpoint");
                InputColumns++;
            }
            if (!measController.expSetup.UseSetpointCalibrationTables | measController.expSetup.ShowConvertedV)
            {
                AddBindedFloatColumn("V1", "V1");
                AddBindedFloatColumn("V2", "V2");
                AddBindedFloatColumn("Sign", "Sign");     
                InputColumns += 3;
            }
            if (measController.expSetup.UseReadoutCalibrationTables)
            {
                AddFloatColumn("B_readout");
                InputColumns++;
            }

            for (int i = 0; i < measController.expSetup.ad_Measurments.Count; i++)
            {
                AddFloatColumn("AD_" + measController.expSetup.ad_Measurments[i].ch_num.ToString());
            }

            for (int i = 0; i < measController.expSetup.visa_Measurments.Count; i++) 
            {
                AddFloatColumn("VISA_" + measController.expSetup.visa_Measurments[i].Type.ToString());
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

        public void Start()  // действия при нажатии на старт
        {
            measController.Start();

            
        }  

        public void WriteNewOutputValue(int row,int column,double value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new WriteOutpuValueDelegate(WriteNewOutputValue), new object[] { row, column, value });
                return;
            }
            dataGridView1.Rows[row].Cells[column+InputColumns].Value = value;

        }

        public void WriteBReadoutValue(int row, double value)
        {

            if (InvokeRequired)
            {
                this.Invoke(new WriteReadoutValueDelegate(WriteBReadoutValue), new object[] { row, value });
                return;
            }
            dataGridView1.Rows[row].Cells[InputColumns-1].Value = value;

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
            PlotForm plotForm = new PlotForm(measController);
            plotForm.Show();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        
        private void AddExps()
        {
            AddLineForm frm4 = new AddLineForm();
            frm4.ShowDialog();
            if (frm4.input_valid)
            {
                for (int i = 0; i < frm4.steps; i++)
                {
                    measController.AddExperiment(frm4.sign,
                        frm4.v1_start + i * frm4.v1_step,
                        frm4.v2_start + i * frm4.v2_step);
                }
            }
        }
        private void AddExpsCalibrationMode()
        {
            AddLineBForm addLineBForm = new AddLineBForm(measController);
            addLineBForm.ShowDialog();
        }

        private void добавитьСтрокиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (measController.expSetup.UseSetpointCalibrationTables) AddExpsCalibrationMode();
            else AddExps();
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

        private void графикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlotForm form = new PlotForm(measController);
            form.Show();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Настраиваем параметры диалога
            saveFileDialog.Filter = "Файлы CSV (*.csv)|*.csv|Все файлы (*.*)|*.*";
            saveFileDialog.FilterIndex = 1; // Устанавливаем фильтр по умолчанию
            saveFileDialog.Title = "Сохранить файл";
            saveFileDialog.DefaultExt = "csv"; // Расширение по умолчанию
            saveFileDialog.AddExtension = true; // Автоматически добавлять расширение
            saveFileDialog.OverwritePrompt = true; // Предупреждать о перезаписи файла

            // Показать диалог и проверить, нажал ли пользователь OK
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Здесь код для сохранения файла
                    string filePath = saveFileDialog.FileName;
                    measController.SaveTableAsCSV(filePath);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
    


}
