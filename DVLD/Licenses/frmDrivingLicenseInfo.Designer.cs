namespace DVLD
{
    partial class frmDrivingLicenseInfo
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
            this.ctrlLicenseInfo1 = new DVLD.ctrlLicenseInfo();
            this.SuspendLayout();
            //
            // ctrlLicenseInfo1
            //
            this.ctrlLicenseInfo1.Location = new System.Drawing.Point(12, 12);
            this.ctrlLicenseInfo1.Name = "ctrlLicenseInfo1";
            this.ctrlLicenseInfo1.Size = new System.Drawing.Size(1453, 629);
            this.ctrlLicenseInfo1.TabIndex = 0;
            //
            // frmDrivingLicenseInfo
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1494, 675);
            this.Controls.Add(this.ctrlLicenseInfo1);
            this.Name = "frmDrivingLicenseInfo";
            this.Text = "frmDrivingLicenseInfo";
            this.Load += new System.EventHandler(this.frmDrivingLicenseInfo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlLicenseInfo ctrlLicenseInfo1;
    }
}
