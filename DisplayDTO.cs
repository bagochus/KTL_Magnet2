using System;

namespace KTL_Magnet2
{
    public class DisplayDTO
    {
        public double bSetpoint
        {
            get { return _bSetpoint; }
            set {
                if (value != _bSetpoint) 
                {
                    dataChanged?.Invoke(this, EventArgs.Empty);
                }
                _bSetpoint = value; 

            }
        }

        public double bReadout
        {
            get { return _bReadout; }
            set
            {
                if (value != _bReadout)
                {
                    dataChanged?.Invoke(this, EventArgs.Empty);
                }
                _bReadout = value;

            }
        }
        public double v1
        {
            get { return _v1; }
            set
            {
                if (value != _v1)
                {
                    dataChanged?.Invoke(this, EventArgs.Empty);
                }
                _v1 = value;

            }
        }

        public double v2
        {
            get { return _v2; }
            set
            {
                if (value != _v2)
                {
                    dataChanged?.Invoke(this, EventArgs.Empty);
                }
                _v2 = value;

            }
        }

        public int Sign
        {
            get { return _Sign; }
            set
            {
                if (value != _Sign)
                {
                    dataChanged?.Invoke(this, EventArgs.Empty);
                }
                _Sign = value;

            }
        }

        public FieldState fieldState
        {
            get { return _fieldState; }
            set
            {
                if (value != _fieldState)
                {
                    dataChanged?.Invoke(this, EventArgs.Empty);
                }
                _fieldState = value;

            }
        }

        public string displayString
        {
            get { return _displayString ?? String.Empty; }
            set
            {
                if (value != _displayString)
                {
                    dataChanged?.Invoke(this, EventArgs.Empty);
                }
                _displayString = value ?? String.Empty;

            }
        }


        private double _bSetpoint, _bReadout, _v1, _v2;
        private FieldState _fieldState;
        private int _Sign;
        private string _displayString;

        public EventHandler dataChanged;
    
    }
}
