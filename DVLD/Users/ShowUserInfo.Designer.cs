namespace DVLD
{
    partial class ShowUserInfo
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
            this.ctrlUserInfo1 = new DVLD.ctrlUserInfo();
            this.SuspendLayout();
            //
            // ctrlUserInfo1
            //
            this.ctrlUserInfo1.Location = new System.Drawing.Point(12, 31);
            this.ctrlUserInfo1.Name = "ctrlUserInfo1";
            this.ctrlUserInfo1.Size = new System.Drawing.Size(1296, 692);
            this.ctrlUserInfo1.TabIndex = 0;
            //
            // ShowUserInfo
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1339, 779);
            this.Controls.Add(this.ctrlUserInfo1);
            this.Name = "ShowUserInfo";
            this.Text = "ShowUserInfo";
            this.ResumeLayout(false);

        }



        #endregion

        private ctrlUserInfo ctrlUserInfo1;
    }
}
