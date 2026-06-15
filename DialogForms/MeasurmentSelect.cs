using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KTL_Magnet2.Measurements;
using KTL_Magnet2.Measurments;

namespace KTL_Magnet2.DialogForms
{
    public partial class MeasurmentSelect : Form
    {
        public object result = null;
        List<Type> derivedTypes = null;


        public MeasurmentSelect()
        {
            InitializeComponent();

            derivedTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ExperimentStep)))
            .ToList();

            foreach (var type in derivedTypes)
            {
                string typeName;
                MethodInfo method = type.GetMethod("GetMeasurmentName");
                typeName = (string)method.Invoke(null, null);
                comboBox_type.Items.Add(typeName);
            }

            if (derivedTypes.Count > 0) 
                comboBox_type.SelectedIndex = derivedTypes.Count - 1;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (comboBox_type.SelectedIndex < 0) return;
            Type _type = derivedTypes[comboBox_type.SelectedIndex];
            result = Activator.CreateInstance(_type );
            Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            result = null;
            Close();
        }
    }
}
