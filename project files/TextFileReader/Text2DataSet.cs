using System;//For strings and things
using System.IO;//to work with files
using System.Text.RegularExpressions;//to parse the file
using System.Data;//to generate a DataSet

namespace ASC_PARSER
{
	/// <summary>
	/// Converts a file to a DataSet
	/// 
	/// Created by John Cardinal Feb 26th 2003
	/// Use at your own risk; this code does not have any 
	/// error handling to simplify the example
	/// mailto:johnc@ayanova.com
	/// 
	/// Note: Any part of this code that is not generic (i.e. is specific to W3C common log file format)
	/// is marked with the following comment:
	/// //W3C SPECIFIC
	/// This will allow anyone using this class to quickly find the bits that 
	/// will affect other implementations and change as required
	/// </summary>
	public class Asc2DataSet
	{
		private static StreamReader stream;
		private static String val;

		public DataSet Convert(string sLogFile)
		{
			if(!System.IO.File.Exists(sLogFile))
				return null;//fail file not found
			
			//Create a new stream for the input file
			stream = new StreamReader(sLogFile, true);
			//Ref:
			//http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfsystemiostreamreaderclasstopic.asp

			//Create a new data set
			DataSet dsFile = new DataSet("File");
			//Ref:
			//http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfsystemdatadatasetclasstopic.asp


			//Create a new data table
			//Note: in the sample this DataSet is bound to 
			//a datagrid control.  You need to provide this name
			//when you do that, see the TestDrive form for example.
			DataTable dtTable = dsFile.Tables.Add("Data");

			

			//Add columns
			//A simple way to add columns.
			//A better and more re-usable way would be to add a function to 
			//Either pass in column names (and / or types) or
			//optionally parse the first line of the input file to generate the field / column names
			
			//However since this is to be used only on text files, it's a safe bet that its
			//ok to make each column a string type and let the caller 
			//do any work specific to the data contained in the text file

			//Many comma delimited text files contain field names in the first row, however
			//our log file does not

			//These are the somewhat arbitrary column names I've
			//given to the data from the Apache common log file
			//that I'm using this for

			//Remember accessing by column name *is* case sensitive
			//W3C SPECIFIC
			dtTable.Columns.Add("RemoteHost", typeof(string));
			dtTable.Columns.Add("rfc931", typeof(string));
			dtTable.Columns.Add("AuthUser", typeof(string));
			dtTable.Columns.Add("Date", typeof(string));
			dtTable.Columns.Add("Request", typeof(string));
			dtTable.Columns.Add("Status", typeof(string));
			dtTable.Columns.Add("Bytes", typeof(string));
			dtTable.Columns.Add("Referrer", typeof(string));
			dtTable.Columns.Add("Agent", typeof(string));
			
			//Set Regular Expression

			//If you were parsing a standard comma delimited text file
			//you would use this expression:
			//Regex r = new Regex(",(?=([^\"]*\"[^\"]*\")*(?![^\"]*\"))");
			//i.e. "a",55,"some text","d"
			//matches (returns) "a" and 55 and "some text" and "d"

			//However, in this case we need to look for whitespace
			//since the W3C common log format uses whitespace for delimiters
			//(as do many other internet related items such as pop3 headers etc)
			//Whitespace could mean a tab or a space etc
			//so we use this instead:			
			Regex r = new Regex("\\s(?=([^\"]*\"[^\"]*\")*(?![^\"]*\"))");

			//Note that the only difference is the first character \s indicating whitespace instead of ,

			//What this means is that it's going to match any substring that is separated by whitespace (tab, space etc)
			//and if that substring is surrounded by quotation marks it's going to ignore any whitespace inside
			//those quotation marks.
			
			//Ref:
			//http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpguide/html/cpconcomregularexpressions.asp
 
			
			// Get the first line of data from the input (log) file.
			val = NextLine(stream);

			//Strip out each field and insert in DataTable
			int sStart;//used as a placeholder for our current location
			int nCount;//used to determine which field / column we are currently extracting
			string sTemp;//used to temporarily hold the field pulled out by RegEx in order to trim the quotation marks from the ends of the fiel (if any)
			
			//Data row object used to set values
			DataRow drRow;

			//DataRow - adding data Ref:
			//http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpguide/html/cpconaddingdatatodatatable.asp

			while(val != "")//val is empty when the input stream has been read to the end
			{
				sStart=0;
				nCount=0;
				
				//create a new row
				drRow=dtTable.NewRow();

				//Iterate through all the matches (match=field in current row)
				foreach (Match m in r.Matches(val))
				{  
					//Retrieve the field based on the results of the match
					sTemp=val.Substring(sStart, m.Index - sStart);
					//Get rid of pesky quotes around certain fields
					sTemp=sTemp.Trim('"');

					//fill the field by index, could also have done it by field name
					//i.e. drRow["FieldName"]="value"
					//but that wouldn't make sense here
                    drRow[nCount]=sTemp;
					
					//keep one step ahead in the matching game..
					sStart = m.Index + 1;

					//keep track of which field is next
					nCount++;
				}
				//squeeze out the last bit which isn't extracted in the loop above
				//(I know, this is a kludge... but who hasn't kludged from time to time!)
				sTemp=val.Substring(sStart, val.Length - sStart);
				//Get rid of pesky quotes around certain fields
				sTemp=sTemp.Trim('"');
				drRow[nCount]=sTemp;

				//Now add the populated row to the data table
				//Note: I don't know why this is required or designed this way since
				//we got this row from the table in the first place but
				//that's what the docs say.
				dtTable.Rows.Add(drRow);

				//Get the next line from the input file
				val = NextLine(stream);

				//And back to the top....
			}
			
			//Return the DataSet in all it's glory
			return dsFile;


		}




		//Extrace one line of text at a time from an input stream
		//Assumption: each line ends in a single \n character
		private static String NextLine(StreamReader stream)
		{
			int stemp = stream.Read();
			String sReturn = "";
						
			while(stemp != -1 && stemp!='\n')//W3C SPECIFIC
			{
				//Convert [ and ] to " because it simplifies 
				//the regular expression for parsing and one of the input 
				//fields (date) is surrounded by them
				//You could remove this if parsing a simple csv file
				//that has nothing but quotes as string delimiters
				//or add your own as required.
				//W3C SPECIFIC
				if(stemp=='[') stemp='\"';
				if(stemp==']') stemp='\"';

				sReturn += (char)stemp;
				stemp = stream.Read();
			}
			return sReturn;
		}
	}
}
