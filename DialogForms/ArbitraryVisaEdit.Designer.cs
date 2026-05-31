namespace KTL_Magnet2.DialogForms
{
    partial class ArbitraryVisaEdit
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBox_cycle = new System.Windows.Forms.RichTextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox_init = new System.Windows.Forms.RichTextBox();
            this.richTextBox_final = new System.Windows.Forms.RichTextBox();
            this.textBox_device = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 80);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(636, 353);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBox_cycle);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(628, 327);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "В цикле";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBox_init);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(596, 274);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "В начале";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBox_final);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(628, 327);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "В конце";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBox_cycle
            // 
            this.richTextBox_cycle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_cycle.Location = new System.Drawing.Point(3, 3);
            this.richTextBox_cycle.Name = "richTextBox_cycle";
            this.richTextBox_cycle.Size = new System.Drawing.Size(622, 321);
            this.richTextBox_cycle.TabIndex = 0;
            this.richTextBox_cycle.Text = "";
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_ok.Location = new System.Drawing.Point(16, 439);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_cancel.Location = new System.Drawing.Point(97, 439);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Отмена";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(12, 12);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(160, 20);
            this.textBox_name.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(178, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Имя переменной для сохранения";
            // 
            // richTextBox_init
            // 
            this.richTextBox_init.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_init.Location = new System.Drawing.Point(3, 3);
            this.richTextBox_init.Name = "richTextBox_init";
            this.richTextBox_init.Size = new System.Drawing.Size(590, 268);
            this.richTextBox_init.TabIndex = 0;
            this.richTextBox_init.Text = "";
            // 
            // richTextBox_final
            // 
            this.richTextBox_final.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_final.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_final.Name = "richTextBox_final";
            this.richTextBox_final.Size = new System.Drawing.Size(628, 327);
            this.richTextBox_final.TabIndex = 0;
            this.richTextBox_final.Text = "";
            // 
            // textBox_device
            // 
            this.textBox_device.Location = new System.Drawing.Point(12, 48);
            this.textBox_device.Name = "textBox_device";
            this.textBox_device.Size = new System.Drawing.Size(160, 20);
            this.textBox_device.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(412, 26);
            this.label2.TabIndex = 6;
            this.label2.Text = "Адрес устройства. Адрес можно узнать в NI-Visa Resource Manager.\r\nОставьте поле п" +
    "устым чтобы использовать первое доступное USB-устройство.";
            // 
            // ArbitraryVisaEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 476);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_device);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.tabControl1);
            this.Name = "ArbitraryVisaEdit";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox richTextBox_cycle;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox_init;
        private System.Windows.Forms.RichTextBox richTextBox_final;
        private System.Windows.Forms.TextBox textBox_device;
        private System.Windows.Forms.Label label2;
    }
}