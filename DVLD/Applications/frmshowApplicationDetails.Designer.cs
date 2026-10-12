namespace DVLD
{
    partial class frmshowApplicationDetails
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
            this.ctrlLicenseApplication1 = new DVLD.ctrlLicenseApplication();
            this.SuspendLayout();
            //
            // ctrlLicenseApplication1
            //
            this.ctrlLicenseApplication1.localDrivingID = -1;
            this.ctrlLicenseApplication1.Location = new System.Drawing.Point(12, 12);
            this.ctrlLicenseApplication1.Name = "ctrlLicenseApplication1";
            this.ctrlLicenseApplication1.Size = new System.Drawing.Size(996, 491);
            this.ctrlLicenseApplication1.TabIndex = 0;
            //
            // frmshowApplicationDetails
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 518);
            this.Controls.Add(this.ctrlLicenseApplication1);
            this.Name = "frmshowApplicationDetails";
            this.Text = "frmshowApplicationDetails";
            this.Load += new System.EventHandler(this.frmshowApplicationDetails_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlLicenseApplication ctrlLicenseApplication1;
    }
}
