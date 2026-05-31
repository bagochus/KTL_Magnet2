namespace KTL_Magnet2.DialogForms
{
    partial class ProfileSelectForm
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
            this.listBox_profiles = new System.Windows.Forms.ListBox();
            this.button_action = new System.Windows.Forms.Button();
            this.checkBox_autosave = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listBox_profiles
            // 
            this.listBox_profiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_profiles.FormattingEnabled = true;
            this.listBox_profiles.Location = new System.Drawing.Point(12, 12);
            this.listBox_profiles.Name = "listBox_profiles";
            this.listBox_profiles.Size = new System.Drawing.Size(384, 277);
            this.listBox_profiles.TabIndex = 0;
            this.listBox_profiles.DoubleClick += new System.EventHandler(this.listBox_profiles_DoubleClick);
            // 
            // button_action
            // 
            this.button_action.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_action.Location = new System.Drawing.Point(12, 301);
            this.button_action.Name = "button_action";
            this.button_action.Size = new System.Drawing.Size(384, 23);
            this.button_action.TabIndex = 1;
            this.button_action.Text = "OK";
            this.button_action.UseVisualStyleBackColor = true;
            this.button_action.Click += new System.EventHandler(this.button_action_Click);
            // 
            // checkBox_autosave
            // 
            this.checkBox_autosave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_autosave.AutoSize = true;
            this.checkBox_autosave.Location = new System.Drawing.Point(12, 339);
            this.checkBox_autosave.Name = "checkBox_autosave";
            this.checkBox_autosave.Size = new System.Drawing.Size(174, 17);
            this.checkBox_autosave.TabIndex = 2;
            this.checkBox_autosave.Text = "Показывать автосохранения";
            this.checkBox_autosave.UseVisualStyleBackColor = true;
            this.checkBox_autosave.CheckedChanged += new System.EventHandler(this.checkBox_autosave_CheckedChanged);
            // 
            // ProfileSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 368);
            this.Controls.Add(this.checkBox_autosave);
            this.Controls.Add(this.button_action);
            this.Controls.Add(this.listBox_profiles);
            this.Name = "ProfileSelectForm";
            this.Text = "ProfileSelectForm";
            this.Load += new System.EventHandler(this.ProfileSelectForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_profiles;
        private System.Windows.Forms.Button button_action;
        private System.Windows.Forms.CheckBox checkBox_autosave;
    }
}