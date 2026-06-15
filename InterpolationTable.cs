using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KTL_Magnet2
{
    internal class InterpolationTable
    {
        private double[] x;
        private double[] y;
        private bool sorted = false;
        bool ready = false;
        double eps = 1e-9;


        public bool ParseFile(string filename)
        {
            char[] delimiters = { ' ', '\t' };
            IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
            List<double> x_readed = new List<double>();
            List<double> y_readed = new List<double>();
            string text;

            using (StreamReader reader = new StreamReader(filename))
            {

                while ((text = reader.ReadLine()) != null) 
                { 
                    double temp_x;
                    double temp_y;

                    bool emptyline = (text.Length == 0);
                    bool commentLine = false;
                    if (!emptyline) { commentLine = text.Contains("*"); }
                    if (!commentLine && !emptyline)
                    {
                        string[] splitted = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        bool validValue = (splitted.Length > 1);
                        validValue &= Double.TryParse(splitted[0], NumberStyles.Any, formatter, out temp_x);
                        validValue &= Double.TryParse(splitted[1], NumberStyles.Any, formatter, out temp_y);
                        if (validValue) { x_readed.Add(temp_x); y_readed.Add(temp_y); }
                    }
                }
            }
            if (x_readed.Count() != 0)
            {
                x = x_readed.ToArray();
                y = y_readed.ToArray();
                ready = true;
                return true;
            }
            else return false;
        }

        private void Sort()
        {
            bool b_sorted = false;
            while (!b_sorted)
            {
                b_sorted = true;
                for (int i = 0; i < (x.Length - 1); i++)
                {
                    if (x[i] > x[i+1])
                    {
                        b_sorted = false;
                        SwapElements(i, i + 1);
                    }
                }
            }
            sorted = true;
        }

        private void SwapElements(int n1, int n2)
        {
            double tempx = x[n1];
            double tempy = y[n1];
            x[n1] = x[n2];
            y[n1] = y[n2];
            x[n2] = tempx;
            y[n2] = tempy;
        }

        private double GetGradient(int n)
        {
            return (y[n + 1] - y[n]) / (x[n + 1] - x[n]);
        }


        public double GetY(double xinput)
        {
            if (!ready) throw new Exception("Calibration file not loaded");
            if (!sorted) Sort();
            bool x_low = (Less(xinput, x[0]));
            bool x_high = (GreaterOrEqual(xinput, x[x.Length-1]));
            if (x_low) return y[0] + GetGradient(0) * (xinput - x[0]);
            if (x_high) return y[y.Length-1] + GetGradient(y.Length-2)* (xinput - x[x.Length - 1]);
            for (int i = 0; i < y.Length-1; i++) 
            {
                if (GreaterOrEqual(xinput, x[i]) && Less(xinput, x[i+1]))
                {
                    return y[i] + GetGradient(i)*(xinput - x[i]);
                }
            }
            return 0;
        }

        private bool GreaterOrEqual(double a, double b)
        {
            if (Equal(a, b)) return true;
            else if (a > b) return true;
            else return false;
        }

        private bool Equal(double a, double b)
        {
            return Math.Abs((a-b)) < this.eps;
        }

        private bool Less (double a, double b)
        {
            if (Equal(a, b)) return false;
            else if (a > b) return false;
            else return true;
        }

    }
}
