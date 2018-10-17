using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using InputTxTFile;
using System.Data.Linq;

namespace TestDrive
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FrmTextFile : System.Windows.Forms.Form
	{
        private System.Windows.Forms.TextBox TxtFileSource;
		private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
		private System.Windows.Forms.Label lblStep2;
		private System.Windows.Forms.Label lblStep1;
		private System.Windows.Forms.DataGrid dataGrid1;
		private System.Windows.Forms.Button btnDataSet;
        private Button btnAddToTable;
        private Button btnBrowseTxt;
        private Label lblStep3;
        private ComboBox cbxSelTxtTable;
		//private W3C_CLF.LogFileToXMLFile l2xml;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmTextFile()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.TxtFileSource = new System.Windows.Forms.TextBox();
            this.lblStep2 = new System.Windows.Forms.Label();
            this.lblStep1 = new System.Windows.Forms.Label();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.btnDataSet = new System.Windows.Forms.Button();
            this.btnAddToTable = new System.Windows.Forms.Button();
            this.btnBrowseTxt = new System.Windows.Forms.Button();
            this.lblStep3 = new System.Windows.Forms.Label();
            this.cbxSelTxtTable = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.ShowReadOnly = true;
            // 
            // SaveFileDialog
            // 
            this.SaveFileDialog.DefaultExt = "xml";
            this.SaveFileDialog.FileName = "log1";
            // 
            // TxtFileSource
            // 
            this.TxtFileSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtFileSource.Location = new System.Drawing.Point(0, 34);
            this.TxtFileSource.Name = "TxtFileSource";
            this.TxtFileSource.Size = new System.Drawing.Size(592, 20);
            this.TxtFileSource.TabIndex = 0;
            this.TxtFileSource.Text = "..\\..\\nic.txt";
            // 
            // lblStep2
            // 
            this.lblStep2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStep2.ForeColor = System.Drawing.Color.Coral;
            this.lblStep2.Location = new System.Drawing.Point(0, 116);
            this.lblStep2.Name = "lblStep2";
            this.lblStep2.Size = new System.Drawing.Size(296, 16);
            this.lblStep2.TabIndex = 7;
            this.lblStep2.Text = "Step 2: Test TextFile in DataSet";
            this.lblStep2.Click += new System.EventHandler(this.label2_Click);
            // 
            // lblStep1
            // 
            this.lblStep1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStep1.ForeColor = System.Drawing.Color.Coral;
            this.lblStep1.Location = new System.Drawing.Point(0, 10);
            this.lblStep1.Name = "lblStep1";
            this.lblStep1.Size = new System.Drawing.Size(296, 16);
            this.lblStep1.TabIndex = 8;
            this.lblStep1.Text = "Step 1: Enter input text file";
            // 
            // dataGrid1
            // 
            this.dataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid1.DataMember = "";
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(0, 174);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(592, 120);
            this.dataGrid1.TabIndex = 9;
            this.dataGrid1.Navigate += new System.Windows.Forms.NavigateEventHandler(this.dataGrid1_Navigate);
            // 
            // btnDataSet
            // 
            this.btnDataSet.Location = new System.Drawing.Point(4, 135);
            this.btnDataSet.Name = "btnDataSet";
            this.btnDataSet.Size = new System.Drawing.Size(152, 33);
            this.btnDataSet.TabIndex = 10;
            this.btnDataSet.Text = "Convert to DataSet";
            this.btnDataSet.Click += new System.EventHandler(this.btnDataSet_Click);
            // 
            // btnAddToTable
            // 
            this.btnAddToTable.Location = new System.Drawing.Point(165, 328);
            this.btnAddToTable.Name = "btnAddToTable";
            this.btnAddToTable.Size = new System.Drawing.Size(97, 30);
            this.btnAddToTable.TabIndex = 11;
            this.btnAddToTable.Text = "Add to Table";
            this.btnAddToTable.UseVisualStyleBackColor = true;
            this.btnAddToTable.Click += new System.EventHandler(this.btnAddToTable_Click);
            // 
            // btnBrowseTxt
            // 
            this.btnBrowseTxt.Location = new System.Drawing.Point(4, 59);
            this.btnBrowseTxt.Name = "btnBrowseTxt";
            this.btnBrowseTxt.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseTxt.TabIndex = 12;
            this.btnBrowseTxt.Text = "Browse >>";
            this.btnBrowseTxt.UseVisualStyleBackColor = true;
            this.btnBrowseTxt.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblStep3
            // 
            this.lblStep3.AutoSize = true;
            this.lblStep3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStep3.ForeColor = System.Drawing.Color.Coral;
            this.lblStep3.Location = new System.Drawing.Point(4, 305);
            this.lblStep3.Name = "lblStep3";
            this.lblStep3.Size = new System.Drawing.Size(327, 17);
            this.lblStep3.TabIndex = 13;
            this.lblStep3.Text = "Step 3: Add text file data to Database Table";
            // 
            // cbxSelTxtTable
            // 
            this.cbxSelTxtTable.FormattingEnabled = true;
            this.cbxSelTxtTable.Items.AddRange(new object[] {
            "StatsBRPrimary",
            "StatsBRSecondary",
            "IRD",
            "NIC",
            "VAT"});
            this.cbxSelTxtTable.Location = new System.Drawing.Point(7, 334);
            this.cbxSelTxtTable.Name = "cbxSelTxtTable";
            this.cbxSelTxtTable.Size = new System.Drawing.Size(152, 21);
            this.cbxSelTxtTable.TabIndex = 14;
            // 
            // FrmTextFile
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(600, 413);
            this.Controls.Add(this.cbxSelTxtTable);
            this.Controls.Add(this.lblStep3);
            this.Controls.Add(this.btnBrowseTxt);
            this.Controls.Add(this.btnAddToTable);
            this.Controls.Add(this.btnDataSet);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.lblStep1);
            this.Controls.Add(this.lblStep2);
            this.Controls.Add(this.TxtFileSource);
            this.Name = "FrmTextFile";
            this.Text = "Input Data Text File";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		/*static void Main() 
		{
			Application.Run(new FrmTextFile());
		} */

		private void lblLogFile_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			OpenFileDialog.ShowDialog();
			this.TxtFileSource.Text=OpenFileDialog.FileName;
		}	

		private void btnDataSet_Click(object sender, System.EventArgs e)
		{
			InputTxTFile.Asc2DataSet T2DS = new InputTxTFile.Asc2DataSet();
			DataSet dsTemp=null;

			Cursor.Current = Cursors.WaitCursor;
			
			dsTemp=T2DS.Convert(TxtFileSource.Text);

            //TODO:
            //From here dataset has data from text file:
            //this is where the magic begins
            //there must be a iteration of datatable in dataset to search all rows of records and follow business update rules...
			
			Cursor.Current = System.Windows.Forms.Cursors.Default;
			
			if(dsTemp==null)
			{
				System.Windows.Forms.MessageBox.Show("Error processing");
				return;
			}

            dataGrid1.SetDataBinding(dsTemp, "TxTData");


		
		}
        private object SearchDataset(DataSet ds, string searchcol, string val, string returncol )
        {
            //var query = from r in ds.Tables["TxTFile"].AsEnumerable()
            //            where r.Field<string>("REG#") == "1"
            //            select r.Field<string>("#EEs");

              var query = from r in ds.Tables["TxTFile"].AsEnumerable()
                        where r.Field<string>(searchcol) == val
                        select r.Field<string>(returncol);

            return query; 
         
        }
        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGrid1_Navigate(object sender, NavigateEventArgs ne)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog.ShowDialog();
            this.TxtFileSource.Text = OpenFileDialog.FileName;
        }

        private void btnAddToTable_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

		
	}
}
