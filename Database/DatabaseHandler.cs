using System;
using System.Data.EntityClient;
using System.Diagnostics;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Globalization;

namespace Database
{
    /// <summary>
    /// Database handler. Provides tools for opening and creating a database.
    /// </summary>
    public sealed class DatabaseHandler : IDisposable
    {
        private MembersContainer _database;
        private readonly string _path;
        private readonly string _password;

        /// <summary>
        /// Construct a new database handler for specified database.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="password"></param>
        public DatabaseHandler(string path, string password)
        {
            _path = path;
            _password = password;
        }

        /// <summary>
        /// Open the database. Will create tables if they don't exist.
        /// Throws exceptions if opening fails!
        /// </summary>
        public void Open()
        {
            if (!IsDatabaseCreated(_path, _password))
                CreateDatabaseTables(_path, _password);
            else
                CreateBakFile(_path);

            Debug.Print("Opened database file {0}", _path);

            _database = new MembersContainerImpl(BuildConnectionString());
        }

        private string BuildConnectionString()
        {
            var ee = new EntityConnectionStringBuilder
                         {
                             Provider = "System.Data.SQLite",
                             Metadata = @"res://*/Members.csdl|res://*/Members.ssdl|res://*/Members.msl",
                             ProviderConnectionString =
                                 String.Format(CultureInfo.CurrentCulture, "Data source=\"{0}\";Version=3;Password={1}",
                                               _path, _password)
                         };
            return ee.ConnectionString;
        }

        private static void CreateBakFile(string path)
        {
            if (File.Exists(path))
            {
                string bakFileName = String.Concat(path, ".bak");
                try
                {
                    File.Copy(path, bakFileName, true);
                }
                catch (IOException) { }
            }
        }

        private static void CreateDatabaseTables(string path, string password)
        {
            string connectionString = String.Format(CultureInfo.CurrentCulture, "Data source=\"{0}\";Version=3;Password={1}", path, password);
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string[] creationStrings = DatabaseSettings.Default.CreationString.Split(';');

                foreach (string createDatabase in creationStrings)
                {
                    DbCommand command = connection.CreateCommand();
                    command.CommandText = createDatabase;
                    command.ExecuteNonQuery();
                    Debug.WriteLine("Executed SQL command {0}", createDatabase);
                }
            }
        }

        /// <summary>
        /// Check if database has been created to opened file.
        /// </summary>
        private static bool IsDatabaseCreated(string path, string password)
        {
            string connectionString = String.Format(CultureInfo.CurrentCulture, "Data source=\"{0}\";Version=3;Password={1}", path, password);
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (DataTable table = connection.GetSchema("Tables"))
                {
                    return table.Rows.Count > 0;
                }
            }
        }

        /// <summary>
        /// Close the database.
        /// </summary>
        public void CloseDatabase()
        {
            Dispose();
        }

        /// <summary>
        /// Get the database container.
        /// </summary>
        public MembersContainer Database
        {
            get
            {
                if (_database == null)
                    throw new InvalidOperationException("Database not opened");

                return _database;
            }
        }

        /// <summary>
        /// Dispose the database handler.
        /// </summary>
        public void Dispose()
        {
            _database.SaveChanges();
            _database.Connection.Close();
            _database.Dispose();
            _database = null;
            Debug.Print("Database saved, closed and disposed");
        }
    }
}
