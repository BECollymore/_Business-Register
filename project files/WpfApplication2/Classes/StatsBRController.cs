using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using System.Data;

namespace StatsBR.Classes
{
    class DBController
    {

        private string ConnectionString;

        public DBController()
        {
            //this.ConnectionString = StatsBR.MainWindow.Properties.Settings.Default.ConnectionString;
             SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();

             if (connection != null)
             {
                 this.ConnectionString = connection.ConnectionString;
             }
             else
             {
                 this.ConnectionString = "";
             }

            // test connection to database...
             testConnection();

        }
        private SqlConnectionStringBuilder BuildConnection()
        {
            // ----- Build a connection string based on user settings.
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();

            try
            {

                /* string connectString =
                "Server=(local);Initial Catalog=AdventureWorks;" +
                "Integrated Security=true";
                 SqlConnectionStringBuilder builder =
                     new SqlConnectionStringBuilder(connectString);
                 * 
                 */

                // ----- Add the server name.                            
                connection.DataSource = @"Server=STATSERV2";

                // ----- Add the authentication.              
                connection.IntegratedSecurity = false;
                connection.UserID = "Administrator";
                connection.Password = "";//TODO:encript login

                // ----- Add the other settings.
                connection.InitialCatalog = "STATSBR";
                connection.ConnectTimeout = 100;//100 seconds
                connection.MultipleActiveResultSets = false;

                // ----- Success.
                return connection;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "ERROR connecting to Database...Contact Administrator");
                return null;
            }
        }

        private void testConnection()
        {
              // ----- See if the specified connection works.
              SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
              SqlConnection testLink = null;

              try
              {
                  // ----- Retrieve the connection.
                  connection = BuildConnection();
                  if (connection == null) return;

                  // ----- Test the connection.
                  testLink = new SqlConnection(connection.ConnectionString);
                  testLink.Open();
                  if (testLink.State == ConnectionState.Open)
                      MessageBox.Show("Connection opened successfully.");
                  else
                      MessageBox.Show("Connection failed to open.");
              }
              catch (Exception ex)
              {
                  MessageBox.Show("Connection failed with the following error: " +
                      ex.Message);
              }
              finally
              {
                  if (testLink != null) testLink.Dispose();
              }
        }




    }


        

    class StatsBRController
    {
        

        private string SourceName;
        

        public StatsBRController()
        {
            this.SourceName = "BR";
           
        }
        public StatsBRController(string SourceName)
        {
            this.SourceName = SourceName;
        }

       
        public string GetSourceName() 
        {
            return this.SourceName;
        }
        public void SetSourceName (string SourceName)  
        {
            this.SourceName = SourceName;
        }
        public int ConnectToDB(String connectionString, String UserName, String Password)
        {              
            return 1;
        }
        /*
        public static SqlConnection GetConnectionInstance()
        {
            return new SqlConnection();
        }
         * */

        public void SetDataGrid(string SourceName)
        {

        }
        public void SetGraph(string SourceName)
        {

        }
    }
}
