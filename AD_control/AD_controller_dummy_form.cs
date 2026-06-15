using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace KTL_Magnet2
{
    public class AD_controller_dummy_form : Form
    {
        private Label labelA;
        private Label labelB;
        private Label label_bb;
        private Label label_vi;
        private Label labelC;

        public double X { get; private set; }
        public double Y { get; private set; }

        public AD_controller_dummy_form()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.labelA = new System.Windows.Forms.Label();
            this.labelB = new System.Windows.Forms.Label();
            this.labelC = new System.Windows.Forms.Label();
            this.label_bb = new System.Windows.Forms.Label();
            this.label_vi = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelA
            // 
            this.labelA.AutoSize = true;
            this.labelA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.labelA.Location = new System.Drawing.Point(12, 18);
            this.labelA.Name = "labelA";
            this.labelA.Size = new System.Drawing.Size(55, 20);
            this.labelA.TabIndex = 0;
            this.labelA.Text = "V1 = 0";
            // 
            // labelB
            // 
            this.labelB.AutoSize = true;
            this.labelB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.labelB.Location = new System.Drawing.Point(12, 48);
            this.labelB.Name = "labelB";
            this.labelB.Size = new System.Drawing.Size(55, 20);
            this.labelB.TabIndex = 1;
            this.labelB.Text = "V2 = 0";
            // 
            // labelC
            // 
            this.labelC.AutoSize = true;
            this.labelC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.labelC.Location = new System.Drawing.Point(12, 78);
            this.labelC.Name = "labelC";
            this.labelC.Size = new System.Drawing.Size(47, 20);
            this.labelC.TabIndex = 2;
            this.labelC.Text = "D = 0";
            // 
            // label_bb
            // 
            this.label_bb.AutoSize = true;
            this.label_bb.Location = new System.Drawing.Point(121, 18);
            this.label_bb.Name = "label_bb";
            this.label_bb.Size = new System.Drawing.Size(23, 13);
            this.label_bb.TabIndex = 3;
            this.label_bb.Text = "B= ";
            // 
            // label_vi
            // 
            this.label_vi.AutoSize = true;
            this.label_vi.Location = new System.Drawing.Point(12, 108);
            this.label_vi.Name = "label_vi";
            this.label_vi.Size = new System.Drawing.Size(27, 13);
            this.label_vi.TabIndex = 4;
            this.label_vi.Text = "vi = ";
            // 
            // AD_controller_dummy_form
            // 
            this.ClientSize = new System.Drawing.Size(300, 150);
            this.Controls.Add(this.label_vi);
            this.Controls.Add(this.label_bb);
            this.Controls.Add(this.labelA);
            this.Controls.Add(this.labelB);
            this.Controls.Add(this.labelC);
            this.Name = "AD_controller_dummy_form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Value Display";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void SetVoltage(double value, int channel)
        {
            if (channel == 0)
            {
                X = value;
                labelA.Text = $"V1 = {X:F2}";
            }
            else if (channel == 1)
            {
                Y = value;
                labelB.Text = $"V2 = {Y:F2}";
            }
        }

        public void SetDigitalOutput(byte value)
        {
            labelC.Text = "D = " + value.ToString();
        }

        public void SetB(string text)
        {
            label_bb.Text = text;
        }

        public void SetVi(string text)
        {
            label_vi.Text = text;
        }


        public double GetVoltage(int chanell, int gain)
        {
            Random random = new Random();
            return Math.Round(random.NextDouble() * 10, 2);
        }
    }
}
