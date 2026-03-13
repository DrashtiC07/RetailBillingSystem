using Npgsql;
using System;
using System.Windows.Forms;

namespace RetailBillingSystem.Database
{
    /// <summary>
    /// Manages database connections for the Retail Billing System
    /// </summary>
    public class DBConnection
    {
        // Database connection string - Modify these settings according to your PostgreSQL setup
        private const string SERVER = "localhost";
        private const string PORT = "5432";
        private const string DATABASE = "retaildb";
        private const string USERNAME = "postgres";
        private const string PASSWORD = "drishayu"; // Change this to your PostgreSQL password

        private static string _connectionString = null;

        /// <summary>
        /// Gets the connection string for PostgreSQL database
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    _connectionString = $"Host={SERVER};Port={PORT};Database={DATABASE};Username={USERNAME};Password={PASSWORD};";
                }
                return _connectionString;
            }
        }

        /// <summary>
        /// Creates and returns a new database connection
        /// </summary>
        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        /// <summary>
        /// Tests the database connection
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return conn.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection failed:\n{ex.Message}",
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Sets a custom connection string (useful for different environments)
        /// </summary>
        public static void SetConnectionString(string server, string port, string database, string username, string password)
        {
            _connectionString = $"Host={server};Port={port};Database={database};Username={username};Password={password};";
        }
    }
}