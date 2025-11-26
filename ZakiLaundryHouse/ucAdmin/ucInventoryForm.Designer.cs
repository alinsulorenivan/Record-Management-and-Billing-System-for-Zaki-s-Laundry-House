namespace ZakiLaundryHouse.ucAdmin
{
    partial class ucInventoryForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbxItemName = new System.Windows.Forms.TextBox();
            this.tbxUnit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBack = new FontAwesome.Sharp.IconButton();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxItemID = new System.Windows.Forms.TextBox();
            this.cbxCategory = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbxLowStockLevel = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbxAvailableStock = new System.Windows.Forms.TextBox();
            this.btnRestock = new ZakiLaundryHouse.ucAdmin.NewButton();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(44, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 35);
            this.label1.TabIndex = 20;
            this.label1.Text = "Inventory Form";
            // 
            // tbxItemName
            // 
            this.tbxItemName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxItemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxItemName.Location = new System.Drawing.Point(108, 175);
            this.tbxItemName.Name = "tbxItemName";
            this.tbxItemName.Size = new System.Drawing.Size(212, 31);
            this.tbxItemName.TabIndex = 45;
            this.tbxItemName.TextChanged += new System.EventHandler(this.tbxItemName_TextChanged);
            this.tbxItemName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxItemName_KeyPress);
            this.tbxItemName.Leave += new System.EventHandler(this.tbxItemName_Leave);
            // 
            // tbxUnit
            // 
            this.tbxUnit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxUnit.Cursor = System.Windows.Forms.Cursors.Default;
            this.tbxUnit.Enabled = false;
            this.tbxUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxUnit.Location = new System.Drawing.Point(108, 310);
            this.tbxUnit.Name = "tbxUnit";
            this.tbxUnit.ReadOnly = true;
            this.tbxUnit.Size = new System.Drawing.Size(212, 31);
            this.tbxUnit.TabIndex = 47;
            this.tbxUnit.Text = "pcs";
            this.tbxUnit.TextChanged += new System.EventHandler(this.tbxUnit_TextChanged);
            this.tbxUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxUnit_KeyPress);
            this.tbxUnit.Leave += new System.EventHandler(this.tbxUnit_Leave);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(104, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 21);
            this.label2.TabIndex = 50;
            this.label2.Text = "Item Name";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(104, 223);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 21);
            this.label4.TabIndex = 51;
            this.label4.Text = "Category";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(104, 286);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 21);
            this.label5.TabIndex = 52;
            this.label5.Text = "Unit";
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
            this.btnBack.Location = new System.Drawing.Point(365, 19);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(47, 34);
            this.btnBack.TabIndex = 81;
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.BoxesStacked;
            this.iconPictureBox1.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.Location = new System.Drawing.Point(12, 24);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(32, 32);
            this.iconPictureBox1.TabIndex = 82;
            this.iconPictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(104, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 21);
            this.label3.TabIndex = 38;
            this.label3.Text = "Item ID";
            // 
            // tbxItemID
            // 
            this.tbxItemID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxItemID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxItemID.Enabled = false;
            this.tbxItemID.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxItemID.Location = new System.Drawing.Point(108, 107);
            this.tbxItemID.Name = "tbxItemID";
            this.tbxItemID.Size = new System.Drawing.Size(212, 31);
            this.tbxItemID.TabIndex = 44;
            this.tbxItemID.TextChanged += new System.EventHandler(this.tbxItemID_TextChanged);
            // 
            // cbxCategory
            // 
            this.cbxCategory.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCategory.FormattingEnabled = true;
            this.cbxCategory.Location = new System.Drawing.Point(108, 247);
            this.cbxCategory.Name = "cbxCategory";
            this.cbxCategory.Size = new System.Drawing.Size(212, 28);
            this.cbxCategory.TabIndex = 83;
            this.cbxCategory.SelectedIndexChanged += new System.EventHandler(this.cbxCategory_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(104, 429);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(189, 21);
            this.label6.TabIndex = 86;
            this.label6.Text = "Low Stock Alert Level";
            // 
            // tbxLowStockLevel
            // 
            this.tbxLowStockLevel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxLowStockLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxLowStockLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxLowStockLevel.Location = new System.Drawing.Point(108, 453);
            this.tbxLowStockLevel.Name = "tbxLowStockLevel";
            this.tbxLowStockLevel.Size = new System.Drawing.Size(212, 31);
            this.tbxLowStockLevel.TabIndex = 85;
            this.tbxLowStockLevel.Text = "10";
            this.tbxLowStockLevel.TextChanged += new System.EventHandler(this.tbxLowStockLevel_TextChanged);
            this.tbxLowStockLevel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxLowStockLevel_KeyPress);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(104, 359);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 21);
            this.label7.TabIndex = 88;
            this.label7.Text = "Available Stock";
            // 
            // tbxAvailableStock
            // 
            this.tbxAvailableStock.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxAvailableStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxAvailableStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxAvailableStock.Location = new System.Drawing.Point(108, 383);
            this.tbxAvailableStock.Name = "tbxAvailableStock";
            this.tbxAvailableStock.Size = new System.Drawing.Size(212, 31);
            this.tbxAvailableStock.TabIndex = 87;
            this.tbxAvailableStock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxAvailableStock_KeyPress);
            // 
            // btnRestock
            // 
            this.btnRestock.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRestock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.btnRestock.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.btnRestock.BorderRadius = 10;
            this.btnRestock.BorderSize = 0;
            this.btnRestock.FlatAppearance.BorderSize = 0;
            this.btnRestock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestock.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnRestock.ForeColor = System.Drawing.Color.White;
            this.btnRestock.Location = new System.Drawing.Point(151, 503);
            this.btnRestock.Name = "btnRestock";
            this.btnRestock.Size = new System.Drawing.Size(119, 43);
            this.btnRestock.TabIndex = 84;
            this.btnRestock.Text = "Add";
            this.btnRestock.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.btnRestock.UseVisualStyleBackColor = false;
            this.btnRestock.Click += new System.EventHandler(this.btnRestock_Click);
            // 
            // ucInventoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbxAvailableStock);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbxLowStockLevel);
            this.Controls.Add(this.btnRestock);
            this.Controls.Add(this.cbxCategory);
            this.Controls.Add(this.iconPictureBox1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxUnit);
            this.Controls.Add(this.tbxItemName);
            this.Controls.Add(this.tbxItemID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "ucInventoryForm";
            this.Size = new System.Drawing.Size(427, 563);
            this.Load += new System.EventHandler(this.ucInventoryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxItemName;
        private System.Windows.Forms.TextBox tbxUnit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private FontAwesome.Sharp.IconButton btnBack;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxItemID;
        private System.Windows.Forms.ComboBox cbxCategory;
        private NewButton btnRestock;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbxLowStockLevel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbxAvailableStock;
    }
}
