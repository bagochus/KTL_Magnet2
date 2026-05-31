using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTL_Magnet2.Measurements
{
    public class VISA_Measurement
    {
        public String DeviceName { get; set; }
        public MeasurementType Type { get; set; }
        public double PLC_time { get; set; }
        public double Limit { get; set; }
        public int Channel { get; set; }
        public int Delay { get; set; }

        public VISA_Measurement()
        {
            PLC_time = -1;
            Limit = -1;
            Channel = -1;
            Delay = -1;

        }


    }
}
