using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Data.Entity.Core.EntityClient;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Text.RegularExpressions;
using System.Windows;
using System.Threading;
using System.Windows.Media;
using Telerik.Windows.Controls;
using System.Globalization;

namespace StatsBR
{
    

    public class DBController
    {
        private string serverName;
        private string dBName;
        private string dBUserName;
        private string dBUserPassword;
        private string dBConnectionString;
        private SqlConnection conn;
        private bool isConnected;
        private BindingSource bindingSrc = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private SqlCommandBuilder commandBuilder = new SqlCommandBuilder();
        

        public static  List<SelectedTable> CurrentTableList;
        private Worksheet LINKWS;

        private MainWindow win;

        public DBController( Worksheet ws, MainWindow mw)
        {
            this.LINKWS = ws;
            this.win = mw;
        }
        public DBController()
        {
            //TODO: constructor that uses the connection string from app.config     
           // bindingSrc = new BindingSource();
           // dataAdapter = new SqlDataAdapter();
           CurrentTableList = new List<SelectedTable>();

        }

        public DBController(string pServerName, string pDBName)
        {
            string connectionStr = "Data Source=" + pServerName + ";database=" + pDBName + ";Integrated Security=true";
            this.ConnectionString = connectionStr;
            CurrentTableList = new List<SelectedTable>();
            
        }
        public DBController(string pServerName, string pdbName, string pDBUserName, string pDBUserPassword)
        {
            this.serverName = pServerName;
            this.dBName = pdbName;
            this.dBUserName = pDBUserName;
            this.dBUserPassword = pDBUserPassword;

            string connectionStr = "Data Source=" + pServerName + ";database= " + pdbName + ";uid=" + pDBUserName + ";pwd=" + pDBUserPassword;
            this.ConnectionString = connectionStr;
            CurrentTableList = new List<SelectedTable>();
        }

     


        public string DBUserName
        {
            get
            {
                return this.dBUserName;
            }
            set
            {
                this.dBUserName = value;
            }
        }

        public SqlCommandBuilder CommandBuilder
        {
            get
            {
                return this.commandBuilder;
            }
            set
            {
                this.commandBuilder = value;
            }
        }

        public BindingSource BindingSrc
        {
            get
            {
                return this.bindingSrc;
            }
            set
            {
                this.bindingSrc = value;
            }
        }

        public SqlConnection Conn
        {
            get
            {
                return conn;
            }
        }

        public string DBName
        {
            get
            {
                return this.dBName;
            }
            set
            {
                this.dBName = value;
            }
        }

        public bool IsConnected
        {
            get
            {
                return this.isConnected;
            }
            set
            {
                this.isConnected = value;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.dBConnectionString;
            }
            set
            {
                this.dBConnectionString = value;
            }
        }

        public string DBUserPassword
        {
            get
            {
                return this.dBUserPassword;
            }
            set
            {
                this.dBUserPassword = value;
            }
        }

      
        public string ServerName
        {
            get
            {
                return this.serverName;
            }
            set
            {
                this.serverName = value;
            }
        }

        public List<string> ShowTableNames()
        {

            if (isConnected || FrmConfigureDBConnect.IsSettingsApplied)
            {
                return GetTables(this.ConnectionString);
            }
            return new List<String>();

        }

        private  List<string> GetTables(string connectionString)
        {
            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SelectedTable table;
                DataTable schema = conn.GetSchema("Tables");
                List<string> tableNames = new List<string>();
                string names;
                foreach (DataRow row in schema.Rows)
                {
                  
                    table.Schema = row[1].ToString();
                    table.TableName =  row[2].ToString();
                    DBController.CurrentTableList.Add(table);//populate tablenames container for future use
                   
                    names = row[1].ToString() + "." + row[2].ToString(); //concatenate to "[schema].[table name]"
                    // row[0] has database name
                    tableNames.Add(names); // row[1] has schema "dbo", "Core", "External" etc
                    // row[2] Table names
                }
                conn.Close();
                conn.Dispose();
                return tableNames;
            }
        }      


        public bool TestConnection()
        {
            bool ok = false;
            try
            {
                using (conn = new SqlConnection(this.ConnectionString))
                {
                    conn.Open();
                    ok = true;
                }
            }
            catch (Exception ex)
            {
                // throw new Exception("Error connecting to DB using connection string, please verify details", ex);                 
                RadMessageBox.Show("Invalid DB SqlConnnection" + Environment.NewLine + ex.Message, "DB Connection Test", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    //OJO: cannot close and dispose of object since it must remain active if apply settings is clicked
                    //one way to get around is to place connection string details in App.config and all methods re initialize conn variable as 
                    //seen in call to readyconnection method...
                    conn.Close();
                    conn.Dispose();
                }
            }

            return ok;
        }

        public void ConnectToDB()
        {
            try
            {
                using (conn = new SqlConnection(this.ConnectionString))
                {                
                    conn.Open();
                    IsConnected = true;

                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Invalid DB SqlConnnection" + Environment.NewLine + ex.Message, "DB Connection Error!", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }


        public string GetAdoConfigurationString(string pName)
        {
            //connection string in App.config is specific to Entity Framework and contains metadata. You need to get your provider connection string from it
            //see:http://stackoverflow.com/questions/12946099/keyword-not-supported-metadata-with-sql-connection-in-entityt-framework-wit
            string results = "";
            try
            {
                string connectionstring = ConfigurationManager.ConnectionStrings[pName].ConnectionString;

                var builder = new EntityConnectionStringBuilder(connectionstring);
                results = builder.ProviderConnectionString;

            }
            catch (Exception)
            {
                RadMessageBox.Show("Error retrieving App.config connection string" + Environment.NewLine, "App.config connection string Error!", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
            return results;

        }

        public void CloseConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
                ClearPool();
            }

        }

        public int ExecuteNonQuery(SqlCommand command)
        {//cannot use static method since conn object must first be initialized in constructor
            try
            {
                //ReadyConnection();
                ConnectToDB();
                command.Connection = conn;
                int result = command.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error running  DB Query", ex);
            }

            finally
            {
                command.Dispose();
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
        }

        public SqlDataReader ExecuteReader(SqlCommand command)
        {
            try
            {
                //ReadyConnection();
                ConnectToDB();
                command.Connection = conn;
                SqlDataReader result = command.ExecuteReader(CommandBehavior.CloseConnection);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("Error Reading data from DB", ex);
            }
        }

        public object ExecuteScalar(SqlCommand command)
        {
            try
            {
                //ReadyConnection();
                ConnectToDB();
                command.Connection = conn;

                object value = command.ExecuteScalar();
                if (value is DBNull)
                {
                    return default(decimal);
                }
                else
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in ExecuteScalar method", ex);
            }
        }

        public static void ClearPool()
        {
            SqlConnection.ClearAllPools();
        }

        private String ChangeConnStringItem(string connString, string option, string value)
        {
            String[] conItems = connString.Split(';');
            String result = "";
            bool isItemFound = false;
            foreach (String item in conItems)
            {
                if (item.StartsWith(option))
                {
                    result += option + "=" + value + ";";
                    isItemFound = true;
                }
                else
                {
                    result += item + ";";
                }
            }
            //if option is not in connection string in app.config file add option
            if (!isItemFound)
            {
                result += option + "=" + value + ";";
            }
            return result;
        }

        /* public void ChangeConnectionSettings(ConnectionProperties cp)
         {
              var cnSection = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
              String connString = cnSection.ConnectionStrings.ConnectionStrings[cp.Name].ConnectionString;
              connString = ChangeConnStringItem(connString, "provider connection string=\"data source", cp.DataSource);
              connString = ChangeConnStringItem(connString, "provider connection string=\"server", cp.DataSource);
              connString = ChangeConnStringItem(connString, "user id", cp.Username);
              connString = ChangeConnStringItem(connString, "password", cp.Password);
              connString = ChangeConnStringItem(connString, "initial catalog", cp.InitCatalogue);
              connString = ChangeConnStringItem(connString, "database", cp.InitCatalogue);
                    cnSection.ConnectionStrings.ConnectionStrings[cp.Name].ConnectionString = connString;
              cnSection.Save();
              ConfigurationManager.RefreshSection("connectionStrings");
         }*/


        public DataTable GetTableData(string tableName)
        {
            string sqlQuery = string.Empty;
            sqlQuery = "SELECT * FROM " + tableName;
            using (conn = new SqlConnection(this.ConnectionString))
            {// modify to use conn variable in this class...
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                dataAdapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable(tableName);
                commandBuilder = new SqlCommandBuilder(dataAdapter);
                if ( dt.Rows.Count > 0 )
                 {
                    dataAdapter.Fill(dt);  //handle exceptions for this especially when table is empty ...
                 }
                bindingSrc.DataSource = dt;
                return dt;
            }

        }

        public void UpdateBusinessTable(DataTable dt)
        {
            // Update the database with the user's changes.
            
         
        }

        public DataTable GetData(string selectCommand)
        {
            DataTable table = new DataTable();

            try
            {                
                // Create a new data adapter based on the specified query.
                dataAdapter = new SqlDataAdapter(selectCommand,this.ConnectionString);

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.

                commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                bindingSrc.DataSource = table; //bind gridview using binding source variable or returned datatable value                


            }
            catch (SqlException)
            {
                RadMessageBox.Show("To run this application, replace the value of the " +
                    "connectionString variable with a connection string that is " +
                    "valid for your system.");
            }
            return table;
        }

        public bool PopulateWithBRMF(Worksheet BRMF_Worksheet, string TabName)
        {
            bool ok = false;
            switch (TabName)
            {
                case "BRMF_Matched_Estab":
                    UpdateBRMFMatched(BRMF_Worksheet);
                    break;

                case "BRMF_Unmatched_Estab":
                    UpdateBRMFUnmatched(BRMF_Worksheet);
                    break;

                case "IRD":
                    // UpdateIRD(BRMF_Worksheet, BRContext); //BRContext was removed...
                    break;

                case "NIC":
                   // UpdateNIC(BRMF_Worksheet, BRContext);
                    break;

                case "VAT":
                   // UpdateVAT(BRMF_Worksheet, BRContext);
                    break;
            }

            return ok;
        }

        public struct BizStatus
        {
            public int statuscode;
            public string label;
            public string description;
        };

        public struct BizType
        {
            public int typecode;
            public string label;
            public string description;
        };

        private string CreateStatisticalNumber(BusinessRegisterEntities BRContext)
        {

            string StatisticalNumber = "";
            bool Statsnumok = false;

            var StatsNum = (from TempBiz in BRContext.Businesses
                            select TempBiz.StatisticalNumber).ToList().LastOrDefault();


            if (String.IsNullOrEmpty(StatsNum) || StatsNum == "NULL" || StatsNum == "null")
            {
                //if no records are found, then, this is first entry. 
                StatisticalNumber = "S00000001";
            }
            else
            {
                //there are already records in database table
                //Get number value from string and increment
                //using regular expressions
                Match match = Regex.Match(StatsNum, @"(\d+)");
                if (match.Success)
                {
                    int tempstatsnum = int.Parse(match.Groups[1].Value);
                    tempstatsnum++;//increment for new stats number
                                   //convert to string to determine zero padding to recreate 9 digit zero filled string
                    string statsnumstring = tempstatsnum.ToString();
                    string aux = "S";
                    for (int i = 1; i <= (StatsNum.Length - statsnumstring.Length) - 1; i++)
                    {  //use (StatsNum.Length - statsnumstring.Length) -1  since aux already contain one char i.e. "S"
                       //e.g using S00000001 we get (9 - 1) -1 , this means we have to pad with 7 zeros
                        aux = aux + "0";
                    }
                    //add the number part to the padded zeros string to get new statistical number;
                    aux = aux + statsnumstring;
                    StatisticalNumber = aux;
                    Statsnumok = true;
                }
                else
                {
                    if (Statsnumok == false)
                    {
                        // use a random number generator
                        //however check must be made that it does not produce
                        //statistical number already in database
                        Random random = new Random();
                        int randomNumber = random.Next(0, 10000000);
                        StatisticalNumber = "S" + randomNumber.ToString();
                    }
                }
            }
            return StatisticalNumber;
        }

        public bool UpdateBRMFMatched(Worksheet BRMF_Worksheet)
        {                     
            bool biztypefound = false;//if not found save new in DB before business
           
            CellRange usedCellRange = BRMF_Worksheet.UsedCellRange;
            CellSelection HeaderSelection;
            CellSelection DataSelection;
            string HeaderName = "";
            int recordcounter = 0; 
           
            int ChildEstabCountNIC = 0; //to keep count of child establishment suffix NIC#
            string TempNic= ""; //keep record of parent child 
            int ChildEstabCountREG = 0; //to keep count of child estblishment suffix for REG#
            string TempReg = "";

            List <string> ErrorLog = new List<string>();
            bool IsDuplicate = false;// is record from BRMF a duplicate already in database
            bool ERRORS = false;
            int ErrorCount = 0;
            //to parse datetime
            string[] formats = { "MM/dd/yyyy" };

            //https://www.codeproject.com/articles/52752/updating-your-form-from-another-thread-without-cre

            Business biz; 
            //OJO: must add statical # key into business before inserting 
            //into exrternal keys table due top referential integrity
            ExternalKey Ekey;           

            //IRD table object for populating IRD table before inserting reference in masterkey (externalkeys) table
            IRD IRDTable;
            //declare NIC object to store NIC object to populate NIC table
            NIC NICTable;
            //declare table object to populate the VAT table before adding referenced value in masrterkey (externalkeys) table
            VAT VATTable;
            //declare table object to populate the Regestrar table before adding referenced value in masterkey (externalkeys) table
            Registrar REGTable;
            //table object to populate contact details
            Contact ContactTable;

            BusinessRegisterEntities BRContext;
            BusinessRegisterEntities BRContextBizType;
            BusinessRegisterEntities BRContextIRD;
            BusinessRegisterEntities BRContextNIC;
            BusinessRegisterEntities BRContextVAT;
            BusinessRegisterEntities BRContextREG;
            BusinessRegisterEntities BRContextExtKey;
            BusinessRegisterEntities BRContextContacts;          

            try
            {

                //CellRange FirstRow = usedCellRange.GetFirstRow(); 
                // CellSelection HeaderSelection = BRMF_Worksheet.Cells[FirstRow.FromIndex, FirstRow.ToIndex];
                // ICellValue cellValue = HeaderSelection.GetValue().Value;

                for (int i = 1; i <= usedCellRange.RowCount; i++)
                {//start i=1 omit header
                    biz = new Business();
                    IRDTable = new IRD();
                    Ekey = new ExternalKey();
                    NICTable = new NIC();
                    ContactTable = new Contact();

                    //set the reference year for nic table as described in data dictionary
                    NICTable.Year = DateTime.Now.Year.ToString();
                    VATTable = new VAT();
                    REGTable = new Registrar();
                    REGTable.CreationDate = default(DateTime);

                    BRContext = new BusinessRegisterEntities();
                    BRContextBizType = new BusinessRegisterEntities();
                    BRContextIRD = new BusinessRegisterEntities();
                    BRContextNIC = new BusinessRegisterEntities();
                    BRContextExtKey = new BusinessRegisterEntities();
                    BRContextVAT = new BusinessRegisterEntities();
                    BRContextREG = new BusinessRegisterEntities();
                    BRContextContacts = new BusinessRegisterEntities();

                    //check that this is not a duplicate record that is already in database
                    DataSelection = BRMF_Worksheet.Cells[i, 0];
                    string tempIRDnum = DataSelection.GetValue().Value.RawValue;
                    var bizcheck = (from TempBiz in BRContext.ExternalKeys
                                    where TempBiz.IRDNumber == tempIRDnum
                                    select TempBiz.IRDNumber).ToList().LastOrDefault();
                    if (!(string.IsNullOrEmpty(bizcheck)))
                    {
                        //the record is already in database  
                        IsDuplicate = true;
                        ErrorCount = ErrorCount + 1;
                        // write to LOGS
                        //to write to logs throw exception and when it is caught info will be placed in logs below                        

                        recordcounter++;
                        //get IRDNumber for logs
                        IRDTable.IRDNumber = DataSelection.GetValue().Value.RawValue;
                        //get name of business for logs
                        DataSelection = BRMF_Worksheet.Cells[i, 4];
                        if (string.IsNullOrEmpty(DataSelection.GetValue().Value.RawValue))
                        {
                            DataSelection = BRMF_Worksheet.Cells[i, 5]; //use operating name
                        }
                        biz.LegalName = DataSelection.GetValue().Value.RawValue;
                        //write to error LOG
                        ERRORS = true;
                        //TODO://add more intuitive error message use: validation message etc...
                        // Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "The following error was found inserting business data into database from excel file: " + ex.Message, "Add Business Data from MS Excel file", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                        string error = "";

                        if (string.IsNullOrEmpty(biz.LegalName))
                            biz.LegalName = "MISSING";
                        error += "XLSX RECORD #: " + i + "\nTAXPAYER #: " + IRDTable.IRDNumber + "\nBusiness Name: " + biz.LegalName + "\nERROR MSSG: " + "DUPLICATE RECORD" + " \n";
                        error += "****************** \n";
                        ErrorLog.Add(error);

                    }
                    else
                    { //new record
                        IsDuplicate = false;
                        for (int j = 0; j <= usedCellRange.ColumnCount; j++)
                        {
                            HeaderSelection = BRMF_Worksheet.Cells[0, j];
                            HeaderName = HeaderSelection.GetValue().Value.RawValue;
                            DataSelection = BRMF_Worksheet.Cells[i, j];
                            // Assign statisitcal number to business record
                            //get last statistical #...if this is first entry ...then
                            //for first record use statistical # S00000001
                            biz.StatisticalNumber = CreateStatisticalNumber(BRContext);
                            //instatiate contact object
                            ContactTable.StatisticalNumber = biz.StatisticalNumber;
                            //assign other values from Excel file
                            switch (HeaderName)
                            {
                                case "TAXPAYER #":
                                    /* FindOptions options = new FindOptions()
                                       {
                                           StartCell = new WorksheetCellIndex(LinkWS, 0, 0),
                                           FindBy = FindBy.Rows,
                                           FindIn = FindInContentType.Values,
                                           FindWhat = DataSelection.GetValue().Value.RawValue,
                                           FindWithin = FindWithin.Sheet,
                                       };
                                       FindResult findResult = LinkWS.Find(options);
                                       int rowindex = findResult.FoundCell.CellIndex.RowIndex;
                                       int colindex = findResult.FoundCell.CellIndex.ColumnIndex;
                                       //now that the index for the search result is found
                                       //for the taxpayer number
                                       //find values for other attributes
                                       string nicnum = "";

                                       if (LinkWS.Cells[0, colindex + 1].GetValue().Value.RawValue == "NIC #")
                                       {
                                           nicnum = LinkWS.Cells[rowindex, colindex + 1].GetValue().Value.RawValue;
                                       }

                                    NIC # NOT NEEDED HERE...SEE STATCAN POWERPOINT FOR UPDATE RULES...
                                       */

                                    //Adding referential record in IRD table.
                                    //before population External key (master) table


                                    //save stat # & IRD # to populate Master key table
                                    //Note: IRD # does not go in main Business table on Stat #
                                    int lenI = DataSelection.GetValue().Value.RawValue.Length;
                                    if (lenI <= 8)
                                    {
                                        IRDTable.IRDNumber = (DataSelection.GetValue().Value.RawValue).Substring(0, lenI);
                                    }
                                    else
                                    {
                                        IRDTable.IRDNumber = (DataSelection.GetValue().Value.RawValue).Substring(0, 8);
                                    }

                                    Ekey.StatisticalNumber = biz.StatisticalNumber;
                                    Ekey.IRDNumber = IRDTable.IRDNumber;
                                    break;
                                case "VAT #":
                                    //vat has max chars of 8 
                                    //vat # has "NA" chars before all businesses with no vat 
                                    //Note: for primary key constraints we had to prefill all businesses without VAT#
                                    //Note also: there is a requirement constraint for month and Year...business rules requirement
                                    int lenV = DataSelection.GetValue().Value.RawValue.Length;
                                    if (lenV <= 8)
                                    {
                                        VATTable.VATNumber = (DataSelection.GetValue().Value.RawValue).Substring(0, lenV);
                                    }
                                    else
                                    {
                                        VATTable.VATNumber = (DataSelection.GetValue().Value.RawValue).Substring(0, 8);
                                    }
                                    //prefill month and year
                                    VATTable.Month = "00";
                                    VATTable.Year = "0000";
                                    Ekey.VATNumber = VATTable.VATNumber;
                                    break;
                                case "NIC #":
                                    int lenN = DataSelection.GetValue().Value.RawValue.Length;
                                    if (lenN <= 8)
                                    {
                                        NICTable.NICNumber = (DataSelection.GetValue().Value.RawValue).Substring(0, lenN);
                                    }
                                    else
                                    {
                                        NICTable.NICNumber = (DataSelection.GetValue().Value.RawValue).Substring(0, 8);
                                    }
                                    Ekey.NICNumber = NICTable.NICNumber;
                                    break;
                                case "REGISTRATION #":
                                    int lenR = DataSelection.GetValue().Value.RawValue.Length;
                                    if (string.IsNullOrEmpty(DataSelection.GetValue().Value.RawValue))
                                    { //for all those external key matches without a registration number ...
                                      //i.e. if excel tab for matched records has all others except missing reg numbers
                                      //use the last 7 chars of  statistical number and prefix for "M" to denote missing
                                        REGTable.RegistrarNumber = "M" + Ekey.StatisticalNumber.Substring(2, 7);
                                    }
                                    else if (lenR <= 8)
                                    {
                                        REGTable.RegistrarNumber = (DataSelection.GetValue().Value.RawValue).Substring(0, lenR);
                                    }
                                    else
                                    {
                                        REGTable.RegistrarNumber = (DataSelection.GetValue().Value.RawValue).Substring(0, 8);
                                    }
                                    Ekey.RegistrarNumber = REGTable.RegistrarNumber;
                                    break;
                                case "LEGAL  NAME":
                                    biz.LegalName = DataSelection.GetValue().Value.RawValue;
                                    IRDTable.Name = biz.LegalName;
                                    //since this is the matched business tab...let's keep the names consistent
                                    NICTable.Name = IRDTable.Name;
                                    REGTable.Name = IRDTable.Name;
                                    break;
                                case "OPERATING NAME":
                                    biz.OperationalName = DataSelection.GetValue().Value.RawValue;
                                    IRDTable.TradeName = biz.OperationalName;
                                    //since this is the matched business tab...let's keep the names consistent
                                    NICTable.TradeName = IRDTable.TradeName;
                                    break;
                                case "LEGAL ADDRESS":
                                    biz.LegalAddress = DataSelection.GetValue().Value.RawValue;
                                    IRDTable.MailingAddress = biz.LegalAddress;// one Address field for IRD table...
                                                                               //since this is the matched business tab...let's keep the names consistent
                                    NICTable.AddressLine1 = IRDTable.MailingAddress;
                                    REGTable.Address = IRDTable.MailingAddress;
                                    break;
                                case "OPERATING ADDRESS":
                                    biz.Address = DataSelection.GetValue().Value.RawValue;
                                    if (IRDTable.MailingAddress.Length == 0 || !(biz.Address.Equals(IRDTable.MailingAddress)))
                                    {
                                        IRDTable.MailingAddress = biz.Address;
                                    }
                                    //since this is the matched business tab...let's keep the names consistent
                                    NICTable.AddressLine2 = biz.Address;
                                    break;
                                case "DISTRICT":
                                    biz.District = DataSelection.GetValue().Value.RawValue;
                                    IRDTable.MailingCity = biz.District;
                                    //since this is the matched business tab...let's keep the names consistent
                                    NICTable.District = biz.District;
                                    break;
                                case "POSTAL CODE":
                                    biz.PostalCode = DataSelection.GetValue().Value.RawValue;
                                    break;
                                case "PNONE #":
                                    biz.PhoneNumber = DataSelection.GetValue().Value.RawValue;
                                    IRDTable.PhoneNumber = biz.PhoneNumber;
                                    //since this is the matched business tab...let's keep the names consistent
                                    NICTable.PhoneNumber = biz.PhoneNumber;
                                    break;
                                case "FAX":
                                    biz.Fax = DataSelection.GetValue().Value.RawValue;
                                    IRDTable.FaxNumber = biz.Fax;
                                    break;
                                case "EMAIL":
                                    biz.Email = DataSelection.GetValue().Value.RawValue;
                                    IRDTable.EmailAddress = biz.Email;
                                    //since this is the matched business tab...let's keep the names consistent
                                    NICTable.EmailAddress = biz.Email;
                                    break;
                                case "URL":
                                    biz.URL = DataSelection.GetValue().Value.RawValue;
                                    break;
                                case "GPS":
                                    biz.GPS = DataSelection.GetValue().Value.RawValue;
                                    break;
                                case "CONTACT TITLE":
                                    //TODO for contact...first insert record with stats# in business core 
                                    //then insert contact for that record                                 
                                    ContactTable.Title = DataSelection.GetValue().Value.RawValue;
                                    break;
                                case "CONTACT NAME":
                                    ContactTable.Name = DataSelection.GetValue().Value.RawValue;
                                    break;
                                case "CONTACT PHONE":
                                    ContactTable.PhoneNumber1 = DataSelection.GetValue().Value.RawValue;
                                    break;
                                case "CONTACT EMAIL":
                                    ContactTable.EmailAddress1 = DataSelection.GetValue().Value.RawValue;
                                    break;
                                case "BUSINESS STATUS":
                                    //TODO: Business status must be standardized
                                    //this must be done after data is received fom ADMIN sources...
                                    int status = 9;//not stated
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        status = Convert.ToInt32(DataSelection.GetValue().Value.RawValue);
                                    }
                                    if (status == 0 || status == 1 || status == 2)
                                    {
                                        biz.BusinessStatusCode = Convert.ToByte(status);
                                    }
                                    else
                                    {  //if is null or empty , then use not stated
                                        biz.BusinessStatusCode = Convert.ToByte(9);//not stated
                                    }
                                    break;

                                case "BUSINESS TYPE":

                                    string biztype = DataSelection.GetValue().Value.RawValue;
                                    if (biztype.Contains("#N/A") || biztype.Contains("#") || biztype.Contains("N/A") || biztype == "" || biztype.Length == 0)
                                    {
                                        // assign the value of NOT STATED
                                        biz.BusinessTypeCode = Convert.ToByte(99);//not stated
                                        break;
                                    }
                                    //query business type table and compare string to get the code for one that matches... if not in table then insert new type...
                                    if (biztype.Length > 0 || !(string.IsNullOrEmpty(biztype)))
                                    {
                                        var BizType = BRContextBizType.BusinessTypes;

                                        foreach (BusinessType btype in BizType)
                                        {  //convert all strings to lowercase before comparing or searching 
                                            if (btype.Label.ToLower().Contains(DataSelection.GetValue().Value.RawValue.ToLower()))
                                            {
                                                if (btype.Label.Length == DataSelection.GetValue().Value.RawValue.Length)
                                                {
                                                    biz.BusinessTypeCode = btype.BusinessTypeCode;
                                                    biztypefound = true;
                                                    break;
                                                }
                                                else if (Math.Abs(btype.Label.Length - DataSelection.GetValue().Value.RawValue.Length) < 5)
                                                { //for cases like LLC,LLP,LLLP
                                                    biz.BusinessTypeCode = btype.BusinessTypeCode;
                                                    biztypefound = true;
                                                    break;
                                                }
                                            }
                                            else if (btype.LongDescription.ToLower().Contains(DataSelection.GetValue().Value.RawValue.ToLower()))
                                            {
                                                if (btype.Label.Length == DataSelection.GetValue().Value.RawValue.Length)
                                                {
                                                    biz.BusinessTypeCode = btype.BusinessTypeCode;
                                                    biztypefound = true;
                                                    break;
                                                }
                                                else if (Math.Abs(btype.LongDescription.Length - DataSelection.GetValue().Value.RawValue.Length) < 5)
                                                { //for cases like LLC,LLP,LLLP
                                                    biz.BusinessTypeCode = btype.BusinessTypeCode;
                                                    biztypefound = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // assign the value of NOT STATED
                                        biz.BusinessTypeCode = Convert.ToByte(99);//not stated
                                        biztypefound = true;
                                        break;
                                    }
                                    //populate IRD table variable for legalstatus
                                    IRDTable.LegalStatus = biz.BusinessTypeCode;

                                    // else add new business type to table and send message to user informing
                                    //them that the new business type has been added
                                    //which is not consistent with those already in table...
                                    if (!biztypefound)
                                    {
                                        var lastBizTypeCodes = (from BizTypes in BRContextBizType.BusinessTypes
                                                                select BizTypes.BusinessTypeCode);

                                        //find the final biztypecode and increment that value for new biztypecode
                                        byte last = 0;
                                        foreach (var typecode in lastBizTypeCodes)
                                        {
                                            if (last < typecode)
                                                last = typecode;
                                        }
                                        //add new business type code to business also...
                                        ++last; //increament from last BusinessTypeCode in table
                                        biz.BusinessTypeCode = last;
                                        // make sure to save in businesstype table before Business
                                        SqlConnection connect = new SqlConnection(GetAdoConfigurationString("BusinessRegisterEntities"));
                                        SqlCommand cmd = new SqlCommand { Connection = connect };
                                        cmd.CommandText = "Insert into [BusinessRegister].[Core].[BusinessType] (BusinessTypeCode, Label,LongDescription) values (@biztypecode,@biztypelabel,@biztypelongdesc)";
                                        cmd.Parameters.AddWithValue("@biztypecode", last);
                                        cmd.Parameters.AddWithValue("@biztypelabel", DataSelection.GetValue().Value.RawValue);
                                        cmd.Parameters.AddWithValue("@biztypelongdesc", DataSelection.GetValue().Value.RawValue);
                                        try
                                        {
                                            connect.Open();
                                            cmd.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "The following error was found inserting business data into database from excel file: " + ex.Message, "Add Business from excel file", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                                        }
                                        finally
                                        {
                                            if (connect != null)
                                                connect.Close();
                                        }

                                    }

                                    break;
                                case "YEAR BEGUN OPERATION":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    { //https://docs.microsoft.com/en-us/dotnet/api/system.datetime.tryparseexact?redirectedfrom=MSDN&view=netframework-4.7.2#System_DateTime_TryParseExact_System_String_System_String___System_IFormatProvider_System_Globalization_DateTimeStyles_System_DateTime__                                       
                                        string dateString = DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("MM/dd/yyyy"));
                                        DateTime parsedDateTime;
                                        if (DateTime.TryParseExact(dateString, formats, new CultureInfo("en-US"),
                                                                       DateTimeStyles.None, out parsedDateTime))
                                        { biz.YearBegunOperation = parsedDateTime; }
                                        else { biz.YearBegunOperation = null; }
                                    }
                                    break;
                                case "FISCAL PERIOD START":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        string dateString = DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("MM/dd/yyyy"));
                                        DateTime parsedDateTime;
                                        if (DateTime.TryParseExact(dateString, formats, new CultureInfo("en-US"),
                                                                       DateTimeStyles.None, out parsedDateTime))
                                        { biz.FiscalPeriodStart = parsedDateTime; }
                                        else { biz.FiscalPeriodStart = null; }
                                    }
                                    break;
                                case "FISCAL PERIOD END":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        string dateString = DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("MM/dd/yyyy"));
                                        DateTime parsedDateTime;
                                        if (DateTime.TryParseExact(dateString, formats, new CultureInfo("en-US"),
                                                                       DateTimeStyles.None, out parsedDateTime))
                                        { biz.FiscalPeriodEnd = parsedDateTime; }
                                        else { biz.FiscalPeriodEnd = null; }
                                    }
                                    break;
                                case "LEGAL CODE":
                                    biz.LegalCode = DataSelection.GetValue().Value.RawValue;
                                    break;
                                case "% FOREIGN OWNED":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        biz.PercentForeignOwn = Convert.ToDouble(DataSelection.GetValue().Value.RawValue);
                                    }
                                    break;
                                case "# OF EMPLOYEES":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        biz.NumberOfEmployees = Convert.ToInt64(DataSelection.GetValue().Value.RawValue);
                                    }
                                    break;
                                case "# EMPLOYEES SOURCE":
                                    biz.NumberOfEmployeesSource = DataSelection.GetValue().Value.RawValue;
                                    break;
                                case "# EMPLOYEES DATE":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        biz.NumberOfEmployeesDate = Convert.ToDateTime(DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("mm/d/yyyy")));
                                    }
                                    break;
                                case "# SALARIED EMPLOYEES":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        biz.NumberOfSalariedEmployees = Convert.ToInt64(DataSelection.GetValue().Value.RawValue);
                                    }
                                    break;
                                case "# SALARIED EMPLOYEES SOURCE":
                                    biz.NumberOfSalariedEmployeesSource = DataSelection.GetValue().Value.RawValue;
                                    break;
                                case "# SALARIED EMPLOYEES DATE":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        biz.NumberOfSalariedEmployeesDate = Convert.ToDateTime(DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("mm/d/yyyy")));
                                    }
                                    break;
                                case "SALES":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        biz.Sales = Convert.ToDouble(DataSelection.GetValue().Value.RawValue);
                                        IRDTable.ValTaxSupply = Convert.ToInt32(biz.Sales);
                                    }
                                    break;
                                case "SALES SOURCE":
                                    biz.SalesSource = DataSelection.GetValue().Value.RawValue;
                                    break;
                                case "SALES DATE":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        biz.SalesDate = Convert.ToDateTime(DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("mm/d/yyyy")));
                                    }
                                    break;
                                case "REVENUE":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        biz.Revenue = Convert.ToDouble(DataSelection.GetValue().Value.RawValue);
                                    }
                                    break;
                                case "REVENUE SOURCE":
                                    biz.RevenueSource = DataSelection.GetValue().Value.RawValue;
                                    break;
                                case "REVENUE DATE":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        biz.RevenueDate = Convert.ToDateTime(DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("mm/d/yyyy")));
                                    }
                                    break;
                                case "WAGES":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        biz.Wages = Convert.ToDouble(DataSelection.GetValue().Value.RawValue);
                                    }
                                    break;
                                case "WAGES SOURCE":
                                    biz.WagesSource = DataSelection.GetValue().Value.RawValue;
                                    break;
                                case "WAGES DATE":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        biz.WagesDate = Convert.ToDateTime(DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("mm/d/yyyy")));
                                    }
                                    break;
                                case "ISIC":
                                    biz.ISIC = DataSelection.GetValue().Value.RawValue;
                                    IRDTable.Primary_ISIC = biz.ISIC;
                                    break;
                                case "ISIC SOURCE":
                                    biz.ISICSource = DataSelection.GetValue().Value.RawValue;
                                    break;
                                case "ISIC DATE":
                                    if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                    {
                                        biz.ISICDate = Convert.ToDateTime(DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("mm/d/yyyy")));
                                    }
                                    break;
                            }
                        }// end of j loop
                    }                   

                    float percent = ((float)i / usedCellRange.RowCount) * 100;
                    string XLSprogress = percent.ToString("0.00");

                    // calculateXLSprogress =  (((Math.Abs(i - usedCellRange.RowCount))/ usedCellRange.RowCount) * 100);
                    // MWIN.statusbarlabel.Content = calculateXLSprogress.ToString() + " % of .XLS file read so far... Please Wait";
                    // MWIN.updatestatusbarlabel(MWIN.statusbarlabel.Content + "TESTING STATUS BAR");
                       win.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            win.statusbarlabel.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkRed);
                            win.updatestatusbarlabel(XLSprogress + "% Successful ... Processing Record #:  " + i);
                            //progressbar
                            //progressbar range settings
                            if (i == 1)
                            {//initialize rage only on first iteration of loop
                                win.statusbarprogressbar.Minimum = 0;
                                win.statusbarprogressbar.Maximum = usedCellRange.RowCount;
                                win.statusbarprogressbar.Value = 0;
                                win.statusbarprogressbar.IsIndeterminate = false;
                                // Set BorderBrush  directly
                                win.statusbarprogressbar.BorderBrush = new SolidColorBrush(Colors.Red);
                                // Set BorderBrush using a Style
                                Style myStyle3 = new Style(typeof(RadProgressBar));
                                myStyle3.Setters.Add(new Setter(RadProgressBar.BorderBrushProperty, new SolidColorBrush(Colors.Red)));
                                win.statusbarprogressbar.Style = myStyle3;
                                // win.statusbarprogressbar.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkRed);
                            }
                            win.statusbarprogressbar.Value = i;
                        }));
                    // populate BR database with data from matched records of spreadsheet
                    // submitting data to database
                    if (!IsDuplicate)
                    {
                     try {                              
                        try {
                            //Add ID details of last user doing updates to database...i.e.LastUserIDupdated
                            biz.LastUserIdUpdated = MainWindow.userID;
                            biz.LastUpdateDate = DateTime.Now;
                            //submitt data to Business table
                            BRContext.Businesses.Add(biz);
                            BRContext.SaveChanges();

                            // submit data to IRD table
                            BRContextIRD.IRDs.Add(IRDTable);
                            BRContextIRD.SaveChanges();

                            // submit changes to VAT table
                            BRContextVAT.VATs.Add(VATTable);
                            BRContextVAT.SaveChanges();

                            // submit changes to NIC table
                            //before submitting changes test for many to one relationships 
                            //i.e. where there are parent and child relationships in IRD linking to one NIC number already in table
                            var NicNum = (from TempBiz in BRContextNIC.NICs
                                          where TempBiz.NICNumber == NICTable.NICNumber
                                          select TempBiz.NICNumber).ToList();
                            if (NicNum.LastOrDefault() == null)
                            {
                                //only add new NIC data
                                BRContextNIC.NICs.Add(NICTable);
                                BRContextNIC.SaveChanges();
                                TempNic = NICTable.NICNumber;
                                ChildEstabCountNIC = 0; // reset alphabet suffix counter for new parent child relationship
                            }
                            else
                            {
                                // add a suffix to distinguish that this is a child or duplicate entry for group of establishments
                                string suffix = "abcdefghijklmnopqrstuvwxyz";
                                if (string.Equals(TempNic, NICTable.NICNumber))
                                {
                                    NICTable.NICNumber = NICTable.NICNumber + suffix[ChildEstabCountNIC];
                                    ChildEstabCountNIC += 1;// increment value to next alphabet in same parent child relationship
                                }
                                else
                                {
                                    //for those parent child relationships not in sequential order in BRMF excel file
                                    //find the number of instances and use as indice for suffix                                    
                                        if (NicNum.Count > 0)
                                        {
                                            NICTable.NICNumber = NICTable.NICNumber + suffix[NicNum.Count];
                                            // no need to increment ChildEstabCountNIC since tempnic will sill not equal next NICTable.NICNumber in excel file
                                        }                                    
                                }
                                BRContextNIC.NICs.Add(NICTable);
                                BRContextNIC.SaveChanges();
                                Ekey.NICNumber = NICTable.NICNumber;
                            }
                            // submit changes to REG table   
                            //before submitting changes test for many to one relationships 
                            //i.e. where there are parent and child relationships in IRD linking to one REG number already in table
                            var RegNum = (from TempBiz in BRContextREG.Registrars
                                          where TempBiz.RegistrarNumber == REGTable.RegistrarNumber
                                          select TempBiz.RegistrarNumber).ToList();
                            if (RegNum.LastOrDefault() == null)
                            {
                                //only add new REG data                 
                                BRContextREG.Registrars.Add(REGTable);
                                BRContextREG.SaveChanges();
                                TempReg = REGTable.RegistrarNumber;
                                ChildEstabCountREG = 0; //reset alphabet suffix counter for new parent child relationship
                            }
                            else
                            {
                                // add a suffix to distinguish that this is a child or duplicate entry for group of establishments
                                //first create a string array of alphabets in case there are many duplicates
                                string suffix = "abcdefghijklmnopqrstuvwxyz";
                                if (string.Equals(TempReg, REGTable.RegistrarNumber))
                                {
                                    REGTable.RegistrarNumber = REGTable.RegistrarNumber + suffix[ChildEstabCountREG];
                                    ChildEstabCountREG += 1; //increment value to next alphabet in current parent child relationship
                                }
                                else
                                {
                                    //for those parent child relationships not in sequential order in BRMF excel file
                                    //find the number of instances and use as indice for suffix                                   
                                        if (RegNum.Count > 0)
                                        {
                                            REGTable.RegistrarNumber = REGTable.RegistrarNumber + suffix[RegNum.Count];
                                        }                                    
                                }
                                //make surre registrar# has a max of 8 chars
                                if (REGTable.RegistrarNumber.Length > 8)
                                {
                                    REGTable.RegistrarNumber = REGTable.RegistrarNumber.Substring(0, 8);
                                }
                                BRContextREG.Registrars.Add(REGTable);
                                BRContextREG.SaveChanges();
                                Ekey.RegistrarNumber = REGTable.RegistrarNumber;
                            }
                            //Submit data to External Key
                            BRContextExtKey.ExternalKeys.Add(Ekey);
                            BRContextExtKey.SaveChanges();

                            // Submit data to Contacts Table
                            BRContextContacts.Contacts.Add(ContactTable);
                            BRContextContacts.SaveChanges();

                            recordcounter++; //keep count of the number of business records.  
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                        {
                            ERRORS = true;
                            ErrorCount = ErrorCount + 1;
                            //TODO://add more intuitive error message use: validation message etc...
                            // Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "The following error was found inserting business data into database from excel file: " + ex.Message, "Add Business Data from MS Excel file", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                            string error = "";
                            string ErrorMsg = (ex.EntityValidationErrors.Select(e => string.Join(Environment.NewLine, e.ValidationErrors.Select(v => string.Format("{0} - {1}", v.PropertyName, v.ErrorMessage))))).FirstOrDefault();

                            string bizname = (string.IsNullOrEmpty(biz.LegalName)) ? biz.OperationalName : biz.LegalName;
                            if (string.IsNullOrEmpty(bizname))
                                bizname = "MISSING";

                            error += "XLSX RECORD #: " + i + "\nTAXPAYER #: " + IRDTable.IRDNumber + "\nBusiness Name: " + bizname + "\nERROR MSSG: " + ErrorMsg + " \n";
                            error += "****************** \n";
                            ErrorLog.Add(error);
                        }
                      }
                        catch(Exception ex)
                        {
                            ERRORS = true;
                            ErrorCount = ErrorCount + 1;
                            string error ="";
                            string bizname = (string.IsNullOrEmpty(biz.LegalName)) ? biz.OperationalName : biz.LegalName;
                            if (string.IsNullOrEmpty(bizname))
                                bizname = "MISSING";
                            error += "XLSX RECORD #: " + i + "\nTAXPAYER #: " + IRDTable.IRDNumber + "\nBusiness Name: " + bizname + "\nERROR MSSG: " + ex.InnerException.InnerException.Message + " \n";
                            error += "****************** \n";
                            ErrorLog.Add(error);
                        }
                    }  
                }// end of i loop
                               
                //if no errors
                if (!ERRORS)
                {
                    RadMessageBox.Show(Environment.NewLine + recordcounter + " Records of Business Data was successfully inserted into the Business Register Database...", "Adding Businesses to BR Database", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                }
                else
                {// there were errors, so give options to see logs or save logs to .txt file
                 // RadMessageBox.SetThemeName("visualStudio2012LightTheme1");
                    DialogResult result = RadMessageBox.Show(Environment.NewLine + recordcounter + " Records FROM " + usedCellRange.RowCount +  " possible records were submitted to the Database \n" + (usedCellRange.RowCount - recordcounter) + " Errors Were encountered \n" + "Would you like to view the LOG data?" , "ERRORS adding Businesses to BR Database", MessageBoxButtons.YesNoCancel, RadMessageIcon.Info);
                    if (result == DialogResult.Yes)
                    {
                        string errorHeader = "***** " + "(UpdateBRMFMatched) Error Logs Generated: " + DateTime.Now.ToString() + " *****" + "\n";
                        errorHeader += "Please rectify data quality errors in BRMF Excel File or Consult Administrators \n";
                        errorHeader += "Number of Records Processed = " + recordcounter + "\n";
                        errorHeader += "Number of Records with ERRORS = " + ErrorCount + "\n";
                        errorHeader += "Total Number of Records in Excel File (BR Matched Records) = " + usedCellRange.RowCount  + "\n";
                        errorHeader += "****************** \n";
                        ErrorLog.Insert(0, errorHeader);
                        //if usedCellRange.RowCount too high delete excess rows from BRMF, system reading blank rows
                        //https://stackoverflow.com/questions/2329978/the-calling-thread-must-be-sta-because-many-ui-components-require-this
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                            win.LogList = ErrorLog;//populate so viewlog button can have data
                            win.BtnViewLogs.Visibility = Visibility.Visible;
                            LogWindow(ErrorLog);
                        });                       
                    }
                    else if (result == DialogResult.No)
                    {
                        //
                    }
                }
            }
            catch ( Exception ex)
            {
                RadMessageBox.Show(Environment.NewLine + "The operation terminated with the following errors: " + ex.InnerException.Message, "Populate BR database", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }                        
            //If no errors in database insert operations, the ERRORS = FALSE
            return ERRORS;
        }

        private static void LogWindow(List<string> LogList)
        {
            LogWindow LogWin = new LogWindow(LogList);
            LogWin.setTxtBox();                 
            LogWin.ShowDialog();
        }      

        public bool UpdateIRDHelper( string irdnum)
        {
            bool ok = false;
            
            return ok;
        }

        public bool UpdateBRMFUnmatched(Worksheet BRMF_Worksheet)
        {
           
            bool biztypefound = false;
            bool ERRORS = false;
            bool IsDuplicate = false;
            CellRange usedCellRange = BRMF_Worksheet.UsedCellRange;
            CellSelection HeaderSelection;
            CellSelection DataSelection;
            string HeaderName;
            int recordcounter = 0;
            int ErrorCount = 0;

            Business biz;          
            ExternalKey Ekey;

            //IRD table object for populating IRD table before inserting reference in masterkey (externalkeys) table
            IRD IRDTable;            
            //declare object to store contact details
            Contact ContactTable;        
           
            BusinessRegisterEntities BRContext;
            BusinessRegisterEntities BRContextIRD;
            BusinessRegisterEntities BRContextExtKey;
            BusinessRegisterEntities BRContextContacts;

            //to parse datetime
            string[] formats = { "MM/dd/yyyy" }; 

            List<string> ErrorLog = new List<string>();

            try
            {   
                for (int i = 1; i <= usedCellRange.RowCount; i++)
                { //start i=1 omit header
                    BRContext = new BusinessRegisterEntities();
                    BRContextIRD = new BusinessRegisterEntities();
                    BRContextExtKey = new BusinessRegisterEntities();
                    BRContextContacts = new BusinessRegisterEntities();
                    biz = new Business();
                    IRDTable = new IRD();
                    ContactTable = new Contact();
                    Ekey = new ExternalKey();

                    try
                    {
                        //check that this is not a duplicate record that is already in database
                        DataSelection = BRMF_Worksheet.Cells[i, 0];
                        string tempird = DataSelection.GetValue().Value.RawValue;
                        var bizcheck = (from TempBiz in BRContext.ExternalKeys
                                        where TempBiz.IRDNumber == tempird
                                        select TempBiz.IRDNumber).ToList().LastOrDefault();
                        if (!(string.IsNullOrEmpty(bizcheck)))
                        {
                            //the record is already in database
                            IsDuplicate = true;
                            ERRORS = true;
                            ErrorCount++;
                            // write to LOGS                           
                            if (recordcounter == 0)
                            {
                                //first record
                                recordcounter++;
                            }

                            //get IRDNumber for logs
                            IRDTable.IRDNumber = DataSelection.GetValue().Value.RawValue;
                            //get name of business for logs
                            DataSelection = BRMF_Worksheet.Cells[i, 1];
                            if (string.IsNullOrEmpty(DataSelection.GetValue().Value.RawValue))
                            {
                                DataSelection = BRMF_Worksheet.Cells[i, 2]; //use operating name
                            }

                            biz.LegalName = DataSelection.GetValue().Value.RawValue;

                            string error = "";
                            error += "XLSX RECORD #: " + i + "\nTAXPAYER #: " + IRDTable.IRDNumber + "\nBusiness Name: " + biz.LegalName + "\nERROR MSSG: DUPLICATE BUSINESS RECORD " + " \n";
                            error += "****************** \n";
                            ErrorLog.Add(error);

                        }
                        else
                        { //new record
                            IsDuplicate = false;
                            for (int j = 0; j <= usedCellRange.ColumnCount; j++)
                            {
                                HeaderSelection = BRMF_Worksheet.Cells[0, j];
                                HeaderName = HeaderSelection.GetValue().Value.RawValue;
                                DataSelection = BRMF_Worksheet.Cells[i, j];
                                biz.StatisticalNumber = CreateStatisticalNumber(BRContext);
                                ContactTable.StatisticalNumber = biz.StatisticalNumber;
                                switch (HeaderName)
                                {
                                    case "TAXPAYER #":

                                        int lenI = DataSelection.GetValue().Value.RawValue.Length;
                                        if (lenI <= 8)
                                        {
                                            IRDTable.IRDNumber = (DataSelection.GetValue().Value.RawValue).Substring(0, lenI);
                                        }
                                        else
                                        {
                                            IRDTable.IRDNumber = (DataSelection.GetValue().Value.RawValue).Substring(0, 8);
                                        }

                                        Ekey.StatisticalNumber = biz.StatisticalNumber;
                                        Ekey.IRDNumber = IRDTable.IRDNumber;
                                        break;
                                    case "LEGAL  NAME":
                                        biz.LegalName = DataSelection.GetValue().Value.RawValue;
                                        IRDTable.Name = biz.LegalName;                                        
                                        break;
                                    case "OPERATING NAME":
                                        biz.OperationalName = DataSelection.GetValue().Value.RawValue;
                                        IRDTable.TradeName = biz.OperationalName;                                        
                                        break;
                                    case "LEGAL ADDRESS":
                                        biz.LegalAddress = DataSelection.GetValue().Value.RawValue;
                                        IRDTable.MailingAddress = biz.LegalAddress;// one Address field for IRD table...
                                                                                   //since this is the matched business tab...let's keep the names consistent                                        
                                        break;
                                    case "OPERATING ADDRESS":
                                        biz.Address = DataSelection.GetValue().Value.RawValue;
                                        if (IRDTable.MailingAddress.Length == 0 || !(biz.Address.Equals(IRDTable.MailingAddress)))
                                        {
                                            IRDTable.MailingAddress = biz.Address;
                                        }
                                        //since this is the matched business tab...let's keep the names consistent                                        
                                        break;
                                    case "DISTRICT":
                                        biz.District = DataSelection.GetValue().Value.RawValue;
                                        IRDTable.MailingCity = biz.District;                                        
                                        break;
                                    case "POSTAL CODE":
                                        biz.PostalCode = DataSelection.GetValue().Value.RawValue;
                                        break;
                                    case "PNONE #":
                                        biz.PhoneNumber = DataSelection.GetValue().Value.RawValue;
                                        IRDTable.PhoneNumber = biz.PhoneNumber;                                       
                                        break;
                                    case "FAX":
                                        biz.Fax = DataSelection.GetValue().Value.RawValue;
                                        IRDTable.FaxNumber = biz.Fax;
                                        break;
                                    case "EMAIL":
                                        biz.Email = DataSelection.GetValue().Value.RawValue;
                                        IRDTable.EmailAddress = biz.Email;
                                        //since this is the matched business tab...let's keep the names consistent
                                        break;
                                    case "URL":
                                        biz.URL = DataSelection.GetValue().Value.RawValue;
                                        break;
                                    case "GPS":
                                        biz.GPS = DataSelection.GetValue().Value.RawValue;
                                        break;
                                    case "CONTACT TITLE":
                                        //TODO for contact...first insert record with stats# in business core 
                                        //then insert contact for that record                                 
                                        ContactTable.Title = DataSelection.GetValue().Value.RawValue;
                                        break;
                                    case "CONTACT NAME":
                                        ContactTable.Name = DataSelection.GetValue().Value.RawValue;
                                        break;
                                    case "CONTACT PHONE":
                                        ContactTable.PhoneNumber1 = DataSelection.GetValue().Value.RawValue;
                                        break;
                                    case "CONTACT EMAIL":
                                        ContactTable.EmailAddress1 = DataSelection.GetValue().Value.RawValue;
                                        break;
                                    case "BUSINESS STATUS":
                                        //TODO: Business status must be standardized
                                        //this must be done after data is received fom ADMIN sources...
                                        int status = 9;//not stated
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            status = Convert.ToInt32(DataSelection.GetValue().Value.RawValue);
                                        }
                                        if (status == 0 || status == 1 || status == 2)
                                        {
                                            biz.BusinessStatusCode = Convert.ToByte(status);
                                        }
                                        else
                                        {  //if is null or empty , then use not stated
                                            biz.BusinessStatusCode = Convert.ToByte(9);//not stated
                                        }
                                        break;
                                    case "BUSINESS TYPE":

                                        string biztype = DataSelection.GetValue().Value.RawValue;

                                        if (biztype.Contains("#N/A") || biztype.Contains("#") || biztype.Contains("N/A") || biztype == "" || biztype.Length == 0)
                                        {
                                            // assign the value of NOT STATED
                                            biz.BusinessTypeCode = Convert.ToByte(99);//not stated
                                            break;
                                        }
                                        //query business type table and compare string to get the code for one that matches... if not in table then insert new type...
                                        if (biztype.Length > 0 || !(string.IsNullOrEmpty(biztype)))
                                        {
                                            var BizType = BRContext.BusinessTypes;
                                            foreach (BusinessType btype in BizType)
                                            {  //convert all strings to lowercase before comparing or searching 
                                                if (btype.Label.ToLower().Contains(DataSelection.GetValue().Value.RawValue.ToLower()))
                                                {
                                                    if (btype.Label.Length == DataSelection.GetValue().Value.RawValue.Length)
                                                    {
                                                        biz.BusinessTypeCode = btype.BusinessTypeCode;
                                                        biztypefound = true;
                                                        break;
                                                    }
                                                    else if (Math.Abs(btype.Label.Length - DataSelection.GetValue().Value.RawValue.Length) < 5)
                                                    { //for cases like LLC,LLP,LLLP
                                                        biz.BusinessTypeCode = btype.BusinessTypeCode;
                                                        biztypefound = true;
                                                        break;
                                                    }
                                                }
                                                else if (btype.LongDescription.ToLower().Contains(DataSelection.GetValue().Value.RawValue.ToLower()))
                                                {
                                                    if (btype.Label.Length == DataSelection.GetValue().Value.RawValue.Length)
                                                    {
                                                        biz.BusinessTypeCode = btype.BusinessTypeCode;
                                                        biztypefound = true;
                                                        break;
                                                    }
                                                    else if (Math.Abs(btype.LongDescription.Length - DataSelection.GetValue().Value.RawValue.Length) < 5)
                                                    { //for cases like LLC,LLP,LLLP
                                                        biz.BusinessTypeCode = btype.BusinessTypeCode;
                                                        biztypefound = true;
                                                        break;
                                                    }

                                                }
                                            }
                                        }
                                        else
                                        {
                                            // assign the value of NOT STATED
                                            biz.BusinessTypeCode = Convert.ToByte(99);//not stated
                                            biztypefound = true;
                                            break;
                                        }
                                        //populate IRD table variable for legalstatus
                                        IRDTable.LegalStatus = biz.BusinessTypeCode;
                                        // else add new business type to table and send message to user informing
                                        //them that the new business type has been added
                                        //which is not consistent with those already in table...
                                        if (!biztypefound)
                                        {
                                            var lastBizTypeCodes = (from BizTypes in BRContext.BusinessTypes
                                                                    select BizTypes.BusinessTypeCode);
                                            //find the final biztypecode and increment that value for new biztypecode
                                            byte last = 0;
                                            foreach (var typecode in lastBizTypeCodes)
                                            {
                                                if (last < typecode)
                                                    last = typecode;
                                            }
                                            //add new business type code to business also...
                                            ++last; //increament from last BusinessTypeCode in table
                                            biz.BusinessTypeCode = last;
                                            // make sure to save in businesstype table before Business

                                            SqlConnection connect = new SqlConnection(GetAdoConfigurationString("BusinessRegisterEntities"));
                                            SqlCommand cmd = new SqlCommand { Connection = connect };
                                            cmd.CommandText = "Insert into [BusinessRegister].[Core].[BusinessType] (BusinessTypeCode, Label,LongDescription) values (@biztypecode,@biztypelabel,@biztypelongdesc)";
                                            cmd.Parameters.AddWithValue("@biztypecode", last);
                                            cmd.Parameters.AddWithValue("@biztypelabel", DataSelection.GetValue().Value.RawValue);
                                            cmd.Parameters.AddWithValue("@biztypelongdesc", DataSelection.GetValue().Value.RawValue);
                                            try
                                            {
                                                connect.Open();
                                                cmd.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "The following error was found inserting business data into database from excel file: " + ex.Message, "Add Business from excel file", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                                            }
                                            finally
                                            {
                                                if (connect != null)
                                                    connect.Close();
                                            }
                                        }
                                        break;
                                    case "YEAR BEGUN OPERATION":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {    //https://docs.microsoft.com/en-us/dotnet/api/system.datetime.tryparseexact?redirectedfrom=MSDN&view=netframework-4.7.2#System_DateTime_TryParseExact_System_String_System_String___System_IFormatProvider_System_Globalization_DateTimeStyles_System_DateTime__                                        
                                            string dateString = DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("MM/dd/yyyy"));                                           
                                            DateTime parsedDateTime;
                                            if (DateTime.TryParseExact(dateString, formats, new CultureInfo("en-US"),
                                                                           DateTimeStyles.None, out parsedDateTime))
                                            {
                                                biz.YearBegunOperation = parsedDateTime;
                                            }
                                            else { biz.YearBegunOperation = null; }
                                        }
                                        break;
                                    case "FISCAL PERIOD START":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            string dateString = DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("MM/dd/yyyy"));
                                            DateTime parsedDateTime;
                                            if (DateTime.TryParseExact(dateString, formats, new CultureInfo("en-US"),
                                                                           DateTimeStyles.None, out parsedDateTime))
                                            { biz.FiscalPeriodStart = parsedDateTime; }
                                            else { biz.FiscalPeriodStart = null; }
                                        }
                                        break;
                                    case "FISCAL PERIOD END":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            string dateString = DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("MM/dd/yyyy"));
                                            DateTime parsedDateTime;
                                            if (DateTime.TryParseExact(dateString, formats, new CultureInfo("en-US"),
                                                                           DateTimeStyles.None, out parsedDateTime))
                                            { biz.FiscalPeriodEnd = parsedDateTime; }
                                            else { biz.FiscalPeriodEnd = null; }
                                        }
                                        break;
                                    case "LEGAL CODE":
                                        biz.LegalCode = DataSelection.GetValue().Value.RawValue;
                                        break;
                                    case "% FOREIGN OWNED":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            biz.PercentForeignOwn = Convert.ToDouble(DataSelection.GetValue().Value.RawValue);
                                        }
                                        break;
                                    case "# OF EMPLOYEES":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            biz.NumberOfEmployees = Convert.ToInt64(DataSelection.GetValue().Value.RawValue);
                                        }
                                        break;
                                    case "# EMPLOYEES SOURCE":
                                        biz.NumberOfEmployeesSource = DataSelection.GetValue().Value.RawValue;
                                        break;
                                    case "# EMPLOYEES DATE":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            biz.NumberOfEmployeesDate = Convert.ToDateTime(DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("mm/d/yyyy")));
                                        }
                                        break;
                                    case "# SALARIED EMPLOYEES":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            biz.NumberOfSalariedEmployees = Convert.ToInt64(DataSelection.GetValue().Value.RawValue);
                                        }
                                        break;
                                    case "# SALARIED EMPLOYEES SOURCE":
                                        biz.NumberOfSalariedEmployeesSource = DataSelection.GetValue().Value.RawValue;
                                        break;
                                    case "# SALARIED EMPLOYEES DATE":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            biz.NumberOfSalariedEmployeesDate = Convert.ToDateTime(DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("mm/d/yyyy")));
                                        }
                                        break;
                                    case "SALES":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            biz.Sales = Convert.ToDouble(DataSelection.GetValue().Value.RawValue);
                                            IRDTable.ValTaxSupply = Convert.ToInt32(biz.Sales);
                                        }
                                        break;
                                    case "SALES SOURCE":
                                        biz.SalesSource = DataSelection.GetValue().Value.RawValue;
                                        break;
                                    case "SALES DATE":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            biz.SalesDate = Convert.ToDateTime(DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("mm/d/yyyy")));
                                        }
                                        break;
                                    case "REVENUE":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            biz.Revenue = Convert.ToDouble(DataSelection.GetValue().Value.RawValue);
                                        }
                                        break;
                                    case "REVENUE SOURCE":
                                        biz.RevenueSource = DataSelection.GetValue().Value.RawValue;
                                        break;
                                    case "REVENUE DATE":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            biz.RevenueDate = Convert.ToDateTime(DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("mm/d/yyyy")));
                                        }
                                        break;
                                    case "WAGES":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            biz.Wages = Convert.ToDouble(DataSelection.GetValue().Value.RawValue);
                                        }
                                        break;
                                    case "WAGES SOURCE":
                                        biz.WagesSource = DataSelection.GetValue().Value.RawValue;
                                        break;
                                    case "WAGES DATE":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            biz.WagesDate = Convert.ToDateTime(DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("mm/d/yyyy")));
                                        }
                                        break;
                                    case "ISIC":
                                        biz.ISIC = DataSelection.GetValue().Value.RawValue;
                                        IRDTable.Primary_ISIC = biz.ISIC;
                                        break;
                                    case "ISIC SOURCE":
                                        biz.ISICSource = DataSelection.GetValue().Value.RawValue;
                                        break;
                                    case "ISIC DATE":
                                        if (DataSelection.GetValue().Value.RawValue.Length > 0)
                                        {
                                            biz.ISICDate = Convert.ToDateTime(DataSelection.GetValue().Value.GetResultValueAsString(new CellValueFormat("mm/d/yyyy")));
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        string message = ex.Message;

                    }

                    float percent = ((float)i / usedCellRange.RowCount) * 100;
                    string XLSprogress = percent.ToString("0.00");                    
                    win.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        win.statusbarlabel.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkRed);
                        win.updatestatusbarlabel(XLSprogress + "% Successful ... Processing Record #:  " + i);
                        //progressbar
                        //progressbar range settings
                        if (i == 1)
                        {//initialize rage only on first iteration of loop
                            win.statusbarprogressbar.Minimum = 0;
                            win.statusbarprogressbar.Maximum = usedCellRange.RowCount;
                            win.statusbarprogressbar.Value = 0;
                            win.statusbarprogressbar.IsIndeterminate = false;
                            // Set BorderBrush  directly
                            win.statusbarprogressbar.BorderBrush = new SolidColorBrush(Colors.Red);
                            // Set BorderBrush using a Style
                            Style myStyle3 = new Style(typeof(RadProgressBar));
                            myStyle3.Setters.Add(new Setter(RadProgressBar.BorderBrushProperty, new SolidColorBrush(Colors.Red)));
                            win.statusbarprogressbar.Style = myStyle3;
                            // win.statusbarprogressbar.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkRed);
                        }
                        win.statusbarprogressbar.Value = i;
                        if (i == usedCellRange.RowCount)
                        {
                            //Create a clickable event to view logs in status bar
                           // win.statusbarlabel.Content = win.statusbarlabel.Content + " Click To View Logs"; 
                        }
                    }));


                    // populate BR database with data from matched records of spreadsheet
                    // submitting data to database
                    if (!IsDuplicate)
                    {
                        try
                        {
                            try
                            {
                                //Add ID details of last user doing updates to database...i.e.LastUserIDupdated
                                biz.LastUserIdUpdated = MainWindow.userID;
                                biz.LastUpdateDate = DateTime.Now;
                                //submitt data to Business table
                                BRContext.Businesses.Add(biz);
                                BRContext.SaveChanges();

                                // submit data to IRD table
                                BRContextIRD.IRDs.Add(IRDTable);
                                BRContextIRD.SaveChanges();

                                //Submit data to External Key
                                //Prefill VAT,NIC,REG numbers for future use and to ensure referential integrity
                                Ekey.NICNumber = "NI" + biz.StatisticalNumber.Substring(4, 5);
                                Ekey.RegistrarNumber = "RE" + biz.StatisticalNumber.Substring(4, 5);
                                Ekey.VATNumber = "VA" + biz.StatisticalNumber.Substring(4, 5);
                                //populate Nic Table for referential integrity...Nic # will be updated manually or via BRMF update
                                NIC NicBiz = new NIC();
                                NicBiz.NICNumber = Ekey.NICNumber;
                                NicBiz.Name = string.IsNullOrEmpty(biz.LegalName) ?  biz.OperationalName: biz.LegalName;
                                NicBiz.AddressLine1 = string.IsNullOrEmpty(biz.LegalAddress) ? biz.Address : biz.LegalAddress;
                                NicBiz.District = biz.District;
                                NicBiz.EmailAddress = biz.Email;
                                NicBiz.PhoneNumber = biz.PhoneNumber;
                                NicBiz.NumberOfEmployees = Convert.ToInt32(biz.NumberOfEmployees);
                                NicBiz.NatureOfBusiness = biz.ISIC;
                                BusinessRegisterEntities BRContextNIC = new BusinessRegisterEntities();
                                BRContextNIC.NICs.Add(NicBiz);
                                BRContextNIC.SaveChanges();
                                BRContextNIC.Dispose();
                                //populate and prefill Registrar...it will be updated later by user
                                Registrar RegBiz = new Registrar();
                                RegBiz.RegistrarNumber = Ekey.RegistrarNumber;
                                RegBiz.Name = string.IsNullOrEmpty(biz.LegalName) ? biz.OperationalName : biz.LegalName;
                                RegBiz.Address = string.IsNullOrEmpty(biz.LegalAddress) ? biz.Address : biz.LegalAddress;
                                BusinessRegisterEntities BRContextREG = new BusinessRegisterEntities();
                                BRContextREG.Registrars.Add(RegBiz);
                                BRContextREG.SaveChanges();
                                BRContextREG.Dispose();
                                //populate VAT and prefill... it will be updated later by user...
                                VAT VatBiz = new VAT();
                                VatBiz.VATNumber = Ekey.VATNumber;
                                //VAT table attributes Year and Month don't accept Nulls so prefill to prevent exception error
                                VatBiz.Year = "0000";
                                VatBiz.Month = "00";
                                BusinessRegisterEntities BRContextVAT = new BusinessRegisterEntities();
                                BRContextVAT.VATs.Add(VatBiz);
                                BRContextVAT.SaveChanges();
                                BRContextVAT.Dispose();

                                //submit external keys to database
                                BRContextExtKey.ExternalKeys.Add(Ekey);
                                BRContextExtKey.SaveChanges();

                                // Submit data to Contacts Table
                                BRContextContacts.Contacts.Add(ContactTable);
                                BRContextContacts.SaveChanges();

                                recordcounter++; //keep count of the number of business records.  
                            }
                            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                            {
                                ERRORS = true;
                                ErrorCount = ErrorCount + 1;
                                //TODO://add more intuitive error message use: validation message etc...
                                // Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "The following error was found inserting business data into database from excel file: " + ex.Message, "Add Business Data from MS Excel file", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                                string error = "";
                                string ErrorMsg = (ex.EntityValidationErrors.Select(e => string.Join(Environment.NewLine, e.ValidationErrors.Select(v => string.Format("{0} - {1}", v.PropertyName, v.ErrorMessage))))).FirstOrDefault();

                                string bizname = (string.IsNullOrEmpty(biz.LegalName)) ? biz.OperationalName : biz.LegalName;
                                if (string.IsNullOrEmpty(bizname))
                                    bizname = "MISSING";

                                error += "XLSX RECORD #: " + i + "\nTAXPAYER #: " + IRDTable.IRDNumber + "\nBusiness Name: " + bizname + "\nERROR MSSG: " + ErrorMsg + " \n";
                                error += "****************** \n";
                                ErrorLog.Add(error);
                            }
                        }
                        catch (Exception ex)
                        {
                            ERRORS = true;
                            ErrorCount = ErrorCount + 1;
                            string error = "";
                            string bizname = (string.IsNullOrEmpty(biz.LegalName)) ? biz.OperationalName : biz.LegalName;
                            if (string.IsNullOrEmpty(bizname))
                                bizname = "MISSING";
                            error += "XLSX RECORD #: " + i + "\nTAXPAYER #: " + IRDTable.IRDNumber + "\nBusiness Name: " + bizname + "\nERROR MSSG: " + ex.InnerException.InnerException.Message + " \n";
                            error += "****************** \n";
                            ErrorLog.Add(error);
                        }
                    }
                }// end of i loop

                //if no errors
                if (!ERRORS)
                {
                    RadMessageBox.Show(Environment.NewLine + recordcounter + " Records of Business Data was successfully inserted into the Business Register Database...", "Adding Businesses to BR Database", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                }
                else
                {// there were errors, so give options to see logs or save logs to .txt file
                 // RadMessageBox.SetThemeName("visualStudio2012LightTheme1");
                    DialogResult result = RadMessageBox.Show(Environment.NewLine + recordcounter + " Records FROM " + usedCellRange.RowCount + " possible records were submitted to the Database \n" + (usedCellRange.RowCount - recordcounter) + " Errors Were encountered \n" + "Would you like to view the LOG data?", "ERRORS adding Businesses to BR Database", MessageBoxButtons.YesNoCancel, RadMessageIcon.Info);
                    if (result == DialogResult.Yes)
                    {
                        string errorHeader = "***** " + "(UpdateBRMFUnmatched) Error Logs Generated: " + DateTime.Now.ToString() + " *****" + "\n";
                        errorHeader += "Please rectify data quality errors in BRMF Excel File or Consult Administrators \n";
                        errorHeader += "Number of Records Processed = " + recordcounter + "\n";
                        errorHeader += "Number of Records with ERRORS = " + ErrorCount + "\n";
                        errorHeader += "Total Number of Records in Excel File (BR Matched Records) = " + usedCellRange.RowCount + "\n";
                        errorHeader += "****************** \n";
                        ErrorLog.Insert(0, errorHeader);
                        //if usedCellRange.RowCount too high delete excess rows from BRMF, system reading blank rows
                        //https://stackoverflow.com/questions/2329978/the-calling-thread-must-be-sta-because-many-ui-components-require-this
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                            win.LogList = ErrorLog;//populate so viewlog button can have data
                            win.BtnViewLogs.Visibility = Visibility.Visible;
                            LogWindow(ErrorLog);
                        });
                    }
                    else if (result == DialogResult.No)
                    {
                        //
                    }
                }               
            }
            catch(Exception ex)
            {
                RadMessageBox.Show(Environment.NewLine + "The operation terminated with the following errors: " + ex.InnerException.Message, "Populate BR database", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }              
            return ERRORS;
        }


        public bool UpdateIRD(Worksheet BRMF_Worksheet)
        {
            bool ok = false;

            
            CellRange usedCellRange = BRMF_Worksheet.UsedCellRange;
            CellSelection HeaderSelection;
            CellSelection DataSelection;
            string HeaderName;

            Business biz;
            BusinessRegisterEntities BRContext;

            for (int i = 1; i <= usedCellRange.RowCount; i++)
            {//start i=1 omit header

                //check record to see if it is a new record and it is not in business table
                //records used in the IRD tab is used for update and not por inilization of database
                //from matched and unmatched business records


                //if it is a new business record, the copy entire row into worksheet and use 
                //UpdateBRMFUnmatched() method

                for (int j = 0; j <= usedCellRange.ColumnCount; j++)
                {
                    HeaderSelection = BRMF_Worksheet.Cells[0, j];
                    HeaderName = HeaderSelection.GetValue().Value.RawValue;
                    DataSelection = BRMF_Worksheet.Cells[i, j];
                    switch (HeaderName)
                    {
                        case "TAXPAYER #":

                        break;
                    }
                }

            }
            return ok;

        }
        public bool UpdateVAT(Worksheet BRMF_Worksheet, BusinessRegisterEntities BRContext)
        {
            bool ok = false;

            return ok;

        }
        public bool UpdateNIC(Worksheet BRMF_Worksheet, BusinessRegisterEntities BRContext)
        {
            bool ok = false;

            //use LINKWS in Constructor to find corresponding IRD key match to populate External Keys table...

            return ok;

        }



    }
}
