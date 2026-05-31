namespace KTL_Magnet2
{
    partial class NewMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel_status = new System.Windows.Forms.Panel();
            this.label_v2 = new System.Windows.Forms.Label();
            this.label_v1 = new System.Windows.Forms.Label();
            this.label_breadout = new System.Windows.Forms.Label();
            this.label_bsetpoint = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_steady = new System.Windows.Forms.RadioButton();
            this.radioButton_list = new System.Windows.Forms.RadioButton();
            this.radioButton_fromto = new System.Windows.Forms.RadioButton();
            this.panel_fromto = new System.Windows.Forms.Panel();
            this.checkBox_path_to_zero = new System.Windows.Forms.CheckBox();
            this.checkBox_reverse = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_bset_delay = new System.Windows.Forms.TextBox();
            this.textBox_b_step = new System.Windows.Forms.TextBox();
            this.textBox_b_end = new System.Windows.Forms.TextBox();
            this.textBox_b_start = new System.Windows.Forms.TextBox();
            this.panel_list = new System.Windows.Forms.Panel();
            this.button_edit_blist = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_bset_delay_list = new System.Windows.Forms.TextBox();
            this.panel_steady = new System.Windows.Forms.Panel();
            this.label_increment = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button_refresh = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label_outer_value = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_steady_delay = new System.Windows.Forms.TextBox();
            this.textBox_outer_value = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.textBox_b_level = new System.Windows.Forms.TextBox();
            this.button_stop = new System.Windows.Forms.Button();
            this.button_start = new System.Windows.Forms.Button();
            this.button_settings = new System.Windows.Forms.Button();
            this.button_measurement = new System.Windows.Forms.Button();
            this.button_data = new System.Windows.Forms.Button();
            this.button_plot = new System.Windows.Forms.Button();
            this.textBox_filename = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label_pol = new System.Windows.Forms.Label();
            this.label_status = new System.Windows.Forms.Label();
            this.panel_status.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel_fromto.SuspendLayout();
            this.panel_list.SuspendLayout();
            this.panel_steady.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_status
            // 
            this.panel_status.Controls.Add(this.label_status);
            this.panel_status.Controls.Add(this.label_pol);
            this.panel_status.Controls.Add(this.label_v2);
            this.panel_status.Controls.Add(this.label_v1);
            this.panel_status.Controls.Add(this.label_breadout);
            this.panel_status.Controls.Add(this.label_bsetpoint);
            this.panel_status.Location = new System.Drawing.Point(12, 12);
            this.panel_status.Name = "panel_status";
            this.panel_status.Size = new System.Drawing.Size(434, 281);
            this.panel_status.TabIndex = 0;
            // 
            // label_v2
            // 
            this.label_v2.AutoSize = true;
            this.label_v2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_v2.Location = new System.Drawing.Point(16, 162);
            this.label_v2.Name = "label_v2";
            this.label_v2.Size = new System.Drawing.Size(241, 24);
            this.label_v2.TabIndex = 2;
            this.label_v2.Text = "U2 =  V (контроль тока)";
            // 
            // label_v1
            // 
            this.label_v1.AutoSize = true;
            this.label_v1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_v1.Location = new System.Drawing.Point(16, 127);
            this.label_v1.Name = "label_v1";
            this.label_v1.Size = new System.Drawing.Size(317, 24);
            this.label_v1.TabIndex = 1;
            this.label_v1.Text = "U1 =  V (контроль напряжения)";
            // 
            // label_breadout
            // 
            this.label_breadout.AutoSize = true;
            this.label_breadout.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_breadout.Location = new System.Drawing.Point(13, 73);
            this.label_breadout.Name = "label_breadout";
            this.label_breadout.Size = new System.Drawing.Size(241, 39);
            this.label_breadout.TabIndex = 1;
            this.label_breadout.Text = "B факт =   Тл";
            // 
            // label_bsetpoint
            // 
            this.label_bsetpoint.AutoSize = true;
            this.label_bsetpoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_bsetpoint.Location = new System.Drawing.Point(13, 15);
            this.label_bsetpoint.Name = "label_bsetpoint";
            this.label_bsetpoint.Size = new System.Drawing.Size(221, 39);
            this.label_bsetpoint.TabIndex = 0;
            this.label_bsetpoint.Text = "B уст. =   Тл";
            this.label_bsetpoint.Click += new System.EventHandler(this.label1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_steady);
            this.groupBox1.Controls.Add(this.radioButton_list);
            this.groupBox1.Controls.Add(this.radioButton_fromto);
            this.groupBox1.Location = new System.Drawing.Point(456, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 129);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Сценарий изменения В";
            // 
            // radioButton_steady
            // 
            this.radioButton_steady.AutoSize = true;
            this.radioButton_steady.Location = new System.Drawing.Point(20, 83);
            this.radioButton_steady.Name = "radioButton_steady";
            this.radioButton_steady.Size = new System.Drawing.Size(141, 17);
            this.radioButton_steady.TabIndex = 2;
            this.radioButton_steady.Text = "Удерживать на уровне";
            this.radioButton_steady.UseVisualStyleBackColor = true;
            this.radioButton_steady.CheckedChanged += new System.EventHandler(this.radioButton_steady_CheckedChanged);
            // 
            // radioButton_list
            // 
            this.radioButton_list.AutoSize = true;
            this.radioButton_list.Location = new System.Drawing.Point(20, 57);
            this.radioButton_list.Name = "radioButton_list";
            this.radioButton_list.Size = new System.Drawing.Size(77, 17);
            this.radioButton_list.TabIndex = 1;
            this.radioButton_list.Text = "По списку";
            this.radioButton_list.UseVisualStyleBackColor = true;
            this.radioButton_list.CheckedChanged += new System.EventHandler(this.radioButton_list_CheckedChanged);
            // 
            // radioButton_fromto
            // 
            this.radioButton_fromto.AutoSize = true;
            this.radioButton_fromto.Checked = true;
            this.radioButton_fromto.Location = new System.Drawing.Point(20, 31);
            this.radioButton_fromto.Name = "radioButton_fromto";
            this.radioButton_fromto.Size = new System.Drawing.Size(59, 17);
            this.radioButton_fromto.TabIndex = 0;
            this.radioButton_fromto.TabStop = true;
            this.radioButton_fromto.Text = "От - до";
            this.radioButton_fromto.UseVisualStyleBackColor = true;
            this.radioButton_fromto.CheckedChanged += new System.EventHandler(this.radioButton_fromto_CheckedChanged);
            // 
            // panel_fromto
            // 
            this.panel_fromto.Controls.Add(this.checkBox_path_to_zero);
            this.panel_fromto.Controls.Add(this.checkBox_reverse);
            this.panel_fromto.Controls.Add(this.label7);
            this.panel_fromto.Controls.Add(this.label6);
            this.panel_fromto.Controls.Add(this.label5);
            this.panel_fromto.Controls.Add(this.label4);
            this.panel_fromto.Controls.Add(this.textBox_bset_delay);
            this.panel_fromto.Controls.Add(this.textBox_b_step);
            this.panel_fromto.Controls.Add(this.textBox_b_end);
            this.panel_fromto.Controls.Add(this.textBox_b_start);
            this.panel_fromto.Location = new System.Drawing.Point(676, 27);
            this.panel_fromto.Name = "panel_fromto";
            this.panel_fromto.Size = new System.Drawing.Size(333, 195);
            this.panel_fromto.TabIndex = 2;
            // 
            // checkBox_path_to_zero
            // 
            this.checkBox_path_to_zero.AutoSize = true;
            this.checkBox_path_to_zero.Location = new System.Drawing.Point(17, 150);
            this.checkBox_path_to_zero.Name = "checkBox_path_to_zero";
            this.checkBox_path_to_zero.Size = new System.Drawing.Size(173, 17);
            this.checkBox_path_to_zero.TabIndex = 9;
            this.checkBox_path_to_zero.Text = "Записывать переходы от \"0\"";
            this.checkBox_path_to_zero.UseVisualStyleBackColor = true;
            // 
            // checkBox_reverse
            // 
            this.checkBox_reverse.AutoSize = true;
            this.checkBox_reverse.Location = new System.Drawing.Point(17, 127);
            this.checkBox_reverse.Name = "checkBox_reverse";
            this.checkBox_reverse.Size = new System.Drawing.Size(100, 17);
            this.checkBox_reverse.TabIndex = 8;
            this.checkBox_reverse.Text = "Туда - обратно";
            this.checkBox_reverse.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(173, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Пауза после установки поля, мс";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(144, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Шаг, Тл";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(72, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Конечное знчение, Тл";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Начальное значение, Тл";
            // 
            // textBox_bset_delay
            // 
            this.textBox_bset_delay.Location = new System.Drawing.Point(196, 103);
            this.textBox_bset_delay.Name = "textBox_bset_delay";
            this.textBox_bset_delay.Size = new System.Drawing.Size(100, 20);
            this.textBox_bset_delay.TabIndex = 3;
            // 
            // textBox_b_step
            // 
            this.textBox_b_step.Location = new System.Drawing.Point(196, 75);
            this.textBox_b_step.Name = "textBox_b_step";
            this.textBox_b_step.Size = new System.Drawing.Size(100, 20);
            this.textBox_b_step.TabIndex = 2;
            // 
            // textBox_b_end
            // 
            this.textBox_b_end.Location = new System.Drawing.Point(196, 47);
            this.textBox_b_end.Name = "textBox_b_end";
            this.textBox_b_end.Size = new System.Drawing.Size(100, 20);
            this.textBox_b_end.TabIndex = 1;
            // 
            // textBox_b_start
            // 
            this.textBox_b_start.Location = new System.Drawing.Point(196, 19);
            this.textBox_b_start.Name = "textBox_b_start";
            this.textBox_b_start.Size = new System.Drawing.Size(100, 20);
            this.textBox_b_start.TabIndex = 0;
            // 
            // panel_list
            // 
            this.panel_list.Controls.Add(this.button_edit_blist);
            this.panel_list.Controls.Add(this.label8);
            this.panel_list.Controls.Add(this.textBox_bset_delay_list);
            this.panel_list.Location = new System.Drawing.Point(676, 236);
            this.panel_list.Name = "panel_list";
            this.panel_list.Size = new System.Drawing.Size(333, 195);
            this.panel_list.TabIndex = 3;
            // 
            // button_edit_blist
            // 
            this.button_edit_blist.Location = new System.Drawing.Point(20, 26);
            this.button_edit_blist.Name = "button_edit_blist";
            this.button_edit_blist.Size = new System.Drawing.Size(281, 21);
            this.button_edit_blist.TabIndex = 16;
            this.button_edit_blist.Text = "Редактировать список значений поля";
            this.button_edit_blist.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(173, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Пауза после установки поля, мс";
            // 
            // textBox_bset_delay_list
            // 
            this.textBox_bset_delay_list.Location = new System.Drawing.Point(201, 58);
            this.textBox_bset_delay_list.Name = "textBox_bset_delay_list";
            this.textBox_bset_delay_list.Size = new System.Drawing.Size(100, 20);
            this.textBox_bset_delay_list.TabIndex = 14;
            // 
            // panel_steady
            // 
            this.panel_steady.Controls.Add(this.label_increment);
            this.panel_steady.Controls.Add(this.label12);
            this.panel_steady.Controls.Add(this.label11);
            this.panel_steady.Controls.Add(this.label_outer_value);
            this.panel_steady.Controls.Add(this.label9);
            this.panel_steady.Controls.Add(this.textBox_steady_delay);
            this.panel_steady.Controls.Add(this.textBox_outer_value);
            this.panel_steady.Controls.Add(this.radioButton5);
            this.panel_steady.Controls.Add(this.radioButton4);
            this.panel_steady.Controls.Add(this.textBox_b_level);
            this.panel_steady.Location = new System.Drawing.Point(12, 309);
            this.panel_steady.Name = "panel_steady";
            this.panel_steady.Size = new System.Drawing.Size(333, 195);
            this.panel_steady.TabIndex = 4;
            // 
            // label_increment
            // 
            this.label_increment.AutoSize = true;
            this.label_increment.Location = new System.Drawing.Point(240, 57);
            this.label_increment.Name = "label_increment";
            this.label_increment.Size = new System.Drawing.Size(0, 13);
            this.label_increment.TabIndex = 22;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 166);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(301, 13);
            this.label12.TabIndex = 21;
            this.label12.Text = "Shift+Enter - инкремент / Ctrl+Enter - обновление величины";
            // 
            // button_refresh
            // 
            this.button_refresh.Location = new System.Drawing.Point(362, 421);
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.Size = new System.Drawing.Size(75, 50);
            this.button_refresh.TabIndex = 6;
            this.button_refresh.Text = "Обновить значения";
            this.button_refresh.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(70, 83);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Пауза, мс";
            // 
            // label_outer_value
            // 
            this.label_outer_value.AutoSize = true;
            this.label_outer_value.Location = new System.Drawing.Point(26, 57);
            this.label_outer_value.Name = "label_outer_value";
            this.label_outer_value.Size = new System.Drawing.Size(104, 13);
            this.label_outer_value.TabIndex = 19;
            this.label_outer_value.Text = "Внешний параметр";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(102, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Индукция поля, Тл";
            // 
            // textBox_steady_delay
            // 
            this.textBox_steady_delay.Location = new System.Drawing.Point(134, 80);
            this.textBox_steady_delay.Name = "textBox_steady_delay";
            this.textBox_steady_delay.Size = new System.Drawing.Size(100, 20);
            this.textBox_steady_delay.TabIndex = 17;
            // 
            // textBox_outer_value
            // 
            this.textBox_outer_value.ContextMenuStrip = this.contextMenuStrip1;
            this.textBox_outer_value.Location = new System.Drawing.Point(134, 54);
            this.textBox_outer_value.Name = "textBox_outer_value";
            this.textBox_outer_value.Size = new System.Drawing.Size(100, 20);
            this.textBox_outer_value.TabIndex = 5;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(190, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(189, 22);
            this.toolStripMenuItem1.Text = "Переименовать";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(189, 22);
            this.toolStripMenuItem2.Text = "Добавить инкремент";
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Checked = true;
            this.radioButton5.Location = new System.Drawing.Point(20, 119);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(179, 17);
            this.radioButton5.TabIndex = 16;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Вести измерения непрерывно";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(20, 142);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(223, 17);
            this.radioButton4.TabIndex = 15;
            this.radioButton4.Text = "Проводить измерения при обновлении";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // textBox_b_level
            // 
            this.textBox_b_level.Location = new System.Drawing.Point(134, 28);
            this.textBox_b_level.Name = "textBox_b_level";
            this.textBox_b_level.Size = new System.Drawing.Size(100, 20);
            this.textBox_b_level.TabIndex = 14;
            // 
            // button_stop
            // 
            this.button_stop.Location = new System.Drawing.Point(362, 365);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(75, 50);
            this.button_stop.TabIndex = 13;
            this.button_stop.Text = "СТОП";
            this.button_stop.UseVisualStyleBackColor = true;
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(362, 311);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 48);
            this.button_start.TabIndex = 12;
            this.button_start.Text = "СТАРТ";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // button_settings
            // 
            this.button_settings.Location = new System.Drawing.Point(455, 150);
            this.button_settings.Name = "button_settings";
            this.button_settings.Size = new System.Drawing.Size(91, 72);
            this.button_settings.TabIndex = 6;
            this.button_settings.Text = "Настройки";
            this.button_settings.UseVisualStyleBackColor = true;
            this.button_settings.Click += new System.EventHandler(this.button_settings_Click);
            // 
            // button_measurement
            // 
            this.button_measurement.Location = new System.Drawing.Point(562, 150);
            this.button_measurement.Name = "button_measurement";
            this.button_measurement.Size = new System.Drawing.Size(91, 72);
            this.button_measurement.TabIndex = 7;
            this.button_measurement.Text = "Измерения";
            this.button_measurement.UseVisualStyleBackColor = true;
            this.button_measurement.Click += new System.EventHandler(this.button_measurement_Click);
            // 
            // button_data
            // 
            this.button_data.Location = new System.Drawing.Point(563, 236);
            this.button_data.Name = "button_data";
            this.button_data.Size = new System.Drawing.Size(91, 72);
            this.button_data.TabIndex = 9;
            this.button_data.Text = "Данные";
            this.button_data.UseVisualStyleBackColor = true;
            this.button_data.Click += new System.EventHandler(this.button_data_Click);
            // 
            // button_plot
            // 
            this.button_plot.Location = new System.Drawing.Point(456, 236);
            this.button_plot.Name = "button_plot";
            this.button_plot.Size = new System.Drawing.Size(91, 72);
            this.button_plot.TabIndex = 8;
            this.button_plot.Text = "График";
            this.button_plot.UseVisualStyleBackColor = true;
            this.button_plot.Click += new System.EventHandler(this.button_plot_Click);
            // 
            // textBox_filename
            // 
            this.textBox_filename.Location = new System.Drawing.Point(456, 356);
            this.textBox_filename.Name = "textBox_filename";
            this.textBox_filename.Size = new System.Drawing.Size(202, 20);
            this.textBox_filename.TabIndex = 10;
            this.textBox_filename.Leave += new System.EventHandler(this.textBox_filename_Leave);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(453, 328);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(147, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Имя файла для сохранения";
            // 
            // label_pol
            // 
            this.label_pol.AutoSize = true;
            this.label_pol.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_pol.Location = new System.Drawing.Point(16, 199);
            this.label_pol.Name = "label_pol";
            this.label_pol.Size = new System.Drawing.Size(140, 24);
            this.label_pol.TabIndex = 3;
            this.label_pol.Text = "Полярность: ";
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_status.Location = new System.Drawing.Point(19, 233);
            this.label_status.MaximumSize = new System.Drawing.Size(380, 0);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(43, 24);
            this.label_status.TabIndex = 4;
            this.label_status.Text = "___";
            // 
            // NewMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 529);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.button_refresh);
            this.Controls.Add(this.textBox_filename);
            this.Controls.Add(this.button_data);
            this.Controls.Add(this.button_plot);
            this.Controls.Add(this.button_measurement);
            this.Controls.Add(this.button_settings);
            this.Controls.Add(this.panel_steady);
            this.Controls.Add(this.panel_list);
            this.Controls.Add(this.panel_fromto);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.panel_status);
            this.Controls.Add(this.button_start);
            this.Name = "NewMainForm";
            this.Text = "NewMainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewMainForm_FormClosing);
            this.panel_status.ResumeLayout(false);
            this.panel_status.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel_fromto.ResumeLayout(false);
            this.panel_fromto.PerformLayout();
            this.panel_list.ResumeLayout(false);
            this.panel_list.PerformLayout();
            this.panel_steady.ResumeLayout(false);
            this.panel_steady.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_status;
        private System.Windows.Forms.Label label_bsetpoint;
        private System.Windows.Forms.Label label_breadout;
        private System.Windows.Forms.Label label_v1;
        private System.Windows.Forms.Label label_v2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_steady;
        private System.Windows.Forms.RadioButton radioButton_list;
        private System.Windows.Forms.RadioButton radioButton_fromto;
        private System.Windows.Forms.Panel panel_fromto;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_bset_delay;
        private System.Windows.Forms.TextBox textBox_b_step;
        private System.Windows.Forms.TextBox textBox_b_end;
        private System.Windows.Forms.TextBox textBox_b_start;
        private System.Windows.Forms.CheckBox checkBox_path_to_zero;
        private System.Windows.Forms.CheckBox checkBox_reverse;
        private System.Windows.Forms.Panel panel_list;
        private System.Windows.Forms.Button button_edit_blist;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_bset_delay_list;
        private System.Windows.Forms.Panel panel_steady;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.TextBox textBox_b_level;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label_outer_value;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_steady_delay;
        private System.Windows.Forms.TextBox textBox_outer_value;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Button button_refresh;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label_increment;
        private System.Windows.Forms.Button button_settings;
        private System.Windows.Forms.Button button_measurement;
        private System.Windows.Forms.Button button_data;
        private System.Windows.Forms.Button button_plot;
        private System.Windows.Forms.TextBox textBox_filename;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Label label_pol;
    }
}