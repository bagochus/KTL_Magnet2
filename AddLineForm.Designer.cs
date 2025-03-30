namespace KTL_Magnet2
{
    partial class AddLineForm
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
            this.v1startBox = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.v1stepBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.v2stepBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.v2startBox = new System.Windows.Forms.TextBox();
            this.stepsBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.signBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // v1startBox
            // 
            this.v1startBox.Location = new System.Drawing.Point(100, 37);
            this.v1startBox.Name = "v1startBox";
            this.v1startBox.Size = new System.Drawing.Size(100, 20);
            this.v1startBox.TabIndex = 0;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(250, 120);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Добавить";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "V1 (Volt)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(97, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Начальное значение";
            // 
            // v1stepBox
            // 
            this.v1stepBox.Location = new System.Drawing.Point(225, 40);
            this.v1stepBox.Name = "v1stepBox";
            this.v1stepBox.Size = new System.Drawing.Size(100, 20);
            this.v1stepBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Шаг";
            // 
            // v2stepBox
            // 
            this.v2stepBox.Location = new System.Drawing.Point(225, 76);
            this.v2stepBox.Name = "v2stepBox";
            this.v2stepBox.Size = new System.Drawing.Size(100, 20);
            this.v2stepBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "V2 (Amp)";
            // 
            // v2startBox
            // 
            this.v2startBox.Location = new System.Drawing.Point(100, 73);
            this.v2startBox.Name = "v2startBox";
            this.v2startBox.Size = new System.Drawing.Size(100, 20);
            this.v2startBox.TabIndex = 6;
            // 
            // stepsBox
            // 
            this.stepsBox.Location = new System.Drawing.Point(100, 118);
            this.stepsBox.Name = "stepsBox";
            this.stepsBox.Size = new System.Drawing.Size(100, 20);
            this.stepsBox.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Шагов";
            // 
            // signBox
            // 
            this.signBox.FormattingEnabled = true;
            this.signBox.Items.AddRange(new object[] {
            "+",
            "-"});
            this.signBox.Location = new System.Drawing.Point(346, 40);
            this.signBox.Name = "signBox";
            this.signBox.Size = new System.Drawing.Size(121, 21);
            this.signBox.TabIndex = 11;
            // 
            // AddLineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 226);
            this.Controls.Add(this.signBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.stepsBox);
            this.Controls.Add(this.v2stepBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.v2startBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.v1stepBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.v1startBox);
            this.Name = "AddLineForm";
            this.Text = "Добавить эксперименты";
            this.Load += new System.EventHandler(this.Form4_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox v1startBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox v1stepBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox v2stepBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox v2startBox;
        private System.Windows.Forms.TextBox stepsBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox signBox;
    }
}