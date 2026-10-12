namespace DVLD
{
    partial class frmShowInternationalLicenseInfo
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
            this.ctrlInternationalDriverLicenseInfo1 = new DVLD.ctrlInternationalDriverLicenseInfo();
            this.SuspendLayout();
            //
            // ctrlInternationalDriverLicenseInfo1
            //
            this.ctrlInternationalDriverLicenseInfo1.INTLicenseID = -1;
            this.ctrlInternationalDriverLicenseInfo1.Location = new System.Drawing.Point(12, 23);
            this.ctrlInternationalDriverLicenseInfo1.Name = "ctrlInternationalDriverLicenseInfo1";
            this.ctrlInternationalDriverLicenseInfo1.Size = new System.Drawing.Size(1277, 632);
            this.ctrlInternationalDriverLicenseInfo1.TabIndex = 0;
            //
            // frmShowInternationalLicenseInfo
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1313, 687);
            this.Controls.Add(this.ctrlInternationalDriverLicenseInfo1);
            this.Name = "frmShowInternationalLicenseInfo";
            this.Text = "frmShowInternationalLicenseInfo";
            this.Load += new System.EventHandler(this.frmShowInternationalLicenseInfo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlInternationalDriverLicenseInfo ctrlInternationalDriverLicenseInfo1;
    }
}
