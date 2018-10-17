using System;
using System.Linq;
using System.Net.Mime;
using System.Security.Principal;
using System.Threading;
using System.Windows;


namespace StatsBR
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    /// 

    public partial class Login : Window
    {
        //private BRDataClassesDataContext BRDataContext= new BRDataClassesDataContext();
        bool authenticateduser = false;
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginHelper(txtboxLoginName.Text, 1);
        }

        private void LoginHelper(string loginname, int opt)
        {
            //TODO:

            //check if loginName is in LOGIN database if not add it for new user and show pop up screen
            //asking user to fill in some details for firt use...
            //this is important to map every user of the ActiveDirectory domain to a user in the LOGIN table for easy control...
            //also this is used to update LastUseridUpdated variable in the core business table


            using (BusinessRegisterEntities BRcontext = new BusinessRegisterEntities())
            {

               IQueryable<LOGIN> LoginInfo  = null;

                // var LoginInfo = BRDataContext.LOGINs.FirstOrDefault(i => i.LoginName == txtboxLoginName.Text);
                if (opt == 1)
                {
                   LoginInfo = from login in BRcontext.LOGINs
                                    where login.LoginName == loginname && login.LoginPassword == passwordBox.Password
                                    select login;
                }
                else if(opt ==2)
                {
                    // there is no password coming from active directory, only user loginname
                     LoginInfo = from login in BRcontext.LOGINs
                                   where login.LoginName == loginname 
                                   select login;
                }

                if (LoginInfo != null)
                {
                    if (LoginInfo.Count() > 0)
                    {
                        //there should only be one matching criteria
                        foreach (var linfo in LoginInfo)
                        {
                            authenticateduser = true;
                            MainWindow.userLogin = linfo.LoginName;
                            MainWindow.userPassword = linfo.LoginPassword;
                            MainWindow.userID = linfo.ID;
                        }
                        Close();
                    }

                }

                // should be refined to give more specific message according to which is incorrect username or password
                labelLoginSuccess.Content = " Please enter valid Login credentials!";
            }
        }

        private bool AuthenticateWinUser()
        {
            //change IsInRole("LenovoStats2\\StatsIT") to IsInRole("Statdom\\BRUserGroup")
            //create BR user group in Active Directory
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated && Thread.CurrentPrincipal.IsInRole("LenovoStats2\\StatsIT"))
            {
                authenticateduser = true;
                return true;
            }

            return false;
        }

        private void AuthenticateUser_Click(object sender, RoutedEventArgs e)
        {
            if(AuthenticateWinUser())
            {
                DateTime dt = DateTime.Now;
                string name= Thread.CurrentPrincipal.Identity.Name;
                LoginHelper(name,2);             
                MessageBox.Show( "UserName: ("+ name + " ) was authenticated sucessfully!!!");
                //  MainWindow.userPassword = passwordBox.Password;//not needed                
                Close();
            }
            else
            {
                //user does not have access
                Application.Current.Shutdown();
            }
        }

        private void frmLogin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if(!authenticateduser)
            {
                //user does not have access
                Application.Current.Shutdown();
            }
            
        }
    }
}
