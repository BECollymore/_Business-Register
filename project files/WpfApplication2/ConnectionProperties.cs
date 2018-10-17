using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;

namespace StatsBR
{
    class ConnectionProperties
    {
        private String name;
        private String dataSource;
        private String username;
        private String password;
        private String initCatalogue;

        /// <summary>
        /// Basic Connection Properties constructor
        /// </summary>
        public ConnectionProperties()
        {
        }

        /// <summary>
        /// Constructor with the needed settings
        /// </summary>
        /// <param name="name">The name identifier of the connection</param>
        /// <param name="dataSource">The url where we connect</param>
        /// <param name="username">Username for connection</param>
        /// <param name="password">Password for connection</param>
        /// <param name="initCat">Initial catalogue</param>
        public ConnectionProperties(String name, String dataSource, String username, String password, String initCat)
        {
            this.name = name;
            this.dataSource = dataSource;
            this.username = username;
            this.password = password;
            this.initCatalogue = initCat;
        }
        
        public string InitCatalogue
        {
            get
            {
                return this.initCatalogue;
            }
            set
            {
                this.initCatalogue = value;
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        public string Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value;
            }
        }

        public string DataSource
        {
            get
            {
                return this.dataSource;
            }
            set
            {
                this.dataSource = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        private String ChangeConnStringItem(string connString,string option, string value)
        {
            String[] conItems = connString.Split(';');
            String result = "";
            foreach (String item in conItems)
            {
                if (item.StartsWith(option))
                {
                    result += option + "=" + value + ";";
                }
                else
                {
                    result += item + ";";
                }
            }
            return result;
       }

        //for ASP.net applications
        private void ChangeConnectionSettings(ConnectionProperties cp)
        {
             var cnSection = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
             String connString = cnSection.ConnectionStrings.ConnectionStrings[cp.Name].ConnectionString;
             connString = ChangeConnStringItem(connString, "provider connection string=\"data source", cp.DataSource);
             //connString = ChangeConnStringItem(connString, "provider connection string=\"server", cp.DataSource);// for mySQL
             connString = ChangeConnStringItem(connString, "user id", cp.Username);
             connString = ChangeConnStringItem(connString, "password", cp.Password);
             connString = ChangeConnStringItem(connString, "initial catalog", cp.InitCatalogue);
             //connString = ChangeConnStringItem(connString, "database", cp.InitCatalogue); // this is for mySQL
             cnSection.ConnectionStrings.ConnectionStrings[cp.Name].ConnectionString = connString;
             cnSection.Save();
             ConfigurationManager.RefreshSection("connectionStrings");
        }

        //for windows App.config
        public static bool ChangeConnectionString(string name, string value, string providerName, string appName)
    {
        bool retVal = false;
        try
        {
            
            string fileName = string.Concat(Application.StartupPath, "\\", appName.Trim(), ".exe.Config"); //the application configuration file name
            XmlTextReader reader = new XmlTextReader(fileName);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            reader.Close();
            string nodeRoute = string.Concat("connectionStrings/add");

            XmlNode cnnStr = null;
            XmlElement root = doc.DocumentElement;
            XmlNodeList settings = root.SelectNodes(nodeRoute);

            for (int i = 0; i < settings.Count; i++)
            {
                cnnStr = settings[i];
                if (cnnStr.Attributes["name"].Value.Equals(name))
                    break;
                cnnStr = null;
            }

            cnnStr.Attributes["connectionString"].Value = value;
            cnnStr.Attributes["providerName"].Value = providerName;
            doc.Save(fileName);
            retVal = true;
        }
        catch (Exception ex)
        {
            retVal = false;
            //TODO: Handle the Exception 
        }
        return retVal;
    }


        private static void SetNewConnectionString(string connstringname, string datasource, string initialcatalog, string user, string pwd, string appname)
        {

            //see:http://www.stievens-corner.be/index.php/11-c/17-change-connectionstring-and-save-to-app-config

          // open config
          Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
 
          // create new connectionString 
          SqlConnectionStringBuilder connbuilder = new SqlConnectionStringBuilder();
          connbuilder.DataSource = datasource;
          connbuilder.InitialCatalog = initialcatalog;
          connbuilder.UserID = user;
          connbuilder.Password = pwd;
          connbuilder.ApplicationName = appname;
 
          // set new connectionstring in config
          config.ConnectionStrings.ConnectionStrings[connstringname].ConnectionString = connbuilder.ConnectionString;
 
          // save and refresh the config file
          config.Save(ConfigurationSaveMode.Minimal);
          ConfigurationManager.RefreshSection("connectionStrings");
        }

    }
}