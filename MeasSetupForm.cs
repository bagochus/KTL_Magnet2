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
using System.Globalization;
using KTL_Magnet2.Measurment;
using System.IO;
using System.Text.Json;
using static System.Runtime.CompilerServices.Unsafe;
using ScottPlot;
using ScottPlot.Interactivity.UserActions;
//using System.Threading.Tasks.Extensions;


namespace KTL_Magnet2
{
    public  delegate void NotifyParentFormDelegate();

    public partial class MeasSetupForm : Form
    {
        IFormatProvider frmt = new NumberFormatInfo { NumberDecimalSeparator = "." };
        public AD_settings ads2;
        public AD_settings ads;
        public bool params_changed;
        public VisaMeasurment[] visaMeasurments;
        private String[] devlist;
        private LoadSetupDelegate loadSetupDelegate;
        private NotifyParentFormDelegate notifyParentForm;
        private bool previous_usect_state;
        private bool input_list_empty;


        private ExpSetup localExpSetup = new ExpSetup();
        





        public MeasSetupForm(MeasController mc, MainForm parent)
        {
            params_changed = false;
            InitializeComponent();
            mc.ScanDevices();
            this.devlist = mc.VisaRes;

            if (devlist != null)
            {
                for (int i = 1; i < 6; i++)
                {
                    (this.Controls["deviceBox" + i.ToString()] as ComboBox).Items.Add("");
                    (this.Controls["deviceBox" + i.ToString()] as ComboBox).Items.AddRange(devlist);
                }
            }
            loadSetupDelegate = mc.LoadSetup;
            notifyParentForm = parent.SetupUpdated;
            InitialInterfaceSetup();
            ConstructFormFromSetup(mc.expSetup);
            previous_usect_state = mc.expSetup.UseSetpointCalibrationTables;
            input_list_empty = mc.inputLines.Count == 0;
        }

        private void InitialInterfaceSetup()
        {
            for (int i = 1; i <= 5 ; i++ )
            {
                (this.Controls["measBox" + i.ToString()] as ComboBox).Items.Clear();
                foreach (String s in Enum.GetNames(typeof(MeasurmentType)))
                    (this.Controls["measBox" + i.ToString()] as ComboBox).Items.Add(s);
            }
            for (int i = 1; i <= 5; i++)
            {
                (this.Controls["deviceBox" + i.ToString()] as ComboBox).TextChanged += new System.EventHandler(this.VisaChanged);
                (this.Controls["measBox" + i.ToString()] as ComboBox).TextChanged += new System.EventHandler(this.VisaChanged);

            }

            for (int i = 0; i < 5; i++)
            {
                (this.Controls["onBox" + i.ToString()] as CheckBox).CheckedChanged += new System.EventHandler(this.ADChanged);
            }

        }

        private void UpdateReadoutSourceList(ExpSetup setup)
        {
            comboBox_readout.Items.Clear();
            foreach (String s in BuildDataSourceList(setup))
                comboBox_readout.Items.Add(s);
            comboBox_readout.SelectedIndex = GetReadoutId(setup);



        }

        private void ConstructFormFromSetup(ExpSetup setup)
        {
            if (setup == null) { return; }
            bool DevNameError = false;
            for (int i = 0 ; i < setup.ad_Measurments.Count() ; i++)
            {
                int ch_number = setup.ad_Measurments[i].ch_num;
                (this.Controls["onBox" + ch_number.ToString()] as CheckBox).Checked = true;
                (this.Controls["GainBox" + ch_number.ToString()] as ComboBox).Text = setup.ad_Measurments[i].Gain.ToString();
                (this.Controls["avgBox" + ch_number.ToString()] as TextBox).Text = setup.ad_Measurments[i].Avg.ToString();
                (this.Controls["k_delayBox" + ch_number.ToString()] as TextBox).Text = setup.ad_Measurments[i].Delay.ToString();
            }

            for (int i = 1; i < setup.visa_Measurments.Count() + 1; i++)
            {

                DevNameError = !devlist.Contains(setup.visa_Measurments[i - 1].DeviceName);
                (this.Controls["deviceBox" + i.ToString()] as ComboBox).Text = setup.visa_Measurments[i - 1].DeviceName;
                (this.Controls["measBox" + i.ToString()] as ComboBox).SelectedIndex = (int)setup.visa_Measurments[i - 1].Type;
                if (setup.visa_Measurments[i - 1].Channel != -1)
                {
                    (this.Controls["channelBox" + i.ToString()] as TextBox).Text = setup.visa_Measurments[i - 1].Channel.ToString();
                }
                if (setup.visa_Measurments[i - 1].Limit != -1)
                {
                    (this.Controls["limitBox" + i.ToString()] as TextBox).Text = setup.visa_Measurments[i - 1].Limit.ToString();
                }
                if (setup.visa_Measurments[i - 1].PLC_time != -1)
                {
                    (this.Controls["timeBox" + i.ToString()] as TextBox).Text = setup.visa_Measurments[i - 1].PLC_time.ToString();
                }
                if (setup.visa_Measurments[i - 1].Delay != -1)
                {
                    (this.Controls["delayBox" + i.ToString()] as TextBox).Text = setup.visa_Measurments[i - 1].Delay.ToString();
                }
                if (DevNameError) MessageBox.Show("Устройство VISA недоступно");
            }
            //if (DevNameError) MessageBox.Show("Устройство VISA недоступно");

            checkBox_use_ct_readout.Checked = setup.UseReadoutCalibrationTables;
            checkBox_use_ct_setpoint.Checked = setup.UseSetpointCalibrationTables;
            checkBox_show_v.Checked = setup.ShowConvertedV;

            textBox_v1_filename.Text = setup.v1_filename;
            textBox_v2_filename.Text = setup.v2_filename;
            textBox_readout_filename.Text = setup.Readout_filename; 


            UpdateReadoutSourceList(setup);

            textBox_dbmax.Text = setup.MaxBStep.ToString();
            textBox_dv1max.Text = setup.MaxV1Step.ToString();
            textBox_dv2max.Text = setup.MaxV2Step.ToString();
            textBox_bsrmax.Text = setup.MaxBSlewrate.ToString();
            textBox_v1srmax.Text = setup.MaxV1SlewRate.ToString();
            textBox_v2srmax.Text = setup.MaxV2SlewRate.ToString();
            textBox_zeroDelay.Text = setup.ZeroCrossingDelay.ToString();

            checkBox_smooth.Checked = setup.UseSmoothZeroCrossing;
            textBox_smoothdelay.Text = setup.SmoothDelay.ToString();
            textBox_smoothstep.Text = setup.SmoothStep.ToString();


        }

        #region Redaout source management
        private List<String> BuildDataSourceList(ExpSetup expSetup)
        {
            List<String> result = new List<String>();

            for (int i = 0 ; i< expSetup.ad_Measurments.Count() ;i++ )
            {
                result.Add("AD_ch_" + expSetup.ad_Measurments[i].ch_num.ToString());
            }

            for (int i = 0; i < expSetup.visa_Measurments.Count(); i++)
            {
                result.Add("VISA_" + expSetup.visa_Measurments[i].Type.ToString()
                    + "_" + (i+1).ToString());
            }
            return result;
        }

        private void SetReadoutSource (ExpSetup setup, int number)
        {
            int ad_count = setup.ad_Measurments.Count();

            if (number >= ad_count)
            {
                setup.readoutSourceType = ReadoutSourceType.AD;
                setup.readoutSourceId = number;
            }
            else 
            {
                setup.readoutSourceType = ReadoutSourceType.Visa;
                setup.readoutSourceId = number - ad_count;
            }
        }

        private int GetReadoutId(ExpSetup setup)
        {
            if (setup.readoutSourceId == -1) return -1;
            int ad_count = setup.ad_Measurments.Count();  
            if (!setup.UseReadoutCalibrationTables) return -1;
            if (setup.readoutSourceType == ReadoutSourceType.AD) return setup.readoutSourceId;
            if (setup.readoutSourceType == ReadoutSourceType.Visa) return setup.readoutSourceId + ad_count;

            return -1;
        }

        private void VisaChanged(object sender, EventArgs e)
        {
            UpdateVisaMeasurments();
            UpdateReadoutSourceList(localExpSetup);
            SetReadoutSource(localExpSetup,comboBox_readout.SelectedIndex);
            if (localExpSetup.readoutSourceType == ReadoutSourceType.Visa)
            {
                comboBox_readout.SelectedIndex = -1;
                localExpSetup.readoutSourceId = -1;
            }
            
        }

        private void ADChanged(object sender, EventArgs e)
        {
            UpdateADS();
            UpdateReadoutSourceList(localExpSetup);
            SetReadoutSource(localExpSetup, comboBox_readout.SelectedIndex);
            if (localExpSetup.readoutSourceType == ReadoutSourceType.AD)
            {
                comboBox_readout.SelectedIndex = -1;
                localExpSetup.readoutSourceId = -1;
            }
        }


        #endregion



        private bool ReadFields()
        {
            bool reading_ok = false;
            try
            {
                UpdateADS();
                UpdateVisaMeasurments();
                localExpSetup.MaxBStep = Double.Parse(textBox_dbmax.Text);
                localExpSetup.MaxV1Step = Double.Parse(textBox_dv1max.Text);
                localExpSetup.MaxV2Step = Double.Parse(textBox_dv2max.Text);
                localExpSetup.MaxBSlewrate = Double.Parse(textBox_bsrmax.Text);
                localExpSetup.MaxV1SlewRate = Double.Parse(textBox_v1srmax.Text);
                localExpSetup.MaxV2SlewRate = Double.Parse(textBox_v2srmax.Text);
                localExpSetup.ZeroCrossingDelay = Int32.Parse(textBox_zeroDelay.Text);

                localExpSetup.UseReadoutCalibrationTables = checkBox_use_ct_readout.Checked;
                if (localExpSetup.UseReadoutCalibrationTables)
                {
                    localExpSetup.Readout_filename = textBox_readout_filename.Text;
                    if (localExpSetup.Readout_filename == "")
                        throw new Exception("Не указано имя файла с калибровочной таблицей");
                    if (comboBox_readout.SelectedIndex == -1)
                        throw new Exception("Не указан источник данных для калибровки");

                }

                localExpSetup.UseSetpointCalibrationTables = checkBox_use_ct_setpoint.Checked;
                localExpSetup.ShowConvertedV = checkBox_show_v.Checked;
                if (localExpSetup.UseSetpointCalibrationTables)
                {
                    localExpSetup.v2_filename = textBox_v2_filename.Text;
                    localExpSetup.v1_filename = textBox_v1_filename.Text;
                    if (localExpSetup.v2_filename == "" | localExpSetup.v1_filename == "")
                        throw new Exception("Не указано имя файла с калибровочной таблицей");
                }

                if (localExpSetup.UseReadoutCalibrationTables)
                {
                    SetReadoutSource(localExpSetup, comboBox_readout.SelectedIndex);
                }

                reading_ok = true;

                if (!previous_usect_state &&
                    localExpSetup.UseSetpointCalibrationTables &&
                    !input_list_empty)
                {
                    DialogResult result = MessageBox.Show(
                            "Включение режима калибровки очистит список экспериментов. \nПродолжить?",
                            "Сообщение",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);

                    reading_ok = (result == DialogResult.Yes);
                }

                localExpSetup.UseSmoothZeroCrossing = checkBox_smooth.Checked;
                if (localExpSetup.UseSmoothZeroCrossing)
                {
                    localExpSetup.SmoothDelay = Int32.Parse (textBox_smoothdelay.Text);
                    localExpSetup.SmoothStep = double.Parse (textBox_smoothstep.Text);
                }




                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); } 
            return reading_ok;

        }


        private void UpdateADS()
        {
            localExpSetup.ad_Measurments.Clear();
            for (int i = 0; i < 5; i++)
            {
                if ((this.Controls["onBox" + i.ToString()] as CheckBox).Checked)
                {
                    AD_Measurment aD_Measurment = new AD_Measurment();
                    aD_Measurment.ch_num = i;

                    if ((this.Controls["GainBox" + i.ToString()] as ComboBox).Text == "")
                    {
                        aD_Measurment.Gain = 1;
                    }
                    else
                    {
                        aD_Measurment.Gain = Int32.Parse((this.Controls["GainBox" + i.ToString()] as ComboBox).Text);
                    }
                    aD_Measurment.Avg = Int32.Parse((this.Controls["avgBox" + i.ToString()] as TextBox).Text);
                    aD_Measurment.Delay = Int32.Parse((this.Controls["k_delayBox" + i.ToString()] as TextBox).Text);
                    localExpSetup.ad_Measurments.Add(aD_Measurment);
                }
            }

        }


        private void UpdateVisaMeasurments()
        {
            localExpSetup.visa_Measurments.Clear();
            for (int i = 1; i <=5; i++)
            {

                bool line_valid = false;
                line_valid = ((this.Controls["deviceBox" + i.ToString()] as ComboBox).Text != "");
                line_valid &= ((this.Controls["measBox" + i.ToString()] as ComboBox).Text != "");

                if (line_valid)
                {

                    VISA_Measurment vtemp = new VISA_Measurment();
                    vtemp.DeviceName = (this.Controls["deviceBox" + i.ToString()] as ComboBox).Text;

                    String comboBox_text = (this.Controls["measBox" + i.ToString()] as ComboBox).Text;
                    for (int j = 0; j < Enum.GetValues(typeof(MeasurmentType)).Length; j++)
                    {
                        if (comboBox_text == ((MeasurmentType)j).ToString()) vtemp.Type = (MeasurmentType)j;

                    }

                    if ((this.Controls["channelBox" + i.ToString()] as TextBox).Text != "")
                    {
                        vtemp.Channel = Int32.Parse((this.Controls["channelBox" + i.ToString()] as TextBox).Text);
                    }
                    if ((this.Controls["limitBox" + i.ToString()] as TextBox).Text != "")
                    {
                        vtemp.Limit = Double.Parse((this.Controls["limitBox" + i.ToString()] as TextBox).Text,frmt);
                    }
                    if ((this.Controls["timeBox" + i.ToString()] as TextBox).Text != "")
                    {
                        vtemp.PLC_time = Double.Parse((this.Controls["timeBox" + i.ToString()] as TextBox).Text,frmt);
                    }
                    if ((this.Controls["delayBox" + i.ToString()] as TextBox).Text != "")
                    {
                        vtemp.Delay = Int32.Parse((this.Controls["delayBox" + i.ToString()] as TextBox).Text);
                    }

                    localExpSetup.visa_Measurments.Add(vtemp);

                }

            }

        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (ReadFields())
            {
                loadSetupDelegate(localExpSetup);
                notifyParentForm();
                params_changed = true;
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            params_changed = false;
            this.Close();
        }

        private void deviceBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void MeasSetupForm_Load(object sender, EventArgs e)
        {

        }
    }
}
