using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace StatsBR
{
    class DetailsViewButton : Telerik.Windows.Controls.GridViewColumn
    {
        
        private BusinessRegisterEntities BRDataContext;
       
        public override FrameworkElement CreateCellElement(GridViewCell cell, object dataItem)
        {
            RadButton btn = new RadButton() { Content = "Details" };
           // Business value must be passed in event handler method, as seen below....
            btn.Click += new RoutedEventHandler((sender,e)=>btn_Click(sender,e, (Business)dataItem));          
                      
            return btn;
        }

        void btn_Click(object sender, RoutedEventArgs e, Business Biz)
        {
            string []sourcearray = {"CSO","IRD","NIC","VAT","REG"};
            BRDataContext = new BusinessRegisterEntities();

            // initialize with the data context
            BRdetails brdetails = new BRdetails();

            //populate with radgridview data before showing...                 
            brdetails.UniqueIDdetails.Text = "Null";
            brdetails.statisticalNOdetails.Text = Biz.StatisticalNumber;
            brdetails.legalnamedetails.Text = Biz.LegalName;
            brdetails.operatingnamedetails.Text = Biz.OperationalName;
            brdetails.operatingaddressdetails.Text = Biz.Address;
            brdetails.legaladdressdetails.Text = "Null";
            brdetails.postalcodedetails.Text = "Null";
            brdetails.gpsdetails.Text = "Null";
            brdetails.districtdetails.Text = Biz.District;
            brdetails.businessteledetails.Text = Biz.PhoneNumber;
            brdetails.businessfaxdetails.Text = "Null";
            brdetails.businessemaildetails.Text = "Null";
            brdetails.websiteurldetails.Text = "Null";
            brdetails.contacttitledetails.Text = "Null";

            //get contact details using statistical number LINQ query...
            var BizContact = BRDataContext.Contacts.FirstOrDefault(i => i.StatisticalNumber == Biz.StatisticalNumber);
            if(BizContact !=null)
            {
                brdetails.contactnamedetails.Text = BizContact.Name;
                brdetails.contactteledetails.Text = BizContact.PhoneNumber1;//there are two numbers so make provisions
                brdetails.emaildetails.Text = BizContact.EmailAddress1;//there are two emails, so make provisions
            }
            else
            {
                brdetails.contactnamedetails.Text = "Null";
                brdetails.contactteledetails.Text = "Null";//there are two numbers so make provisions
                brdetails.emaildetails.Text = "Null";
            }

            //get demographic details using LINQ
            //Find all business statuses to populate combobox.            
            //query cbxBusinessStatus Lookup table combobox
            var bizStatusLabels = from BizStatus in BRDataContext.BusinessStatus
                                  select BizStatus.Label;
            //populate combobox
            foreach (var item in bizStatusLabels)
            {
                brdetails.cbxbizstatusdetails.Items.Add(item);
            }
            // show the current business status for the selected business from comboxbox items
            var CurrentBizStatus = BRDataContext.BusinessStatus.FirstOrDefault(i => i.BusinessStatusCode == Biz.BusinessStatusCode);
            if (CurrentBizStatus != null)
            { //the status code should map the index in combobox
              // set combobox display value to the correct business status              
               
                brdetails.cbxbizstatusdetails.SelectedValue = CurrentBizStatus.Label;
            }

            //Get business type detils using LINQ           
            //Find all business types to populate combobox.            
            //query cbxBusinessStatus Lookup table combobox
            var bizTypeLabels = from TempBizType in BRDataContext.BusinessTypes
                                  select TempBizType.Label;
            //populate combobox
            foreach (var item in bizTypeLabels)
            {
                brdetails.cbxbiztypedetails.Items.Add(item);
            }
            // show the current business status for the selected business from comboxbox items
            var BizType = BRDataContext.BusinessTypes.FirstOrDefault(i => i.BusinessTypeCode == Biz.BusinessTypeCode);
            if(BizType !=null)
            {
                //brdetails.businessstypedetails.Text = BizType.Label;//show label instead of business status code
                brdetails.cbxbiztypedetails.SelectedValue = BizType.Label;
            }
            
            
            //this needs to be added to data model
            brdetails.yearbeunoperationdetails.Text = "Null";
            brdetails.fiscalstartdetails.Text = "Null";
            brdetails.fiscalenddetails.Text = "Null";
            brdetails.legalcodedetails.Text = "Null"; // statscan document on caricom br guideline
            brdetails.foreignowndetails.Text = "Null";

            //populate form with economic data...
            brdetails.numempdetails.Text = Biz.NumberOfEmployees.ToString();
           // brdetails.numempsourcedetails.Text = Biz.NumberOfEmployeesSource.ToString();
            // brdetails.numempdatedetails.Text = Biz.NumberOfEmployeesDate.ToString();
            brdetails.dateNoEmployees.SelectedDate = Biz.NumberOfEmployeesDate;

            //salaried employees
            brdetails.numsalariedempdetails.Text = Biz.NumberOfSalariedEmployees.ToString();
           // brdetails.numsalariedempsourcedetails.Text = Biz.NumberOfSalariedEmployeesSource.ToString();
            //brdetails.numsalariedempdatedetails.Text = Biz.NumberOfSalariedEmployeesDate.ToString();
            brdetails.dateSalariedEmployees.SelectedDate = Biz.NumberOfSalariedEmployeesDate;

            //sales
            brdetails.salesdetails.Text = Biz.Sales.ToString();            
            brdetails.salessourcedetails.Text = Biz.SalesSource ;
                       
            // brdetails.salesdatedetails.Text = Biz.SalesDate.ToString();
            brdetails.dateSales.SelectedDate = Biz.SalesDate;

            //revenue
            brdetails.revenuedetails.Text = Biz.Revenue.ToString();
            brdetails.cbxRevenueSRC.Text = Biz.RevenueSource.ToString();
            // brdetails.revenuedatedetails.Text = Biz.RevenueDate.ToString();
            brdetails.daterevenue.SelectedDate = Biz.RevenueDate;

            //wages
            brdetails.wagesdetails.Text = Biz.Wages.ToString();
          //  brdetails.wagessourcedetails.Text = Biz.WagesSource.ToString();
            //brdetails.wagesdatedetails.Text = Biz.WagesDate.ToString();
            brdetails.datewages.SelectedDate = Biz.WagesDate;

            //isic
          //  brdetails.isicdetails.Text = Biz.ISIC.ToString();
          //  brdetails.isicsourcedetails.Text = Biz.ISICSource.ToString();
            //brdetails.isicdatedetails.Text = Biz.ISICDate.ToString();
           // brdetails.dateisic.SelectedDate = Biz.ISICDate;

            brdetails.Show();
        }

    }
}
