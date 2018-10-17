using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Telerik.Windows.Controls;
namespace StatsBR
{
    /// <summary>
    /// Interaction logic for BRdetails.xaml
    /// </summary>
    public partial class BRdetails : Window
    {       
        public BRdetails()
        {
            InitializeComponent();              
        }       
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void saveidentificatioctxmenu_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                bool changes = false;                              
                // var BizDetail = BrDatacontext.Businesses(i => i.Statistcal == statisticalNOdetails.Text);
                              
                using (BusinessRegisterEntities brcontext = new BusinessRegisterEntities())
                {
                    var biz = (from c in brcontext.Businesses
                              where c.StatisticalNumber.Equals(statisticalNOdetails.Text)
                                select c).First();

                    //statitical number and unique identifier are disabled so user cannot change in details view...
                    //... no check needed...

                    // need to compare with initial value to see if it has changes...               

                    if (biz.LegalName != legalnamedetails.Text)
                    {
                        biz.LegalName = legalnamedetails.Text;
                        changes = true;
                    }                    
                    if(biz.OperationalName != operatingnamedetails.Text)
                    {
                        biz.OperationalName = operatingnamedetails.Text;
                        changes = true;
                    }
                    // submitting to database 
                    if (changes)
                    {                  
                        brcontext.SaveChanges();
                        // if there is error in database insert then try block will catch and send alternate message...
                        Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Business data was successfully inserted in database...", "Add New Business", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                    }
                }
            }
            catch(Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Business data was unsuccessfully inserted in database... with following error: "+ Environment.NewLine + ex.Message, "Add New Business", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }
        }

        private void savegeographyctxmenu_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                bool changes = false;             
                using (BusinessRegisterEntities brcontext = new BusinessRegisterEntities())
                {
                    var biz = (from c in brcontext.Businesses
                               where c.StatisticalNumber.Equals(statisticalNOdetails.Text)
                               select c).First();
                    
                    // need to compare with initial value to see if it has changes...
                    if (biz.Address != operatingaddressdetails.Text)
                    {
                        biz.Address = operatingaddressdetails.Text;
                        changes = true;
                    }

                    //the following must be added when database is modified to include new variables
                    //that are now in details view...
                    /* if (biz.LegalAddress != legaladdressdetails.Text)
                     {
                         biz.LegalAddress = legaladdressdetails.Text;
                         changes = true;
                     }
                      if (biz.PostalCode != postalcodedetails.Text)
                    {
                        biz.PostalCode = postalcodedetails.Text;
                        changes = true;
                    }
                     if (biz.GPS != gpsdetails.Text)
                    {
                        biz.GPS = gpsdetails.Text;
                        changes = true;
                    }
                     */

                    if (biz.District != districtdetails.Text)
                    {
                        biz.District = districtdetails.Text;
                        changes = true;
                    }

                    // submitting to database 
                    if (changes)
                    {
                        brcontext.SaveChanges();
                        // if there is error in database insert then try block will catch and send alternate message...
                        Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Business data was successfully inserted in database...", "Add New Business", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Business data was unsuccessfully inserted in database... with following error: " + Environment.NewLine + ex.Message, "Add New Business", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }
        }

        private void savebizcontactctxmenu_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                bool changes = false;
                // var BizDetail = BrDatacontext.Businesses(i => i.Statistcal == statisticalNOdetails.Text);

                using (BusinessRegisterEntities brcontext = new BusinessRegisterEntities())
                {
                    var biz = (from c in brcontext.Businesses
                               where c.StatisticalNumber.Equals(statisticalNOdetails.Text)
                               select c).First();
                    
                    // need to compare with initial value to see if it has changes...    
                    if (biz.PhoneNumber != businessteledetails.Text)
                    {
                        biz.PhoneNumber = businessteledetails.Text;
                        changes = true;
                    }


                    // add the following variables in DB schema first...
                    /*
                   if (biz.BusinessFAX != businessfaxdetails.Text)
                   {
                       biz.BusinessFAX = businessfaxdetails.Text;
                       changes = true;
                   }
                   if (biz.BusinessEMAIL != businessemaildetails.Text)
                   {
                       biz.BusinessEMAIL = businessemaildetails.Text;
                       changes = true;
                   }

                   if (biz.BusinessURL != websiteurldetails.Text)
                   {
                       biz.BusinessURL = websiteurldetails.Text;
                       changes = true;
                   */

                    // submitting to database 
                    if (changes)
                    {
                        brcontext.SaveChanges();
                        // if there is error in database insert then try block will catch and send alternate message...
                        Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Business data was successfully inserted in database...", "Add New Business", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Business data was unsuccessfully inserted in database... with following error: " + Environment.NewLine + ex.Message, "Add New Business", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }
        }

        private void savemamagecontactctxmenu_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                bool changes = false;
                using (BusinessRegisterEntities brcontext = new BusinessRegisterEntities())
                {  
                    //get contact details using statistical number LINQ query...
                    var BizContact = brcontext.Contacts.FirstOrDefault(i => i.StatisticalNumber == statisticalNOdetails.Text);
                    if (BizContact != null)
                    {
                        // need to compare with initial value to see if it has changes...
                       
                       /* if (BizContact.Title != contacttitledetails.Text)
                        {
                            BizContact.Name = contactnamedetails.Text;
                            changes = true;
                        }
                        */

                        if (BizContact.Name != contactnamedetails.Text)
                        {
                            BizContact.Name = contactnamedetails.Text;
                            changes = true;
                        }
                        if (BizContact.PhoneNumber1 != contactteledetails.Text)
                        { //there are two numbers so make provisions
                            BizContact.PhoneNumber1 = contactteledetails.Text;
                            changes = true;
                        }
                        if (BizContact.EmailAddress1 != emaildetails.Text)
                        {//there are two emails, so make provisions
                            BizContact.EmailAddress1 = emaildetails.Text;
                            changes = true;
                        }
                    }
                    
                    // submitting to database 
                    if (changes)
                    {
                        brcontext.SaveChanges();
                        // if there is error in database insert then try block will catch and send alternate message...
                        Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Business data was successfully inserted in database...", "Add New Business", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                    }
                }

            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Business data was unsuccessfully inserted in database... with following error: " + Environment.NewLine + ex.Message, "Add New Business", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }
        }

        private void savedemographyctxmenu_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                bool changes = false;
                using (BusinessRegisterEntities brcontext = new BusinessRegisterEntities())
                {
                    var biz = (from c in brcontext.Businesses
                               where c.StatisticalNumber.Equals(statisticalNOdetails.Text)
                               select c).First();

                    //get demographic details using LINQ
                    //Business status

                    //get businessstatus for selected item to return statuscode
                    var BizStatus = brcontext.BusinessStatus.FirstOrDefault(i => i.Label == cbxbizstatusdetails.SelectedValue.ToString());
                    if (BizStatus != null)
                    { 
                        if(biz.BusinessStatusCode != BizStatus.BusinessStatusCode)
                        {   //user changed it, so update DB
                            biz.BusinessStatusCode = BizStatus.BusinessStatusCode;
                            changes = true;
                        }                            
                    }

                    //get businesstype for selected item to return typecode
                    var BizType = brcontext.BusinessTypes.FirstOrDefault(i => i.Label == cbxbiztypedetails.SelectedValue.ToString());
                    if (BizType != null)
                    {
                        if (biz.BusinessTypeCode != BizType.BusinessTypeCode)
                        {  //user changed it, so update DB
                            biz.BusinessTypeCode = BizType.BusinessTypeCode;
                            changes = true;
                        }
                    }

                    //the following variables is to be included in DB model..then this code will work
                    /*
                    if (biz.YearBegunOperation != yearbeunoperationdetails.Text)
                    {
                        biz.YearBegunOperation = yearbeunoperationdetails.Text;
                        changes = true;
                    }
                    if (biz.FiscalPeriodStart != fiscalstartdetails.Text)
                    {
                        biz.FiscalPeriodStart = fiscalstartdetails.Text;
                        changes = true;
                    }
                    if (biz.FiscalPeriodEnd != fiscalenddetails.Text)
                    {
                        biz.FiscalPeriodEnd = fiscalenddetails.Text;
                        changes = true;
                    }
                    if (biz.LegalCode != legalcodedetails.Text)
                    {
                        biz.LegalCode = legalcodedetails.Text;
                        changes = true;
                    }
                    if (biz.ForeignOwned != foreignowndetails.Text)
                    {
                        biz.ForeignOwned = foreignowndetails.Text;
                        changes = true;
                    }
                    */

                   

                    // submitting to database 
                    if (changes)
                    {
                        brcontext.SaveChanges();
                        // if there is error in database insert then try block will catch and send alternate message...
                        Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Business data was successfully inserted in database...", "Add New Business", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                    }
                }

            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Business data was unsuccessfully inserted in database... with following error: " + Environment.NewLine + ex.Message, "Add New Business", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }
        }

        private void saveeconomicctxmenu_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                bool changes = false;
                using (BusinessRegisterEntities brcontext = new BusinessRegisterEntities())
                {
                    var biz = (from c in brcontext.Businesses
                               where c.StatisticalNumber.Equals(statisticalNOdetails.Text)
                               select c).First();

                    //get Economic activity details using LINQ
                    //get businessstatus for selected item to return statuscode
                    //the following variables is to be included in DB model..then this code will work

                    // Employees data
                    if (biz.NumberOfEmployees != Convert.ToInt64(numempdetails.Text))
                    {
                        biz.NumberOfEmployees = Convert.ToInt64(numempdetails.Text);
                        changes = true;
                    }
                    if (biz.NumberOfEmployeesSource != numempsourcedetails.Text)
                    {
                        biz.NumberOfEmployeesSource = numempsourcedetails.Text;
                        changes = true;
                    }
                    if (biz.NumberOfEmployeesDate != dateNoEmployees.SelectedDate)
                    {
                        biz.NumberOfEmployeesDate = dateNoEmployees.SelectedDate;
                        changes = true;
                    }

                    // Salaried employees data
                    if (biz.NumberOfSalariedEmployees != Convert.ToInt64(numsalariedempdetails.Text))
                    {
                        biz.NumberOfSalariedEmployees = Convert.ToInt64(numsalariedempdetails.Text);
                        changes = true;
                    }
                    if (biz.NumberOfSalariedEmployeesSource != numsalariedempsourcedetails.Text)
                    {
                        biz.NumberOfSalariedEmployeesSource = numsalariedempsourcedetails.Text;
                        changes = true;
                    }
                    if (biz.NumberOfSalariedEmployeesDate != dateSalariedEmployees.SelectedDate)
                    {
                        biz.NumberOfSalariedEmployeesDate = dateSalariedEmployees.SelectedDate;
                        changes = true;
                    }

                    //sales data
                    if (biz.Sales != Convert.ToDouble(salesdetails.Text))
                    {
                        biz.Sales = Convert.ToDouble(salesdetails.Text);
                        changes = true;
                    }
                    if (biz.SalesSource != salessourcedetails.Text)
                    {
                        biz.SalesSource = salessourcedetails.Text;
                        changes = true;
                    }
                    if (biz.SalesDate != dateSales.SelectedDate)
                    {
                        biz.SalesDate = dateSales.SelectedDate;
                        changes = true;
                    }

                    //revenue data
                    if (biz.Revenue != Convert.ToDouble(revenuedetails.Text))
                    {
                        biz.Revenue = Convert.ToDouble(revenuedetails.Text);
                        changes = true;
                    }
                    if (biz.RevenueSource != cbxRevenueSRC.Text)
                    {
                        biz.RevenueSource = cbxRevenueSRC.Text;
                        changes = true;
                    }
                    if (biz.RevenueDate != daterevenue.SelectedDate)
                    {
                        biz.RevenueDate = daterevenue.SelectedDate;
                        changes = true;
                    }

                    //wages data
                    if (biz.Wages != Convert.ToDouble(wagesdetails.Text))
                    {
                        biz.Wages = Convert.ToDouble(wagesdetails.Text);
                        changes = true;
                    }
                    if (biz.WagesSource != wagessourcedetails.Text)
                    {
                        biz.WagesSource = wagessourcedetails.Text;
                        changes = true;
                    }
                    if (biz.WagesDate != datewages.SelectedDate)
                    {
                        biz.WagesDate = datewages.SelectedDate;
                        changes = true;
                    }

                    //ISIC data
                    if (biz.ISIC != isicdetails.Text)
                    {
                        biz.ISIC = isicdetails.Text;
                        changes = true;
                    }
                    if (biz.ISICSource != isicsourcedetails.Text)
                    {
                        biz.ISICSource = isicsourcedetails.Text;
                        changes = true;
                    }
                    if (biz.ISICDate != dateisic.SelectedDate)
                    {
                        biz.ISICDate = dateisic.SelectedDate;
                        changes = true;
                    }

                    // submitting to database 
                    if (changes)
                    {
                        brcontext.SaveChanges();
                        // if there is error in database insert then try block will catch and send alternate message...
                        Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Business data was successfully inserted in database...", "Add New Business", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                    }
                }

            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(Environment.NewLine + "Business data was unsuccessfully inserted in database... with following error: " + Environment.NewLine + ex.Message, "Add New Business", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
            }
        }
    }
}
