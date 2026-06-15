namespace KTL_Magnet2.DialogForms
{
    partial class VisaVoltageEditForm
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
            this.comboBox_device = new System.Windows.Forms.ComboBox();
            this.comboBox_limit = new System.Windows.Forms.ComboBox();
            this.comboBox_chanel = new System.Windows.Forms.ComboBox();
            this.comboBox_plc = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_plc = new System.Windows.Forms.Label();
            this.textBox_avg = new System.Windows.Forms.TextBox();
            this.textBox_delay = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.label_device_status = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox_device
            // 
            this.comboBox_device.FormattingEnabled = true;
            this.comboBox_device.Location = new System.Drawing.Point(29, 28);
            this.comboBox_device.Name = "comboBox_device";
            this.comboBox_device.Size = new System.Drawing.Size(121, 21);
            this.comboBox_device.TabIndex = 0;
            // 
            // comboBox_limit
            // 
            this.comboBox_limit.FormattingEnabled = true;
            this.comboBox_limit.Location = new System.Drawing.Point(29, 141);
            this.comboBox_limit.Name = "comboBox_limit";
            this.comboBox_limit.Size = new System.Drawing.Size(121, 21);
            this.comboBox_limit.TabIndex = 1;
            // 
            // comboBox_chanel
            // 
            this.comboBox_chanel.FormattingEnabled = true;
            this.comboBox_chanel.Location = new System.Drawing.Point(29, 179);
            this.comboBox_chanel.Name = "comboBox_chanel";
            this.comboBox_chanel.Size = new System.Drawing.Size(121, 21);
            this.comboBox_chanel.TabIndex = 2;
            // 
            // comboBox_plc
            // 
            this.comboBox_plc.FormattingEnabled = true;
            this.comboBox_plc.Location = new System.Drawing.Point(29, 217);
            this.comboBox_plc.Name = "comboBox_plc";
            this.comboBox_plc.Size = new System.Drawing.Size(121, 21);
            this.comboBox_plc.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(156, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Устройство";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(156, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Предел измерения,В";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(156, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Канал";
            // 
            // label_plc
            // 
            this.label_plc.AutoSize = true;
            this.label_plc.Location = new System.Drawing.Point(156, 221);
            this.label_plc.Name = "label_plc";
            this.label_plc.Size = new System.Drawing.Size(286, 13);
            this.label_plc.TabIndex = 9;
            this.label_plc.Text = "Время интегрирования измерения, PLC (1 PLC = 20мс)";
            // 
            // textBox_avg
            // 
            this.textBox_avg.Location = new System.Drawing.Point(29, 255);
            this.textBox_avg.Name = "textBox_avg";
            this.textBox_avg.Size = new System.Drawing.Size(121, 20);
            this.textBox_avg.TabIndex = 10;
            // 
            // textBox_delay
            // 
            this.textBox_delay.Location = new System.Drawing.Point(29, 292);
            this.textBox_delay.Name = "textBox_delay";
            this.textBox_delay.Size = new System.Drawing.Size(121, 20);
            this.textBox_delay.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(156, 263);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(209, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Усреднять результат по N измерениям";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(156, 299);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Задержка перед измерением";
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(119, 333);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 14;
            this.button_cancel.Text = "Отмена";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(29, 333);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 15;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // label_device_status
            // 
            this.label_device_status.AutoSize = true;
            this.label_device_status.Location = new System.Drawing.Point(26, 64);
            this.label_device_status.Name = "label_device_status";
            this.label_device_status.Size = new System.Drawing.Size(0, 13);
            this.label_device_status.TabIndex = 16;
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(29, 104);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(121, 20);
            this.textBox_name.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(159, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(185, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Имя переменной для отображения";
            // 
            // VisaVoltageEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 380);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label_device_status);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_delay);
            this.Controls.Add(this.textBox_avg);
            this.Controls.Add(this.label_plc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_plc);
            this.Controls.Add(this.comboBox_chanel);
            this.Controls.Add(this.comboBox_limit);
            this.Controls.Add(this.comboBox_device);
            this.Name = "VisaVoltageEditForm";
            this.Text = "VisaVoltageEditForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_device;
        private System.Windows.Forms.ComboBox comboBox_limit;
        private System.Windows.Forms.ComboBox comboBox_chanel;
        private System.Windows.Forms.ComboBox comboBox_plc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_plc;
        private System.Windows.Forms.TextBox textBox_avg;
        private System.Windows.Forms.TextBox textBox_delay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Label label_device_status;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label4;
    }
}