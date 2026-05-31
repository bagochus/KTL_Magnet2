namespace KTL_Magnet2.DialogForms
{
    partial class AdEditForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_delay = new System.Windows.Forms.TextBox();
            this.textBox_avg = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label_plc = new System.Windows.Forms.Label();
            this.comboBox_gain = new System.Windows.Forms.ComboBox();
            this.textBox_channel = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(152, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(185, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Имя переменной для отображения";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(22, 22);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(121, 20);
            this.textBox_name.TabIndex = 34;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(22, 251);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 32;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(112, 251);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 31;
            this.button_cancel.Text = "Отмена";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(149, 217);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "Задержка перед измерением";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(149, 181);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(209, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Усреднять результат по N измерениям";
            // 
            // textBox_delay
            // 
            this.textBox_delay.Location = new System.Drawing.Point(22, 210);
            this.textBox_delay.Name = "textBox_delay";
            this.textBox_delay.Size = new System.Drawing.Size(121, 20);
            this.textBox_delay.TabIndex = 28;
            // 
            // textBox_avg
            // 
            this.textBox_avg.Location = new System.Drawing.Point(22, 173);
            this.textBox_avg.Name = "textBox_avg";
            this.textBox_avg.Size = new System.Drawing.Size(121, 20);
            this.textBox_avg.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(149, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Канал";
            // 
            // label_plc
            // 
            this.label_plc.AutoSize = true;
            this.label_plc.Location = new System.Drawing.Point(149, 139);
            this.label_plc.Name = "label_plc";
            this.label_plc.Size = new System.Drawing.Size(87, 13);
            this.label_plc.TabIndex = 26;
            this.label_plc.Text = "Коэф. усиления";
            // 
            // comboBox_gain
            // 
            this.comboBox_gain.FormattingEnabled = true;
            this.comboBox_gain.Location = new System.Drawing.Point(22, 135);
            this.comboBox_gain.Name = "comboBox_gain";
            this.comboBox_gain.Size = new System.Drawing.Size(121, 21);
            this.comboBox_gain.TabIndex = 22;
            // 
            // textBox_channel
            // 
            this.textBox_channel.Location = new System.Drawing.Point(22, 99);
            this.textBox_channel.Name = "textBox_channel";
            this.textBox_channel.Size = new System.Drawing.Size(121, 20);
            this.textBox_channel.TabIndex = 36;
            // 
            // AdEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 309);
            this.Controls.Add(this.textBox_channel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_delay);
            this.Controls.Add(this.textBox_avg);
            this.Controls.Add(this.label_plc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_gain);
            this.Name = "AdEditForm";
            this.Text = "AdEditForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_delay;
        private System.Windows.Forms.TextBox textBox_avg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_plc;
        private System.Windows.Forms.ComboBox comboBox_gain;
        private System.Windows.Forms.TextBox textBox_channel;
    }
}