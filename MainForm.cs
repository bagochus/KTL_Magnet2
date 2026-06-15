using NationalInstruments.VisaNS;
using OpenLayers.Base;
using ScottPlot;
using ScottPlot.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;



namespace KTL_Magnet2
{
    public delegate void WriteOutpuValueDelegate(int row,int column,double value);
    public delegate void WriteReadoutValueDelegate(int row, double value);

    public partial class MainForm : Form
    {
        MeasController measController;
        Measurer msr;
        AD_settings ads3;
        Thread m_thread;
        bool running = false;
        bool dt_ready = false;
        String[] VisaRes;
        VisaMeasurement[] vm;

        private int InputColumns; 


        public MainForm()
        {
            InitializeComponent();
            //msr = new Measurer();
            //msr.Initialize();
            measController = new MeasController(this);
            running = false;
            ScanDevices();
            measController.writeOutput = WriteNewOutputValue;
            measController.writeReadout = WriteBReadoutValue;

            try { measController.LoadSettings("last.json"); }
            catch {
                measController = new MeasController(this);   
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

            for (int i = 0; i < measController.expSetup.ad_Measurements.Count; i++)
            {
                AddFloatColumn("AD_" + measController.expSetup.ad_Measurements[i].ch_num.ToString());
            }

            for (int i = 0; i < measController.expSetup.visa_Measurements.Count; i++) 
            {
                AddFloatColumn("VISA_" + measController.expSetup.visa_Measurements[i].Type.ToString());
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

        private void LoadProfile()
        {
            SelectProfileForm form = new SelectProfileForm();   
            form.ShowDialog();
            if (form.selected_name != null) DB_Manager.LoadProfile(form.selected_name);
        }

      /*  private void SaveProfile()
        {
            string userInput = Interaction.InputBox("Введите ширину активной области в пикселях:",
                "Определение активной области",
                "1024");


        }
      */



        private void button4_Click(object sender, EventArgs e)
        {
            measController.terminated = true;
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

        private void MainForm_Resize(object sender, EventArgs e)
        {
            dataGridView1.Width = this.Width-50;
            dataGridView1.Height = this.Height-120;
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            measController.inputLines.Clear();
        }

        public void WriteStatus(string status)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(WriteStatus), new object[] { status });
                return;
            }
            toolStripStatusLabel1.Text = status;
        }

        public void MarkErrorLine(int line)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<int>(MarkErrorLine), new object[] { line });
                return;
            }
            if (line >= dataGridView1.RowCount) return;
            dataGridView1.Rows[line].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
        }

        public void MarkCompleteLine(int line)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<int>(MarkCompleteLine), new object[] { line });
                return;
            }
            if (line >= dataGridView1.RowCount) return;
            dataGridView1.Rows[line].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
        }

        public void ResetTableColors()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(ResetTableColors), new object[] {  });
                return;
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
            }
        }

        private void сохранитьКакTXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Настраиваем параметры диалога
            saveFileDialog.Filter = "Файлы TXT (*.txt)|*.txt|Все файлы (*.*)|*.*";
            saveFileDialog.FilterIndex = 1; // Устанавливаем фильтр по умолчанию
            saveFileDialog.Title = "Сохранить файл";
            saveFileDialog.DefaultExt = "txt"; // Расширение по умолчанию
            saveFileDialog.AddExtension = true; // Автоматически добавлять расширение
            saveFileDialog.OverwritePrompt = true; // Предупреждать о перезаписи файла

            // Показать диалог и проверить, нажал ли пользователь OK
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Здесь код для сохранения файла
                    string filePath = saveFileDialog.FileName;
                    measController.SaveTableAsTxt(filePath);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void загрузитьНаборИзмеренийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadProfile();
        }

        private void сохранитьПрофильToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
    


}
