using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace KTL_Magnet2
{
    public class Settings
    {


        private static readonly string DbPath;
        private static readonly string ConnectionString;
        private static readonly object LockObject = new object();

        // Статический конструктор - инициализирует БД при первом обращении
        static Settings()
        {
            // Путь к БД в AppData\Roaming
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appDataPath, "KTL_Magnet");

            if (!Directory.Exists(appFolder))
                Directory.CreateDirectory(appFolder);

            DbPath = Path.Combine(appFolder, "settings.db");
            ConnectionString = $"Data Source={DbPath};Version=3;";

            // Создаем БД и таблицы если их нет
            InitializeDatabase();
        }

        private static void InitializeDatabase()
        {
            lock (LockObject)
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    // Таблица для int
                    string createIntTable = @"
                        CREATE TABLE IF NOT EXISTS settings_int (
                            key TEXT PRIMARY KEY,
                            value INTEGER NOT NULL
                        )";

                    // Таблица для double
                    string createDoubleTable = @"
                        CREATE TABLE IF NOT EXISTS settings_double (
                            key TEXT PRIMARY KEY,
                            value REAL NOT NULL
                        )";

                    // Таблица для bool
                    string createBoolTable = @"
                        CREATE TABLE IF NOT EXISTS settings_bool (
                            key TEXT PRIMARY KEY,
                            value INTEGER NOT NULL
                        )";

                    // Таблица для string
                    string createStringTable = @"
                        CREATE TABLE IF NOT EXISTS settings_string (
                            key TEXT PRIMARY KEY,
                            value TEXT NOT NULL
                        )";

                    using (var command = new SQLiteCommand(createIntTable, connection))
                        command.ExecuteNonQuery();

                    using (var command = new SQLiteCommand(createDoubleTable, connection))
                        command.ExecuteNonQuery();

                    using (var command = new SQLiteCommand(createBoolTable, connection))
                        command.ExecuteNonQuery();

                    using (var command = new SQLiteCommand(createStringTable, connection))
                        command.ExecuteNonQuery();
                }
            }
        }

        // Получение значения
        public static T GetValue<T>(string key, T defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty");

            lock (LockObject)
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    Type type = typeof(T);

                    if (type == typeof(int))
                    {
                        string query = "SELECT value FROM settings_int WHERE key = @key";
                        using (var command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@key", key);
                            var result = command.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                                return (T)Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (type == typeof(double))
                    {
                        string query = "SELECT value FROM settings_double WHERE key = @key";
                        using (var command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@key", key);
                            var result = command.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                                return (T)Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (type == typeof(bool))
                    {
                        string query = "SELECT value FROM settings_bool WHERE key = @key";
                        using (var command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@key", key);
                            var result = command.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                                return (T)(object)(Convert.ToInt32(result) == 1);
                        }
                    }
                    else if (type == typeof(string))
                    {
                        string query = "SELECT value FROM settings_string WHERE key = @key";
                        using (var command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@key", key);
                            var result = command.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                                return (T)Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else
                    {
                        throw new NotSupportedException($"Type {type.Name} is not supported");
                    }

                    return defaultValue;
                }
            }
        }

        // Установка значения
        public static void SetValue<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty");

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            lock (LockObject)
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    Type type = typeof(T);

                    if (type == typeof(int))
                    {
                        string query = @"
                            INSERT OR REPLACE INTO settings_int (key, value) 
                            VALUES (@key, @value)";

                        using (var command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@key", key);
                            command.Parameters.AddWithValue("@value", Convert.ToInt32(value));
                            command.ExecuteNonQuery();
                        }
                    }
                    else if (type == typeof(double))
                    {
                        string query = @"
                            INSERT OR REPLACE INTO settings_double (key, value) 
                            VALUES (@key, @value)";

                        using (var command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@key", key);
                            command.Parameters.AddWithValue("@value", Convert.ToDouble(value));
                            command.ExecuteNonQuery();
                        }
                    }
                    else if (type == typeof(bool))
                    {
                        string query = @"
                            INSERT OR REPLACE INTO settings_bool (key, value) 
                            VALUES (@key, @value)";

                        using (var command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@key", key);
                            command.Parameters.AddWithValue("@value", (bool)(object)value ? 1 : 0);
                            command.ExecuteNonQuery();
                        }
                    }
                    else if (type == typeof(string))
                    {
                        string query = @"
                            INSERT OR REPLACE INTO settings_string (key, value) 
                            VALUES (@key, @value)";

                        using (var command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@key", key);
                            command.Parameters.AddWithValue("@value", value.ToString());
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        throw new NotSupportedException($"Type {type.Name} is not supported");
                    }
                }
            }
        }

    }
}
