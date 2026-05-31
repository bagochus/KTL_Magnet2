using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using KTL_Magnet2.Measurments;
using KTL_Magnet2.BModes;
using NCalc;
using System.Data;

namespace KTL_Magnet2
{
    public class MagnetController
    {
        const double eps = 1e-9;

        private List<ExperimentStep> _experiments = new List<ExperimentStep>();

        public static List<ExperimentStep> Experiments 
        { 
            get { return _instance._experiments; }
            set 
            {
                if (!running)
                _instance._experiments = value;
            }
        }

        private static bool running = false;

        public static int lastUsedProfileId = -1;
        public static bool experimentalPlanChanged = false;


        private MagnetController() { }
        private static MagnetController _instance;

        private string v1_table_plus, v2_table_plus, v1_table_minus, v2_table_minus, table_readout;

        private InterpolationTable v1p = new InterpolationTable();
        private InterpolationTable v1m = new InterpolationTable();
        private InterpolationTable v2p = new InterpolationTable();
        private InterpolationTable v2m = new InterpolationTable();
        private InterpolationTable ro = new InterpolationTable();

        private double v1_max_step, v2_max_step, b_max_step, v1_slewrate, v2_slewrate, b_slewrate;

        private bool bMismatchStop;

        private double maxBMismatch;

        private double B_current, V1_current, V2_current;
        private double B_readout;


        public static readonly int MaxChanell = 5;

        private string ReadoutFormula;

        private string readoutGains = String.Empty;
        private string ReadoutGains 
        {
            get { return readoutGains; } 
            set { readoutGains = value; Gains = ParseGXY(readoutGains); } 
        }

        private List<(int x, int y)> Gains;

        private static CancellationTokenSource cts;

        private IADController adController;


        private Action BGood;
        private Action BBad;
        private Action BTransitioning;

        public static Action disableStartButton = () => { };
        public static Action disableStopButton = () => { };
        public static Action enableStartButton = () => { };
        public static Action enableStopButton = () => { };

        public static DataTable table;

        public static EventHandler DataUpdated;

        private static string fullFilename
        {
            get
            {
                return "data/" + filename + ".txt";
            }
        }
        private static string filename;

        private static StreamWriter writer = null;

        public static void Init(bool debugMode = false)
        {
            if (_instance is null)
            { 
                _instance = new MagnetController();
                if (debugMode) _instance.adController = new AD_Controller_dummy();
                else _instance.adController = new AD_controller();
                _instance._init();
            }

            lastUsedProfileId = Settings.GetValue<int>("lastUsedProfileId", -1);
            if (lastUsedProfileId > 0) 
            {
                _instance._experiments = ExperimentsDB.GetExperiments(lastUsedProfileId, out _);
            }
        }

        private void _init()
        {

            adController.Init();
            InitSettings();
        }

        private void InitSettings()
        {
            v1_max_step = Settings.GetValue<double>("v1_max_step", 0.1);
            v2_max_step = Settings.GetValue<double>("v2_max_step", 0.1);
            b_max_step = Settings.GetValue<double>("b_max_step", 0.05);

            v1_slewrate = Settings.GetValue<double>("v1_slewrate", 0.1);
            v2_slewrate = Settings.GetValue<double>("v2_slewrate", 0.1);
            b_slewrate = Settings.GetValue<double>("b_slewrate", 0.05);

            bMismatchStop = Settings.GetValue<bool>("bMismatchStop", false);
            maxBMismatch = Settings.GetValue<double>("maxBMismatch", 0.1);

            ReadoutGains = Settings.GetValue<string>("ReadoutGains", "");
            ReadoutFormula = Settings.GetValue<string>("ReadoutFormula", "");

            v1_table_minus = Path.Combine(Directory.GetCurrentDirectory(), "tables", "v1_minus.txt");
            v2_table_minus = Path.Combine(Directory.GetCurrentDirectory(), "tables", "v2_minus.txt");
            v1_table_plus = Path.Combine(Directory.GetCurrentDirectory(), "tables", "v1_plus.txt");
            v2_table_plus = Path.Combine(Directory.GetCurrentDirectory(), "tables", "v2_plus.txt");
            table_readout = Path.Combine(Directory.GetCurrentDirectory(), "tables", "readout.txt");

            if (!v1p.ParseFile(v1_table_plus)) throw new Exception("Не удалось прочитать таблицу v1_plus");
            if (!v1m.ParseFile(v1_table_minus)) throw new Exception("Не удалось прочитать таблицу v1_minus");
            if (!v2p.ParseFile(v2_table_plus)) throw new Exception("Не удалось прочитать таблицу v2_plus");
            if (!v2m.ParseFile(v2_table_minus)) throw new Exception("Не удалось прочитать таблицу v2_minus");
            if (!ro.ParseFile(table_readout)) throw new Exception("Не удалось прочитать таблицу readout");
        }

        public static void ClearList()
        {
            if (!running) _instance._experiments.Clear();
            lastUsedProfileId = -1;
            experimentalPlanChanged = false;
        
        
        }

        public static void SetFilename(string s)
        { 
            filename = GenerateValidFileName(s);
        }

        public static string GetFilename()
        {
            return filename;
        }



        public static void OnClosing()
        {
            if (experimentalPlanChanged && Experiments?.Count > 0)
                lastUsedProfileId = ExperimentsDB.SaveExperimentSet(_instance._experiments);
            Settings.SetValue<int>("lastUsedProfileId", lastUsedProfileId);
        
        }

        public static IADController GetController()
        {
            return _instance.adController;

        }

        private void ExecuteExperiment(BModeFromTo plan, CancellationToken ct)
        {
            int expectedSteps = 0;
            if (plan.PathToZero) 
                expectedSteps += (int)Math.Floor(Math.Abs(plan.BFrom / plan.BStep));
            int mainSteps = (int)Math.Floor(Math.Abs((plan.BTo - plan.BFrom)/ plan.BStep));
            if (plan.Reverse)
                expectedSteps += 2 * mainSteps;
            else
                expectedSteps += mainSteps;
            if (plan.PathToZero)
            {
                int backSteps = 0;
                if (plan.Reverse)
                    backSteps += (int)Math.Floor(Math.Abs(plan.BFrom / plan.BStep));
                else
                    backSteps += (int)Math.Floor(Math.Abs(plan.BTo / plan.BStep));
                expectedSteps += backSteps;
            }

            int currentStep = 0;

            Action<double> SetBAndWork = (targetB) =>
            {
                SetB(targetB);
                Thread.Sleep(plan.Delay);
                PerformExperiments();
                UpdateExperimentProgess(currentStep, expectedSteps);
            };


            if (plan.PathToZero)
            {
                int sign = Math.Sign(plan.BFrom);
                while (Math.Abs(B_current - plan.BFrom) < eps)
                {
                    double targetB = B_current + plan.BStep * sign;
                    if (sign * (targetB - plan.BFrom) > eps)
                        targetB = plan.BFrom;
                    SetBAndWork(targetB);
                    if (ct.IsCancellationRequested) return;
                }
            }
            //main 

            //first point
            SetB(plan.BFrom);
            Thread.Sleep(plan.Delay);
            PerformExperiments();
            if (ct.IsCancellationRequested) return;

            while (Math.Abs(B_current - plan.BTo) < eps)
            {
                int sign = Math.Sign(plan.BTo - plan.BFrom);
                double targetB = B_current + plan.BStep * sign;
                if (sign * (targetB - plan.BTo) > eps)
                    targetB = plan.BTo;
                SetBAndWork(targetB);
                if (ct.IsCancellationRequested) return;
            }

            //main

            if (plan.Reverse)
            { 
                int sign = Math.Sign(plan.BFrom - plan.BTo);
                while (Math.Abs(B_current - plan.BFrom) < eps)
                {
                    double targetB = B_current + plan.BStep * sign;
                    if (sign * (targetB - plan.BFrom) > eps)
                        targetB = plan.BFrom;
                    SetBAndWork(targetB);
                    if (ct.IsCancellationRequested) return;
                }    
            }

            if (plan.PathToZero)
            {
                int sign = Math.Sign(-B_current);

                while (Math.Abs(B_current) < eps)
                {
                    double targetB = B_current + plan.BStep * sign;
                    if (sign * (targetB - plan.BFrom) > eps)
                        targetB = plan.BFrom;
                    SetBAndWork(targetB);
                    if (ct.IsCancellationRequested) return;
                }
            }
        }

        private void PerformExperiments()
        {
            UpdateReadout();
            Dictionary<string, double> values = new Dictionary<string, double>();
            foreach (var e in _experiments)
                values.Add(e.Name, default);

            for (int i = 0; i < _experiments.Count; i++)
            {
                if (_experiments[i] is Calculation) continue;
                _experiments[i].Execute();
                values[_experiments[i].Name] = _experiments[i].GetValue();
            }
            for (int i = 0; i < _experiments.Count; i++)
            {
                if (!(_experiments[i] is Calculation)) continue;
                (_experiments[i] as Calculation).getVariables = () => values;
                _experiments[i].Execute();
                values[_experiments[i].Name] = _experiments[i].GetValue();
            }

            DataRow newRow = table.NewRow();
            foreach (var kvp in values)
            {
                if (table.Columns.Contains(kvp.Key))
                {
                    newRow[kvp.Key] = kvp.Value;
                }
            }
            if (table.Columns.Contains("B_setpoint"))
                newRow["B_setpoint"] = B_current;
            if (table.Columns.Contains("B_readout"))
                newRow["B_readout"] = B_readout;

            lock (table)
            {
                table.Rows.Add(newRow);
            }

            DataUpdated?.Invoke(this, EventArgs.Empty);
            string valueLine = String.Empty;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                valueLine += "\t";
                valueLine += newRow[i].ToString();
            }
            writer?.WriteLine(valueLine);
        }

        private void InitExperiments()
        {
            table = new DataTable();
            table.Columns.Add("B_setpoint", typeof(double));
            table.Columns.Add("B_readout", typeof(double));
            for (int i = 0; i < _experiments.Count; i++)
            {
                table.Columns.Add(new DataColumn(_experiments[i].Name,typeof(double)));
            }
            foreach (var e in _experiments)
                e.Init();

            string headerLine = String.Empty;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                headerLine += "\t";
                headerLine += table.Columns[i].ColumnName;
            }
            writer?.WriteLine(headerLine);
        }

        private void FinalizeExperiments()
        {
            foreach (var e in _experiments)
                e.Finish();
        }


        private void UpdateExperimentProgess(int current, int total)
        { }

        public static void Run(IBMode bMode)
        {
            if (running)   
            {
                MessageBox.Show("Процесс еще продолжается");
                return;
            }
            try
            {
                writer = new StreamWriter(fullFilename);
                disableStartButton();
                running = true;
                cts = new CancellationTokenSource();
                _instance.InitExperiments();
                if (bMode is BModeFromTo)
                    _instance.ExecuteExperiment(bMode as BModeFromTo, cts.Token);
                else if (bMode is BModeList) { }
                else if (bMode is BModeSteady) { }
                _instance.FinalizeExperiments();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                writer.Dispose();
                _instance.SetB(0);
                running = false;
                enableStartButton();
                enableStopButton();
            }
        }



        private void UpdateReadout(bool checkSetpointB = false)
        { 
            
        
        }

        private (double,double) GetVoltage(double b)
        {
            double v1, v2;
            if (b > 0)
            {
                v1 = v1p.GetY(b);
                v2 = v2p.GetY(b);
            }
            else
            {
                v1 = v1m.GetY(b);
                v2 = v2m.GetY(b);
            }
            return (v1,v2);
        }

        private void SetB(double b)
        {
            while (Math.Abs(B_current - b) < eps)
            {
                bool b_raising = b > B_current;
                double b_step = b_raising ? b_max_step : -b_max_step;
                double B_target = B_current;
                B_target += b_step;
                if ((b_raising && B_target > b + eps) || (!b_raising && B_target < b - eps))
                    B_target = b;
                else
                    BTransitioning?.Invoke();
                if (Math.Sign(B_target) != Math.Sign(B_current))
                {
                    GoToV(0, 0);
                    if (B_target >= 0) SetSignPlus();
                    else SetSignMinus();
                    B_current = 0;
                }
                double v1, v2;
                (v1, v2) = GetVoltage(B_target);
                int b_delay = (int)Math.Round(((B_target - B_current) / b_slewrate) * 1000);
                GoToV(v1, v2, b_delay);
                B_current = B_target;
                UpdateReadout(true);
            }
        }

        private void GoToV(double v1, double v2, int externalDelay = 0)
        {
            bool additionalStepsNeeded = (Math.Abs(v1 - V1_current) > v1_max_step
                || Math.Abs(v2 - V2_current) > v2_max_step);
            if (additionalStepsNeeded)
            {
                BTransitioning?.Invoke();
            }


            bool v1_raising = v1 < V1_current;
            bool v2_raising = v2 < V2_current;
            double v1_step, v2_step;
            
            if( additionalStepsNeeded) v1_step = v1_raising ? v1_max_step : v1-V1_current;
            else v1_step = v1 - V1_current;

            if (additionalStepsNeeded) v2_step = v2_raising ? v2_max_step : v2 - V2_current;
            else v2_step = v2 - V2_current;

            int v1_delay = (int)Math.Round((Math.Abs(v1_step) / v1_slewrate) * 1000);
            int v2_delay = (int)Math.Round((Math.Abs(v2_step) / v2_slewrate) * 1000);
            int delay = v1_delay > v2_delay ? v1_delay : v2_delay;


            while (Math.Abs(v1 - V1_current) > eps
                || Math.Abs(v2 - V2_current) > eps)
            {
                double v1_next = V1_current + v1_step;
                if ((v1_raising && v1_next > v1 - eps) || (!v1_raising && v1_next < v1 + eps))
                    v1_next = v1;

                double v2_next = V2_current + v2_step;
                if ((v2_raising && v2_next > v2 - eps) || (!v2_raising && v2_next < v2 + eps))
                    v2_next = v2;

                adController.SetVoltage(v1_next, 0);
                adController.SetVoltage(v2_next, 1);
                V1_current = v1_next;
                V2_current = v2_next;
                Thread.Sleep(delay);
                externalDelay -= delay;
                UpdateReadout();
            }
            if (externalDelay > 0) Thread.Sleep(externalDelay);
        }

        private void SetSignPlus()
        {
            adController.SetDigitalOutput(1);
        }

        private void SetSignMinus()
        {
            adController.SetDigitalOutput(3);
        }

        public static List<(int x, int y)> ParseGXY(string input)
        {
            var result = new List<(int x, int y)>();

            // Регулярное выражение: g(число)=(число)
            // \b - граница слова, чтобы не захватывать part of word
            // (\d+) - захватывает число для x
            // (\d+) - захватывает число для y
            string pattern = @"\bg(\d+)=(\d+)\b";

            MatchCollection matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
            {
                if (match.Success && match.Groups.Count == 3)
                {
                    int x = int.Parse(match.Groups[1].Value);
                    int y = int.Parse(match.Groups[2].Value);
                    result.Add((x, y));
                }
            }

            return result;
        }

        public static string GenerateValidFileName(string fileName, string directoryPath = "/data")
        {
            // 1. Удаляем пробелы в начале и конце
            fileName = fileName?.Trim() ?? "";

            // 2. Удаляем запрещённые символы (для Windows/Linux)
            // Запрещённые символы в именах файлов: \ / : * ? " < > |
            string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            string pattern = "[" + Regex.Escape(invalidChars) + "]";
            fileName = Regex.Replace(fileName, pattern, "");

            // Если после очистки имя пустое, используем значение по умолчанию
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = "file";
            }

            // 3. Формируем полный путь и проверяем существование
            string fullPath = Path.Combine(directoryPath, fileName + ".txt");

            if (!File.Exists(fullPath))
            {
                return fileName; // Возвращаем оригинальное имя, если файл не существует
            }

            // 4. Если файл существует, добавляем индекс
            int index = 1;
            string newFileName;
            string baseName = fileName;

            // Убираем существующий индекс, если он есть (например, filename_1 -> filename)
            Match match = Regex.Match(fileName, @"_(\d+)$");
            if (match.Success)
            {
                baseName = fileName.Substring(0, fileName.LastIndexOf('_'));
                index = int.Parse(match.Groups[1].Value);
            }

            do
            {
                newFileName = $"{baseName}_{index}";
                fullPath = Path.Combine(directoryPath, newFileName + ".txt");
                index++;
            }
            while (File.Exists(fullPath));

            return newFileName;
        }

    }
}
