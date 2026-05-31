namespace KTL_Magnet2
{
    partial class PlotForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_xname = new System.Windows.Forms.ComboBox();
            this.comboBox_yname = new System.Windows.Forms.ComboBox();
            this.formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            this.button_Plot = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Y";
            // 
            // comboBox_xname
            // 
            this.comboBox_xname.FormattingEnabled = true;
            this.comboBox_xname.Location = new System.Drawing.Point(44, 6);
            this.comboBox_xname.Name = "comboBox_xname";
            this.comboBox_xname.Size = new System.Drawing.Size(121, 21);
            this.comboBox_xname.TabIndex = 2;
            // 
            // comboBox_yname
            // 
            this.comboBox_yname.FormattingEnabled = true;
            this.comboBox_yname.Location = new System.Drawing.Point(44, 33);
            this.comboBox_yname.Name = "comboBox_yname";
            this.comboBox_yname.Size = new System.Drawing.Size(121, 21);
            this.comboBox_yname.TabIndex = 3;
            // 
            // formsPlot1
            // 
            this.formsPlot1.AutoSize = true;
            this.formsPlot1.DisplayScale = 0F;
            this.formsPlot1.Location = new System.Drawing.Point(15, 73);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(750, 480);
            this.formsPlot1.TabIndex = 4;
            // 
            // button_Plot
            // 
            this.button_Plot.Location = new System.Drawing.Point(188, 4);
            this.button_Plot.Name = "button_Plot";
            this.button_Plot.Size = new System.Drawing.Size(90, 50);
            this.button_Plot.TabIndex = 5;
            this.button_Plot.Text = "Построить";
            this.button_Plot.UseVisualStyleBackColor = true;
            this.button_Plot.Click += new System.EventHandler(this.button1_Click);
            // 
            // PlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.button_Plot);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.comboBox_yname);
            this.Controls.Add(this.comboBox_xname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "PlotForm";
            this.Text = "График";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PlotForm_FormClosed);
            this.Resize += new System.EventHandler(this.PlotForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_xname;
        private System.Windows.Forms.ComboBox comboBox_yname;
        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private System.Windows.Forms.Button button_Plot;
    }
}