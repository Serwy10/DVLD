namespace DVLD
{
    partial class frmManageLocalDrivingApplications
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
            this.lbTitle = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.TbFilter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbRecords = new System.Windows.Forms.Label();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddNewApplication = new System.Windows.Forms.Button();
            this.showApplicationInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelApplication = new System.Windows.Forms.ToolStripMenuItem();
            this.editApplication = new System.Windows.Forms.ToolStripMenuItem();
            this.ScheduleTest = new System.Windows.Forms.ToolStripMenuItem();
            this.VisionTestTool = new System.Windows.Forms.ToolStripMenuItem();
            this.WrittenTestTool = new System.Windows.Forms.ToolStripMenuItem();
            this.StreetTestTool = new System.Windows.Forms.ToolStripMenuItem();
            this.Issue = new System.Windows.Forms.ToolStripMenuItem();
            this.showLicenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showPersonLicenseHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.deleteApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            //
            // lbTitle
            //
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Stencil", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.ForeColor = System.Drawing.Color.Red;
            this.lbTitle.Location = new System.Drawing.Point(361, 181);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(609, 38);
            this.lbTitle.TabIndex = 5;
            this.lbTitle.Text = "Local Driving License Applications";
            //
            // dataGridView1
            //
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(12, 328);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1287, 339);
            this.dataGridView1.TabIndex = 7;
            //
            // contextMenuStrip1
            //
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showApplicationInfoToolStripMenuItem,
            this.cancelApplication,
            this.deleteApplicationToolStripMenuItem,
            this.editApplication,
            this.toolStripSeparator3,
            this.ScheduleTest,
            this.toolStripSeparator1,
            this.Issue,
            this.toolStripSeparator2,
            this.showLicenseToolStripMenuItem,
            this.showPersonLicenseHistoryToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(347, 311);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            //
            // toolStripSeparator1
            //
            this.toolStripSeparator1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(343, 6);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Franklin Gothic Demi", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 291);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Filter By :";
            //
            // cbFilter
            //
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.FormattingEnabled = true;
            this.cbFilter.Items.AddRange(new object[] {
            "None",
            "LDLAppID",
            "NationalNo",
            "FullName",
            "AppStatus"});
            this.cbFilter.Location = new System.Drawing.Point(127, 291);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(166, 28);
            this.cbFilter.TabIndex = 9;
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            //
            // TbFilter
            //
            this.TbFilter.Location = new System.Drawing.Point(311, 291);
            this.TbFilter.Name = "TbFilter";
            this.TbFilter.Size = new System.Drawing.Size(219, 26);
            this.TbFilter.TabIndex = 10;
            this.TbFilter.Visible = false;
            this.TbFilter.TextChanged += new System.EventHandler(this.TbFilter_TextChanged);
            this.TbFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbFilter_KeyPress);
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Franklin Gothic Demi", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(26, 683);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 25);
            this.label2.TabIndex = 11;
            this.label2.Text = "#Records : ";
            //
            // lbRecords
            //
            this.lbRecords.AutoSize = true;
            this.lbRecords.Font = new System.Drawing.Font("Franklin Gothic Demi", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRecords.Location = new System.Drawing.Point(134, 683);
            this.lbRecords.Name = "lbRecords";
            this.lbRecords.Size = new System.Drawing.Size(0, 25);
            this.lbRecords.TabIndex = 12;
            //
            // toolStripSeparator2
            //
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(343, 6);
            //
            // toolStripSeparator3
            //
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(343, 6);
            //
            // btnAddNewApplication
            //
            this.btnAddNewApplication.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnAddNewApplication.Image = global::DVLD.Properties.Resources.icons8_add_new_64;
            this.btnAddNewApplication.Location = new System.Drawing.Point(1202, 259);
            this.btnAddNewApplication.Name = "btnAddNewApplication";
            this.btnAddNewApplication.Size = new System.Drawing.Size(87, 60);
            this.btnAddNewApplication.TabIndex = 14;
            this.btnAddNewApplication.UseVisualStyleBackColor = false;
            this.btnAddNewApplication.Click += new System.EventHandler(this.btnAddNewApplication_Click);
            //
            // showApplicationInfoToolStripMenuItem
            //
            this.showApplicationInfoToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.showApplicationInfoToolStripMenuItem.Image = global::DVLD.Properties.Resources.icons8_applicant_30;
            this.showApplicationInfoToolStripMenuItem.Name = "showApplicationInfoToolStripMenuItem";
            this.showApplicationInfoToolStripMenuItem.Size = new System.Drawing.Size(346, 32);
            this.showApplicationInfoToolStripMenuItem.Text = "Show Application Info";
            this.showApplicationInfoToolStripMenuItem.Click += new System.EventHandler(this.showApplicationInfoToolStripMenuItem_Click);
            //
            // cancelApplication
            //
            this.cancelApplication.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelApplication.Image = global::DVLD.Properties.Resources.icons8_cancel_48;
            this.cancelApplication.Name = "cancelApplication";
            this.cancelApplication.Size = new System.Drawing.Size(346, 32);
            this.cancelApplication.Text = "Cancel Application";
            this.cancelApplication.Click += new System.EventHandler(this.cancelApplicationToolStripMenuItem_Click);
            //
            // editApplication
            //
            this.editApplication.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editApplication.Image = global::DVLD.Properties.Resources.edit;
            this.editApplication.Name = "editApplication";
            this.editApplication.Size = new System.Drawing.Size(346, 32);
            this.editApplication.Text = "Edit Application";
            this.editApplication.Click += new System.EventHandler(this.editApplicationToolStripMenuItem_Click);
            //
            // ScheduleTest
            //
            this.ScheduleTest.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.VisionTestTool,
            this.WrittenTestTool,
            this.StreetTestTool});
            this.ScheduleTest.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScheduleTest.Image = global::DVLD.Properties.Resources.icons8_schedule_48;
            this.ScheduleTest.Name = "ScheduleTest";
            this.ScheduleTest.Size = new System.Drawing.Size(346, 32);
            this.ScheduleTest.Text = "Schedule Test";
            //
            // VisionTestTool
            //
            this.VisionTestTool.Enabled = false;
            this.VisionTestTool.Image = global::DVLD.Properties.Resources.icons8_vision_40;
            this.VisionTestTool.Name = "VisionTestTool";
            this.VisionTestTool.Size = new System.Drawing.Size(287, 34);
            this.VisionTestTool.Text = "Schedule Vision Test";
            this.VisionTestTool.Click += new System.EventHandler(this.VisionTestTool_Click);
            //
            // WrittenTestTool
            //
            this.WrittenTestTool.Enabled = false;
            this.WrittenTestTool.Image = global::DVLD.Properties.Resources.icons8_write_100;
            this.WrittenTestTool.Name = "WrittenTestTool";
            this.WrittenTestTool.Size = new System.Drawing.Size(287, 34);
            this.WrittenTestTool.Text = "Schedule Written Test";
            this.WrittenTestTool.Click += new System.EventHandler(this.WrittenTestTool_Click);
            //
            // StreetTestTool
            //
            this.StreetTestTool.Enabled = false;
            this.StreetTestTool.Image = global::DVLD.Properties.Resources.icons8_fiat_500_100;
            this.StreetTestTool.Name = "StreetTestTool";
            this.StreetTestTool.Size = new System.Drawing.Size(287, 34);
            this.StreetTestTool.Text = "Schedule Street Test";
            this.StreetTestTool.Click += new System.EventHandler(this.StreetTestTool_Click);
            //
            // Issue
            //
            this.Issue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.Issue.Image = global::DVLD.Properties.Resources.icons8_driving_license_64;
            this.Issue.Name = "Issue";
            this.Issue.Size = new System.Drawing.Size(346, 32);
            this.Issue.Text = "IssueDrivingLicense (First Time)";
            this.Issue.Click += new System.EventHandler(this.Issue_Click);
            //
            // showLicenseToolStripMenuItem
            //
            this.showLicenseToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.showLicenseToolStripMenuItem.Image = global::DVLD.Properties.Resources.driving_license;
            this.showLicenseToolStripMenuItem.Name = "showLicenseToolStripMenuItem";
            this.showLicenseToolStripMenuItem.Size = new System.Drawing.Size(346, 32);
            this.showLicenseToolStripMenuItem.Text = "Show License";
            this.showLicenseToolStripMenuItem.Click += new System.EventHandler(this.showLicenseToolStripMenuItem_Click);
            //
            // showPersonLicenseHistoryToolStripMenuItem
            //
            this.showPersonLicenseHistoryToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.showPersonLicenseHistoryToolStripMenuItem.Image = global::DVLD.Properties.Resources.icons8_time_machine_50;
            this.showPersonLicenseHistoryToolStripMenuItem.Name = "showPersonLicenseHistoryToolStripMenuItem";
            this.showPersonLicenseHistoryToolStripMenuItem.Size = new System.Drawing.Size(346, 32);
            this.showPersonLicenseHistoryToolStripMenuItem.Text = "Show Person License History";
            this.showPersonLicenseHistoryToolStripMenuItem.Click += new System.EventHandler(this.showPersonLicenseHistoryToolStripMenuItem_Click);
            //
            // pictureBox1
            //
            this.pictureBox1.Image = global::DVLD.Properties.Resources.document__1_;
            this.pictureBox1.Location = new System.Drawing.Point(526, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(263, 156);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            //
            // deleteApplicationToolStripMenuItem
            //
            this.deleteApplicationToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.deleteApplicationToolStripMenuItem.Image = global::DVLD.Properties.Resources.delete;
            this.deleteApplicationToolStripMenuItem.Name = "deleteApplicationToolStripMenuItem";
            this.deleteApplicationToolStripMenuItem.Size = new System.Drawing.Size(346, 32);
            this.deleteApplicationToolStripMenuItem.Text = "Delete Application";
            this.deleteApplicationToolStripMenuItem.Click += new System.EventHandler(this.deleteApplicationToolStripMenuItem_Click);
            //
            // frmManageLocalDrivingApplications
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1345, 736);
            this.Controls.Add(this.btnAddNewApplication);
            this.Controls.Add(this.lbRecords);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TbFilter);
            this.Controls.Add(this.cbFilter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbTitle);
            this.Name = "frmManageLocalDrivingApplications";
            this.Text = "frmManageLocalDrivingApplications";
            this.Load += new System.EventHandler(this.frmManageLocalDrivingApplications_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.TextBox TbFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbRecords;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cancelApplication;
        private System.Windows.Forms.Button btnAddNewApplication;
        private System.Windows.Forms.ToolStripMenuItem editApplication;
        private System.Windows.Forms.ToolStripMenuItem ScheduleTest;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem VisionTestTool;
        private System.Windows.Forms.ToolStripMenuItem WrittenTestTool;
        private System.Windows.Forms.ToolStripMenuItem StreetTestTool;
        private System.Windows.Forms.ToolStripMenuItem Issue;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem showLicenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showApplicationInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem showPersonLicenseHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteApplicationToolStripMenuItem;
    }
}
