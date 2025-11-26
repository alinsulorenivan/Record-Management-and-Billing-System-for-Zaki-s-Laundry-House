namespace ZakiLaundryHouse.ucAdmin
{
    partial class ucPaymentForm
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
            this.tbxCustName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbxPaymentMethod = new System.Windows.Forms.ComboBox();
            this.tbxTotAmount = new System.Windows.Forms.TextBox();
            this.tbxAmountReceived = new System.Windows.Forms.TextBox();
            this.tbxChange = new System.Windows.Forms.TextBox();
            this.btnBack = new FontAwesome.Sharp.IconButton();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.lblAmountReceivedValidation = new System.Windows.Forms.Label();
            this.tbxReferenceNumber = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnViewSummary = new ZakiLaundryHouse.ucAdmin.NewButton();
            this.newButton1 = new ZakiLaundryHouse.ucAdmin.NewButton();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbxCustName
            // 
            this.tbxCustName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxCustName.Location = new System.Drawing.Point(265, 87);
            this.tbxCustName.Multiline = true;
            this.tbxCustName.Name = "tbxCustName";
            this.tbxCustName.Size = new System.Drawing.Size(218, 27);
            this.tbxCustName.TabIndex = 58;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(123, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 21);
            this.label3.TabIndex = 57;
            this.label3.Text = "Customer Name:";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(44, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 35);
            this.label1.TabIndex = 55;
            this.label1.Text = "Payment Form";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(117, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 21);
            this.label2.TabIndex = 68;
            this.label2.Text = "Total Amount Due:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(123, 205);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 21);
            this.label4.TabIndex = 70;
            this.label4.Text = "Payment Method:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(123, 306);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 21);
            this.label5.TabIndex = 72;
            this.label5.Text = "Amount Received:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(123, 359);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 21);
            this.label6.TabIndex = 74;
            this.label6.Text = "Change:";
            // 
            // cbxPaymentMethod
            // 
            this.cbxPaymentMethod.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbxPaymentMethod.FormattingEnabled = true;
            this.cbxPaymentMethod.Location = new System.Drawing.Point(265, 205);
            this.cbxPaymentMethod.Name = "cbxPaymentMethod";
            this.cbxPaymentMethod.Size = new System.Drawing.Size(218, 21);
            this.cbxPaymentMethod.TabIndex = 76;
            // 
            // tbxTotAmount
            // 
            this.tbxTotAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxTotAmount.Location = new System.Drawing.Point(265, 143);
            this.tbxTotAmount.Multiline = true;
            this.tbxTotAmount.Name = "tbxTotAmount";
            this.tbxTotAmount.Size = new System.Drawing.Size(218, 27);
            this.tbxTotAmount.TabIndex = 77;
            // 
            // tbxAmountReceived
            // 
            this.tbxAmountReceived.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxAmountReceived.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxAmountReceived.Location = new System.Drawing.Point(265, 300);
            this.tbxAmountReceived.Multiline = true;
            this.tbxAmountReceived.Name = "tbxAmountReceived";
            this.tbxAmountReceived.Size = new System.Drawing.Size(218, 27);
            this.tbxAmountReceived.TabIndex = 78;
            this.tbxAmountReceived.TextChanged += new System.EventHandler(this.tbxAmountReceived_TextChanged_1);
            // 
            // tbxChange
            // 
            this.tbxChange.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxChange.Location = new System.Drawing.Point(265, 358);
            this.tbxChange.Multiline = true;
            this.tbxChange.Name = "tbxChange";
            this.tbxChange.Size = new System.Drawing.Size(218, 27);
            this.tbxChange.TabIndex = 79;
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.IconChar = FontAwesome.Sharp.IconChar.MailReply;
            this.btnBack.IconColor = System.Drawing.Color.Black;
            this.btnBack.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnBack.IconSize = 35;
            this.btnBack.Location = new System.Drawing.Point(539, 13);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(47, 34);
            this.btnBack.TabIndex = 80;
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.CashRegister;
            this.iconPictureBox1.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.Location = new System.Drawing.Point(12, 24);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(32, 32);
            this.iconPictureBox1.TabIndex = 82;
            this.iconPictureBox1.TabStop = false;
            // 
            // lblAmountReceivedValidation
            // 
            this.lblAmountReceivedValidation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblAmountReceivedValidation.AutoSize = true;
            this.lblAmountReceivedValidation.BackColor = System.Drawing.Color.Transparent;
            this.lblAmountReceivedValidation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmountReceivedValidation.ForeColor = System.Drawing.Color.Red;
            this.lblAmountReceivedValidation.Location = new System.Drawing.Point(262, 330);
            this.lblAmountReceivedValidation.Name = "lblAmountReceivedValidation";
            this.lblAmountReceivedValidation.Size = new System.Drawing.Size(102, 13);
            this.lblAmountReceivedValidation.TabIndex = 114;
            this.lblAmountReceivedValidation.Text = "No Results Found!";
            this.lblAmountReceivedValidation.Visible = false;
            // 
            // tbxReferenceNumber
            // 
            this.tbxReferenceNumber.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxReferenceNumber.Location = new System.Drawing.Point(265, 250);
            this.tbxReferenceNumber.Multiline = true;
            this.tbxReferenceNumber.Name = "tbxReferenceNumber";
            this.tbxReferenceNumber.Size = new System.Drawing.Size(218, 27);
            this.tbxReferenceNumber.TabIndex = 115;
            this.tbxReferenceNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxReferenceNumber_KeyPress);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(117, 256);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(136, 21);
            this.label7.TabIndex = 116;
            this.label7.Text = "Reference Number:";
            // 
            // btnViewSummary
            // 
            this.btnViewSummary.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnViewSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(101)))), ((int)(((byte)(159)))));
            this.btnViewSummary.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(101)))), ((int)(((byte)(159)))));
            this.btnViewSummary.BorderRadius = 10;
            this.btnViewSummary.BorderSize = 0;
            this.btnViewSummary.FlatAppearance.BorderSize = 0;
            this.btnViewSummary.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnViewSummary.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnViewSummary.ForeColor = System.Drawing.Color.White;
            this.btnViewSummary.Location = new System.Drawing.Point(126, 416);
            this.btnViewSummary.Name = "btnViewSummary";
            this.btnViewSummary.Size = new System.Drawing.Size(155, 27);
            this.btnViewSummary.TabIndex = 86;
            this.btnViewSummary.Text = "View Order Summary";
            this.btnViewSummary.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(101)))), ((int)(((byte)(159)))));
            this.btnViewSummary.UseVisualStyleBackColor = false;
            this.btnViewSummary.Click += new System.EventHandler(this.btnViewSummary_Click);
            // 
            // newButton1
            // 
            this.newButton1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.newButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(110)))), ((int)(((byte)(253)))));
            this.newButton1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(110)))), ((int)(((byte)(253)))));
            this.newButton1.BorderRadius = 10;
            this.newButton1.BorderSize = 0;
            this.newButton1.FlatAppearance.BorderSize = 0;
            this.newButton1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.newButton1.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this.newButton1.ForeColor = System.Drawing.Color.White;
            this.newButton1.Location = new System.Drawing.Point(328, 416);
            this.newButton1.Name = "newButton1";
            this.newButton1.Size = new System.Drawing.Size(155, 27);
            this.newButton1.TabIndex = 85;
            this.newButton1.Text = "Confirm and Print Receipt";
            this.newButton1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(110)))), ((int)(((byte)(253)))));
            this.newButton1.UseVisualStyleBackColor = false;
            this.newButton1.Click += new System.EventHandler(this.newButton1_Click);
            // 
            // ucPaymentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbxReferenceNumber);
            this.Controls.Add(this.lblAmountReceivedValidation);
            this.Controls.Add(this.btnViewSummary);
            this.Controls.Add(this.newButton1);
            this.Controls.Add(this.iconPictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.tbxChange);
            this.Controls.Add(this.tbxAmountReceived);
            this.Controls.Add(this.tbxTotAmount);
            this.Controls.Add(this.cbxPaymentMethod);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxCustName);
            this.Controls.Add(this.label3);
            this.Name = "ucPaymentForm";
            this.Size = new System.Drawing.Size(598, 490);
            this.Load += new System.EventHandler(this.ucPaymentForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbxCustName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbxPaymentMethod;
        private System.Windows.Forms.TextBox tbxTotAmount;
        private System.Windows.Forms.TextBox tbxAmountReceived;
        private System.Windows.Forms.TextBox tbxChange;
        private FontAwesome.Sharp.IconButton btnBack;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private NewButton newButton1;
        private NewButton btnViewSummary;
        private System.Windows.Forms.Label lblAmountReceivedValidation;
        private System.Windows.Forms.TextBox tbxReferenceNumber;
        private System.Windows.Forms.Label label7;
    }
}
