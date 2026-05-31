using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTL_Magnet2
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //user code
            string[] args = Environment.GetCommandLineArgs();
            bool debugMode = args.Contains("-d");
            MagnetController.Init(debugMode);
            ExperimentsDB.InitializeDatabase();

            //
            Application.Run(new NewMainForm());
        }
    }
}
