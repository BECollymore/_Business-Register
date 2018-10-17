using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using ASC_PARSER;

namespace TestDrive
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FrmTextFile : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox edLogFile;
		private System.Windows.Forms.LinkLabel lblLogFile;
		private System.Windows.Forms.LinkLabel lblXML;
		private System.Windows.Forms.TextBox edXMLFile;
		private System.Windows.Forms.OpenFileDialog OpenFileDialog;
		private System.Windows.Forms.SaveFileDialog SaveFileDialog;
		private System.Windows.Forms.Button btnConvert;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DataGrid dataGrid1;
		private System.Windows.Forms.Button btnDataSet;
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
			this.edLogFile = new System.Windows.Forms.TextBox();
			this.lblLogFile = new System.Windows.Forms.LinkLabel();
			this.lblXML = new System.Windows.Forms.LinkLabel();
			this.edXMLFile = new System.Windows.Forms.TextBox();
			this.btnConvert = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.btnDataSet = new System.Windows.Forms.Button();
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
			// edLogFile
			// 
			this.edLogFile.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.edLogFile.Location = new System.Drawing.Point(0, 40);
			this.edLogFile.Name = "edLogFile";
			this.edLogFile.Size = new System.Drawing.Size(592, 20);
			this.edLogFile.TabIndex = 0;
			this.edLogFile.Text = "..\\..\\samplelog.txt";
			// 
			// lblLogFile
			// 
			this.lblLogFile.Location = new System.Drawing.Point(0, 24);
			this.lblLogFile.Name = "lblLogFile";
			this.lblLogFile.Size = new System.Drawing.Size(100, 16);
			this.lblLogFile.TabIndex = 2;
			this.lblLogFile.TabStop = true;
			this.lblLogFile.Text = "Log file (in):";
			this.lblLogFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblLogFile_LinkClicked);
			// 
			// lblXML
			// 
			this.lblXML.Location = new System.Drawing.Point(0, 128);
			this.lblXML.Name = "lblXML";
			this.lblXML.Size = new System.Drawing.Size(100, 16);
			this.lblXML.TabIndex = 4;
			this.lblXML.TabStop = true;
			this.lblXML.Text = "XML file (out):";
			this.lblXML.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblXML_LinkClicked);
			// 
			// edXMLFile
			// 
			this.edXMLFile.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.edXMLFile.Location = new System.Drawing.Point(0, 144);
			this.edXMLFile.Name = "edXMLFile";
			this.edXMLFile.Size = new System.Drawing.Size(592, 20);
			this.edXMLFile.TabIndex = 3;
			this.edXMLFile.Text = "..\\..\\samplelog.xml";
			// 
			// btnConvert
			// 
			this.btnConvert.Location = new System.Drawing.Point(0, 176);
			this.btnConvert.Name = "btnConvert";
			this.btnConvert.Size = new System.Drawing.Size(152, 23);
			this.btnConvert.TabIndex = 5;
			this.btnConvert.Text = "Convert to XML file";
			this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(0, 104);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(296, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Test convert text log file to XML file";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(0, 232);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(296, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "Test convert text log file to DataSet";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(296, 16);
			this.label3.TabIndex = 8;
			this.label3.Text = "Step 1: Enter input text file";
			// 
			// dataGrid1
			// 
			this.dataGrid1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.dataGrid1.DataMember = "";
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(0, 288);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.Size = new System.Drawing.Size(592, 120);
			this.dataGrid1.TabIndex = 9;
			// 
			// btnDataSet
			// 
			this.btnDataSet.Location = new System.Drawing.Point(0, 256);
			this.btnDataSet.Name = "btnDataSet";
			this.btnDataSet.Size = new System.Drawing.Size(152, 23);
			this.btnDataSet.TabIndex = 10;
			this.btnDataSet.Text = "Convert to DataSet";
			this.btnDataSet.Click += new System.EventHandler(this.btnDataSet_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(600, 413);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnDataSet,
																		  this.dataGrid1,
																		  this.label3,
																		  this.label2,
																		  this.label1,
																		  this.btnConvert,
																		  this.lblXML,
																		  this.edXMLFile,
																		  this.lblLogFile,
																		  this.edLogFile});
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new FrmTextFile());
		}

		private void lblLogFile_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			OpenFileDialog.ShowDialog();
			this.edLogFile.Text=OpenFileDialog.FileName;
		}

		private void lblXML_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			SaveFileDialog.ShowDialog();
			this.edXMLFile.Text=SaveFileDialog.FileName;
		}

		private void btnConvert_Click(object sender, System.EventArgs e)
		{
			ASC_PARSER.Asc2XML l2xml = new ASC_PARSER.Asc2XML();
			int nResult=-1;

			Cursor.Current = Cursors.WaitCursor;
			
			nResult=l2xml.Convert(edLogFile.Text,edXMLFile.Text);
			
			Cursor.Current = System.Windows.Forms.Cursors.Default;
			
			if(nResult!=0)
			  System.Windows.Forms.MessageBox.Show("Error processing");
			  
		}

		private void btnDataSet_Click(object sender, System.EventArgs e)
		{
			ASC_PARSER.Asc2DataSet T2DS = new ASC_PARSER.Asc2DataSet();
			DataSet dsTemp=null;

			Cursor.Current = Cursors.WaitCursor;
			
			dsTemp=T2DS.Convert(edLogFile.Text);
			
			Cursor.Current = System.Windows.Forms.Cursors.Default;
			
			if(dsTemp==null)
			{
				System.Windows.Forms.MessageBox.Show("Error processing");
				return;
			}

			dataGrid1.SetDataBinding(dsTemp,"Data");


		
		}

		
	}
}
