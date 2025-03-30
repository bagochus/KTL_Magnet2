namespace KTL_Magnet2
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.SaveButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Sign = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.plotButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.экспериментыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьСтрокиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиПрограммыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьНаборИзмеренийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.графикToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(12, 28);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 0;
            this.SaveButton.Text = "Save File";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(632, 28);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 2;
            this.StartButton.Text = "START";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(713, 28);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(75, 23);
            this.StopButton.TabIndex = 3;
            this.StopButton.Text = "STOP";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.button4_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Sign,
            this.Column2,
            this.Column3});
            this.dataGridView1.Location = new System.Drawing.Point(12, 57);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(776, 468);
            this.dataGridView1.TabIndex = 4;
            // 
            // Sign
            // 
            this.Sign.HeaderText = "Sign";
            this.Sign.Name = "Sign";
            this.Sign.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "V1 (Volts)";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "V2 (Amps)";
            this.Column3.Name = "Column3";
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 361);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel1.Text = "Hello there!";
            // 
            // plotButton
            // 
            this.plotButton.Location = new System.Drawing.Point(174, 28);
            this.plotButton.Name = "plotButton";
            this.plotButton.Size = new System.Drawing.Size(75, 23);
            this.plotButton.TabIndex = 6;
            this.plotButton.Text = "График";
            this.plotButton.UseVisualStyleBackColor = true;
            this.plotButton.Click += new System.EventHandler(this.plotButton_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(255, 28);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(113, 23);
            this.addButton.TabIndex = 7;
            this.addButton.Text = "Добавить строки";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.экспериментыToolStripMenuItem,
            this.настройкиToolStripMenuItem,
            this.графикToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // экспериментыToolStripMenuItem
            // 
            this.экспериментыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьСтрокиToolStripMenuItem,
            this.загрузитьToolStripMenuItem,
            this.сохранитьToolStripMenuItem});
            this.экспериментыToolStripMenuItem.Name = "экспериментыToolStripMenuItem";
            this.экспериментыToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.экспериментыToolStripMenuItem.Text = "Эксперименты";
            // 
            // добавитьСтрокиToolStripMenuItem
            // 
            this.добавитьСтрокиToolStripMenuItem.Name = "добавитьСтрокиToolStripMenuItem";
            this.добавитьСтрокиToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.добавитьСтрокиToolStripMenuItem.Text = "Добавить строки";
            this.добавитьСтрокиToolStripMenuItem.Click += new System.EventHandler(this.добавитьСтрокиToolStripMenuItem_Click);
            // 
            // загрузитьToolStripMenuItem
            // 
            this.загрузитьToolStripMenuItem.Name = "загрузитьToolStripMenuItem";
            this.загрузитьToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.загрузитьToolStripMenuItem.Text = "Загрузить";
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настройкиПрограммыToolStripMenuItem,
            this.загрузитьНаборИзмеренийToolStripMenuItem});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // настройкиПрограммыToolStripMenuItem
            // 
            this.настройкиПрограммыToolStripMenuItem.Name = "настройкиПрограммыToolStripMenuItem";
            this.настройкиПрограммыToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.настройкиПрограммыToolStripMenuItem.Text = "Настройки программы";
            this.настройкиПрограммыToolStripMenuItem.Click += new System.EventHandler(this.настройкиПрограммыToolStripMenuItem_Click);
            // 
            // загрузитьНаборИзмеренийToolStripMenuItem
            // 
            this.загрузитьНаборИзмеренийToolStripMenuItem.Name = "загрузитьНаборИзмеренийToolStripMenuItem";
            this.загрузитьНаборИзмеренийToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.загрузитьНаборИзмеренийToolStripMenuItem.Text = "Загрузить набор измерений";
            // 
            // графикToolStripMenuItem
            // 
            this.графикToolStripMenuItem.Name = "графикToolStripMenuItem";
            this.графикToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.графикToolStripMenuItem.Text = "График";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 383);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.plotButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.SaveButton);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sign;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button plotButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem экспериментыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьСтрокиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиПрограммыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem графикToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьНаборИзмеренийToolStripMenuItem;
    }
}

