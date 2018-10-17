namespace StatsBR
{
    partial class FrmConfigureDBConnect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.radTitleBar1 = new Telerik.WinControls.UI.RadTitleBar();
            this.roundRectShapeTitle = new Telerik.WinControls.RoundRectShape(this.components);
            this.radScrollablePanel1 = new Telerik.WinControls.UI.RadScrollablePanel();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.chboxSqlAuth = new Telerik.WinControls.UI.RadCheckBox();
            this.gboxSaDetails = new Telerik.WinControls.UI.RadGroupBox();
            this.tbPassword = new Telerik.WinControls.UI.RadTextBox();
            this.tbUserName = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.btnSaveConnection = new Telerik.WinControls.UI.RadButton();
            this.radSeparator2 = new Telerik.WinControls.UI.RadSeparator();
            this.lbConnectionTest = new Telerik.WinControls.UI.RadLabel();
            this.tbDBName = new Telerik.WinControls.UI.RadTextBox();
            this.radSeparator1 = new Telerik.WinControls.UI.RadSeparator();
            this.btnTestConnection = new Telerik.WinControls.UI.RadButton();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.tbServerName = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.btnConfigClose = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).BeginInit();
            this.radScrollablePanel1.PanelContainer.SuspendLayout();
            this.radScrollablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chboxSqlAuth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gboxSaDetails)).BeginInit();
            this.gboxSaDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveConnection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSeparator2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbConnectionTest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDBName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSeparator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTestConnection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbServerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConfigClose)).BeginInit();
            this.SuspendLayout();
            // 
            // radTitleBar1
            // 
            this.radTitleBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radTitleBar1.Location = new System.Drawing.Point(1, 1);
            this.radTitleBar1.Name = "radTitleBar1";
            // 
            // 
            // 
            this.radTitleBar1.RootElement.ApplyShapeToControl = true;
            this.radTitleBar1.RootElement.Shape = this.roundRectShapeTitle;
            this.radTitleBar1.Size = new System.Drawing.Size(480, 23);
            this.radTitleBar1.TabIndex = 0;
            this.radTitleBar1.TabStop = false;
            this.radTitleBar1.Text = "ConfigureDBConnect";
            // 
            // roundRectShapeTitle
            // 
            this.roundRectShapeTitle.BottomLeftRounded = false;
            this.roundRectShapeTitle.BottomRightRounded = false;
            // 
            // radScrollablePanel1
            // 
            this.radScrollablePanel1.Location = new System.Drawing.Point(12, 30);
            this.radScrollablePanel1.Name = "radScrollablePanel1";
            // 
            // radScrollablePanel1.PanelContainer
            // 
            this.radScrollablePanel1.PanelContainer.Controls.Add(this.radGroupBox1);
            this.radScrollablePanel1.PanelContainer.Size = new System.Drawing.Size(456, 376);
            this.radScrollablePanel1.Size = new System.Drawing.Size(458, 378);
            this.radScrollablePanel1.TabIndex = 3;
            this.radScrollablePanel1.Text = "radScrollablePanel1";
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.btnConfigClose);
            this.radGroupBox1.Controls.Add(this.chboxSqlAuth);
            this.radGroupBox1.Controls.Add(this.gboxSaDetails);
            this.radGroupBox1.Controls.Add(this.btnSaveConnection);
            this.radGroupBox1.Controls.Add(this.radSeparator2);
            this.radGroupBox1.Controls.Add(this.lbConnectionTest);
            this.radGroupBox1.Controls.Add(this.tbDBName);
            this.radGroupBox1.Controls.Add(this.radSeparator1);
            this.radGroupBox1.Controls.Add(this.btnTestConnection);
            this.radGroupBox1.Controls.Add(this.radLabel2);
            this.radGroupBox1.Controls.Add(this.tbServerName);
            this.radGroupBox1.Controls.Add(this.radLabel1);
            this.radGroupBox1.HeaderText = "Database Configuration Settings";
            this.radGroupBox1.Location = new System.Drawing.Point(14, 20);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(420, 353);
            this.radGroupBox1.TabIndex = 3;
            this.radGroupBox1.Text = "Database Configuration Settings";
            // 
            // chboxSqlAuth
            // 
            this.chboxSqlAuth.ForeColor = System.Drawing.Color.OrangeRed;
            this.chboxSqlAuth.Location = new System.Drawing.Point(19, 107);
            this.chboxSqlAuth.Name = "chboxSqlAuth";
            this.chboxSqlAuth.Size = new System.Drawing.Size(216, 18);
            this.chboxSqlAuth.TabIndex = 17;
            this.chboxSqlAuth.Text = "Click: To use SQL Server Authentication";
            this.chboxSqlAuth.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.radCheckBox1_ToggleStateChanged);
            // 
            // gboxSaDetails
            // 
            this.gboxSaDetails.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.gboxSaDetails.Controls.Add(this.tbPassword);
            this.gboxSaDetails.Controls.Add(this.tbUserName);
            this.gboxSaDetails.Controls.Add(this.radLabel4);
            this.gboxSaDetails.Controls.Add(this.radLabel3);
            this.gboxSaDetails.HeaderText = "Enter sa details";
            this.gboxSaDetails.Location = new System.Drawing.Point(17, 136);
            this.gboxSaDetails.Name = "gboxSaDetails";
            this.gboxSaDetails.Size = new System.Drawing.Size(382, 89);
            this.gboxSaDetails.TabIndex = 16;
            this.gboxSaDetails.Text = "Enter sa details";
            this.gboxSaDetails.Visible = false;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(124, 57);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(211, 20);
            this.tbPassword.TabIndex = 16;
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(124, 20);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(211, 20);
            this.tbUserName.TabIndex = 15;
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(15, 57);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(56, 18);
            this.radLabel4.TabIndex = 14;
            this.radLabel4.Text = "Password:";
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(15, 23);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(57, 18);
            this.radLabel3.TabIndex = 13;
            this.radLabel3.Text = "Login (sa):";
            // 
            // btnSaveConnection
            // 
            this.btnSaveConnection.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSaveConnection.Location = new System.Drawing.Point(17, 311);
            this.btnSaveConnection.Name = "btnSaveConnection";
            this.btnSaveConnection.Size = new System.Drawing.Size(110, 24);
            this.btnSaveConnection.TabIndex = 15;
            this.btnSaveConnection.Text = "Apply Settings";
            this.btnSaveConnection.Click += new System.EventHandler(this.btnSaveConnection_Click);
            // 
            // radSeparator2
            // 
            this.radSeparator2.Location = new System.Drawing.Point(17, 291);
            this.radSeparator2.Name = "radSeparator2";
            this.radSeparator2.Size = new System.Drawing.Size(382, 14);
            this.radSeparator2.TabIndex = 14;
            this.radSeparator2.Text = "radSeparator2";
            // 
            // lbConnectionTest
            // 
            this.lbConnectionTest.ForeColor = System.Drawing.Color.OrangeRed;
            this.lbConnectionTest.Location = new System.Drawing.Point(149, 261);
            this.lbConnectionTest.Name = "lbConnectionTest";
            this.lbConnectionTest.Size = new System.Drawing.Size(14, 18);
            this.lbConnectionTest.TabIndex = 13;
            this.lbConnectionTest.Text = "...";
            // 
            // tbDBName
            // 
            this.tbDBName.Location = new System.Drawing.Point(141, 69);
            this.tbDBName.Name = "tbDBName";
            this.tbDBName.Size = new System.Drawing.Size(211, 20);
            this.tbDBName.TabIndex = 10;
            this.tbDBName.TextChanged += new System.EventHandler(this.tbDBName_TextChanged);
            // 
            // radSeparator1
            // 
            this.radSeparator1.Location = new System.Drawing.Point(17, 227);
            this.radSeparator1.Name = "radSeparator1";
            this.radSeparator1.Size = new System.Drawing.Size(379, 16);
            this.radSeparator1.TabIndex = 8;
            this.radSeparator1.Text = "radSeparator1";
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(17, 255);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(110, 24);
            this.btnTestConnection.TabIndex = 7;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.Click += new System.EventHandler(this.BtnTestConnectionClick);
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(32, 69);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(88, 18);
            this.radLabel2.TabIndex = 4;
            this.radLabel2.Text = "Database Name:";
            // 
            // tbServerName
            // 
            this.tbServerName.Location = new System.Drawing.Point(141, 34);
            this.tbServerName.Name = "tbServerName";
            this.tbServerName.Size = new System.Drawing.Size(211, 20);
            this.tbServerName.TabIndex = 3;
            this.tbServerName.TextChanged += new System.EventHandler(this.radTextBox1_TextChanged);
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(32, 37);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(73, 18);
            this.radLabel1.TabIndex = 2;
            this.radLabel1.Text = "Server Name:";
            // 
            // btnConfigClose
            // 
            this.btnConfigClose.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnConfigClose.Location = new System.Drawing.Point(286, 311);
            this.btnConfigClose.Name = "btnConfigClose";
            this.btnConfigClose.Size = new System.Drawing.Size(110, 24);
            this.btnConfigClose.TabIndex = 18;
            this.btnConfigClose.Text = "Close";
            this.btnConfigClose.Click += new System.EventHandler(this.btnConfigClose_Click);
            // 
            // frmConfigureDBConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 420);
            this.Controls.Add(this.radScrollablePanel1);
            this.Controls.Add(this.radTitleBar1);
            this.Name = "frmConfigureDBConnect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ConfigureDBConnect";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmConfigureDBConnect_FormClosed);
            this.Load += new System.EventHandler(this.ConfigureDBConnect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).EndInit();
            this.radScrollablePanel1.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).EndInit();
            this.radScrollablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chboxSqlAuth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gboxSaDetails)).EndInit();
            this.gboxSaDetails.ResumeLayout(false);
            this.gboxSaDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveConnection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSeparator2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbConnectionTest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDBName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSeparator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTestConnection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbServerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConfigClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadTitleBar radTitleBar1;
        private Telerik.WinControls.RoundRectShape roundRectShapeTitle;
        private Telerik.WinControls.UI.RadScrollablePanel radScrollablePanel1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadTextBox tbDBName;
        private Telerik.WinControls.UI.RadSeparator radSeparator1;
        private Telerik.WinControls.UI.RadButton btnTestConnection;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadTextBox tbServerName;
        private Telerik.WinControls.UI.RadLabel lbConnectionTest;
        private Telerik.WinControls.UI.RadButton btnSaveConnection;
        private Telerik.WinControls.UI.RadSeparator radSeparator2;
        private Telerik.WinControls.UI.RadGroupBox gboxSaDetails;
        private Telerik.WinControls.UI.RadTextBox tbPassword;
        private Telerik.WinControls.UI.RadTextBox tbUserName;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadCheckBox chboxSqlAuth;
        private Telerik.WinControls.UI.RadButton btnConfigClose;
    }
}
