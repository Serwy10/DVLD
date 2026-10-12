namespace DVLD
{
    partial class ShowPersonDetailscs
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
            this.ctrlShowDetailscs1 = new DVLD.CtrlShowDetailscs();
            this.SuspendLayout();
            //
            // ctrlShowDetailscs1
            //
            this.ctrlShowDetailscs1._personID = 0;
            this.ctrlShowDetailscs1.Location = new System.Drawing.Point(12, 8);
            this.ctrlShowDetailscs1.Name = "ctrlShowDetailscs1";
            this.ctrlShowDetailscs1.Size = new System.Drawing.Size(1098, 430);
            this.ctrlShowDetailscs1.TabIndex = 0;
            //
            // ShowPersonDetailscs
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1136, 485);
            this.Controls.Add(this.ctrlShowDetailscs1);
            this.Name = "ShowPersonDetailscs";
            this.Text = "ShowPersonDetailscs";
            this.ResumeLayout(false);

        }

        #endregion

        private CtrlShowDetailscs ctrlShowDetailscs1;
    }
}
