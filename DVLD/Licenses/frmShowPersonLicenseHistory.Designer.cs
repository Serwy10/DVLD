namespace DVLD
{
    partial class frmShowPersonLicenseHistory
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbRecords = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.localdataGrid = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.internationalDataGrid = new System.Windows.Forms.DataGridView();
            this.lbInternationalRecords = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ctrlShowDetailscs1 = new DVLD.CtrlShowDetailscs();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.localdataGrid)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.internationalDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(637, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(275, 48);
            this.label1.TabIndex = 1;
            this.label1.Text = "License History";
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(33, 474);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1423, 399);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Driver Licenses";
            //
            // tabControl1
            //
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(22, 33);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1373, 347);
            this.tabControl1.TabIndex = 0;
            //
            // tabPage1
            //
            this.tabPage1.Controls.Add(this.lbRecords);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.localdataGrid);
            this.tabPage1.Location = new System.Drawing.Point(4, 37);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1365, 306);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Local";
            this.tabPage1.UseVisualStyleBackColor = true;
            //
            // lbRecords
            //
            this.lbRecords.AutoSize = true;
            this.lbRecords.Location = new System.Drawing.Point(148, 267);
            this.lbRecords.Name = "lbRecords";
            this.lbRecords.Size = new System.Drawing.Size(0, 28);
            this.lbRecords.TabIndex = 6;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 267);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 28);
            this.label2.TabIndex = 5;
            this.label2.Text = "#Records :";
            //
            // localdataGrid
            //
            this.localdataGrid.AllowUserToAddRows = false;
            this.localdataGrid.AllowUserToDeleteRows = false;
            this.localdataGrid.AllowUserToOrderColumns = true;
            this.localdataGrid.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.localdataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.localdataGrid.Location = new System.Drawing.Point(22, 22);
            this.localdataGrid.Name = "localdataGrid";
            this.localdataGrid.ReadOnly = true;
            this.localdataGrid.RowHeadersWidth = 62;
            this.localdataGrid.RowTemplate.Height = 28;
            this.localdataGrid.Size = new System.Drawing.Size(1319, 229);
            this.localdataGrid.TabIndex = 4;
            //
            // tabPage2
            //
            this.tabPage2.Controls.Add(this.lbInternationalRecords);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.internationalDataGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 37);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1365, 306);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "International";
            this.tabPage2.UseVisualStyleBackColor = true;
            //
            // internationalDataGrid
            //
            this.internationalDataGrid.AllowUserToAddRows = false;
            this.internationalDataGrid.AllowUserToDeleteRows = false;
            this.internationalDataGrid.AllowUserToOrderColumns = true;
            this.internationalDataGrid.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.internationalDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.internationalDataGrid.Location = new System.Drawing.Point(19, 18);
            this.internationalDataGrid.Name = "internationalDataGrid";
            this.internationalDataGrid.ReadOnly = true;
            this.internationalDataGrid.RowHeadersWidth = 62;
            this.internationalDataGrid.RowTemplate.Height = 28;
            this.internationalDataGrid.Size = new System.Drawing.Size(1319, 236);
            this.internationalDataGrid.TabIndex = 5;
            //
            // lbInternationalRecords
            //
            this.lbInternationalRecords.AutoSize = true;
            this.lbInternationalRecords.Location = new System.Drawing.Point(133, 266);
            this.lbInternationalRecords.Name = "lbInternationalRecords";
            this.lbInternationalRecords.Size = new System.Drawing.Size(0, 28);
            this.lbInternationalRecords.TabIndex = 8;
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 266);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 28);
            this.label4.TabIndex = 7;
            this.label4.Text = "#Records :";
            //
            // pictureBox1
            //
            this.pictureBox1.Image = global::DVLD.Properties.Resources.icons8_history_folder_94;
            this.pictureBox1.Location = new System.Drawing.Point(23, 148);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(305, 233);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            //
            // ctrlShowDetailscs1
            //
            this.ctrlShowDetailscs1._nationaNO = "";
            this.ctrlShowDetailscs1._personID = -1;
            this.ctrlShowDetailscs1.Location = new System.Drawing.Point(351, 106);
            this.ctrlShowDetailscs1.Name = "ctrlShowDetailscs1";
            this.ctrlShowDetailscs1.Size = new System.Drawing.Size(1126, 349);
            this.ctrlShowDetailscs1.TabIndex = 0;
            //
            // frmShowPersonLicenseHistory
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1489, 885);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ctrlShowDetailscs1);
            this.Name = "frmShowPersonLicenseHistory";
            this.Text = "frmShowPersonLicenseHistory";
            this.Load += new System.EventHandler(this.frmShowPersonLicenseHistory_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.localdataGrid)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.internationalDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CtrlShowDetailscs ctrlShowDetailscs1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView localdataGrid;
        private System.Windows.Forms.Label lbRecords;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView internationalDataGrid;
        private System.Windows.Forms.Label lbInternationalRecords;
        private System.Windows.Forms.Label label4;
    }
}
