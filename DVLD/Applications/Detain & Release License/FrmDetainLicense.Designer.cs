namespace DVLD
{
    partial class FrmDetainLicense
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
            this.lbFrmTitle = new System.Windows.Forms.Label();
            this.linkLabelShowNewLicense = new System.Windows.Forms.LinkLabel();
            this.linklabelShowLicensesHistory = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbFineFees = new System.Windows.Forms.MaskedTextBox();
            this.lbCreatedBY = new System.Windows.Forms.Label();
            this.lbLicenseID = new System.Windows.Forms.Label();
            this.lbDetainDate = new System.Windows.Forms.Label();
            this.lbDetainID = new System.Windows.Forms.Label();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btnDetain = new System.Windows.Forms.Button();
            this.ctrlFindDriver1 = new DVLD.ctrlFindDriver();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            //
            // lbFrmTitle
            //
            this.lbFrmTitle.AutoSize = true;
            this.lbFrmTitle.Font = new System.Drawing.Font("Segoe UI Black", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFrmTitle.ForeColor = System.Drawing.Color.Red;
            this.lbFrmTitle.Location = new System.Drawing.Point(628, 9);
            this.lbFrmTitle.Name = "lbFrmTitle";
            this.lbFrmTitle.Size = new System.Drawing.Size(246, 45);
            this.lbFrmTitle.TabIndex = 13;
            this.lbFrmTitle.Text = "Detain License";
            //
            // linkLabelShowNewLicense
            //
            this.linkLabelShowNewLicense.AutoSize = true;
            this.linkLabelShowNewLicense.Enabled = false;
            this.linkLabelShowNewLicense.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelShowNewLicense.Location = new System.Drawing.Point(293, 1162);
            this.linkLabelShowNewLicense.Name = "linkLabelShowNewLicense";
            this.linkLabelShowNewLicense.Size = new System.Drawing.Size(163, 22);
            this.linkLabelShowNewLicense.TabIndex = 17;
            this.linkLabelShowNewLicense.TabStop = true;
            this.linkLabelShowNewLicense.Text = "Show New License";
            this.linkLabelShowNewLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelShowNewLicense_LinkClicked);
            //
            // linklabelShowLicensesHistory
            //
            this.linklabelShowLicensesHistory.AutoSize = true;
            this.linklabelShowLicensesHistory.Enabled = false;
            this.linklabelShowLicensesHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linklabelShowLicensesHistory.Location = new System.Drawing.Point(87, 1162);
            this.linklabelShowLicensesHistory.Name = "linklabelShowLicensesHistory";
            this.linklabelShowLicensesHistory.Size = new System.Drawing.Size(192, 22);
            this.linklabelShowLicensesHistory.TabIndex = 16;
            this.linklabelShowLicensesHistory.TabStop = true;
            this.linklabelShowLicensesHistory.Text = "Show Licenses History";
            this.linklabelShowLicensesHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linklabelShowLicensesHistory_LinkClicked);
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.tbFineFees);
            this.groupBox1.Controls.Add(this.lbCreatedBY);
            this.groupBox1.Controls.Add(this.lbLicenseID);
            this.groupBox1.Controls.Add(this.lbDetainDate);
            this.groupBox1.Controls.Add(this.lbDetainID);
            this.groupBox1.Controls.Add(this.pictureBox8);
            this.groupBox1.Controls.Add(this.pictureBox5);
            this.groupBox1.Controls.Add(this.pictureBox4);
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(88, 876);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1094, 266);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Detain Info";
            //
            // tbFineFees
            //
            this.tbFineFees.Location = new System.Drawing.Point(286, 155);
            this.tbFineFees.Mask = "0000";
            this.tbFineFees.Name = "tbFineFees";
            this.tbFineFees.Size = new System.Drawing.Size(100, 32);
            this.tbFineFees.TabIndex = 24;
            this.tbFineFees.ValidatingType = typeof(int);
            //
            // lbCreatedBY
            //
            this.lbCreatedBY.AutoSize = true;
            this.lbCreatedBY.Location = new System.Drawing.Point(858, 94);
            this.lbCreatedBY.Name = "lbCreatedBY";
            this.lbCreatedBY.Size = new System.Drawing.Size(0, 25);
            this.lbCreatedBY.TabIndex = 23;
            //
            // lbLicenseID
            //
            this.lbLicenseID.AutoSize = true;
            this.lbLicenseID.Location = new System.Drawing.Point(858, 50);
            this.lbLicenseID.Name = "lbLicenseID";
            this.lbLicenseID.Size = new System.Drawing.Size(0, 25);
            this.lbLicenseID.TabIndex = 20;
            //
            // lbDetainDate
            //
            this.lbDetainDate.AutoSize = true;
            this.lbDetainDate.Location = new System.Drawing.Point(281, 106);
            this.lbDetainDate.Name = "lbDetainDate";
            this.lbDetainDate.Size = new System.Drawing.Size(0, 25);
            this.lbDetainDate.TabIndex = 17;
            //
            // lbDetainID
            //
            this.lbDetainID.AutoSize = true;
            this.lbDetainID.Location = new System.Drawing.Point(281, 50);
            this.lbDetainID.Name = "lbDetainID";
            this.lbDetainID.Size = new System.Drawing.Size(0, 25);
            this.lbDetainID.TabIndex = 16;
            //
            // pictureBox8
            //
            this.pictureBox8.Image = global::DVLD.Properties.Resources.incorporation__2_;
            this.pictureBox8.Location = new System.Drawing.Point(800, 82);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(37, 37);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox8.TabIndex = 15;
            this.pictureBox8.TabStop = false;
            //
            // pictureBox5
            //
            this.pictureBox5.Image = global::DVLD.Properties.Resources.driving_license;
            this.pictureBox5.Location = new System.Drawing.Point(800, 38);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(37, 37);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 12;
            this.pictureBox5.TabStop = false;
            //
            // pictureBox4
            //
            this.pictureBox4.Image = global::DVLD.Properties.Resources.coins;
            this.pictureBox4.Location = new System.Drawing.Point(218, 153);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(37, 37);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 11;
            this.pictureBox4.TabStop = false;
            //
            // pictureBox2
            //
            this.pictureBox2.Image = global::DVLD.Properties.Resources.icons8_schedule_48;
            this.pictureBox2.Location = new System.Drawing.Point(218, 94);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(37, 37);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            //
            // pictureBox1
            //
            this.pictureBox1.Image = global::DVLD.Properties.Resources.driving_license;
            this.pictureBox1.Location = new System.Drawing.Point(218, 38);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(37, 37);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(603, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(109, 25);
            this.label8.TabIndex = 7;
            this.label8.Text = "Created BY :";
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(603, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 25);
            this.label5.TabIndex = 4;
            this.label5.Text = "Replaced.LicenseID :";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "Fine Fees :";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Detain Date :";
            //
            // label9
            //
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(36, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 25);
            this.label9.TabIndex = 0;
            this.label9.Text = "Detain ID :";
            //
            // button2
            //
            this.button2.AutoSize = true;
            this.button2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Image = global::DVLD.Properties.Resources.close__3_;
            this.button2.Location = new System.Drawing.Point(1256, 1022);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(175, 54);
            this.button2.TabIndex = 19;
            this.button2.Text = "Close";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            //
            // btnDetain
            //
            this.btnDetain.Enabled = false;
            this.btnDetain.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDetain.Image = global::DVLD.Properties.Resources.icons8_hand_30;
            this.btnDetain.Location = new System.Drawing.Point(1256, 953);
            this.btnDetain.Name = "btnDetain";
            this.btnDetain.Size = new System.Drawing.Size(175, 54);
            this.btnDetain.TabIndex = 18;
            this.btnDetain.Text = "Detain";
            this.btnDetain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDetain.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDetain.UseVisualStyleBackColor = true;
            this.btnDetain.Click += new System.EventHandler(this.btnDetain_Click);
            //
            // ctrlFindDriver1
            //
            this.ctrlFindDriver1.Location = new System.Drawing.Point(12, 57);
            this.ctrlFindDriver1.Name = "ctrlFindDriver1";
            this.ctrlFindDriver1.Size = new System.Drawing.Size(1521, 877);
            this.ctrlFindDriver1.TabIndex = 14;
            //
            // errorProvider1
            //
            this.errorProvider1.ContainerControl = this;
            //
            // FrmDetainLicense
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1511, 1201);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnDetain);
            this.Controls.Add(this.linkLabelShowNewLicense);
            this.Controls.Add(this.linklabelShowLicensesHistory);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ctrlFindDriver1);
            this.Controls.Add(this.lbFrmTitle);
            this.Name = "FrmDetainLicense";
            this.Text = "FrmDetainLicense";
            this.Load += new System.EventHandler(this.FrmDetainLicense_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbFrmTitle;
        private ctrlFindDriver ctrlFindDriver1;
        private System.Windows.Forms.LinkLabel linkLabelShowNewLicense;
        private System.Windows.Forms.LinkLabel linklabelShowLicensesHistory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbCreatedBY;
        private System.Windows.Forms.Label lbLicenseID;
        private System.Windows.Forms.Label lbDetainDate;
        private System.Windows.Forms.Label lbDetainID;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnDetain;
        private System.Windows.Forms.MaskedTextBox tbFineFees;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
