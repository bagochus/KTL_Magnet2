using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTL_Magnet2.BModes
{
    public class BModeSteady : IBMode
    {
        public double BLevel {  get; set; }
       
        public double ExternalVariable { get; set; }

        public string ExternalVarName;
        public int Delay { get; set; }
       
        public bool Continous { get; set; }
        




    }
}
