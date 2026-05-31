using KTL_Magnet2.DialogForms;
using KTL_Magnet2.Measurments;
using ScottPlot.PlotStyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTL_Magnet2.Measurements
{
    public class AD_Measurement : ExperimentStep
    {
        public int ch_num { get; set; } = 0;
        public int Avg { get; set; } = 1;
        public int Delay { get; set; } = 0;
        public int Gain { get; set; } = 1;

        public override string Description
        {
            get => GetDescription();
        }

        IADController aDcontroller;

        private double result = 0;

        public override bool EditForm(string[] exisitngNames)
        {
            AdEditForm form = new AdEditForm(exisitngNames, this);
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
            {
                Name = form._Name;
                Gain = form.Gain;
                ch_num = form.ch_num;
                Delay = form.Delay;
                Avg = form.Avg;
                return true;
            }
            else return false;
        }

        public static new string GetMeasurmentName()
        {
            return "Напряжение DC (Блок АЦП/ЦАП)";
        }

        private string GetDescription()
        {
            return $"Напр. DC (блок АЦП). CH{ch_num}, Gain = {Gain}";
        }

        public override void Execute()
        {

            Thread.Sleep(Delay);
            double tempsum = 0;
            for (int k = 0; k < Avg; k++)
            {
                tempsum += aDcontroller.GetVoltage(ch_num, Gain);
            }
            result = tempsum / Avg;

        }

        public override void Finish()
        {

        }

        public override double GetValue()
        {
            return result;
        }

        public override void Init()
        {
            if (Avg < 1) Avg = 1;
            if (ch_num > MagnetController.MaxChanell)
                throw new Exception($"Невозможно использовать канал {ch_num}");
            aDcontroller = MagnetController.GetController();
        }
    }
}
