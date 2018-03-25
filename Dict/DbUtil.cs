using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Dict
{
    class DbUtil
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public DbUtil()
        {
            Initialize();
        }

        private static DbUtil m_instance = null;

        public static DbUtil GetInstance()
        {
            if (m_instance == null)
                m_instance = new DbUtil();
            return m_instance;
        }

        //Initialize values
        private void Initialize()
        {
            server = "35.194.139.242";
            database = "word_book";
            uid = "caoyu";
            password = "123456";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string BuildInsertSql(Word word)
        {
            // replace sepical characters
            string strPhonetic = word.phonetic.Replace("'", "\\'");

            // compose the insert SQL
            string sql = "insert into word_raw (word, phonetic, translation, explains, time) SELECT '"
                + word.word + "', '"
                + word.phonetic.Replace("'", "\\'") + "', '" /* replace sepical characters */
                + word.translation + "', '"
                + word.explains + "', '"
                + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' "
                + "where not exists (select id from word_raw where word = '" + word.word + "')";

            return sql;
        }

        /// <summary>
        /// Add new word into database
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool AddNewWord(Word word)
        {
            bool bIsSuccess = false;
            try
            {
                // Open connection
                if (this.OpenConnection() == true)
                {
                    // Create Command
                    MySqlCommand cmd = new MySqlCommand(BuildInsertSql(word), connection);
                    // Execute the cmd
                    cmd.ExecuteNonQueryAsync();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        bIsSuccess = true;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                //close Connection
                this.CloseConnection();
            }
            return bIsSuccess;
        }

        public void AsyncAddNewWord(Word word)
        {
            if (!OpenConnection())
            {
                return;
            }
            try
            {
                // Create Command
                MySqlCommand cmd = new MySqlCommand(BuildInsertSql(word), connection);
                AsyncCallback callback = new AsyncCallback(HandleCallback);
                cmd.BeginExecuteNonQuery(callback, cmd);
            }
            catch (Exception ex)
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private void HandleCallback(IAsyncResult result)
        {
            try
            {
                // Retrieve the original cmd object, passed
                // to this procedure in the AsyncState property
                // of the IAsyncResult parameter.
                MySqlCommand cmd = (MySqlCommand)result.AsyncState;
                int rowCount = cmd.EndExecuteNonQuery(result);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
}
}
