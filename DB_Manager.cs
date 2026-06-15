using KTL_Magnet2.Measurements;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text.Json;

namespace KTL_Magnet2
{
    public  class DB_Manager
    {

        private static string databaseFile = "Settings.db";
        private static string connectionString = $"Data Source={databaseFile};Version=3;";

        public static void InitializeDatabase()
        {


            if (!File.Exists(databaseFile)) SQLiteConnection.CreateFile(databaseFile);




            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createTables = @"
                CREATE TABLE IF NOT EXISTS Settings (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    ad_Measurements_ids TEXT,
                    visa_Measurements_ids TEXT,
                    safety_params_id INTEGER NOT NULL,
                    readout_filename TEXT,
                    v1_filename TEXT,
                    v2_filename TEXT,
                    use_setpoint_ct INTEGER NOT NULL,
                    use_readout_ct INTEGER NOT NULL,
                    FOREIGN KEY (safety_params_id) REFERENCES SafetyParameters(Id) ON DELETE SET NULL
                );

                CREATE TABLE IF NOT EXISTS AD_Measurements (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    chanell INTEGER NOT NULL,
                    range INTEGER NOT NULL,
                    avg INTEGER NOT NULL,
                    delay INTEGER NOT NULL,
                    gain INTEGER NOT NULL

                );

                CREATE TABLE IF NOT EXISTS VISA_Measurements (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    device_name TEXT NOT NULL,
                    chanell INTEGER,
                    delay INTEGER,
                    measurement_limit DOUBLE,
                    plc_time DOUBLE
                );

                CREATE TABLE IF NOT EXISTS SafetyParameters (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    max_v1_step DOUBLE NOT NULL,
                    max_v2_step DOUBLE NOT NULL,
                    max_v1_sr DOUBLE NOT NULL,
                    max_v2_sr DOUBLE NOT NULL,
                    max_b_step DOUBLE NOT NULL,
                    max_b_sr DOUBLE NOT NULL,
                    smooth_step DOUBLE NOT NULL,
                    zero_crossing_delay INTEGER NOT NULL,
                    smooth_delay INTEGER NOT NULL,
                    use_smooth_zero_crossing INTEGER NOT NULL

                    
                )";


                var command = new SQLiteCommand(createTables, connection);
                command.ExecuteNonQuery();
            }

        }


        public static ExpSetup LoadProfile(string profilename)
        {
            ExpSetup profile = new ExpSetup();
            int session_id = 0;
            int safety_params_id = 0;
            List<int> ads_ids = new List<int>();
            List<int> visa_ids = new List<int>();


            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql_main = "SELECT * FROM Settings WHERE Name = @Name";
                string sql_safety = "SELECT * FROM SafetyParameters WHERE safety_params_id = @safety_params_id";
                //string sql3 = "SELECT * FROM AxisSessions WHERE session_id = @session_id";

                using (SQLiteCommand command = new SQLiteCommand(sql_main, connection))
                {
                    command.Parameters.AddWithValue("@Name", profilename);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            safety_params_id = Convert.ToInt32(reader["safety_params_id"]);
                            session_id = Convert.ToInt32(reader["Id"]);
                            string ads_json = reader.IsDBNull(2) ? null : reader.GetString(2);
                            string visa_json = reader.IsDBNull(3) ? null : reader.GetString(3);
                            if (ads_json != null)
                            {
                                ads_ids = JsonSerializer.Deserialize<List<int>>(ads_json);
                            }
                            if (visa_json != null)
                            {
                                visa_ids = JsonSerializer.Deserialize<List<int>>(visa_json);
                            }
                            profile.UseReadoutCalibrationTables = Convert.ToInt32(reader["use_readout_ct"]) != 0;
                            profile.UseSetpointCalibrationTables = Convert.ToInt32(reader["use_setpoint_ct"]) != 0;
                            if (profile.UseReadoutCalibrationTables)
                            {
                                string readout_filename = reader.IsDBNull(5) ? "" : reader.GetString(5);
                                if (readout_filename == "") profile.UseReadoutCalibrationTables = false;
                                else profile.Readout_filename = readout_filename;
                            }
                            if (profile.UseSetpointCalibrationTables)
                            {
                                string v1_filename = reader.IsDBNull(6) ? "" : reader.GetString(6);
                                string v2_filename = reader.IsDBNull(7) ? "" : reader.GetString(7);
                                if (v1_filename =="" || v2_filename =="") profile.UseSetpointCalibrationTables = false;
                                else
                                {
                                    profile.v1_filename = v1_filename;
                                    profile.v2_filename = v2_filename;
                                }
                            }



                        }
                    }
                }


                using (SQLiteCommand command = new SQLiteCommand(sql_safety, connection))
                {
                    command.Parameters.AddWithValue("@safety_params_id", safety_params_id);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            profile.MaxV1Step = Convert.ToDouble(reader["max_v1_step"]);
                            profile.MaxV2Step = Convert.ToDouble(reader["max_v2_step"]);
                            profile.MaxBStep = Convert.ToDouble(reader["max_b_step"]);

                            profile.MaxV1SlewRate = Convert.ToDouble(reader["max_v1_sr"]);
                            profile.MaxV2SlewRate = Convert.ToDouble(reader["max_v2_sr"]);
                            profile.MaxBSlewrate = Convert.ToDouble(reader["max_b_sr"]);

                            profile.UseSmoothZeroCrossing = Convert.ToInt32(reader["use_smooth_zero_crossing"]) != 0;

                            profile.SmoothDelay = Convert.ToInt32(reader["smooth_delay"]);
                            profile.ZeroCrossingDelay = Convert.ToInt32(reader["zero_crossing_delay"]);

                        }
                    }
                }

            }
            foreach (int i in ads_ids) profile.ad_Measurements.Add(Load_ADMeasurement(i));
            foreach (int i in visa_ids) profile.visa_Measurements.Add(LoadVisaMeasurement(i));

            return profile; 
        }
        private static AD_Measurement Load_ADMeasurement(int id)
        {
            AD_Measurement Measurement = new AD_Measurement();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql_main = "SELECT * FROM AD_Measurements WHERE Id = @Id";

                using (SQLiteCommand command = new SQLiteCommand(sql_main, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            Measurement.ch_num = Convert.ToInt32(reader["chanell"]);
                            Measurement.Avg = Convert.ToInt32(reader["avg"]);
                            Measurement.Range = Convert.ToInt32(reader["range"]);
                            Measurement.Delay = Convert.ToInt32(reader["delay"]);
                            Measurement.Gain = Convert.ToInt32(reader["gain"]);
                        }


                    }

                    
                }
            }
            return Measurement;
        }
        private static VISA_Measurement LoadVisaMeasurement(int id) 
        {
            VISA_Measurement vm = new VISA_Measurement();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql_main = "SELECT * FROM VISA_Measurements WHERE Id = @Id";

                using (SQLiteCommand command = new SQLiteCommand(sql_main, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            vm.DeviceName = reader.GetString(0);
                            vm.Channel = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
                            vm.Delay = reader.IsDBNull(3) ? -1 : reader.GetInt32(3);
                            vm.Limit = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                            vm.PLC_time = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);
                        }
                    }
                }
            }
            return vm;
        }


        private static int SaveVisaMeasurement(VISA_Measurement vm)
        {
            if (vm == null) return -1;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = @"INSERT INTO VISA_Measurements
                     (device_name, chanell, delay, measurement_limit, plc_time)
                     VALUES (@device_name, @chanell, @delay, @limit, @plc_time); 
                    SELECT last_insert_rowid()";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@device_name", vm.DeviceName);
                    command.Parameters.AddWithValue("@chanell", vm.Channel);
                    command.Parameters.AddWithValue("@delay", vm.Delay);
                    command.Parameters.AddWithValue("@limit", vm.Limit);
                    command.Parameters.AddWithValue("@plc_time", vm.PLC_time);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        private static int SaveADMeasurement(AD_Measurement adm)
        {
            if (adm == null) return -1;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = @"INSERT INTO AD_Measurements
                     (chanell, range, avg, delay, gain)
                     VALUES (@chanell, @range, @avg, @delay, @gain); 
                    SELECT last_insert_rowid()";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@chanell", adm.ch_num);
                    command.Parameters.AddWithValue("@range", adm.Range);
                    command.Parameters.AddWithValue("@avg", adm.Avg);
                    command.Parameters.AddWithValue("@delay", adm.Delay);
                    command.Parameters.AddWithValue("@gain", adm.Gain);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        private static void SaveProfile(ExpSetup setup, string name)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = @"INSERT INTO Settings
                     (Name, ad_Measurements_ids, visa_Measurements_ids, safety_params_id, readout_filename,
                       v1_filename, v2_filename, use_setpoint_ct, use_readout_ct )
                     VALUES (@Name, @ad_Measurements_ids, @visa_Measurements_ids, @safety_params_id, @readout_filename,
                       @v1_filename, @v2_filename, @use_setpoint_ct, @use_readout_ct )";

                string ads_json = null;
                if (setup.ad_Measurements.Count > 0)
                {
                    List<int> ads_ids = new List<int>();
                    foreach (var adm in setup.ad_Measurements) ads_ids.Add(SaveADMeasurement(adm));
                    ads_json = JsonSerializer.Serialize(ads_ids);
                }

                string visa_json = null;
                if (setup.visa_Measurements.Count > 0)
                {
                    List<int> ids = new List<int>();
                    foreach (var vm in setup.visa_Measurements) ids.Add(SaveVisaMeasurement(vm));
                    visa_json = JsonSerializer.Serialize(ids);
                }


                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ad_Measurements_ids", ads_json ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@visa_Measurements_ids", visa_json ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@readout_filename", setup.Readout_filename ?? "");
                    command.Parameters.AddWithValue("@v1_filename", setup.v1_filename ?? "");
                    command.Parameters.AddWithValue("@v2_filename", setup.v2_filename ?? "");
                    command.Parameters.AddWithValue("@use_setpoint_ct", setup.UseSetpointCalibrationTables ? 0 : 1);
                    command.Parameters.AddWithValue("@use_readout_ct", setup.UseReadoutCalibrationTables ? 0 : 1);

                    command.Parameters.AddWithValue("@safety_params_id", SaveSafetyParams(setup));

                }
            }



        }
        private static int SaveSafetyParams(ExpSetup setup)
        {
            if (setup == null) return -1;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = @"INSERT INTO SafetyParameters
                     (max_v1_step, max_v2_step, max_v1_sr, max_v2_sr, max_b_step, max_b_sr, smooth_step,
                        zero_crossing_delay, smooth_delay, use_smooth_zero_crossing)
                     VALUES (@max_v1_step, @max_v2_step, @max_v1_sr, @max_v2_sr, @max_b_step, @max_b_sr, @smooth_step,
                        @zero_crossing_delay, @smooth_delay, @use_smooth_zero_crossing); 
                    SELECT last_insert_rowid()";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {

                    command.Parameters.AddWithValue("@max_v1_step", setup.MaxV1Step);
                    command.Parameters.AddWithValue("@max_v2_step", setup.MaxV2Step);
                    command.Parameters.AddWithValue("@max_b_step", setup.MaxBStep);

                    command.Parameters.AddWithValue("@max_v1_sr", setup.MaxV1SlewRate);
                    command.Parameters.AddWithValue("@max_v2_sr", setup.MaxV2SlewRate);
                    command.Parameters.AddWithValue("@max_b_sr", setup.MaxBSlewrate);

                    command.Parameters.AddWithValue("@smooth_step", setup.SmoothStep);
                    command.Parameters.AddWithValue("@smooth_delay", setup.SmoothDelay);
                    command.Parameters.AddWithValue("@zero_crossing_delay", setup.ZeroCrossingDelay);
                    command.Parameters.AddWithValue("@use_smooth_zero_crossing", setup.UseSmoothZeroCrossing ? 0 : 1);


                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }


        }

        public static List<string> GetProfilesNames()
        {
            List<string> names = new List<string>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Settings";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) names.Add(Convert.ToString(reader["Name"]));
                    }
                }

            }

            return names;
        }








    }
}
