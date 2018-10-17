using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Charting;
using Telerik.Windows.Controls.Data.DataForm;

namespace StatsBR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    

    struct SelectedTable
    {
       public string Schema;
       public string TableName;
        public SelectedTable(string p1, string p2)
        {
            Schema = p1;
            TableName = p2;
        }
    }

    public partial class MainWindow : Window
    {
        public static string CONNECTIONSTRING; // use this to update the the App.config connection string
        private bool tableNamesDisplayed;
        private DBController dBControl;
        private SelectedTable selectedTable;

        private BRDataClassesDataContext BRDataContext = new BRDataClassesDataContext();
        public MainWindow()
        {
            InitializeComponent();
            //TelerikRadControls trc = new TelerikRadControls();
            //this.radGridView1.ItemsSource = trc.GetBusinessData();//changed to following for cleaner data results


            dBControl = new DBController();
            dBControl.ConnectionString = dBControl.GetAdoConfigurationString("BRDBContainer");
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

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //populate property grid...
            //this.DataContext = (new TelerikRadControls()).GetBusinessData();
            //testing RadChart
            radGridView1.ItemsSource = BRDataContext.Businesses;
            this.selectedTable = "Core.Business";
            // this.DataContext =  BRDataContext.Businesses;
            LoadChart();

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
        public class ProductSales
        {
            public ProductSales(int quantity, int month, string monthName)
            {
                this.Quantity = quantity;
                this.Month = month;
                this.MonthName = monthName;
            }
            public int Quantity
            {
                get;
                set;
            }
            public int Month
            {
                get;
                set;
            }
            public string MonthName
            {
                get;
                set;
            }
        }
        private List<ProductSales> CreateData()
        {
            List<ProductSales> persons = new List<ProductSales>();
            persons.Add(new ProductSales(154, 1, "January"));
            persons.Add(new ProductSales(138, 2, "February"));
            persons.Add(new ProductSales(143, 3, "March"));
            persons.Add(new ProductSales(120, 4, "April"));
            persons.Add(new ProductSales(135, 5, "May"));
            persons.Add(new ProductSales(125, 6, "June"));
            persons.Add(new ProductSales(179, 7, "July"));
            persons.Add(new ProductSales(170, 8, "August"));
            persons.Add(new ProductSales(198, 9, "September"));
            persons.Add(new ProductSales(187, 10, "October"));
            persons.Add(new ProductSales(193, 11, "November"));
            persons.Add(new ProductSales(212, 12, "December"));
            return persons;
        }

        private void LoadChart()
        {
            SeriesMapping seriesMapping = new SeriesMapping();
            seriesMapping.LegendLabel = "Product Sales";
            seriesMapping.SeriesDefinition = new SplineSeriesDefinition();
            seriesMapping.ItemMappings.Add(new ItemMapping("Month", DataPointMember.XValue));
            seriesMapping.ItemMappings.Add(new ItemMapping("Quantity", DataPointMember.YValue));
            this.radchart1.SeriesMappings.Add(seriesMapping);
            this.radchart1.ItemsSource = this.CreateData();
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

            if (cbxTables.SelectedIndex != -1)
            {
                //combobox has schema specifier before table name...eg: Core.Business 
                this.selectedTable  = cbxTables.SelectedItem.ToString();
                radGridView1.ItemsSource = dBControl.GetTableData(this.selectedTable).DefaultView;
                
            }

        }

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

            // dBControl.UpdateBusinessTable((DataTable) dBControl.BindingSrc.DataSource);

            //BRDataContext.SubmitChanges();

            // dBControl.UpdateBusinessTable((DataTable) dBControl.BindingSrc.DataSource);
            bool ok = false;
            try
            {
                BRDataContext.SubmitChanges();
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
            tbxSearchTable.Clear();
            //TODO: implement auto suggestion based on previous search texts
        }

        private void btnSearchTable_Click(object sender, RoutedEventArgs e)
        {
            
            
            string search = tbxSearchTable.Text;
            bool ok = true;
            object results= null;

            if(cbxTables.Text)

            try
            {

           
                switch(cbxSearchOption.Text)
                {
                    case "Statistical Number":
                        {
                             results = (from business in BRDataContext.Businesses
                                 where business.StatisticalNumber.Contains(search) 
                                 select business);
                        }
                        break;  
                    case "Business Name":
                        { // search table by business name ...i.e by legal name or operational name 
                           results = (from business in BRDataContext.Businesses
                                 where business.LegalName.Contains(search) || business.OperationalName.Contains(search) 
                                 select business);
                        }
                         
                        break;
                    case "District":
                        {
                           results = (from business in BRDataContext.Businesses
                                 where business.District.Contains(search) 
                                 select business);
                        }
                        break;
                    case "ISIC":
                        {
                           results = (from business in BRDataContext.Businesses
                                 where business.ISIC.Contains(search)
                                 select business);
                        }
                        break;
                }
     
           

           
            }
            catch(Exception ex)
            {
              ok = false;
             Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "The search operation terminated with errors: " + ex.Message, "Edit Business data", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }

            if (ok)
            {              
                radGridView1.ItemsSource = results;                
                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "The search operation was successfully completed..." , "Edit Business data", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }
            //TODO: write database select script to search database base on search options and values...
            

        }

        private void cbxSearchOption_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
           
        }
    }
}
