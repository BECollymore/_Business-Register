using System;//For strings and things
using System.IO;//to work with files
using System.Text.RegularExpressions;//to parse the file
using System.Data;
using System.Windows.Forms;//to generate a DataSet

namespace InputTxTFile

{
	/// <summary>
	/// Converts a file to a DataSet
	/// 	
	/// Add error handling for better management of errors		
	/// </summary>
	public class Asc2DataSet
	{
		private static StreamReader stream;
		private static String val;

		public DataSet Convert(string sLogFile)
		{
			const string message = "The file selected does not exist please select Input file";
            const string caption = "Select Input File";
            //DialogResult result;

            if (!System.IO.File.Exists(sLogFile))
            {
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;//fail file not good...
            }

            // If file is found check that file is of extension .txt
            //this will limit errors by constraining the options for input file type

            //if file estension is OK


			//Create a new stream for the input file
			stream = new StreamReader(sLogFile, true);
			//Ref:
            //https://msdn.microsoft.com/en-us/library/system.io.streamreader(v=vs.110).aspx

			//Create a new data set
			DataSet dsFile = new DataSet("TxTFile");
			//Ref:
            //https://msdn.microsoft.com/en-us/library/system.data.dataset(v=vs.110).aspx


			//Create a new data table
			//Note: this DataSet is bound to 
			//a datagrid control.  You need to provide this name
			//when you do that, see the FrmTextFile form...
			DataTable dtTable = dsFile.Tables.Add("TxTData");

			

			//Add columns
			
			//A better and more re-usable way would be to add a function to 
			//Either pass in column names (and / or types) or
			//optionally parse the first line of the input file to generate the field / column names
            //In the case of NIC this is in line #2...
            //Many comma delimited text files contain field names in the first row, however
            //our text file does not..
			//However since this is to be used only on text files and it is assumed that the column structure
            //will not be modified, for now, it's a safe bet to hard code the names...
            //further iterations should concentrate on providing a UI for customization of column names...
			
            			
			//accessing by column name *is* case sensitive
			//W3C SPECIFIC
            dtTable.Columns.Add("REG#", typeof(string));
            dtTable.Columns.Add("NAME", typeof(string));
            dtTable.Columns.Add("REGISTRAR#", typeof(string));
            dtTable.Columns.Add("TRADE/REG/NAME", typeof(string));
            dtTable.Columns.Add("ADDRESS1", typeof(string));
            dtTable.Columns.Add("ADDRESS2", typeof(string));
            dtTable.Columns.Add("DISTRICT", typeof(string));
            dtTable.Columns.Add("LOCATION", typeof(string));
            dtTable.Columns.Add("PHONE", typeof(string));
            dtTable.Columns.Add("EMAIL", typeof(string));
            dtTable.Columns.Add("REGISTRATION_DATE", typeof(string));
            dtTable.Columns.Add("#EEs", typeof(string));
            dtTable.Columns.Add("TOTAL_CONTRIBUTION", typeof(string));
            dtTable.Columns.Add("SECTOR", typeof(string));
            dtTable.Columns.Add("NATURE_OF_BUSINESS", typeof(string));
			
			//Set Regular Expression						
			//Regex r = new Regex("\\s(?=([^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            //match any substring that is separated by whitespace (tab, space etc)
			//and if that substring is surrounded by quotation marks then ignore any whitespace inside
			//those quotation marks.
			
			//Ref:
            //https://social.msdn.microsoft.com/Search/en-US?query=regular%20expressions&emptyWatermark=true&ac=4
 
			
			// Get the first line of data from the input (log) file.
            // In the text file from NIC this begins on line 5 since there are headers in file
            //Code must be modified to recognize when data starts...
			val = NextLine(stream);
           
           
			//Strip out each field and insert in DataTable
			//int sStart;//used as a placeholder for our current location
			//int nCount;//used to determine which field / column we are currently extracting
			string sTemp ="";//used to temporarily hold the field pulled out by RegEx in order to trim the quotation marks from the ends of the fiel (if any)
			
			//Data row object used to set values
			DataRow drRow;

			//DataRow - adding data Ref:
			//http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpguide/html/cpconaddingdatatodatatable.asp


            
			while(val != "" || (val.Length > 0))//val is empty when the input stream has been read to the end
			{
				
				//create a new row
				drRow=dtTable.NewRow();
                //these column ranges are hard coded but UI must be provided to customize based on file structure
                int [,] TextFileColumns = new int [,]{{0,9},{10,46},{47,68},{69,121},{122,143},{144,162},{163,170},{171,212},{213,221},{222,264},{265,281},{282,289},{290,305},{306,310},{318,323}};

				//Iterate through all the matches (match=field in current row)
                
				 for(int i=0; i<TextFileColumns.GetLength(0); i++)
				   {
                      
                       int start = TextFileColumns[i, 0];
                       int stringlength = (TextFileColumns[i, 1] - TextFileColumns[i, 0]);

                           sTemp = val.Substring(start, stringlength);                     
                          
                           try
                           {
                               //Get rid of troublesome quotes around certain fields
                               sTemp = sTemp.Trim('"');

                               drRow[i] = sTemp;
                           }
                           catch (System.ArgumentOutOfRangeException)
                           {
                               //to be used when UI is implemented in subsequent iterations...
                               MessageBox.Show("Please review settings for text file columns", "Text file column ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                           }
                      
                          //check for end of line
                           if (sTemp.Contains("\r"))
                           {
                               // There is a Environment.NewLine in the string
                               val= "";// double stop...just in case
                               break;//break from loop and read next line
                           }

				}
				

				//Now add the populated row to the data table              
                dtTable.Rows.Add(drRow);
                 
                //Get the next line from the input file
                val = NextLine(stream);
            
                if (val.Length == 1 && val.Contains("\r"))
                {
                    break;//end of file ...break from while loop
                }
				//And back to the top....
			}
			
			//Return the DataSet 
			return dsFile;


		}

       
		//Extrace one line of text at a time from an input stream
		//Assumption: each line ends in a single \n character
		private static String NextLine(StreamReader stream)
		{
			int stemp = stream.Read();
			String sReturn = "";

            while (stemp != -1 && stemp != '\n')  //TODO: can modify code to include "\r"
			{
				//Convert [ and ] to " because it simplifies 
				//the regular expression for parsing and one of the input 
				//fields (date) is surrounded by them
				//You could remove this if parsing a simple csv file
				
				if(stemp=='[') stemp='\"';
				if(stemp==']') stemp='\"';

				sReturn += (char)stemp;
				stemp = stream.Read();
			}
			return sReturn;
		}
	}
}
