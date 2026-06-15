using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTL_Magnet2.DialogForms
{
    public partial class TextInput : Form
    {
        public string InputText { get; private set; }

        Func<string, bool> _validate = (x) => { return true; };

        public TextInput(Func<string,bool> validate,string defaultText = "")
        {
            InitializeComponent();
            DialogResult = DialogResult.No;
            _validate = validate;
            textBox_input.Text = defaultText;
        }

        private void HighlightControl(Control control)
        {
            Task.Run(() =>
            {
                control.Invoke(new Action(() => { control.BackColor = Color.MistyRose; }));
                Thread.Sleep(1000);
                control.Invoke(new Action(() => { control.BackColor = SystemColors.Window; }));
            });
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (_validate(textBox_input.Text))
            {
                InputText = textBox_input.Text;
                DialogResult = DialogResult.Yes;
                Close();
            }
            else HighlightControl(textBox_input);
        }

        private void textBox_input_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (_validate(textBox_input.Text))
                {
                    InputText = textBox_input.Text;
                    DialogResult = DialogResult.Yes;
                    Close();
                }
                else HighlightControl(textBox_input);
            }
        }



    }
}
