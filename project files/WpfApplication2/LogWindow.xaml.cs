using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace StatsBR
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        private List<string> LogList { get; set; }
        public LogWindow(List<string> Logs)
        {
            
            InitializeComponent();
            LogList = Logs;
           
            // TxtBokLogWindow.GetBindingExpression(TextBox.TextProperty).UpdateTarget();

        }
        private string ConvertListToString(List<string> Logs)
        {

            string txt = "";
            foreach(string str in Logs)
            {
                txt += str;
            }
            return txt;
        }
        public void setTxtBox()
        {
           this.TxtBokLogWindow.Text = ConvertListToString(this.LogList);
        }

        private void BtnSaveLog_Click(object sender, RoutedEventArgs e)
        {
            //save to text file..
            SaveFileDialog save = new SaveFileDialog();           
            save.FileName = "BRLogFile.csv";
            save.Filter = " CSV File | *.csv |Text File | *.txt | MSWord file (*.doc)| *.doc| MSExcel File (*.xls)| *.xls";
            
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)                
            {
                StreamWriter writer = new StreamWriter(save.OpenFile());
                writer.Write(TxtBokLogWindow.Text);                
                writer.Dispose();
                writer.Close();

            }
        }

        private void BtnCloseLog_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


       
        }

    

}
