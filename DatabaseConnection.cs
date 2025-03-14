//class in-charge for database connection
using System.Data;
using MySql.Data.MySqlClient;

namespace Aquino_JohnNelson_Act_GUI
{
    public class DatabaseConnection
    {
        // dito yung database connection details
        private const string serverName = "localhost";
        private const string dataBaseName = "StudentRecordDB";
        private const string uid = "root";
        private const string password = ""; //since my current database config doesn't have a password, I will leave this empty

        // Properties for MySQL connection, command, and data reader
        public MySqlConnection Connection { get; private set; }
        public MySqlCommand Command { get; set; }
        public MySqlDataReader DataReader { get; set; }

        // Constructor to initialize the connection and command
        public DatabaseConnection()
        {
            string connectionString = $"Server = {serverName} ; Database = {dataBaseName}; Uid = {uid}; Pwd = {password};";
            Connection = new MySqlConnection(connectionString);
            Command = new MySqlCommand();
            Command.Connection = Connection;
        }

        // method in opening database connection
        public bool Connect()
        {
            if (Connection.State == ConnectionState.Closed || Connection.State == ConnectionState.Broken)
            {
                try
                {
                    Connection.Open();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        // method in closing database connection
        public void Disconnect()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
        }
    }
}