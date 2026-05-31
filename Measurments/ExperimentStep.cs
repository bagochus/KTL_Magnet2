using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTL_Magnet2.Measurments
{
    public abstract class ExperimentStep
    {

        public static string GetMeasurmentName() { return string.Empty;}
        public virtual string Name {  get; set; }
        public virtual string Description { get; } = "";
        public abstract void Execute();

        public abstract void Init();

        public abstract void Finish();

        public abstract bool EditForm(string[] exisitngNames);

        public abstract double GetValue();


        public static bool IsValidVariableNameManual(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            // Первый символ: буква или подчеркивание
            char first = name[0];
            if (!char.IsLetter(first) && first != '_')
                return false;

            // Остальные символы: буквы, цифры или подчеркивание
            for (int i = 1; i < name.Length; i++)
            {
                char c = name[i];
                if (!char.IsLetterOrDigit(c) && c != '_')
                    return false;
            }

            return true;
        }


    }
}
