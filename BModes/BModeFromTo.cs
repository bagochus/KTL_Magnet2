using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTL_Magnet2.BModes
{
    public interface IBMode { }
    public class BModeFromTo : IBMode
    {
        public double BFrom;
        public double BTo;
        public double BStep;
        public int Delay;
        /// <summary>
        /// Режим туда-обратно: после перехода от BFrom к BTo
        /// </summary>
        public bool Reverse;
        /// Регистрируем в выходных данных шаги от нуля до стартового значения
        /// и от конечного значения до нуля с шагом BStep
        /// </summary>
        public bool PathToZero;



    }
}
