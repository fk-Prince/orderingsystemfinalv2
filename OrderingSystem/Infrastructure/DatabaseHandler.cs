using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace OrderingSystem.DatabaseConnection
{
    internal class DatabaseHandler
    {
        private static DatabaseHandler instance;
        private static readonly object lockObject = new object();
        private MySqlConnection conn;
        private DatabaseHandler()
        {
        }

        public static DatabaseHandler getInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new DatabaseHandler();
                    }
                }
            }
            return instance;
        }
        public MySqlConnection getConnection()
        {

            try
            {

                conn = new MySqlConnection(getConnectionString());

                while (conn.State == ConnectionState.Connecting)
                {
                    Task.Delay(10);
                }


                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return conn;

        }
        public void closeConnection()
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }

        }
        private string getConnectionString()
        {
            return new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                UserID = "root",
                Password = "root",
                Database = "xdxd",
                SslMode = MySqlSslMode.None,
                AllowPublicKeyRetrieval = true,
                AllowUserVariables = true
            }.ConnectionString;
        }
    }
}
