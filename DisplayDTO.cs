using System;

namespace KTL_Magnet2
{

    public class DisplayUpdateArgs : EventArgs
    {
        public string ParamName {  get; set; }  
    }


    public class DisplayDTO
    {
        public double bSetpoint
        {
            get { return _bSetpoint; }
            set
            {
                _bSetpoint = value;
                DataChanged("bSetpoint");
            }
        }

        public double bReadout
        {
            get { return _bReadout; }
            set
            {
                _bReadout = value;
                DataChanged("bReadout");
            }
        }
        public double v1
        {
            get { return _v1; }
            set
            {
                if (value != _v1)
                {
                    _v1 = value;
                    DataChanged("v1");
                }
            }
        }

        public double v2
        {
            get { return _v2; }
            set
            {
                _v2 = value;
                DataChanged("v2");
            }
        }

        public int Sign
        {
            get { return _Sign; }
            set
            {
                _Sign = value;
                DataChanged("Sign");
            }
        }

        public FieldState fieldState
        {
            get { return _fieldState; }
            set
            {
                _fieldState = value;
                DataChanged("fieldState");
            }
        }

        public string displayString
        {
            get { return _displayString ?? String.Empty; }
            set
            {
                _displayString = value ?? String.Empty;
                DataChanged("displayString");
            }
        }





        private double _bSetpoint, _bReadout, _v1, _v2;
        private FieldState _fieldState;
        private int _Sign;
        private string _displayString;

        public EventHandler<DisplayUpdateArgs> dataChanged;

        private void DataChanged(string name)
        {
            DisplayUpdateArgs args = new DisplayUpdateArgs { ParamName = name };
            dataChanged?.Invoke(this, args);
        }

    }
}
