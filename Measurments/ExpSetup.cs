using KTL_Magnet2.Measurements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTL_Magnet2
{

    public enum ReadoutSourceType {AD, Visa, None}

    
    public class ExpSetup
    {
        public bool UseSetpointCalibrationTables { get ; set; }
        public bool UseReadoutCalibrationTables { get; set; }
        public bool ShowConvertedV {  get; set; }
        public ReadoutSourceType readoutSourceType { get; set; }
        public int readoutSourceId { get; set; }
        public double MaxBStep { get; set; }
        public double MaxV1Step { get; set; }
        public double MaxV2Step { get; set; }
        public double MaxBSlewrate { get; set; }
        public double MaxV1SlewRate {  get; set; }
        public double MaxV2SlewRate { get; set; }

        public int ZeroCrossingDelay { get; set; }
        public bool UseSmoothZeroCrossing {  get; set; }
        public double SmoothStep { get; set; }
        public int SmoothDelay { get; set; }

        public List<AD_Measurement> ad_Measurements { get; set; }
        public List<VISA_Measurement> visa_Measurements { get; set;}

        public String v1_filename { get; set; }    
        public String v2_filename { get; set; }
        public String Readout_filename { get; set; }

        public double V1_max {  get; set; }
        public double V2_max { get; set; }



        public ExpSetup() 
        {
            ad_Measurements = new List<AD_Measurement>();
            visa_Measurements = new List<VISA_Measurement>();
            readoutSourceType = ReadoutSourceType.None;
            Readout_filename = "";
            v2_filename = "";
            v1_filename = "";
            V1_max = 10;
            V2_max = 10;

            UseReadoutCalibrationTables = false;
            UseSetpointCalibrationTables = false;
            ShowConvertedV = false;

            MaxV1Step = 10;
            MaxV2Step = 10;
            MaxV1SlewRate = 10;
            MaxV2SlewRate = 10;
            MaxBStep = 10;
            MaxBSlewrate = 10;
            ZeroCrossingDelay = 0;

            
            ZeroCrossingDelay = 100;
            UseSmoothZeroCrossing = false;
            SmoothStep = 0.1;
            SmoothDelay = 100;


        }


    }
}
