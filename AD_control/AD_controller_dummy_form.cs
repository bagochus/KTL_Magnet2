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
        private Label labelC;

        public double X { get; private set; }
        public double Y { get; private set; }

        public AD_controller_dummy_form()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.labelA = new Label();
            this.labelB = new Label();
            this.labelC = new Label();

            // Настройка формы
            this.Text = "Value Display";
            this.ClientSize = new System.Drawing.Size(300, 150);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Настройка labelA
            this.labelA.Text = "A = 0";
            this.labelA.Location = new System.Drawing.Point(50, 50);
            this.labelA.AutoSize = true;
            this.labelA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);

            // Настройка labelB
            this.labelB.Text = "B = 0";
            this.labelB.Location = new System.Drawing.Point(50, 80);
            this.labelB.AutoSize = true;
            this.labelB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);

            // Настройка labelB
            this.labelC.Text = "C = 0";
            this.labelC.Location = new System.Drawing.Point(50, 110);
            this.labelC.AutoSize = true;
            this.labelC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);

            // Добавление элементов на форму
            this.Controls.Add(this.labelA);
            this.Controls.Add(this.labelB);
            this.Controls.Add(this.labelC);
        }

        public void SetVoltage(double value, int channel)
        {
            if (channel == 0)
            {
                X = value;
                labelA.Text = $"A = {X:F2}";
            }
            else if (channel == 1)
            {
                Y = value;
                labelB.Text = $"B = {Y:F2}";
            }
        }

        public void SetDigitalOutput(byte value)
        {
            labelC.Text = "C = " + value.ToString();
        }

        public double GetVoltage(int chanell, int gain)
        {
            Random random = new Random();
            return Math.Round(random.NextDouble() * 10, 2);
        }
    }
}
