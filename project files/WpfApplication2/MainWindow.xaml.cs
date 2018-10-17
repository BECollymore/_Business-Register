using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Telerik.WinControls.UI;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Charting;
using Telerik.Windows.Controls.Data.DataForm;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace StatsBR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    /// 
    //public static class ExtensionMethods
    //{

    //    private static Action EmptyDelegate = delegate () { };


    //    public static void Refresh(this UIElement uiElement)
    //    {
    //        uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
    //    }
    //}

    public partial class MainWindow : Window 
    {
        public static string CONNECTIONSTRING; // use this to update the the App.config connection string        
        private bool tableNamesDisplayed;
        private DBController dBControl;
        private SelectedTable currentTable;
        private bool isSearching;
        
        private bool isShowTableButtonClicked;       
        private Telerik.Windows.Data.FilterDescriptor datagridFilter;
        private Workbook BRWorkBook;
       

        //to store user's login data for populating update user info when making changes to database...tracking also
        public static string userLogin;
        public static string userPassword;
        public static int userID;
        public static string UserName;

        //store key identifier for current selected row in datagridview
        private static string currentstatsID;

        private BusinessRegisterEntities BRDataContext = new BusinessRegisterEntities();
        
        private Login varLogin = new Login();
        private int chartquery;
        private int charttype;

        private String ErrorMessage;
        private bool IsEnteringTab = true;

        private DBController dbcontroller;
        private Worksheet ws;
        private string worksheetname;

        public List<string> LogList;

        public MainWindow() 
        {

            try
            {


                InitializeComponent();
                // TelerikRadControls trc = new TelerikRadControls();
                // this.radGridView1.ItemsSource = trc.GetBusinessData();//changed to following for cleaner data results

                //activate login form
                // varLogin.frmLogin.ShowDialog();

                dBControl = new DBController();
                dBControl.ConnectionString = dBControl.GetAdoConfigurationString("BusinessRegisterEntities");
                if (!dBControl.IsConnected)
                {
                    dBControl.ConnectToDB();
                }

                if (dBControl.IsConnected)
                {

                    // radGridView1.ItemsSource = dBControl.GetTableData("Core.Business").DefaultView;
                    Loaded += new RoutedEventHandler(MainWindow_Loaded);

                }
            }
            catch(Exception Ex)
            {
                ErrorMessage = Ex.Message;
            }
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //populate property grid...
            //this.DataContext = (new TelerikRadControls()).GetBusinessData();            

            // filter values coming from look up tables i.e. business type etc..
            // if filter is not done datacontext attached to grid gives garbage...

            /* DataLoadOptions options = new DataLoadOptions();
             options.LoadWith<BusinessType>( detail => detail.Label);
             options.LoadWith<BusinessStatus>(detail => detail.Label);
             options.LoadWith<Contact>(detail => detail.Name);
             options.LoadWith<Sector>(detail => detail.Label);
             options.LoadWith<LOGIN>(detail => detail.UserName);
             options.LoadWith<BusinessLog>(detail => detail.StatisticalNumber);
             BRDataContext.LoadOptions = options; */

            //populate main gridview with formated table for greater control and consistency checks...                                          

            /*     var businessRecords = (from Biz in BRDataContext.Businesses
                                                        join BizType in BRDataContext.BusinessTypes on Biz.BusinessTypeCode equals BizType.BusinessTypeCode
                                                        join BizStatus in BRDataContext.BusinessStatus on Biz.BusinessStatusCode equals BizStatus.BusinessStatusCode
                                                        from BizContact in BRDataContext.Contacts //.DefaultIfEmpty(new Contact { Name = String.Empty, StatisticalNumber = "0" })
                                                        from BizSector in BRDataContext.Sectors// Left joins since relations muy be void...i.e there may be business records with no sectors 
                                                        from BizLogin in BRDataContext.LOGINs// Left join
                                        select new
                                        {
                                            Biz.StatisticalNumber,
                                            Biz.OperationalName,
                                            Biz.LegalName,
                                            Business_Type = BizType.Label,
                                            Business_Status = BizStatus.Label,
                                            Biz.Address,
                                            Biz.District,
                                            Biz.PhoneNumber,
                                            Contact_Person = BizContact.StatisticalNumber != Biz.StatisticalNumber ? "No contact available" : BizContact.Name,
                                            Biz.Revenue,
                                            Biz.RevenueSource,
                                            Biz.RevenueDate,
                                            Biz.NumberOfEmployees,
                                            Biz.NumberOfEmployeesSource,
                                            Biz.NumberOfEmployeesDate,
                                            Biz.NumberOfSalariedEmployees,
                                            Biz.NumberOfSalariedEmployeesSource,
                                            Biz.NumberOfSalariedEmployeesDate,
                                            Biz.Sales,
                                            Biz.SalesSource,
                                            Biz.SalesDate,
                                            Biz.Wages,
                                            Biz.WagesSource,
                                            Biz.WagesDate,
                                            Biz.ISIC,
                                            Biz.ISICSource,
                                            Biz.ISICDate,
                                            SectorName = BizSector.SectorCode != Biz.StatisticalNumber ? "No Sector available" : BizSector.Label,
                                            Last_Update_By = BizLogin.LoginName,
                                            Biz.LastUpdateDate
                                        });    // all authorized users should have a login so last user update must have a value in business records...no need to to use conditional statement


              */




            // Bind RadGridView          
            // radGridView1.ItemsSource = BRDataContext.Businesses; //.Take<Business>(3) //to filter amount of records returned....for long loading periods if data is big
            //TO ALLOW FUNCTIONALITY BETWEEN DATA PAGER AND RAD GRIDVIEW
            //BIND DATA TO DATA PAGER INSTEAD AND BIND GRIDVIEW TO DATAPAGER IN XAML CODE...
            //THIS EXPOSES THE QueryableCollectionView WHICH IS THE CLASS FOR ENABLING PAGING
            if (BRDataContext.Businesses != null)
            { 
               xRadDataPager.Source = BRDataContext.Businesses;
            }

            //ALTERNATE METHOD IS TO DO THE FOLLOW ...THEN REMOVE THE BINDING IN XAML VIEW...
            /*
            var pagedSource = new Telerik.Windows.Data.QueryableCollectionView(BRDataContext.Businesses);
            this.xRadDataPager.Source = pagedSource;
            this.radGridView1.ItemsSource = pagedSource;
            */

            //this code will load the detail view button in grid...
            //this will enable users to see a detailed view of selected records or business
            DetailsViewButton detailviewbtn = new DetailsViewButton();
            detailviewbtn.Header = "View Details";
            //detailviewbtn.Btn.Click += new RoutedEventHandler(btn_Click);
            // radGridView1.Columns.Add(detailviewbtn);
            radGridView1.Columns.Insert(0, detailviewbtn);


            //To remove undesired columns from gridview
            //radGridView1.MasterTableView.GetColumn("ProductName").Display = false; 
            //radGridView1.Columns.Remove(radGridView1.Columns[9]);
            //radGridView1.Columns.RemoveAt((radGridView1.Columns.Count)-1);
            //radGridView1.Columns.Remove(radGridView1.Columns["FirstColumn"]);

            //set static gridvie variable to the current one

            currentTable.Schema = "Core";
            currentTable.TableName = "Business";            

            BRWorkBook = new Workbook();
            LogList = new List<string>();//instatiate Log List...it will be polulated when data is read from BRMF
               

        }

        public void updatestatusbarlabel( string text)
        {            
            statusbarlabel.Content = text;
        }

       /* internal Telerik.Windows.Controls.RadGridView GetGridView()
        {
            return this.radGridView1;
        }
        */

        public static string CurrentStatsID
        {
            get
            {
                return currentstatsID;
            }

            set
            {
                currentstatsID = value;
            }
        }


        private void RadPaneGroup_SelectionChanged_1(object sender, Telerik.Windows.Controls.RadSelectionChangedEventArgs e)
        {

         
        }

        private void RadRibbonView_SelectionChanged_1(object sender, Telerik.Windows.Controls.RadSelectionChangedEventArgs e)
        {

        }

        private void RadMenuItem_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TestDrive.FrmTextFile frmTxtfile = new TestDrive.FrmTextFile();
            frmTxtfile.ShowDialog();
        }

        //testing RadChart
        public class BusinessSales
        {
            public BusinessSales(string sales, int year, string businessName)
            {
                this.Sales = sales;
                this.Year = year;
                this.BusinessName = businessName;
            }
            public string Sales
            {
                get;
                set;
            }
            public int Year
            {
                get;
                set;
            }
            public string BusinessName
            {
                get;
                set;
            }
        }
        

        private void LoadChart(int querytype, int charttype)
        {
            //http://docs.telerik.com/devtools/wpf/controls/radchart/features/chart-types/2d-charts
            //This chart type displays a set of data points connected by a line. A common use for the line chart is to show trends over a period of time.


            chartquery = querytype;// store querytype for use in togglegraph method...
            this.charttype = charttype;
            SeriesMapping seriesMapping = new SeriesMapping();
            
            if (charttype == 1)
            {
                seriesMapping.SeriesDefinition = new BarSeriesDefinition();
            }
            else if (charttype == 2)
            {
                seriesMapping.SeriesDefinition = new PieSeriesDefinition();
            }
            else if (charttype == 3)
            {
                seriesMapping.SeriesDefinition = new SplineSeriesDefinition();
            }
            switch (querytype)
            {
                case 1:
                    // Top ten businesses by sales value
                    seriesMapping.LegendLabel = "Top Business Sales";
                    seriesMapping.ItemMappings.Add(new ItemMapping("Sales", DataPointMember.YValue));
                    if (charttype != 2)
                    {
                        seriesMapping.ItemMappings.Add(new ItemMapping("BusinessName", DataPointMember.XCategory));
                        
                    }
                    else
                    {
                        //pie chart                       
                        seriesMapping.ItemMappings.Add(new ItemMapping("BusinessName", DataPointMember.LegendLabel));
                        this.radchart1.DefaultView.ChartLegend.Header = "Top Business Sales";
                        
                    }
                    this.radchart1.SeriesMappings.Clear();
                    this.radchart1.SeriesMappings.Add(seriesMapping);
                    this.radchart1.DefaultView.ChartArea.AxisX.LabelRotationAngle = 45;
                    this.radchart1.ItemsSource = this.TopTenSales();

                break;
                case 2:
                    // top 10 employers
                    seriesMapping.LegendLabel = "Top 10 Employers";
                    seriesMapping.ItemMappings.Add(new ItemMapping("NumberOfEmployees", DataPointMember.YValue));
                    if (charttype != 2)
                    {
                        seriesMapping.ItemMappings.Add(new ItemMapping("OperationalName", DataPointMember.XCategory));
                        
                    }
                    else
                    {
                        //pie chart
                        seriesMapping.ItemMappings.Add(new ItemMapping("OperationalName", DataPointMember.LegendLabel));
                        this.radchart1.DefaultView.ChartLegend.Header = "Top 10 Employers";
                    }
                    this.radchart1.SeriesMappings.Clear();
                    this.radchart1.SeriesMappings.Add(seriesMapping);
                    this.radchart1.DefaultView.ChartArea.AxisX.LabelRotationAngle = 45;
                    this.radchart1.ItemsSource = this.Top10Employer();

                    break;
                case 3:
                    // top 10 revenue
                    seriesMapping.LegendLabel = "Top 10 Revenue";
                    seriesMapping.ItemMappings.Add(new ItemMapping("Revenue", DataPointMember.YValue));
                    if (charttype != 2)
                    {
                        seriesMapping.ItemMappings.Add(new ItemMapping("OperationalName", DataPointMember.XCategory));
                        
                    }
                    else
                    {
                        //pie chart
                        seriesMapping.ItemMappings.Add(new ItemMapping("OperationalName", DataPointMember.LegendLabel));                        
                        this.radchart1.DefaultView.ChartLegend.Header = "Top 10 Revenue";
                    }

                    this.radchart1.SeriesMappings.Clear();
                    this.radchart1.SeriesMappings.Add(seriesMapping);
                    this.radchart1.DefaultView.ChartArea.AxisX.LabelRotationAngle = 45;
                    this.radchart1.ItemsSource = this.Top10Revenue();
                    break;

            }
           
            
        }

        public List<BusinessSales> TopTenSales()
        {

            // for other useful LINQ and Lamda solutions see: http://stackoverflow.com/questions/4872946/linq-query-to-select-top-five
            //see also: http://stackoverflow.com/questions/8603039/linq-query-to-select-top-records
            /* var list = BRDataContext.Businesses
                        .Where(t => t.Sales >18000) // constrain to businesses over VAT threshold
                        .OrderBy(t => t.Sales)
                        .Take(10);
                        */
           
            List<BusinessSales> BizSales = new List<BusinessSales>();
            try
            {
                var temp = BRDataContext.Businesses.OrderByDescending(i => i.Sales).ToList().Take(10);
                int year = 0;
                foreach (var item in temp)
                {
                   if(string.IsNullOrEmpty(item.SalesDate.ToString()))
                    {
                        year = DateTime.Now.Year;
                    }
                   else
                    {
                        year = Convert.ToDateTime(item.SalesDate).Year;
                    }

                    if(item.OperationalName.Length > 10)
                    {
                        //truncate the business name shown on axes labels to maximize graph display
                        item.OperationalName = item.OperationalName.Substring(0, 10);
                    }                 
                    
                    BizSales.Add(new BusinessSales(item.Sales.ToString(), year, item.OperationalName));
                }
            }
            catch(Exception e)
            {
                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "There was an error accessing the Top 10 sales data...The system provided the following error message: "+ e.Message + Environment.NewLine, "Top 10 sales", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }

            //return Top10Biz;
            return BizSales;

        }  
        public List<Business> Top10Employer()
        {
            List<Business> Top10Biz = new List<Business>();
            try
            {
                var temp = BRDataContext.Businesses.OrderByDescending(i => i.NumberOfEmployees).ToList().Take(10);
                
                foreach (var item in temp)
                {
                    //truncate the business name shown on axes labels to maximize graph display
                    if (item.OperationalName.Length > 10)
                    {
                        item.OperationalName = item.OperationalName.Substring(0, 10);
                    }
                    Top10Biz.Add(item);
                   
                }
            }
            catch (Exception e)
            {
                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "There was an error accessing the Top 10 Employer data...The system provided the following error message: " + e.Message + Environment.NewLine, "Top 10 sales", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }

            //return Top10Biz;
            return Top10Biz;
        }

        public List<Business> Top10Revenue()
        {
            List<Business> Top10Rev = new List<Business>();
            try
            {
                var temp = BRDataContext.Businesses.OrderByDescending(i => i.Revenue).ToList().Take(10);

                foreach (var item in temp)
                {
                    //truncate the business name shown on axes labels to maximize graph display
                    if (item.OperationalName.Length > 10)
                    {
                        item.OperationalName = item.OperationalName.Substring(0, 10);
                    }
                    Top10Rev.Add(item);

                }
            }
            catch (Exception e)
            {
                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "There was an error accessing the Top 10 Revenue data...The system provided the following error message: " + e.Message + Environment.NewLine, "Top 10 sales", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }

            //return Top10Biz;
            return Top10Rev;
        }


        private void ConfigureDB_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            FrmConfigureDBConnect frmConfigDB = new FrmConfigureDBConnect();
            frmConfigDB.ShowDialog();
        }
        private void PopulateTablesComboBox()
        {
            List<string> tableNames = dBControl.ShowTableNames();
            if (tableNames.Count > 0) // -1 for the fixed default text
            {
                foreach (string item in tableNames)
                {
                    cbxTables.Items.Add(item);
                    tableNamesDisplayed = true;
                }
            }

        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            //as form gains focus populate Tablenames combobox from database
            //checks to make sure that user saves the DB settings before retrieving table names 
            //has already been done in ShowTableNames method.


            if (!tableNamesDisplayed)
            {
                if (!cbxTables.Items.IsEmpty)
                {
                    cbxTables.Items.Clear();     //clear data the populate       
                    cbxTables.Text = string.Empty;
                }
                PopulateTablesComboBox();
            }
        }

        private void btnshowtable_Click(object sender, RoutedEventArgs e)
        {
            //first make sure that object is set...i.e. user has pressed combobox before pressing view table button.
            if (cbxTables.IsInitialized)
            { 
                //if(cbxTables.SelectedItem.)
                if(cbxTables.SelectedItem.ToString().Contains("Core.Business"))
                {
                    //for the main BR table i.e Core.Business populate gridview with data from entity framework context
                    //for all other tables make query to database since there will be no editing in gridview for these tables.

                    radGridView1.ItemsSource = BRDataContext.Businesses;
                    currentTable.TableName = "Business";

                    //TODO: this code can be optimized by returning data for tables from entity framework dataset and filtering columns in grid view accordingly
                    //this will result in more customization of code via switch statements but faster since no additional database access is needed
                }
                else
                {
                    string [] split = cbxTables.SelectedItem.ToString().Split(new char[] { '.' }); // split string to separate table name and schema

                    if (cbxTables.SelectedIndex != -1)
                    {
                        //combobox has schema specifier before table name...eg: Core.Business                
                        currentTable.Schema = split[0]; 
                        currentTable.TableName  = split[1];
                        isShowTableButtonClicked = true;
                        radGridView1.ItemsSource = dBControl.GetTableData(cbxTables.SelectedItem.ToString()).DefaultView;
               
                
                    }
                }
             }

        }
       /* private string GetSchema(string tablename)
        {
            string schema =""; 
            foreach (SelectedTable names in CurrentTableList)
            {
                if (names.TableName == tablename)
                    schema = names.Schema;
            }
            return schema;

        }*/

        private void PropertyGrid1_AutoGeneratingPropertyDefinition(object sender, Telerik.Windows.Controls.Data.PropertyGrid.AutoGeneratingPropertyDefinitionEventArgs e)
        {
            //TODO: find a way to switch or locate definitions and give appropriate descriptions..
            //this can be retrieved from DB table...search list of column names for selected table and use to map description column which has description


            /*if(e.PropertyDefinition.DisplayName == "Background")
              {
                 e.PropertyDefinition.Description = "This property displays the background property of the RadButton";       
              }*/
        }

        private void ExportTable_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            string extension = "xls";
            SaveFileDialog dialog = new SaveFileDialog()
           {
               DefaultExt = extension,
               Filter = String.Format("{1} files (*.{0})|*.{0}|All files(*.*)|*.*", extension, "Excel"),
               FilterIndex = 1
           };

            if (dialog.ShowDialog().ToString() == "OK")
            {
                using (Stream stream = dialog.OpenFile())
                {
                    radGridView1.Export(stream, new GridViewExportOptions()
                     {
                         Format = ExportFormat.ExcelML,
                         ShowColumnHeaders = true,
                         ShowColumnFooters = true,
                         ShowGroupFooters = false,

                     });
                }
            }
        }

        private void RadDataForm1_EditEnded(object sender, EditEndedEventArgs e)
        {
                        
            bool ok = false;
            try
            {
               BRDataContext.SaveChanges();
               ok = true;
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message);
            }

            if (ok)
            {
                // Telerik.WinControls.RadMessageBox.SetThemeName("Breeze");
                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "The Data Edit operation was successfully completed...", "Edit Business data", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }
        }

        private void tbxSearchTable_GotFocus(object sender, RoutedEventArgs e)
        {
            //tbxSearchTable.Clear();
            //TODO: implement auto suggestion based on previous search texts
        }

        private void btnSearchTable_Click(object sender, RoutedEventArgs e)
        {
            
            bool ok = false;

            //reset the datagrid filter if active
            if (datagridFilter != null)
            {
                if (datagridFilter.IsActive)
                {
                    datagridFilter.Value = Telerik.Windows.Data.FilterDescriptor.UnsetValue;
                }
            }
            
           if(!(currentTable.TableName == "Business"))
           {
             Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "This search option is only for the main BR table..."+ Environment.NewLine  + " For other Tables like " + currentTable.TableName + ", Please use the search filters from the DataGrid: ", "Search Business data", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
           }else
             {

                try
                {
                    // this code is modified to filter gridview results instead of making separate database calls, adding overheads...                   
           
                    isSearching = true;

                    if (tbxSearchTable.Text == "---Enter Search Value---")
                    {   
                       if ( ! btnSearchTable.Content.ToString().Contains("Reset"))
                        // user must provide a search value
                         Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Please provide a valid search value for the search phrase", "search Business data", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                    }
                    else
                    {

                        datagridFilter = GridViewSearchFilters(cbxSearchOption.Text, tbxSearchTable.Text);
                        this.radGridView1.FilterDescriptors.Add(datagridFilter);
                        ok = true;
                    }
                }
                catch(Exception ex)
                {
                  ok = false;
                 Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "The search operation terminated with errors: " + ex.Message, "Search Business data", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                }

                if (ok)
                {
                    Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "The search operation was successfully completed..." , "Search Business data", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                }

                tbxSearchTable.Text = "---Enter Search Value---"; // reset default search text
                btnSearchTable.Content = "Reset";
                btnSearchTable.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                // disable filter when not searching "business" table 
            }
            

        }
        private void RadGridView1CustomFiltering(object sender, GridViewCustomFilteringEventArgs e)
         {
              //e.Visible = (decimal)e.Row.Cells["UnitPrice"].Value > 30;
         }

        private void cbxSearchOption_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
           
        }

        private void radGridView1_Loaded(object sender, RoutedEventArgs e)
        {
         
        }

        private void radGridView1_DataLoaded(object sender, EventArgs e)
        {
            try
            {
                // the following code will filter the data shown in radgridview to hide undesirable columns with entity framework relational errors
                if (!isSearching && !isShowTableButtonClicked)
                {
                    // if index already in last column ...no need to filter columns being displayed

                    if (radGridView1.Columns.Count > 0)
                    {
                        if (radGridView1.Columns[radGridView1.Columns.Count - 1].UniqueName != "LastUpdateDate")
                        {
                            // endition was initialized this way so that if first "for loop" fails
                            //the second will not run and won't produce an outofbounds array error...
                            int endcondition = radGridView1.Columns.Count;

                            // the following code will locate the LastUpdateDate column 
                            //signalling the end of columns to be filtered 
                            //using the dichotomous serach(busqueda binaria) algorithm 
                            for (int i = (radGridView1.Columns.Count - 1); i >= (radGridView1.Columns.Count) / 2; i--)
                            {
                                if (radGridView1.Columns[i].UniqueName == "LastUpdateDate")
                                {
                                    endcondition = i;  //end iteration if Business type column is reached
                                    break;
                                }
                            }

                            for (int i = (radGridView1.Columns.Count - 1); i >= endcondition; i--)
                            {
                                if (radGridView1.Columns[i].UniqueName != null)
                                {
                                    if (radGridView1.Columns[i].UniqueName == "LastUpdateDate")
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        radGridView1.Columns.Remove(radGridView1.Columns[i]);
                                        // radGridView1.Columns.Remove(radGridView1.Columns["Sector"]);
                                        // radGridView1.Columns.RemoveAt((radGridView1.Columns.Count)-i);// can us this but increment i starting from 2 since last item at index 38 is some base field debug to examine...
                                    }
                                }
                            }
                        }
                    }
                }

                //this code will load the detail view button in grid...
                //this will enable users to see a detailed view of selected records or business

                /* GridDeleteButton detailviewbtn = new GridDeleteButton();
                 detailviewbtn.Header = "View Details";
                 radGridView1.Columns.Add(detailviewbtn);  causes button to repeat once at end or final column i.e. first and last column has button...but repeats when searching*/


                
            }
            catch (Exception Ex)
            {
                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "The search operation terminated with errors: " + Ex.Message, " Telerik RadDataGridView error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }

        }

        private Telerik.Windows.Data.FilterDescriptor GridViewSearchFilters(string searchcriterior, string searchvalue)
        {
            Telerik.Windows.Data.FilterDescriptor gridSearchFilter = new Telerik.Windows.Data.FilterDescriptor();

              switch(cbxSearchOption.Text)
                    {
                        case "StatisticalNumber":
                            {
                                gridSearchFilter = new Telerik.Windows.Data.FilterDescriptor(searchcriterior, Telerik.Windows.Data.FilterOperator.Contains, searchvalue );
                            }
                            break;  
                        case "LegalName":
                            { 
                                gridSearchFilter = new Telerik.Windows.Data.FilterDescriptor(searchcriterior, Telerik.Windows.Data.FilterOperator.Contains, searchvalue );
                            }
                         
                            break;
                      case "OperationalName":
                            {
                              gridSearchFilter = new Telerik.Windows.Data.FilterDescriptor(searchcriterior, Telerik.Windows.Data.FilterOperator.Contains, searchvalue );
                            }
                            break;
                        case "District":
                            {
                              gridSearchFilter = new Telerik.Windows.Data.FilterDescriptor(searchcriterior, Telerik.Windows.Data.FilterOperator.Contains, searchvalue );
                            }
                            break;
                       case "PhoneNumber":
                            {
                              gridSearchFilter = new Telerik.Windows.Data.FilterDescriptor(searchcriterior, Telerik.Windows.Data.FilterOperator.Contains, searchvalue );
                            }
                            break;
                       case "Revenue":
                            {
                              gridSearchFilter = new Telerik.Windows.Data.FilterDescriptor(searchcriterior, Telerik.Windows.Data.FilterOperator.IsGreaterThanOrEqualTo, searchvalue );
                            }
                            break;
                       case "NumberOfEmployees":
                            {
                              gridSearchFilter = new Telerik.Windows.Data.FilterDescriptor(searchcriterior, Telerik.Windows.Data.FilterOperator.IsGreaterThanOrEqualTo, searchvalue );
                            }
                            break;
                       case "Sales":
                            {
                              gridSearchFilter = new Telerik.Windows.Data.FilterDescriptor(searchcriterior, Telerik.Windows.Data.FilterOperator.IsGreaterThanOrEqualTo, searchvalue );
                            }
                            break;
                        case "ISIC":
                            {
                               gridSearchFilter = new Telerik.Windows.Data.FilterDescriptor(searchcriterior, Telerik.Windows.Data.FilterOperator.Contains, searchvalue );
                            }
                            break;
                    } 
                   

                    return gridSearchFilter;
                    
        }

        private void RadDataForm1_Loaded(object sender, RoutedEventArgs e)
        { 
        
        }

        private void RadDataForm1_AutoGeneratingField(object sender, AutoGeneratingFieldEventArgs e)
        {
            // the following code will hide the undesirable data in RadDataForm

            if (!isShowTableButtonClicked)
            {
                //var biz = this.RadDataForm1.CurrentItem as Business;

                if (e.PropertyName == "Sector")
                {
                    e.DataField.Visibility = Visibility.Collapsed;
                    
                }
                if (e.PropertyName == "LOGIN")
                {
                    e.DataField.Visibility = Visibility.Collapsed;
                }
                if (e.PropertyName == "OptionalBusinessAttribute")
                {
                    e.DataField.Visibility = Visibility.Collapsed;
                }
                if (e.PropertyName == "StatisticalStructure")
                {
                    e.DataField.Visibility = Visibility.Collapsed;
                }
                if (e.PropertyName == "ExternalKey")
                {
                    e.DataField.Visibility = Visibility.Collapsed;
                }
                if (e.PropertyName == "Contact")
                {
                    e.DataField.Visibility = Visibility.Collapsed;
                }
                if (e.PropertyName == "BusinessStatu")
                {
                    e.DataField.Visibility = Visibility.Collapsed;
                }
                if (e.PropertyName == "BusinessType")
                {
                    e.DataField.Visibility = Visibility.Collapsed;
                }
                if (e.PropertyName == "BusinessLogs")
                {
                    e.DataField.Visibility = Visibility.Collapsed;
                }

                if (e.PropertyName == "StatisticalNumber")
                {
                    e.DataField.Label = "Statistical Number";
                    //e.DataField.IsEnabled = false; // due to relational dependencies users should not modify statistical number
                    e.DataField.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                }
                else
                {
                    e.DataField.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Teal);
                }
            }
        }

        private void radGridView1_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {

        }

      

        private void RadButton_Click(object sender, RoutedEventArgs e)
        {

            if (!(cbxBusinessType.SelectedIndex >= 0))
            {
                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Please enter a valid value for Business Type variable ", "Business Type is required!", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                cbxBusinessType.Focus();
                return;
            }
            if (!(cbxBusinessStatus.SelectedIndex >= 0))
            {
                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Please enter a valid value for Business Status variable ", "Business Status is required!", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                cbxBusinessStatus.Focus();
                return;
            }
            // TODO: continue for all required fields

            try
            {


                using (BusinessRegisterEntities BRcontext = new BusinessRegisterEntities())
                {
                    
                   

                    Business biz = new Business();
                    biz.StatisticalNumber = radMaskedStatsNum.Text;
                    biz.LegalName = radMaskedLegalName.Text;
                    biz.OperationalName = radMaskedOperationalName.Text;                   
                    biz.Address = radMaskedAddress.Text;                   
                    biz.District = cbxDistrict.Text;
                    biz.PhoneNumber = radMaskedTeleNum.Text;

                    //---------------------
                    //Business type
                    //Find business code that corresponds to label that user selects in combobox
                    //var BizTypeCode = from Biz in BRDataContext.BusinessTypes
                    //                  where Biz.Label == cbxBusinessType.SelectedValue.ToString()
                    //                  select Biz.BusinessTypeCode;

                    // the above code gives a iquerable<byte> to byte cast error ....to avoid see:
                    //use the SingleOrDefault selector for linq, so you can get only a single object and not an IQueryable. 
                    //Also in SingleOrDefault<> you can specify the type so you can avoid casting
                    //http://stackoverflow.com/questions/24685832/how-to-convert-type-system-linq-iqueryablebyte-to-byte

                    var BizType = BRDataContext.BusinessTypes.FirstOrDefault(i => i.Label== cbxBusinessType.SelectedValue.ToString());
                    if(BizType !=null)
                    {
                        biz.BusinessTypeCode = BizType.BusinessTypeCode;
                    }                    

                    //Business status
                    var BizStatus = BRDataContext.BusinessStatus.FirstOrDefault(i => i.Label == cbxBusinessStatus.SelectedValue.ToString());
                    if(BizStatus !=null)
                    {
                        biz.BusinessStatusCode = BizStatus.BusinessStatusCode;
                    }
                    
                    //------------------


                    biz.Revenue = Convert.ToSingle(radMaskedRevenue.Value);
                    biz.RevenueSource = cbxRevenueSRC.Text;// this should be a look up and needs additional code
                    biz.RevenueDate = dateRevenue.SelectedDate;
                    biz.NumberOfEmployees = Convert.ToInt64(radMaskedNumEmploy.Value);
                    biz.NumberOfEmployeesSource = cbxEmployeesSRC.Text; //should  be lookup table
                    biz.NumberOfEmployeesDate = dateEmployees.SelectedDate;
                    biz.NumberOfSalariedEmployees = Convert.ToInt64(radMaskedSalEmploy.Value);
                    biz.NumberOfSalariedEmployeesSource = cbxSalariedEmployeesSRC.Text; // should be lookup
                    biz.NumberOfSalariedEmployeesDate = dateSalariedEmployees.SelectedDate;
                    biz.Sales = Convert.ToSingle(radMaskedSales.Value);
                    biz.SalesSource = cbxSalesSRC.Text;
                    biz.SalesDate = dateSales.SelectedDate;
                    biz.Wages = Convert.ToSingle(radMaskedWages.Value);
                    biz.WagesSource = cbxWagesSRC.Text;
                    biz.WagesDate = dateWages.SelectedDate;
                    biz.ISIC = cbxISIC.Text.Substring(0, 5);
                    biz.ISICSource = cbxISICSRC.Text;
                    biz.ISICDate = dateISIC.SelectedDate;

                    //User should already have login before entering system...this value should be stored globally and used here:
                    biz.LastUserIdUpdated = 1; // TODO remove hard code... this value will come from LOGIN interface
                    biz.LastUpdateDate = DateTime.Now;

                    //--Business sector --------------
                    //biz.SectorCode = (cbxSector.SelectedValue.ToString()).Substring(0, 5);// ADDED sector...had none in log table nor in add business interface ...so this stays blank for now...

                    var bizSector = BRDataContext.Sectors.FirstOrDefault(i => i.Label == cbxSector.SelectedValue.ToString());
                    if(bizSector !=null)
                    {
                        biz.SectorCode = bizSector.SectorCode;
                    }
                    
                    //------------------
                   
                    // submitting data to database                     
                       BRcontext.Businesses.Add(biz);
                       BRcontext.SaveChanges();
                    // if there is error in database insert then try block will catch and send alternate message...
                    Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Business data was successfully inserted in database...", "Add New Business", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);


                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message);
            }
        }

        private string ExtractNumber(string original)
        {
            return new string(original.Where(c => Char.IsDigit(c)).ToArray());
        }

        private bool VerifyStatsNumber( string userstatsnum)
        {
            // this can be done in more ways than one...
            // since the data context is already loaded in memory, to avoid DB calls use it.
            // OR connect to DB and check business table or external keys table...since all businesses should be in external keys table (master key) 
            bool isUniqueStatsNum = false;
            try
            {
                // check to see if statistical nummber is unique for new business being added...
                var StatsNum = from Biz in BRDataContext.Businesses
                               where Biz.StatisticalNumber == userstatsnum
                               select Biz.StatisticalNumber;

                int count = StatsNum.Count();

                if (!(count > 0))
                {
                    //if record is not found then stats number is unique 
                    isUniqueStatsNum = true;
                }              

            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message);
            }

            return isUniqueStatsNum;

        }

        private void btnVerifyStatsNo_Click(object sender, RoutedEventArgs e)
        {
            // first thing: verify uniqueness of statstical #
            bool isUniqueStatsNum = VerifyStatsNumber(radMaskedStatsNum.Text);
            if (isUniqueStatsNum)
            {
                
                //add message in status bar label
                statusbarlabel.Content = "Validation was completed Successfully...";
                // enable the remaining input controls for adding remaining business fields
                radMaskedUniqueID.IsEnabled = true;
                radMaskedLegalName.IsEnabled = true;
                radMaskedOperationalName.IsEnabled = true;               
                radMaskedAddress.IsEnabled = true;
                radMaskedLegalAddress.IsEnabled = true;
                cbxDistrict.IsEnabled = true;
                radMaskedPostalCode.IsEnabled = true;
                radMaskedTeleNum.IsEnabled = true;
                radMaskedFAX.IsEnabled = true;
                radMaskedEMAIL.IsEnabled = true;
                radMaskedGPS.IsEnabled = true;
                radMaskedURL.IsEnabled = true;
                cbxContactTitle.IsEnabled = true;
                radMaskedContactName.IsEnabled = true;
                radMaskedContactPhone.IsEnabled = true;
                radMaskedContactEmail.IsEnabled = true;
                dateYearBegunOper.IsEnabled = true;
                dateFiscalPeriodStart.IsEnabled = true;
                dateFiscalPeriodEnd.IsEnabled = true;
                radMaskedPercentForeign.IsEnabled = true;
                radMaskedRevenue.IsEnabled = true;

                //Enable Business Type combobox and fill with data from BusinessType Table data context
                cbxBusinessType.IsEnabled = true;
                //query BusinessTypes Lookup table combobox
                var bizTypeCodesLabels = from BizTypes in BRDataContext.BusinessTypes
                                         select BizTypes.Label;
                
                //populate combobox
                foreach (var item in bizTypeCodesLabels)
                {
                    cbxBusinessType.Items.Add(item);
                }

                //--------------------------------


                //Enable Business status combobox and fill with data from cbxBusinessStatus Table data context
                cbxBusinessStatus.IsEnabled = true;
                //query cbxBusinessStatus Lookup table combobox
                var bizStatusLabels = from BizStatus in BRDataContext.BusinessStatus
                                      select BizStatus.Label;
                //populate combobox
                foreach (var item in bizStatusLabels)
                {
                    cbxBusinessStatus.Items.Add(item);
                }
                // cbxBusinessStatus.DataContext = bizStatusLabels;//incorrect
                //-------------------------------

                dateRevenue.IsEnabled = true;
                cbxRevenueSRC.IsEnabled = true;                
                radMaskedNumEmploy.IsEnabled = true;
                cbxEmployeesSRC.IsEnabled = true;
                dateEmployees.IsEnabled = true;
                radMaskedSalEmploy.IsEnabled = true;
                cbxSalariedEmployeesSRC.IsEnabled = true;
                dateSalariedEmployees.IsEnabled = true;
                radMaskedSales.IsEnabled = true;
                cbxSalesSRC.IsEnabled = true;
                dateSales.IsEnabled = true;
                radMaskedSales.IsEnabled = true;
                radMaskedWages.IsEnabled = true;
                dateWages.IsEnabled = true;
                cbxWagesSRC.IsEnabled = true;
                cbxISIC.IsEnabled = true;
                dateISIC.IsEnabled = true;
                cbxISICSRC.IsEnabled = true;
                radMaskedLastUpdate.IsEnabled = true;

                
                //Enable Sector combobox and fill with data from sector Table data context
                cbxSector.IsEnabled = true;
                //query sector Lookup table combobox
                var bizSectorLabels = from BizSector in BRDataContext.Sectors
                                         select BizSector.Label;
                //populate combobox
                foreach (var item in bizSectorLabels)
                {
                    cbxSector.Items.Add(item);
                }

                //--------------------------------
                btnSubmitToDB.IsEnabled = true;

            }
            else
            {
                Telerik.WinControls.RadMessageBox.Show(" The statistical# already exists for this business...Please contact Administrator.");
            }

        }


       

        // TODO://refine by merging all got focus events into one event handler use the e parament or sender to access object

        private void radMaskedStatsNum_GotFocus(object sender, RoutedEventArgs e)
        {
            lblStatNum.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedStatsNum_LostFocus(object sender, RoutedEventArgs e)
        {
            lblStatNum.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void radMaskedLegalName_GotFocus(object sender, RoutedEventArgs e)
        {
            lblLegalName.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedLegalName_LostFocus(object sender, RoutedEventArgs e)
        {
            lblLegalName.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
      

        private void radMaskedOperationalName_GotFocus(object sender, RoutedEventArgs e)
        {
            lblOperationalName.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedOperationalName_LostFocus(object sender, RoutedEventArgs e)
        {
            lblOperationalName.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void radMaskedAddress_GotFocus(object sender, RoutedEventArgs e)
        {
            lblAddress.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedAddress_LostFocus(object sender, RoutedEventArgs e)
        {
            lblAddress.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void cbxDistrict_GotFocus(object sender, RoutedEventArgs e)
        {
            lblDistrict.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void cbxDistrict_LostFocus(object sender, RoutedEventArgs e)
        {
            lblDistrict.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void radMaskedTeleNum_GotFocus(object sender, RoutedEventArgs e)
        {
            lblTeleNum.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedTeleNum_LostFocus(object sender, RoutedEventArgs e)
        {
            lblTeleNum.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void cbxBusinessType_GotFocus(object sender, RoutedEventArgs e)
        {
            lblBusiType.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void cbxBusinessType_LostFocus(object sender, RoutedEventArgs e)
        {
            lblBusiType.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void cbxBusinessStatus_GotFocus(object sender, RoutedEventArgs e)
        {
            lblBusiStatus.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void cbxBusinessStatus_LostFocus(object sender, RoutedEventArgs e)
        {
            lblBusiStatus.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void radMaskedRevenue_GotFocus(object sender, RoutedEventArgs e)
        {
            lblrevenue.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedRevenue_LostFocus(object sender, RoutedEventArgs e)
        {
            lblrevenue.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void cbxRevenueSRC_GotFocus(object sender, RoutedEventArgs e)
        {
            lblrevenueSRC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void cbxRevenueSRC_LostFocus(object sender, RoutedEventArgs e)
        {
            lblrevenueSRC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void dateRevenue_GotFocus(object sender, RoutedEventArgs e)
        {
            lbldateRevenue.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void dateRevenue_LostFocus(object sender, RoutedEventArgs e)
        {
            lbldateRevenue.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void radMaskedNumEmploy_GotFocus(object sender, RoutedEventArgs e)
        {
            lblNumEmploy.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedNumEmploy_LostFocus(object sender, RoutedEventArgs e)
        {
            lblNumEmploy.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void cbxEmployeesSRC_GotFocus(object sender, RoutedEventArgs e)
        {
            lblEmployeeSRC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void cbxEmployeesSRC_LostFocus(object sender, RoutedEventArgs e)
        {
            lblEmployeeSRC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void dateEmployees_GotFocus(object sender, RoutedEventArgs e)
        {
            lbldateEmployee.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void dateEmployees_LostFocus(object sender, RoutedEventArgs e)
        {
            lbldateEmployee.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void radMaskedSalEmploy_GotFocus(object sender, RoutedEventArgs e)
        {
            lblSalEmploy.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedSalEmploy_LostFocus(object sender, RoutedEventArgs e)
        {
            lblSalEmploy.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void cbxSalariedEmployeesSRC_GotFocus(object sender, RoutedEventArgs e)
        {
            lblSalEmpSRC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void cbxSalariedEmployeesSRC_LostFocus(object sender, RoutedEventArgs e)
        {
            lblSalEmpSRC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void dateSalariedEmployees_GotFocus(object sender, RoutedEventArgs e)
        {
            lblDateSalEmp.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void dateSalariedEmployees_LostFocus(object sender, RoutedEventArgs e)
        {
            lblDateSalEmp.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void radMaskedSales_GotFocus(object sender, RoutedEventArgs e)
        {
            lblSales.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedSales_LostFocus(object sender, RoutedEventArgs e)
        {
            lblSales.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void cbxSalesSRC_GotFocus(object sender, RoutedEventArgs e)
        {
            lblSalesSRC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void cbxSalesSRC_LostFocus(object sender, RoutedEventArgs e)
        {
            lblSalesSRC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void dateSales_GotFocus(object sender, RoutedEventArgs e)
        {
            lbldateSales.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void dateSales_LostFocus(object sender, RoutedEventArgs e)
        {
            lbldateSales.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void radMaskedWages_GotFocus(object sender, RoutedEventArgs e)
        {
            lblWages.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedWages_LostFocus(object sender, RoutedEventArgs e)
        {
            lblWages.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void cbxWagesSRC_GotFocus(object sender, RoutedEventArgs e)
        {
            lblWagesSRC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void cbxWagesSRC_LostFocus(object sender, RoutedEventArgs e)
        {
            lblWagesSRC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void dateWages_GotFocus(object sender, RoutedEventArgs e)
        {
            lblDateWages.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void dateWages_LostFocus(object sender, RoutedEventArgs e)
        {
            lblDateWages.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        

        private void cbxISICSRC_GotFocus(object sender, RoutedEventArgs e)
        {
            lblISICSRC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void cbxISICSRC_LostFocus(object sender, RoutedEventArgs e)
        {
            lblISICSRC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void dateISIC_GotFocus(object sender, RoutedEventArgs e)
        {
            lblDateISIC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void dateISIC_LostFocus(object sender, RoutedEventArgs e)
        {
            lblDateISIC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void radMaskedLastUpdate_GotFocus(object sender, RoutedEventArgs e)
        {
            lblLastUpdate.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedLastUpdate_LostFocus(object sender, RoutedEventArgs e)
        {
            lblLastUpdate.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

       /* private void dateUpdated_GotFocus(object sender, RoutedEventArgs e)
        {
            cbxSector.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void dateUpdated_LostFocus(object sender, RoutedEventArgs e)
        {
            cbxSector.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        } */

        private void radMaskedSales_LostFocus_1(object sender, RoutedEventArgs e)
        {
            lblSales.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void cbxSector_GotFocus(object sender, RoutedEventArgs e)
        {
            lblSector.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }

        private void cbxSector_LostFocus(object sender, RoutedEventArgs e)
        {
            lblSector.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void radMaskedUniqueID_GotFocus(object sender, RoutedEventArgs e)
        {
            lblUniqueID.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedUniqueID_LostFocus(object sender, RoutedEventArgs e)
        {
            lblUniqueID.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black); 
        }
        private void radMaskedLegalAddress_GotFocus(object sender, RoutedEventArgs e)
        {
            lblLegalAddress.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedLegalAddress_LostFocus(object sender, RoutedEventArgs e)
        {
            lblLegalAddress.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void radMaskedPostalCode_GotFocus(object sender, RoutedEventArgs e)
        {
            lblPostalCode.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedPostalCode_LostFocus(object sender, RoutedEventArgs e)
        {
            lblPostalCode.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void radMaskedFAX_GotFocus(object sender, RoutedEventArgs e)
        {
            lblFAX.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedFAX_LostFocus(object sender, RoutedEventArgs e)
        {
            lblFAX.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void radMaskedEMAIL_GotFocus(object sender, RoutedEventArgs e)
        {
            lblEMAIL.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedEMAIL_LostFocus(object sender, RoutedEventArgs e)
        {
            lblEMAIL.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void radMaskedURL_GotFocus(object sender, RoutedEventArgs e)
        {
            lblURL.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedURL_LostFocus(object sender, RoutedEventArgs e)
        {
            lblURL.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void radMaskedGPS_GotFocus(object sender, RoutedEventArgs e)
        {
            lblGPS.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedGPS_LostFocus(object sender, RoutedEventArgs e)
        {
            lblGPS.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void cbxContactTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            lblContactTitle.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void cbxContactTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            lblContactTitle.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void radMaskedContactName_GotFocus(object sender, RoutedEventArgs e)
        {
            lblContactName.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedContactName_LostFocus(object sender, RoutedEventArgs e)
        {
            lblContactName.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void radMaskedContactPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            lblContactPhone.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedContactPhone_LostFocus(object sender, RoutedEventArgs e)
        {
            lblContactPhone.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void radMaskedContactEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            lblContactEmail.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedContactEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            lblContactEmail.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void dateYearBegunOper_GotFocus(object sender, RoutedEventArgs e)
        {
            lblYearBegunOper.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void dateYearBegunOper_LostFocus(object sender, RoutedEventArgs e)
        {
            lblYearBegunOper.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void dateFiscalPeriodStart_GotFocus(object sender, RoutedEventArgs e)
        {
            lblFiscalPeriodStart.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void dateFiscalPeriodStart_LostFocus(object sender, RoutedEventArgs e)
        {
            lblFiscalPeriodStart.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void dateFiscalPeriodEnd_GotFocus(object sender, RoutedEventArgs e)
        {
            lblFiscalPeriodEnd.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void dateFiscalPeriodEnd_LostFocus(object sender, RoutedEventArgs e)
        {
            lblFiscalPeriodEnd.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void radMaskedPercentForeign_GotFocus(object sender, RoutedEventArgs e)
        {
            lblPercentForeign.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void radMaskedPercentForeign_LostFocus(object sender, RoutedEventArgs e)
        {
            lblPercentForeign.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }
        private void cbxISIC_GotFocus(object sender, RoutedEventArgs e)
        {
            lblISIC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
        }
        private void cbxISIC_LostFocus(object sender, RoutedEventArgs e)
        {
            lblISIC.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }




        private void ShowBRdetails_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            BRdetails brdetails = new BRdetails();
            brdetails.Show();
        }

        private void radGridView1_CellLoaded(object sender, CellEventArgs e)
        {
            
        }

        private void SelectQueryType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            // only perform if there is data in radgridview control
            if(radGridView1.Items.Count == 0)
            {
                return;
            }

            try
            {
                switch (SelectQueryType.Text)
                    {
                    case "Top 10 Sales":
                        LoadChart(1,this.charttype); //(int querytype, int charttype)
                        break;
                    case "Top 10 Employers":
                        LoadChart(2, this.charttype);
                        break;
                    case "Top 10 Revenue":
                        LoadChart(3, this.charttype);
                        break;

                    default:
                        //
                        break;

                    }

            }
            catch(Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message);
            }
        }

        private void barchbox_Checked(object sender, RoutedEventArgs e)
        {
            // When user clicks Bar chart checkbox...the other options must be unchecked..
            if(barchbox.IsChecked == true)
            {
                piechbox.IsChecked = false;
                linechbox.IsChecked = false;

                ToggleGraph(1);
            }


        }
        private void ToggleGraph(int index)
        {
            switch (index)
            {
                case 1:
                    //show bar graph for current chartquery type...                  
                    LoadChart(chartquery, 1);
                    break;
                case 2:
                    //show pie chart
                    LoadChart(chartquery, 2);
                    break;
                case 3:
                    //show line graph
                    LoadChart(chartquery, 3);
                    break;
                default:
                    //
                    break;
            }
        }

        private void piechbox_Checked(object sender, RoutedEventArgs e)
        {
            if (piechbox.IsChecked == true)
            {
                barchbox.IsChecked = false;
                linechbox.IsChecked = false;

                ToggleGraph(2);
            }
        }

        private void linechbox_Checked(object sender, RoutedEventArgs e)
        {
            if (linechbox.IsChecked == true)
            {
                barchbox.IsChecked = false;
                piechbox.IsChecked = false;

                ToggleGraph(3);
            }
        }

        private void GraphExport()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "png";
            dialog.Filter = "PNG File (*.png) | *.png";            
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.radchart1.ExportToImage(dialog.FileName, new PngBitmapEncoder());
            }          

        }

        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //export the graph when the user clicks the context menu...
            GraphExport();
        }

        private void exitprogram_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {            
            this.Close();
        }

        private void BusinessRegister_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void BRMFTab_GotFocus(object sender, RoutedEventArgs e)
        {

            //if this is the first time entering tab set initial look
            if(IsEnteringTab == true)
            {

                 IsEnteringTab = false;
                 Workbook BRMFWorkbook = new Workbook();
                 BRMFWorkbook.Worksheets.Add();


                 Worksheet BRMFWorksheet = BRMFWorkbook.ActiveWorksheet;
                 BRMFWorksheet.Name = "BRMF";
                 BRMFWorksheet.Cells[0, 0].SetValue("Legal Name");
                 BRMFWorksheet.Cells[0, 1].SetValue("Operating Name");
                 BRMFWorksheet.Cells[0, 2].SetValue("Operating Address");
                 BRMFWorksheet.Cells[0, 3].SetValue("Legal Address");
                 BRMFWorksheet.Cells[0, 4].SetValue("District");
                 BRMFWorksheet.Cells[0, 5].SetValue("Phone");
                 BRMFWorksheet.Cells[0, 6].SetValue("Fax");
                 BRMFWorksheet.Cells[0, 7].SetValue("Email");
                 BRMFWorksheet.Cells[0, 8].SetValue("URL");            
                 BRMFWorksheet.Cells[0, 9].SetValue("Postal Code");
                 BRMFWorksheet.Cells[0, 10].SetValue("GPS#");
                 BRMFWorksheet.Cells[0, 11].SetValue("Contact Title");
                 BRMFWorksheet.Cells[0, 12].SetValue("Contact Name");
                 BRMFWorksheet.Cells[0, 13].SetValue("Contact Phone");
                 BRMFWorksheet.Cells[0, 14].SetValue("Contact Email");
                 BRMFWorksheet.Cells[0, 15].SetValue("Business Status");
                 BRMFWorksheet.Cells[0, 16].SetValue("Business Type");
                 BRMFWorksheet.Cells[0, 17].SetValue("Year Begun Operation");
                 BRMFWorksheet.Cells[0, 18].SetValue("Fiscal Period Start");
                 BRMFWorksheet.Cells[0, 19].SetValue("Fiscal Period End");
                 BRMFWorksheet.Cells[0, 20].SetValue("Legal Code");
                 BRMFWorksheet.Cells[0, 21].SetValue("% Foreign Owned");
                 BRMFWorksheet.Cells[0, 22].SetValue("# of Employees");
                 BRMFspreadsheet.Workbook = BRMFWorkbook;

             

                //http://docs.telerik.com/devtools/document-processing/libraries/radspreadprocessing/working-with-worksheets/iterate-through-worksheets  
                  ThemableColor foregroundColor = new ThemableColor(Colors.Red);
                  System.Windows.Media.Color backgroundColor = Colors.Aqua; 
                  GradientFill greenGradientFill = new GradientFill(GradientType.Horizontal, ThemableColor.FromArgb(255, 46, 204, 113), ThemableColor.FromArgb(255, 0, 134, 56));
                IFill backgroundFill = new PatternFill(PatternType.Solid, backgroundColor, backgroundColor);

                  foreach (Worksheet worksheet in BRMFWorkbook.Worksheets)
                  {

                    bool cellempty = false;

                     for ( int i=0; i< 50; i++)
                    {
                        if (cellempty) break;
                        CellSelection cell = worksheet.Cells[0, i];
                        RangePropertyValue<ICellValue> rangeValue = worksheet.Cells[0, i].GetValue();
                        ICellValue cellvalue = rangeValue.Value;
                        if (!(cellvalue.RawValue == ""))
                        {
                            // cell.SetValue("The name of this worksheet is: " + worksheet.Name);
                            cell.SetForeColor(foregroundColor);
                           // cell.SetFill(backgroundFill);
                            cell.SetFill(greenGradientFill);
                        }
                        else
                        {
                            cellempty = true;
                        }
                    }
                }
            }

            //http://www.telerik.com/forums/how-to-find-if-workbook-is-empty
            /*
            CellRange range = this.radSpreadsheet.Workbook.Worksheets.First().UsedCellRange;
            CellIndex cellIndex = new CellIndex(0, 0);

            CellSelection cell = this.radSpreadsheet.Workbook.Worksheets.First().Cells[cellIndex];
            ICellValue value = cell.GetValue().Value;
            if (range.ColumnCount == 1 && range.RowCount == 1 && value.ValueType == CellValueType.Empty)
            {
                MessageBox.Show("The document is empty");
            }
            else
            {
                MessageBox.Show("The document is NOT empty");
            }
            */

            /*
            Hello,

            The Spreadsheet Control doesn't implement the same interface as the SpreadProcessing library, so there are some differences to be expected. Here's some sample code that populates the Spreadsheet with data from the Products table of the Northwind database, which should point you in the right track:
            protected void Page_Load(object sender, EventArgs e)
            {
            RadSpreadSheet1.Sheets.Clear();

            foreach (var sheet in GetSheets())
            {
            RadSpreadSheet1.Sheets.Add(sheet);
            }
            }
            public List<Worksheet> GetSheets()
            {
            var result = new List<Worksheet>();

            DataTable data = GetData();

            var sheet = new Worksheet();

            int rowIndex = 0;
            foreach (DataRow dataRow in data.Rows)
            {
            var row = new Row() { Index = rowIndex++ };

            int columnIndex = 0;
            foreach (DataColumn dataColumn in data.Columns)
            {
            if (dataColumn.ColumnName == "ID") continue; // Skip the ID column

            string cellValue = dataRow[dataColumn.ColumnName].ToString();

            var cell = new Cell() { Index = columnIndex++, Value = cellValue };

            row.AddCell(cell);
            }

            sheet.AddRow(row);
            }

            result.Add(sheet);

            return result;
            }

            public DataTable GetData()
            {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString))
            {
            conn.Open();
            string query = "SELECT * FROM [Products]";
            SqlCommand cmd = new SqlCommand(query, conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            return dt;
            }
            } */



        }

        private void btnBrowseBRMF_Click(object sender, RoutedEventArgs e)
        {

          try {
                BusyIndicator.IsBusy = true;//start the busy indicator for the spreadsheet control

                //http://docs.telerik.com/devtools/document-processing/libraries/radspreadprocessing/formats-and-conversion/import-export-format-providers-manager#export


                // Register Xlsx format
                WorkbookFormatProvidersManager.RegisterFormatProvider(new XlsxFormatProvider());
                OpenFileDialog openFileDialog = new OpenFileDialog();

            /*
             string extension = "xls";
             SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = extension,
                Filter = String.Format("{1} files (*.{0})|*.{0}|All files(*.*)|*.*", extension, "Excel"),
                FilterIndex = 1
            };
            */

                openFileDialog.Filter = Telerik.Windows.Controls.Spreadsheet.Utilities.FileDialogsHelper.GetOpenFileDialogFilter();
                    if (openFileDialog.ShowDialog().ToString() == "OK")
                    {
                        try
                        {
                        
                        string extension = Path.GetExtension(openFileDialog.SafeFileName);
                            using (Stream input = openFileDialog.OpenFile())
                            {
                              BRWorkBook = WorkbookFormatProvidersManager.Import(extension, input);
                              BRMFspreadsheet.Workbook = BRWorkBook;
                            //BRMFspreadsheet.DataContext = BRWorkBook.Worksheets;
                           // BRMFspreadsheet.ActiveWorksheet = BRMFspreadsheet.Workbook.Worksheets[0];
                        }
                        }
                        catch (IOException ex)
                        {
                            throw new IOException("The file cannot be opened. It might be locked by another application.", ex);
                        }
                    }
                    else
                    {
                      BusyIndicator.IsBusy = false;
                    }

            }
            catch(Exception Ex)
            {
                Telerik.WinControls.RadMessageBox.Show(" There was an error loading the Excel file : " + Ex.Message);
            }            

        }

        private void btnUpdateBRMF_Click(object sender, RoutedEventArgs e)
        {
                        
            //Show message to confirm users intent to udate using data from selected Excel tab...
            DialogResult res =System.Windows.Forms.DialogResult.Cancel;

            //initialize object with first tab containing matched records
            Worksheet SelectedWorksheet = BRMFspreadsheet.Workbook.Worksheets[0];

            //Find the worksheet according to the Tab Selected
            switch (cbxSelectBRMFTab.Text)
            {
                case "BRMF_Matched_Estab":
                    res = Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "You are attempting to update the BR database using data from the BRMF_Matched_Estab tab" + Environment.NewLine + " Press \"OK\" to continue", "Updating BR database from Excel", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info);
                    SelectedWorksheet = BRMFspreadsheet.Workbook.Worksheets["BRMF_Matched_Estab"];                   
                    break;
                case "BRMF_Unmatched_Estab":
                    res = Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "You are attempting to update the BR database using data from the BRMF_Unmatched_Estab tab" + Environment.NewLine + " Press \"OK\" to continue", "Updating BR database from Excel", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info);
                    SelectedWorksheet = BRMFspreadsheet.Workbook.Worksheets["BRMF_Unmatched_Estab"];
                    break;
                case "IRD":
                    res = Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "You are attempting to update the BR database using data from the IRD tab" + Environment.NewLine + " Press \"OK\" to continue", "Updating BR database from Excel", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info);
                    SelectedWorksheet = BRMFspreadsheet.Workbook.Worksheets["IRD"];
                    break;
                case "NIC":
                    res = Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "You are attempting to update the BR database using data from the NIC tab" + Environment.NewLine + " Press \"OK\" to continue", "Updating BR database from Excel", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info);
                    SelectedWorksheet = BRMFspreadsheet.Workbook.Worksheets["NIC"];
                    break;
                case "VAT":
                    res = Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "You are attempting to update the BR database using data from the VAT tab" + Environment.NewLine + " Press \"OK\" to continue", "Updating BR database from Excel", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info);
                    SelectedWorksheet = BRMFspreadsheet.Workbook.Worksheets["VAT"];
                    break;

            }
            
            if(res.ToString() == "OK")
            {
                //read data from excel file and populate database
                DBController DBCtrl = new DBController(BRMFspreadsheet.Workbook.Worksheets["LINK"], this);
                //DBCtrl.PopulateWithBRMF(SelectedWorksheet,SelectedWorksheet.Name);//use delegate instead for new thread
               
                dbcontroller = DBCtrl;
                ws = SelectedWorksheet;
                worksheetname = SelectedWorksheet.Name;
                
                Thread t = new Thread(PopulateDelegate);
                t.Start();
            }

        }
        public void PopulateDelegate()
        {
            //use global methods here after assigning them in click event
            // this uses main window thread and will still freeze UI...not good
            /*this.Dispatcher.BeginInvoke(new Action(() =>
           {
                dbcontroller.PopulateWithBRMF(ws, worksheetname);

           }));
           */
            dbcontroller.PopulateWithBRMF(ws, worksheetname);

        }
        
        private void BRMFspreadsheet_WorkbookContentChanged(object sender, EventArgs e)
        {            
            BusyIndicator.IsBusy = false;

        }
      
        private void tbxSearchTable_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (cbxSearchOption.SelectedIndex != -1)
            {
                if (!tbxSearchTable.Text.Contains("---Enter Search Value---"))
                {
                    // if a value has been entered then enable search button                 
                    btnSearchTable.IsEnabled = true;
                    btnSearchTable.Content = "Search";
                    btnSearchTable.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
                }
            }
        }

        private void BRMFspreadsheet_WorkbookChanged(object sender, EventArgs e)
        {
            BusyIndicator.IsBusy = false;
        }

        private void viewgraphtab_Loaded(object sender, RoutedEventArgs e)
        {
            if (radGridView1.Items.Count != 0)
            {
                // RadChart with top ten sales and splinechart
                //LoadChart(1, 3);
            }
        }

        private void BtnViewLogs_Click(object sender, RoutedEventArgs e)
        {
            LogWindow LogWin = new LogWindow(LogList);
            LogWin.setTxtBox();
            LogWin.ShowDialog();
        }
    }
}


