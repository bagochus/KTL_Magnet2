using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTL_Magnet2
{
    public class InputLine
    {
        public double B_Setpoint {  get; set; }
        public double B_Readout;
        public double V1 {  get; set; }
        public double V2 { get; set; }
        public int Sign {
            get { return p_sign;  }
            set { SetSign(value); } 
        }
        private int p_sign = 1;
        private void SetSign(int sign)
        {
            if (sign < 0) p_sign = -1;
            else p_sign = 1;
        }
    }




}
