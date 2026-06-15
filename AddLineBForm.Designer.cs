namespace KTL_Magnet2
{
    partial class AddLineBForm
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
            this.button_add = new System.Windows.Forms.Button();
            this.button_calcel = new System.Windows.Forms.Button();
            this.textBox_bstart = new System.Windows.Forms.TextBox();
            this.textBox_nsteps = new System.Windows.Forms.TextBox();
            this.textBox_bstep = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_add
            // 
            this.button_add.Location = new System.Drawing.Point(284, 39);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(75, 23);
            this.button_add.TabIndex = 0;
            this.button_add.Text = "Добавить";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // button_calcel
            // 
            this.button_calcel.Location = new System.Drawing.Point(284, 68);
            this.button_calcel.Name = "button_calcel";
            this.button_calcel.Size = new System.Drawing.Size(75, 23);
            this.button_calcel.TabIndex = 1;
            this.button_calcel.Text = "Отмена";
            this.button_calcel.UseVisualStyleBackColor = true;
            this.button_calcel.Click += new System.EventHandler(this.button_calcel_Click);
            // 
            // textBox_bstart
            // 
            this.textBox_bstart.Location = new System.Drawing.Point(46, 42);
            this.textBox_bstart.Name = "textBox_bstart";
            this.textBox_bstart.Size = new System.Drawing.Size(100, 20);
            this.textBox_bstart.TabIndex = 2;
            // 
            // textBox_nsteps
            // 
            this.textBox_nsteps.Location = new System.Drawing.Point(46, 84);
            this.textBox_nsteps.Name = "textBox_nsteps";
            this.textBox_nsteps.Size = new System.Drawing.Size(100, 20);
            this.textBox_nsteps.TabIndex = 3;
            // 
            // textBox_bstep
            // 
            this.textBox_bstep.Location = new System.Drawing.Point(165, 42);
            this.textBox_bstep.Name = "textBox_bstep";
            this.textBox_bstep.Size = new System.Drawing.Size(100, 20);
            this.textBox_bstep.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "B start";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(162, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "B_step";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Шагов";
            // 
            // AddLineBForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 154);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_bstep);
            this.Controls.Add(this.textBox_nsteps);
            this.Controls.Add(this.textBox_bstart);
            this.Controls.Add(this.button_calcel);
            this.Controls.Add(this.button_add);
            this.Name = "AddLineBForm";
            this.Text = "Добавсить эксперименты";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Button button_calcel;
        private System.Windows.Forms.TextBox textBox_bstart;
        private System.Windows.Forms.TextBox textBox_nsteps;
        private System.Windows.Forms.TextBox textBox_bstep;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}