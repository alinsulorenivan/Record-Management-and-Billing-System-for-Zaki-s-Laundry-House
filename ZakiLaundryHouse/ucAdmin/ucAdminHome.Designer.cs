namespace ZakiLaundryHouse.ucAdmin
{
    partial class ucAdminHome
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
            this.components = new System.ComponentModel.Container();
            this.panelPopup = new System.Windows.Forms.Panel();
            this.lblAdminName = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.timerSlide = new System.Windows.Forms.Timer(this.components);
            this.panelPopupStaff = new System.Windows.Forms.Panel();
            this.lblStaffName = new System.Windows.Forms.Label();
            this.timerSlideStaff = new System.Windows.Forms.Timer(this.components);
            this.panelPopupDown = new System.Windows.Forms.Panel();
            this.timerSlideDown = new System.Windows.Forms.Timer(this.components);
            this.panelPopupDownStaff = new System.Windows.Forms.Panel();
            this.btnLogoutStaff = new System.Windows.Forms.Button();
            this.timerSlideDownStaff = new System.Windows.Forms.Timer(this.components);
            this.btnProfileAdmin = new FontAwesome.Sharp.IconButton();
            this.btnProfileStaff = new System.Windows.Forms.Button();
            this.panelPopup.SuspendLayout();
            this.panelPopupStaff.SuspendLayout();
            this.panelPopupDown.SuspendLayout();
            this.panelPopupDownStaff.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPopup
            // 
            this.panelPopup.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panelPopup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.panelPopup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPopup.Controls.Add(this.lblAdminName);
            this.panelPopup.Location = new System.Drawing.Point(587, 37);
            this.panelPopup.Name = "panelPopup";
            this.panelPopup.Size = new System.Drawing.Size(117, 46);
            this.panelPopup.TabIndex = 6;
            // 
            // lblAdminName
            // 
            this.lblAdminName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAdminName.AutoSize = true;
            this.lblAdminName.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdminName.ForeColor = System.Drawing.Color.White;
            this.lblAdminName.Location = new System.Drawing.Point(28, 8);
            this.lblAdminName.Name = "lblAdminName";
            this.lblAdminName.Size = new System.Drawing.Size(69, 25);
            this.lblAdminName.TabIndex = 1;
            this.lblAdminName.Text = "Admin";
            this.lblAdminName.Click += new System.EventHandler(this.lblAdminName_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.Location = new System.Drawing.Point(-1, -1);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(167, 42);
            this.btnLogout.TabIndex = 40;
            this.btnLogout.Text = "Log Out";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click_1);
            // 
            // timerSlide
            // 
            this.timerSlide.Tick += new System.EventHandler(this.timerSlide_Tick);
            // 
            // panelPopupStaff
            // 
            this.panelPopupStaff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPopupStaff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.panelPopupStaff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPopupStaff.Controls.Add(this.lblStaffName);
            this.panelPopupStaff.Location = new System.Drawing.Point(587, 37);
            this.panelPopupStaff.Name = "panelPopupStaff";
            this.panelPopupStaff.Size = new System.Drawing.Size(117, 46);
            this.panelPopupStaff.TabIndex = 8;
            // 
            // lblStaffName
            // 
            this.lblStaffName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStaffName.AutoSize = true;
            this.lblStaffName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStaffName.ForeColor = System.Drawing.Color.White;
            this.lblStaffName.Location = new System.Drawing.Point(35, 7);
            this.lblStaffName.Name = "lblStaffName";
            this.lblStaffName.Size = new System.Drawing.Size(44, 24);
            this.lblStaffName.TabIndex = 1;
            this.lblStaffName.Text = "Staff";
            // 
            // timerSlideStaff
            // 
            this.timerSlideStaff.Tick += new System.EventHandler(this.timerSlideStaff_Tick);
            // 
            // panelPopupDown
            // 
            this.panelPopupDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPopupDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.panelPopupDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPopupDown.Controls.Add(this.btnLogout);
            this.panelPopupDown.Location = new System.Drawing.Point(581, 85);
            this.panelPopupDown.Name = "panelPopupDown";
            this.panelPopupDown.Size = new System.Drawing.Size(167, 46);
            this.panelPopupDown.TabIndex = 7;
            this.panelPopupDown.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // timerSlideDown
            // 
            this.timerSlideDown.Tick += new System.EventHandler(this.timerSlideDown_Tick);
            // 
            // panelPopupDownStaff
            // 
            this.panelPopupDownStaff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPopupDownStaff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.panelPopupDownStaff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPopupDownStaff.Controls.Add(this.btnLogoutStaff);
            this.panelPopupDownStaff.Location = new System.Drawing.Point(587, 85);
            this.panelPopupDownStaff.Name = "panelPopupDownStaff";
            this.panelPopupDownStaff.Size = new System.Drawing.Size(161, 46);
            this.panelPopupDownStaff.TabIndex = 41;
            // 
            // btnLogoutStaff
            // 
            this.btnLogoutStaff.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLogoutStaff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogoutStaff.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogoutStaff.Location = new System.Drawing.Point(-5, -1);
            this.btnLogoutStaff.Name = "btnLogoutStaff";
            this.btnLogoutStaff.Size = new System.Drawing.Size(165, 42);
            this.btnLogoutStaff.TabIndex = 42;
            this.btnLogoutStaff.Text = "Log Out";
            this.btnLogoutStaff.UseVisualStyleBackColor = false;
            this.btnLogoutStaff.Click += new System.EventHandler(this.btnLogoutStaff_Click_1);
            // 
            // timerSlideDownStaff
            // 
            this.timerSlideDownStaff.Tick += new System.EventHandler(this.timerSlideDownStaff_Tick);
            // 
            // btnProfileAdmin
            // 
            this.btnProfileAdmin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProfileAdmin.BackColor = System.Drawing.Color.Transparent;
            this.btnProfileAdmin.BackgroundImage = global::ZakiLaundryHouse.Properties.Resources.boss__2_;
            this.btnProfileAdmin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnProfileAdmin.FlatAppearance.BorderSize = 0;
            this.btnProfileAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProfileAdmin.IconChar = FontAwesome.Sharp.IconChar.None;
            this.btnProfileAdmin.IconColor = System.Drawing.Color.Black;
            this.btnProfileAdmin.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnProfileAdmin.Location = new System.Drawing.Point(701, 37);
            this.btnProfileAdmin.Name = "btnProfileAdmin";
            this.btnProfileAdmin.Size = new System.Drawing.Size(47, 46);
            this.btnProfileAdmin.TabIndex = 39;
            this.btnProfileAdmin.UseVisualStyleBackColor = false;
            this.btnProfileAdmin.Click += new System.EventHandler(this.btnProfileAdmin_Click);
            // 
            // btnProfileStaff
            // 
            this.btnProfileStaff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProfileStaff.BackColor = System.Drawing.Color.Transparent;
            this.btnProfileStaff.BackgroundImage = global::ZakiLaundryHouse.Properties.Resources.user__2_;
            this.btnProfileStaff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnProfileStaff.FlatAppearance.BorderSize = 0;
            this.btnProfileStaff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProfileStaff.Location = new System.Drawing.Point(701, 37);
            this.btnProfileStaff.Name = "btnProfileStaff";
            this.btnProfileStaff.Size = new System.Drawing.Size(47, 46);
            this.btnProfileStaff.TabIndex = 40;
            this.btnProfileStaff.UseVisualStyleBackColor = false;
            this.btnProfileStaff.Click += new System.EventHandler(this.btnProfileStaff_Click_2);
            // 
            // ucAdminHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.panelPopup);
            this.Controls.Add(this.btnProfileAdmin);
            this.Controls.Add(this.panelPopupStaff);
            this.Controls.Add(this.btnProfileStaff);
            this.Controls.Add(this.panelPopupDown);
            this.Controls.Add(this.panelPopupDownStaff);
            this.Name = "ucAdminHome";
            this.Size = new System.Drawing.Size(771, 490);
            this.Load += new System.EventHandler(this.ucAdminHome_Load);
            this.panelPopup.ResumeLayout(false);
            this.panelPopup.PerformLayout();
            this.panelPopupStaff.ResumeLayout(false);
            this.panelPopupStaff.PerformLayout();
            this.panelPopupDown.ResumeLayout(false);
            this.panelPopupDownStaff.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelPopup;
        private System.Windows.Forms.Label lblAdminName;
        private System.Windows.Forms.Timer timerSlide;
        private System.Windows.Forms.Panel panelPopupStaff;
        private System.Windows.Forms.Label lblStaffName;
        private System.Windows.Forms.Timer timerSlideStaff;
        private FontAwesome.Sharp.IconButton btnProfileAdmin;
        private System.Windows.Forms.Panel panelPopupDown;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Timer timerSlideDown;
        private System.Windows.Forms.Button btnProfileStaff;
        private System.Windows.Forms.Panel panelPopupDownStaff;
        private System.Windows.Forms.Button btnLogoutStaff;
        private System.Windows.Forms.Timer timerSlideDownStaff;
    }
}
