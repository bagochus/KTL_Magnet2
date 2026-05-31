using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTL_Magnet2
{
    public interface IADController
    {
        int AvaiableChannels { get; }
        double[] AvaiableGains { get; }
        void Init();
        void Dispose();
        void SetVoltage(double voltage, int chanell);
        void SetDigitalOutput(byte value);
        double GetVoltage(int chanell, int gain);
    }
}
