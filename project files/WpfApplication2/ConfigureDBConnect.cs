using System;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
//using Telerik.Windows.Controls;
using Telerik.WinControls.Themes;
using System.Drawing;
namespace StatsBR
{
   
    

    public partial class FrmConfigureDBConnect : Telerik.WinControls.UI.ShapedForm
    {
        private DBController dBConfiguration;
        private static bool isSettingsApplied;
        private bool isConnectionTested;

        //telerik controls theme customization
        DesertTheme themeDesert = new DesertTheme();
        AquaTheme themeAqua = new AquaTheme();
        BreezeTheme themeBreeze = new BreezeTheme();
        TelerikMetroTheme themeMetro = new TelerikMetroTheme();
        Windows7Theme  themeWin7 = new Windows7Theme();

        public FrmConfigureDBConnect()
        {
            InitializeComponent();            
        }
       
        public static bool IsSettingsApplied
        {
            get
            {
                return isSettingsApplied;
            }
        }

        private void ConfigureDBConnect_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, "Windows7"); // themeWin7.ThemeName            
        }

        private void radLabel1_Click(object sender, EventArgs e)
        {

        }

        private void radTextBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void BtnTestConnectionClick(object sender, EventArgs e)
        {
            bool okToTest = false;
            isConnectionTested = false;    
            
            if (!string.IsNullOrEmpty(tbServerName.Text))
            {
                if (!string.IsNullOrEmpty(tbDBName.Text))
                {
                    okToTest = true;
                    if (chboxSqlAuth.Checked)
                    {
                        okToTest = false;
                        if (!string.IsNullOrEmpty(tbUserName.Text))
                        {
                            if (!string.IsNullOrEmpty(tbPassword.Text))
                            {
                                okToTest = true;                               
                            }
                        }
                    }
                }
            }
            
            if (okToTest)
            { 
                try
                {
                    if (chboxSqlAuth.Checked)
                    { 
                        //sql authentication
                        dBConfiguration = new DBController(tbServerName.Text, tbDBName.Text,tbUserName.Text, tbPassword.Text);                                   
                    }
                    else
                    { 
                        //windows authentication
                        dBConfiguration = new DBController(tbServerName.Text, tbDBName.Text);                      
                    }
                    if (dBConfiguration.TestConnection())
                    {
                        lbConnectionTest.Text = "Connection to: " + tbDBName.Text + " was successfull!...";
                        isConnectionTested = true;                        
                    }
                }
                catch (Exception ex)
                {
                    if (chboxSqlAuth.Checked)
                    {
                        FormatRadMessageBox(themeAqua.ThemeName);
                        RadMessageBox.Show("Error: connecting to DB using SQL Authentication failed!" + Environment.NewLine + ex.Message, "DATABASE CONNECTION ERROR!", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                    else
                    {
                        FormatRadMessageBox(themeAqua.ThemeName);
                        RadMessageBox.Show("Error: connecting to DB using Windows Authentication failed!" + Environment.NewLine + ex.Message, "DATABASE CONNECTION ERROR!", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            else
            {
                FormatRadMessageBox(themeAqua.ThemeName);
                RadMessageBox.Show(Environment.NewLine +"Please fill in all the necessary details" + Environment.NewLine + "to connect to the SQLServer Database.","Missing Fields!", MessageBoxButtons.OK, RadMessageIcon.Info);
            }
        }

        private void FormatRadMessageBox(string themeName)
        {
            RadMessageBox.Instance.FormElement.TitleBar.Font = new Font("Arial", 12);
            RadMessageBox.SetThemeName(themeName);
            foreach (Control ctrl in RadMessageBox.Instance.Controls)
            {
                if (ctrl is RadLabel)
                {
                    ctrl.Font = new System.Drawing.Font("Tahoma", 12); //change the main text font     
                }
                if (ctrl is RadButton)  // && ctrl.Name == "radButton1"
                {
                    ctrl.Font = new System.Drawing.Font("Consolas", 12); //change the button font   
                    
                }
            }
        }

        private void tbDBName_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void tbUserName_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
         
        }

        private void radCheckBox1_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if(chboxSqlAuth.Checked)
            {
              gboxSaDetails.Visible = true;
            }
            
            else
            {
              gboxSaDetails.Visible = false;
            }

        }

        private void btnConfigClose_Click(object sender, EventArgs e)
        {
            if(!isSettingsApplied)
            {
                if (isConnectionTested)
                {
                    dBConfiguration.CloseConnection();                    
                }
             
            }
            Close();
        }

        private void btnSaveConnection_Click(object sender, EventArgs e)
        {
           if(!isConnectionTested)
           {
              // FormatRadMessageBox(themeAqua.ThemeName);
                RadMessageBox.Show(Environment.NewLine +"Please test the connection before applying settings...","Apply settings message", MessageBoxButtons.OK, RadMessageIcon.Info);
           }
           else
           { 

                isSettingsApplied = true;            
                MainWindow.CONNECTIONSTRING = dBConfiguration.ConnectionString;
               

                //TODO: modify connectionstring in appconfig and send messagebox when done

                ConnectionProperties cP = new ConnectionProperties();
                cP.Name = "BRDBContainer";
                cP.DataSource = tbServerName.Text;
                cP.InitCatalogue = tbDBName.Text;
                cP.Username = tbUserName.Text;
                cP.Password = tbPassword.Text;
           }
        }

        private void frmConfigureDBConnect_FormClosed(object sender, FormClosedEventArgs e)
        {
             if(!isSettingsApplied)
            {
                if (isConnectionTested)
                {
                    dBConfiguration.CloseConnection();
                }
             
            }
        }
    }
}
