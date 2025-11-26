using System.Windows.Forms;

namespace ZakiLaundryHouse
{
    partial class formLogin
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

        private void InitializeComponent()
        {
            this.btnExit = new FontAwesome.Sharp.IconButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ucPanel11 = new ZakiLaundryHouse.UcPanel1();
            this.btnLogin = new ZakiLaundryHouse.ucAdmin.NewButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxUsername = new System.Windows.Forms.TextBox();
            this.tbxPassword = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnHidepass = new FontAwesome.Sharp.IconButton();
            this.btnShowpass = new FontAwesome.Sharp.IconButton();
            this.ucPanel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Flip = FontAwesome.Sharp.FlipOrientation.Horizontal;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.IconChar = FontAwesome.Sharp.IconChar.X;
            this.btnExit.IconColor = System.Drawing.Color.White;
            this.btnExit.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btnExit.IconSize = 15;
            this.btnExit.Location = new System.Drawing.Point(816, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(30, 20);
            this.btnExit.TabIndex = 10;
            this.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.xbtn_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Location = new System.Drawing.Point(776, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(83, 20);
            this.panel1.TabIndex = 10;
            // 
            // ucPanel11
            // 
            this.ucPanel11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ucPanel11.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ucPanel11.Controls.Add(this.btnLogin);
            this.ucPanel11.Controls.Add(this.label3);
            this.ucPanel11.Controls.Add(this.label2);
            this.ucPanel11.Controls.Add(this.tbxUsername);
            this.ucPanel11.Controls.Add(this.tbxPassword);
            this.ucPanel11.Controls.Add(this.pictureBox1);
            this.ucPanel11.Controls.Add(this.btnHidepass);
            this.ucPanel11.Controls.Add(this.btnShowpass);
            this.ucPanel11.ForeColor = System.Drawing.Color.White;
            this.ucPanel11.Location = new System.Drawing.Point(216, 79);
            this.ucPanel11.Name = "ucPanel11";
            this.ucPanel11.Size = new System.Drawing.Size(448, 362);
            this.ucPanel11.TabIndex = 8;
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.btnLogin.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.btnLogin.BorderRadius = 10;
            this.btnLogin.BorderSize = 0;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(194, 262);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 27);
            this.btnLogin.TabIndex = 9;
            this.btnLogin.Text = "Login";
            this.btnLogin.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(47, 218);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(47, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username";
            // 
            // tbxUsername
            // 
            this.tbxUsername.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxUsername.Location = new System.Drawing.Point(144, 181);
            this.tbxUsername.Name = "tbxUsername";
            this.tbxUsername.Size = new System.Drawing.Size(193, 24);
            this.tbxUsername.TabIndex = 3;
            // 
            // tbxPassword
            // 
            this.tbxPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxPassword.Location = new System.Drawing.Point(144, 211);
            this.tbxPassword.Name = "tbxPassword";
            this.tbxPassword.PasswordChar = '*';
            this.tbxPassword.Size = new System.Drawing.Size(193, 24);
            this.tbxPassword.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::ZakiLaundryHouse.Properties.Resources.Zaki;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(118, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(232, 165);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // btnHidepass
            // 
            this.btnHidepass.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnHidepass.BackColor = System.Drawing.Color.Transparent;
            this.btnHidepass.FlatAppearance.BorderSize = 0;
            this.btnHidepass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHidepass.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            this.btnHidepass.IconColor = System.Drawing.Color.Black;
            this.btnHidepass.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnHidepass.IconSize = 23;
            this.btnHidepass.Location = new System.Drawing.Point(343, 214);
            this.btnHidepass.Name = "btnHidepass";
            this.btnHidepass.Size = new System.Drawing.Size(31, 21);
            this.btnHidepass.TabIndex = 7;
            this.btnHidepass.UseVisualStyleBackColor = false;
            this.btnHidepass.Click += new System.EventHandler(this.btnHidepass_Click);
            // 
            // btnShowpass
            // 
            this.btnShowpass.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnShowpass.BackColor = System.Drawing.Color.Transparent;
            this.btnShowpass.FlatAppearance.BorderSize = 0;
            this.btnShowpass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowpass.IconChar = FontAwesome.Sharp.IconChar.Eye;
            this.btnShowpass.IconColor = System.Drawing.Color.Black;
            this.btnShowpass.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnShowpass.IconSize = 23;
            this.btnShowpass.Location = new System.Drawing.Point(343, 214);
            this.btnShowpass.Name = "btnShowpass";
            this.btnShowpass.Size = new System.Drawing.Size(31, 21);
            this.btnShowpass.TabIndex = 6;
            this.btnShowpass.UseVisualStyleBackColor = false;
            this.btnShowpass.Click += new System.EventHandler(this.btnShowpass_Click);
            // 
            // formLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::ZakiLaundryHouse.Properties.Resources.Login;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(858, 535);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.ucPanel11);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(604, 386);
            this.Name = "formLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.formLogin_Load);
            this.ucPanel11.ResumeLayout(false);
            this.ucPanel11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxUsername;
        private System.Windows.Forms.TextBox tbxPassword;
        private FontAwesome.Sharp.IconButton btnShowpass;
        private FontAwesome.Sharp.IconButton btnHidepass;
        private UcPanel1 ucPanel11;
        private PictureBox pictureBox1;
        private FontAwesome.Sharp.IconButton btnExit;
        private Panel panel1;
        private ucAdmin.NewButton btnLogin;
    }
}
