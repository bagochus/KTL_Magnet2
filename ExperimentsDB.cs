using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using KTL_Magnet2.Measurments;
//using Microsoft.Data.Sqlite;
using System.Xml.Linq;
using System.Text.Json;
using KTL_Magnet2.Measurements;
using KTL_Magnet2.DialogForms;
using System.Windows.Input;


namespace KTL_Magnet2
{
    public static class ExperimentsDB
    {
        private enum TypeCodes
        {
            VisaVDC = 1,
            VisaArb = 2,
            Ad = 3,
            Calculation = 4
        }

        private static readonly string DbPath;
        private static readonly string ConnectionString;
        private static readonly object LockObject = new object();

        // Статический конструктор - инициализирует БД при первом обращении
        static ExperimentsDB()
        {
            // Путь к БД в AppData\Roaming
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appDataPath, "KTL_Magnet");

            if (!Directory.Exists(appFolder))
                Directory.CreateDirectory(appFolder);

            DbPath = Path.Combine(appFolder, "experiments.db");
            ConnectionString = $"Data Source={DbPath};Version=3;";

            // Создаем БД и таблицы если их нет
            //InitializeDatabase();
        }

        public static void InitializeDatabase()
        {
            lock (LockObject)
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    //exp sets
                    string createExpTable = @"
                        CREATE TABLE IF NOT EXISTS ExperimentSet (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name Text NOT NULL,
                            AutoSave BIT DEFAULT 0
                        )";

                    // links
                    string createLinksTable = @"
                        CREATE TABLE IF NOT EXISTS ExperimentLinks (
                           Id INTEGER PRIMARY KEY AUTOINCREMENT,
                           SetId INTEGER NOT NULL, 
                           ExpId INTEGER NOT NULL, 
                           ExpType INTEGER NOT NULL,
                           FOREIGN KEY (SetId) REFERENCES ExperimentSet(Id) ON DELETE CASCADE
                        )";

                    //visa vdc
                    string createVisaVDCTable = @"
                        CREATE TABLE IF NOT EXISTS VisaVDC (
                           Id INTEGER PRIMARY KEY, 
                           Name TEXT NOT NULL,
                           Device TEXT,
                           PLC REAL,
                           MLimit Real,
                           Channel INTEGER,
                           Delay INTEGER NOT NULL DEFAULT 0,
                           MAvg INTEGER NOT NULL DEFAULT 1
                        )";

                    //visa arb
                    string createVisaArbTable = @"
                        CREATE TABLE IF NOT EXISTS VisaArb (
                           Id INTEGER PRIMARY KEY, 
                           Name TEXT NOT NULL,
                           Device TEXT,
                           StrInit TEXT,
                           StrCycle TEXT,
                           StrFinal TEXT
                        )";

                    //ad
                    string createAdTable = @"
                        CREATE TABLE IF NOT EXISTS Ad (
                           Id INTEGER PRIMARY KEY, 
                           Name TEXT NOT NULL,
                           Chanell INTEGER NOT NULL DEFAULT 0,
                           MAvg INTEGER NOT NULL DEFAULT 1,
                           Delay INTEGER NOT NULL DEFAULT 0,
                           Gain INTEGER NOT NULL DEFAULT 1
                        )";

                    // calc
                    string createCalcTable = @"
                        CREATE TABLE IF NOT EXISTS Calculation (
                           Id INTEGER PRIMARY KEY, 
                           Name TEXT NOT NULL,
                           Formula TEXT
                        )";

                    using (var command = new SQLiteCommand(createExpTable, connection))
                        command.ExecuteNonQuery();
                    using (var command = new SQLiteCommand(createLinksTable, connection))
                        command.ExecuteNonQuery();
                    using (var command = new SQLiteCommand(createVisaVDCTable, connection))
                        command.ExecuteNonQuery();
                    using (var command = new SQLiteCommand(createVisaArbTable, connection))
                        command.ExecuteNonQuery();
                    using (var command = new SQLiteCommand(createAdTable, connection))
                        command.ExecuteNonQuery();
                    using (var command = new SQLiteCommand(createCalcTable, connection))
                        command.ExecuteNonQuery();
                }
            }
        }

        private static VisaVoltageMeasurment GetVDC(int Id)
        {
            VisaVoltageMeasurment result = null;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT Name, Device ,PLC, MLimit, Channel, Delay, MAvg FROM VisaVDC" +
                    $" WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = new VisaVoltageMeasurment();
                            result.Name = reader.GetString(0);
                            result.DeviceName = reader.IsDBNull(1) ? null : reader.GetString(1);
                            result.PLC_time = reader.IsDBNull(2) ? -1 : reader.GetDouble(2);
                            result.Limit = reader.IsDBNull(3) ? -1 : reader.GetDouble(3);
                            result.Channel = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                            result.Delay = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                            result.Avg = reader.IsDBNull(6) ? 1 : reader.GetInt32(6);
                        }
                    }
                }
            }

            return result;
        }

        private static int SaveVisaVoltageMeasurement(VisaVoltageMeasurment measurement)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = @"
            INSERT INTO VisaVDC (Name, Device, PLC, MLimit, Channel, Delay, MAvg) 
            VALUES (@name, @Device ,@plc, @limit, @channel, @delay, @avg);
            SELECT last_insert_rowid();";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", measurement.Name);
                    command.Parameters.AddWithValue("@Device", measurement.DeviceName);
                    command.Parameters.AddWithValue("@plc", measurement.PLC_time);
                    command.Parameters.AddWithValue("@limit", measurement.Limit);
                    command.Parameters.AddWithValue("@channel", measurement.Channel);
                    command.Parameters.AddWithValue("@delay", measurement.Delay);
                    command.Parameters.AddWithValue("@avg", measurement.Avg);

                    // Выполняем запрос и получаем последний вставленный ID
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        private static ArbitraryVisaMesurement GetArbVisa(int Id)
        {
            ArbitraryVisaMesurement result = null;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT Name, Device, StrInit, StrCycle, StrFinal FROM VisaArb" +
                    $" WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = new ArbitraryVisaMesurement();
                            result.Name =  reader.GetString(0);
                            result.DeviceName = reader.IsDBNull(1) ? null : reader.GetString(1);
                            result.InitStrings = 
                                reader.IsDBNull(2) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(reader.GetString(2));
                            result.MeasureStrings =
                                reader.IsDBNull(3) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(reader.GetString(3));
                            result.FinishStrings =
                                reader.IsDBNull(4) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(reader.GetString(4));
                        }
                    }
                }
            }

            return result;
        }

        private static int SaveArbVisaMeasurement(ArbitraryVisaMesurement measurement)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = @"
            INSERT INTO VisaArb (Name, Device, StrInit, StrCycle, StrFinal) 
            VALUES (@Name, @Device, @StrInit, @StrCycle, @StrFinal);
            SELECT last_insert_rowid();";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", measurement.Name);
                    command.Parameters.AddWithValue("@Device", measurement.Name);
                    command.Parameters.AddWithValue("@StrInit", JsonSerializer.Serialize<List<string>>(measurement.InitStrings));
                    command.Parameters.AddWithValue("@StrCycle", JsonSerializer.Serialize<List<string>>(measurement.MeasureStrings));
                    command.Parameters.AddWithValue("@StrFinal", JsonSerializer.Serialize<List<string>>(measurement.FinishStrings));

                    // Выполняем запрос и получаем последний вставленный ID
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        private static AD_Measurement GetAd(int Id)
        {
            AD_Measurement result = null;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT Name, Chanell, MAvg, Delay, Gain FROM Ad" +
                    $" WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = new AD_Measurement();
                            result.Name = reader.GetString(0);
                            result.ch_num = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            result.Avg = reader.IsDBNull(2) ? 1 : reader.GetInt32(2);
                            result.Delay = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                            result.Gain = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                        }
                    }
                }
            }

            return result;
        }

        private static int SaveAdMeasurement(AD_Measurement measurement)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();



                string query = @"
            INSERT INTO Ad (Name, Chanell, MAvg, Delay, Gain ) 
            VALUES (@Name, @Chanell, @MAvg, @Delay, @Gain );
            SELECT last_insert_rowid();";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", measurement.Name);
                    command.Parameters.AddWithValue("@Gain", measurement.Gain);
                    command.Parameters.AddWithValue("@Chanell", measurement.ch_num);
                    command.Parameters.AddWithValue("@Delay", measurement.Delay);
                    command.Parameters.AddWithValue("@MAvg", measurement.Avg);

                    // Выполняем запрос и получаем последний вставленный ID
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }


        private static Calculation GetCalculation(int Id)
        {
            Calculation result = null;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT Name, Formula FROM Calculation " +
                    $"WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = new Calculation();
                            result.Name = reader.GetString(0);
                            result.Formula = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        }
                    }
                }
            }

            return result;
        }

        private static int SaveCalculation(Calculation measurement)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();



                string query = @"
            INSERT INTO Calculation (Name, Formula ) 
            VALUES (@Name, @Formula);
            SELECT last_insert_rowid();";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", measurement.Name);
                    command.Parameters.AddWithValue("@Formula", measurement.Formula);

                    // Выполняем запрос и получаем последний вставленный ID
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public static List<(string,int)> GetProfiles(bool includeAutosaves = false)
        {
            List<(string, int)> result = new List<(string, int)>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT Name, Id FROM ExperimentSet";
                if (!includeAutosaves) query += " WHERE AutoSave = 0";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            result.Add((reader.GetString(0),reader.GetInt32(1)));
                    }
                }

            }

            return result;
        }


        private static int SaveExperimet(ExperimentStep experiment, out int typeId)
        {
            typeId = 0;
            if (experiment is VisaVoltageMeasurment)
            {
                typeId = (int)TypeCodes.VisaVDC;
                return SaveVisaVoltageMeasurement(experiment as VisaVoltageMeasurment);
            }
            else if (experiment is ArbitraryVisaMesurement)
            {
                typeId = (int)TypeCodes.VisaArb;
                return SaveArbVisaMeasurement(experiment as ArbitraryVisaMesurement);
            }
            else if (experiment is AD_Measurement)
            {
                typeId = (int)TypeCodes.Ad;
                return SaveAdMeasurement(experiment as AD_Measurement);
            }
            else if (experiment is Calculation)
            {
                typeId = (int)TypeCodes.Calculation;
                return SaveCalculation(experiment as Calculation);
            }

            return -1;
        
        }

        private static ExperimentStep GetExperiment(int id, int typeId)
        {
            if (!Enum.IsDefined(typeof(TypeCodes), typeId)) return null;
            TypeCodes typeCode = (TypeCodes)typeId;
            switch (typeCode)
            {
                case TypeCodes.VisaVDC:     return GetVDC(id);
                case TypeCodes.VisaArb:     return GetArbVisa(id);
                case TypeCodes.Ad:          return GetAd(id);
                case TypeCodes.Calculation: return GetCalculation(id);
                default: return null;
            }
        }

        public static int SaveExperimentSet(IList<ExperimentStep> experiments, string Name = null)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                int setId = 0;
                bool autoSave = false;
                if (String.IsNullOrEmpty(Name))
                {
                    Name = $"autosave_{DateTime.Now:yyyyMMdd_HHmm}";
                    autoSave = true;
                }


                string query = @"INSERT INTO ExperimentSet (Name,AutoSave) VALUES 
                                (@Name, @AutoSave);
                                SELECT last_insert_rowid()";

                using (var commnad = new SQLiteCommand(query, connection))
                {
                    commnad.Parameters.AddWithValue("@Name", Name);
                    commnad.Parameters.AddWithValue("@AutoSave", autoSave);
                    setId = Convert.ToInt32(commnad.ExecuteScalar());
                }

                List<(int, int)> idAndTypes = new List<(int, int)>();

                for (int i = 0; i < experiments.Count(); i++)
                {
                    int id = SaveExperimet(experiments[i], out int t);
                    if (t > 0)
                        idAndTypes.Add((id, t));
                }

                for (int i = 0; i < idAndTypes.Count(); i++)
                {
                    string queryLinks = @"INSERT INTO ExperimentLinks (SetId, ExpId, ExpType)
                                           VALUES (@SetId, @ExpId, @ExpType)";

                    using (var command = new SQLiteCommand(queryLinks, connection))
                    {
                        command.Parameters.AddWithValue("@SetId", setId);
                        command.Parameters.AddWithValue("@ExpId", idAndTypes[i].Item1);
                        command.Parameters.AddWithValue("@ExpType", idAndTypes[i].Item2);

                        command.ExecuteNonQuery();
                    }
                }
                return setId;
            }
        }

        public static List<ExperimentStep> GetExperiments(int id, out int failed)
        {
            
            failed = 0;
            List<ExperimentStep> result = new List<ExperimentStep>();

            List<(int, int)> idsAndTypes = new List<(int, int)>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = @"select ExpId, ExpType FROM ExperimentLinks
                                  WHERE SetId = @SetId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SetId", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int eid = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                            int etype = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                            if (eid > 0 && etype > 0) idsAndTypes.Add((eid, etype));
                            else failed++;
                        }
                    }
                }
            }
            for (int i = 0; i < idsAndTypes.Count(); i++)
            {
                var exp = GetExperiment(idsAndTypes[i].Item1, idsAndTypes[i].Item2);
                if (exp is null) failed++;
                else result.Add(exp);
            
            }
            return result;
        }


    }
}
