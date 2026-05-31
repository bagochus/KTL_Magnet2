using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KTL_Magnet2.DialogForms;
using NCalc;

namespace KTL_Magnet2.Measurments
{
    public delegate Dictionary<string,double> GetVariables();


    public class Calculation : ExperimentStep
    {
        public string Formula { get; set; }

        public override string Description
        {
            get => "Вычисление "+Name+"="+Formula;
        }

        private Expression expression;

        public GetVariables getVariables;

        double result;

        public override bool EditForm(string[] exisitngNames)
        {
            CalculateEditForm form = new CalculateEditForm(exisitngNames, this);
            form.ShowDialog();
            if (form.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                Formula = form.Formula;
                Name = form._Name;
                return true;
            }
            return false;

        }

        public static new string GetMeasurmentName()
        {
            return "Вычисление переменной по формуле";
        }

        public override void Execute()
        {
            try
            {
                foreach (var v in getVariables())
                {
                    expression.Parameters[v.Key] = v.Value;
                }

                result = (double)expression.Evaluate();

            }
            catch { result = Double.NaN; }
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
            expression = new Expression(Formula);
        }
    }
}
