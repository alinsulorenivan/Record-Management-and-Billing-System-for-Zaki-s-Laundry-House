namespace ZakiLaundryHouse.ucAdmin
{
    partial class SearchResultControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblContact = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.picProfile = new ZakiLaundryHouse.RoundPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // lblContact
            // 
            this.lblContact.AutoSize = true;
            this.lblContact.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContact.ForeColor = System.Drawing.Color.DimGray;
            this.lblContact.Location = new System.Drawing.Point(54, 19);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(96, 15);
            this.lblContact.TabIndex = 3;
            this.lblContact.Text = "Contact Number";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(52, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(118, 16);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Complete Name";
            // 
            // picProfile
            // 
            this.picProfile.Image = global::ZakiLaundryHouse.Properties.Resources.profilePic;
            this.picProfile.Location = new System.Drawing.Point(7, 0);
            this.picProfile.Name = "picProfile";
            this.picProfile.Size = new System.Drawing.Size(37, 37);
            this.picProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picProfile.TabIndex = 4;
            this.picProfile.TabStop = false;
            // 
            // SearchResultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picProfile);
            this.Controls.Add(this.lblContact);
            this.Controls.Add(this.lblName);
            this.Name = "SearchResultControl";
            this.Size = new System.Drawing.Size(257, 40);
            this.Load += new System.EventHandler(this.SearchResultControl_Load);
            this.MouseLeave += new System.EventHandler(this.SearchResultControl_MouseLeave);
            this.MouseHover += new System.EventHandler(this.SearchResultControl_MouseHover);
            ((System.ComponentModel.ISupportInitialize)(this.picProfile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.Label lblName;
        private RoundPictureBox picProfile;
    }
}
