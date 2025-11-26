namespace ZakiLaundryHouse.ucAdmin
{

    partial class ucOrderForm
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
            this.panelOrderForm = new System.Windows.Forms.Panel();
            this.lblCustomerNameValidation = new System.Windows.Forms.Label();
            this.lblWeightValidation = new System.Windows.Forms.Label();
            this.lblAddressValidation = new System.Windows.Forms.Label();
            this.lblContactValidation = new System.Windows.Forms.Label();
            this.panelSuggestion = new System.Windows.Forms.Panel();
            this.btnAddList = new ZakiLaundryHouse.ucAdmin.NewButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxItemName = new System.Windows.Forms.ComboBox();
            this.tbxCustomerName = new System.Windows.Forms.TextBox();
            this.tbxQtyOthers = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxWeightItems = new System.Windows.Forms.TextBox();
            this.tbxContactNum = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxAddress = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbxNotes = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxCategory = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.dtgOrderForm = new System.Windows.Forms.DataGridView();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Items = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.btnCancel = new ZakiLaundryHouse.ucAdmin.NewButton();
            this.btnPayment = new ZakiLaundryHouse.ucAdmin.NewButton();
            this.btnSave = new ZakiLaundryHouse.ucAdmin.NewButton();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panelOrderForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgOrderForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(44, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 35);
            this.label1.TabIndex = 92;
            this.label1.Text = "Order Form";
            // 
            // panelOrderForm
            // 
            this.panelOrderForm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelOrderForm.BackColor = System.Drawing.SystemColors.Window;
            this.panelOrderForm.Controls.Add(this.lblCustomerNameValidation);
            this.panelOrderForm.Controls.Add(this.lblWeightValidation);
            this.panelOrderForm.Controls.Add(this.lblAddressValidation);
            this.panelOrderForm.Controls.Add(this.lblContactValidation);
            this.panelOrderForm.Controls.Add(this.panelSuggestion);
            this.panelOrderForm.Controls.Add(this.btnAddList);
            this.panelOrderForm.Controls.Add(this.label2);
            this.panelOrderForm.Controls.Add(this.cbxItemName);
            this.panelOrderForm.Controls.Add(this.tbxCustomerName);
            this.panelOrderForm.Controls.Add(this.tbxQtyOthers);
            this.panelOrderForm.Controls.Add(this.label9);
            this.panelOrderForm.Controls.Add(this.label14);
            this.panelOrderForm.Controls.Add(this.label3);
            this.panelOrderForm.Controls.Add(this.tbxWeightItems);
            this.panelOrderForm.Controls.Add(this.tbxContactNum);
            this.panelOrderForm.Controls.Add(this.label7);
            this.panelOrderForm.Controls.Add(this.label4);
            this.panelOrderForm.Controls.Add(this.tbxAddress);
            this.panelOrderForm.Controls.Add(this.label6);
            this.panelOrderForm.Controls.Add(this.tbxNotes);
            this.panelOrderForm.Controls.Add(this.label8);
            this.panelOrderForm.Controls.Add(this.cbxCategory);
            this.panelOrderForm.Location = new System.Drawing.Point(3, 51);
            this.panelOrderForm.Name = "panelOrderForm";
            this.panelOrderForm.Size = new System.Drawing.Size(306, 399);
            this.panelOrderForm.TabIndex = 109;
            // 
            // lblCustomerNameValidation
            // 
            this.lblCustomerNameValidation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCustomerNameValidation.AutoSize = true;
            this.lblCustomerNameValidation.BackColor = System.Drawing.SystemColors.Window;
            this.lblCustomerNameValidation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerNameValidation.ForeColor = System.Drawing.Color.Red;
            this.lblCustomerNameValidation.Location = new System.Drawing.Point(12, 58);
            this.lblCustomerNameValidation.Name = "lblCustomerNameValidation";
            this.lblCustomerNameValidation.Size = new System.Drawing.Size(102, 13);
            this.lblCustomerNameValidation.TabIndex = 116;
            this.lblCustomerNameValidation.Text = "No Results Found!";
            this.lblCustomerNameValidation.Visible = false;
            // 
            // lblWeightValidation
            // 
            this.lblWeightValidation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWeightValidation.AutoSize = true;
            this.lblWeightValidation.BackColor = System.Drawing.Color.Transparent;
            this.lblWeightValidation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWeightValidation.ForeColor = System.Drawing.Color.Red;
            this.lblWeightValidation.Location = new System.Drawing.Point(13, 347);
            this.lblWeightValidation.Name = "lblWeightValidation";
            this.lblWeightValidation.Size = new System.Drawing.Size(102, 13);
            this.lblWeightValidation.TabIndex = 115;
            this.lblWeightValidation.Text = "No Results Found!";
            this.lblWeightValidation.Visible = false;
            // 
            // lblAddressValidation
            // 
            this.lblAddressValidation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAddressValidation.AutoSize = true;
            this.lblAddressValidation.BackColor = System.Drawing.Color.Transparent;
            this.lblAddressValidation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddressValidation.ForeColor = System.Drawing.Color.Red;
            this.lblAddressValidation.Location = new System.Drawing.Point(13, 178);
            this.lblAddressValidation.Name = "lblAddressValidation";
            this.lblAddressValidation.Size = new System.Drawing.Size(102, 13);
            this.lblAddressValidation.TabIndex = 114;
            this.lblAddressValidation.Text = "No Results Found!";
            this.lblAddressValidation.Visible = false;
            // 
            // lblContactValidation
            // 
            this.lblContactValidation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblContactValidation.AutoSize = true;
            this.lblContactValidation.BackColor = System.Drawing.SystemColors.Window;
            this.lblContactValidation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContactValidation.ForeColor = System.Drawing.Color.Red;
            this.lblContactValidation.Location = new System.Drawing.Point(13, 118);
            this.lblContactValidation.Name = "lblContactValidation";
            this.lblContactValidation.Size = new System.Drawing.Size(102, 13);
            this.lblContactValidation.TabIndex = 113;
            this.lblContactValidation.Text = "No Results Found!";
            this.lblContactValidation.Visible = false;
            // 
            // panelSuggestion
            // 
            this.panelSuggestion.BackColor = System.Drawing.Color.Lavender;
            this.panelSuggestion.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelSuggestion.Location = new System.Drawing.Point(15, 58);
            this.panelSuggestion.Name = "panelSuggestion";
            this.panelSuggestion.Size = new System.Drawing.Size(275, 0);
            this.panelSuggestion.TabIndex = 91;
            this.panelSuggestion.Visible = false;
            this.panelSuggestion.Paint += new System.Windows.Forms.PaintEventHandler(this.panelSuggestion_Paint);
            // 
            // btnAddList
            // 
            this.btnAddList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(60)))), ((int)(((byte)(93)))));
            this.btnAddList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(60)))), ((int)(((byte)(93)))));
            this.btnAddList.BorderRadius = 10;
            this.btnAddList.BorderSize = 0;
            this.btnAddList.FlatAppearance.BorderSize = 0;
            this.btnAddList.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddList.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnAddList.ForeColor = System.Drawing.Color.White;
            this.btnAddList.Location = new System.Drawing.Point(184, 316);
            this.btnAddList.Name = "btnAddList";
            this.btnAddList.Size = new System.Drawing.Size(106, 29);
            this.btnAddList.TabIndex = 112;
            this.btnAddList.Text = "Add to List";
            this.btnAddList.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(60)))), ((int)(((byte)(93)))));
            this.btnAddList.UseVisualStyleBackColor = false;
            this.btnAddList.Click += new System.EventHandler(this.btnAddList_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 17);
            this.label2.TabIndex = 54;
            this.label2.Text = "Customer Name";
            // 
            // cbxItemName
            // 
            this.cbxItemName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxItemName.FormattingEnabled = true;
            this.cbxItemName.Location = new System.Drawing.Point(130, 270);
            this.cbxItemName.Name = "cbxItemName";
            this.cbxItemName.Size = new System.Drawing.Size(157, 23);
            this.cbxItemName.TabIndex = 89;
            this.cbxItemName.SelectedIndexChanged += new System.EventHandler(this.cbxItemName_SelectedIndexChanged);
            // 
            // tbxCustomerName
            // 
            this.tbxCustomerName.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxCustomerName.Location = new System.Drawing.Point(15, 30);
            this.tbxCustomerName.Multiline = true;
            this.tbxCustomerName.Name = "tbxCustomerName";
            this.tbxCustomerName.Size = new System.Drawing.Size(275, 28);
            this.tbxCustomerName.TabIndex = 53;
            this.tbxCustomerName.TextChanged += new System.EventHandler(this.tbxCustomerName_TextChanged);
            this.tbxCustomerName.Enter += new System.EventHandler(this.tbxCustomerName_Enter);
            this.tbxCustomerName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxCustomerName_KeyPress);
            this.tbxCustomerName.Leave += new System.EventHandler(this.tbxCustomerName_Leave);
            // 
            // tbxQtyOthers
            // 
            this.tbxQtyOthers.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxQtyOthers.Location = new System.Drawing.Point(130, 321);
            this.tbxQtyOthers.Multiline = true;
            this.tbxQtyOthers.Name = "tbxQtyOthers";
            this.tbxQtyOthers.Size = new System.Drawing.Size(48, 23);
            this.tbxQtyOthers.TabIndex = 80;
            this.tbxQtyOthers.TextChanged += new System.EventHandler(this.tbxQtyOthers_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(127, 252);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 15);
            this.label9.TabIndex = 88;
            this.label9.Text = "Items";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(127, 302);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(28, 15);
            this.label14.TabIndex = 78;
            this.label14.Text = "Qty.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 16);
            this.label3.TabIndex = 55;
            this.label3.Text = "Contact Number";
            // 
            // tbxWeightItems
            // 
            this.tbxWeightItems.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxWeightItems.Location = new System.Drawing.Point(16, 321);
            this.tbxWeightItems.Multiline = true;
            this.tbxWeightItems.Name = "tbxWeightItems";
            this.tbxWeightItems.Size = new System.Drawing.Size(112, 23);
            this.tbxWeightItems.TabIndex = 65;
            this.tbxWeightItems.TextChanged += new System.EventHandler(this.tbxWeightItems_TextChanged);
            this.tbxWeightItems.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxWeightItems_KeyPress);
            // 
            // tbxContactNum
            // 
            this.tbxContactNum.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxContactNum.Location = new System.Drawing.Point(15, 89);
            this.tbxContactNum.Multiline = true;
            this.tbxContactNum.Name = "tbxContactNum";
            this.tbxContactNum.Size = new System.Drawing.Size(275, 28);
            this.tbxContactNum.TabIndex = 58;
            this.tbxContactNum.TextChanged += new System.EventHandler(this.tbxContactNum_TextChanged);
            this.tbxContactNum.Enter += new System.EventHandler(this.tbxContactNum_Enter);
            this.tbxContactNum.Leave += new System.EventHandler(this.tbxContactNum_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(9, 302);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 17);
            this.label7.TabIndex = 64;
            this.label7.Text = " Weight of items";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 17);
            this.label4.TabIndex = 56;
            this.label4.Text = "Address";
            // 
            // tbxAddress
            // 
            this.tbxAddress.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxAddress.Location = new System.Drawing.Point(15, 149);
            this.tbxAddress.Multiline = true;
            this.tbxAddress.Name = "tbxAddress";
            this.tbxAddress.Size = new System.Drawing.Size(275, 28);
            this.tbxAddress.TabIndex = 59;
            this.tbxAddress.TextChanged += new System.EventHandler(this.tbxAddress_TextChanged);
            this.tbxAddress.Enter += new System.EventHandler(this.tbxAddress_Enter);
            this.tbxAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxAddress_KeyPress);
            this.tbxAddress.Leave += new System.EventHandler(this.tbxAddress_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 17);
            this.label6.TabIndex = 61;
            this.label6.Text = "Remarks";
            // 
            // tbxNotes
            // 
            this.tbxNotes.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxNotes.Location = new System.Drawing.Point(15, 210);
            this.tbxNotes.Multiline = true;
            this.tbxNotes.Name = "tbxNotes";
            this.tbxNotes.Size = new System.Drawing.Size(275, 36);
            this.tbxNotes.TabIndex = 62;
            this.tbxNotes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxNotes_KeyPress);
            this.tbxNotes.Leave += new System.EventHandler(this.tbxNotes_Leave);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 251);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 17);
            this.label8.TabIndex = 66;
            this.label8.Text = "Category";
            // 
            // cbxCategory
            // 
            this.cbxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCategory.FormattingEnabled = true;
            this.cbxCategory.Location = new System.Drawing.Point(15, 270);
            this.cbxCategory.Name = "cbxCategory";
            this.cbxCategory.Size = new System.Drawing.Size(109, 23);
            this.cbxCategory.TabIndex = 67;
            this.cbxCategory.SelectedIndexChanged += new System.EventHandler(this.cbxCategory_SelectedIndexChanged_1);
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(426, 465);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(126, 17);
            this.label15.TabIndex = 105;
            this.label15.Text = "Total Amount Due:";
            this.label15.Click += new System.EventHandler(this.label15_Click);
            // 
            // dtgOrderForm
            // 
            this.dtgOrderForm.AllowUserToAddRows = false;
            this.dtgOrderForm.AllowUserToDeleteRows = false;
            this.dtgOrderForm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgOrderForm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgOrderForm.BackgroundColor = System.Drawing.Color.White;
            this.dtgOrderForm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgOrderForm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Category,
            this.Items,
            this.Qty,
            this.Weight,
            this.Price,
            this.Amount});
            this.dtgOrderForm.Location = new System.Drawing.Point(315, 51);
            this.dtgOrderForm.Name = "dtgOrderForm";
            this.dtgOrderForm.ReadOnly = true;
            this.dtgOrderForm.Size = new System.Drawing.Size(320, 399);
            this.dtgOrderForm.TabIndex = 111;
            // 
            // Category
            // 
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            // 
            // Items
            // 
            this.Items.HeaderText = "Items";
            this.Items.Name = "Items";
            this.Items.ReadOnly = true;
            // 
            // Qty
            // 
            this.Qty.HeaderText = "Qty";
            this.Qty.Name = "Qty";
            this.Qty.ReadOnly = true;
            // 
            // Weight
            // 
            this.Weight.HeaderText = "Weight";
            this.Weight.Name = "Weight";
            this.Weight.ReadOnly = true;
            // 
            // Price
            // 
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            // 
            // Amount
            // 
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.BasketShopping;
            this.iconPictureBox1.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.Location = new System.Drawing.Point(12, 24);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(32, 32);
            this.iconPictureBox1.TabIndex = 110;
            this.iconPictureBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnCancel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnCancel.BorderRadius = 10;
            this.btnCancel.BorderSize = 0;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(196, 456);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 36);
            this.btnCancel.TabIndex = 115;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPayment
            // 
            this.btnPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPayment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(110)))), ((int)(((byte)(253)))));
            this.btnPayment.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(110)))), ((int)(((byte)(253)))));
            this.btnPayment.BorderRadius = 10;
            this.btnPayment.BorderSize = 0;
            this.btnPayment.FlatAppearance.BorderSize = 0;
            this.btnPayment.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPayment.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnPayment.ForeColor = System.Drawing.Color.White;
            this.btnPayment.Location = new System.Drawing.Point(105, 456);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(85, 36);
            this.btnPayment.TabIndex = 114;
            this.btnPayment.Text = "Proceed to Payment";
            this.btnPayment.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(110)))), ((int)(((byte)(253)))));
            this.btnPayment.UseVisualStyleBackColor = false;
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(101)))), ((int)(((byte)(159)))));
            this.btnSave.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(101)))), ((int)(((byte)(159)))));
            this.btnSave.BorderRadius = 10;
            this.btnSave.BorderSize = 0;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(14, 456);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 36);
            this.btnSave.TabIndex = 113;
            this.btnSave.Text = "Save";
            this.btnSave.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(101)))), ((int)(((byte)(159)))));
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(552, 464);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(0, 17);
            this.lblTotal.TabIndex = 117;
            // 
            // ucOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dtgOrderForm);
            this.Controls.Add(this.btnPayment);
            this.Controls.Add(this.iconPictureBox1);
            this.Controls.Add(this.panelOrderForm);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label1);
            this.Name = "ucOrderForm";
            this.Size = new System.Drawing.Size(638, 498);
            this.Load += new System.EventHandler(this.ucOrderForm_Load);
            this.panelOrderForm.ResumeLayout(false);
            this.panelOrderForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgOrderForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelOrderForm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxItemName;
        private System.Windows.Forms.TextBox tbxCustomerName;
        private System.Windows.Forms.TextBox tbxQtyOthers;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxWeightItems;
        private System.Windows.Forms.TextBox tbxContactNum;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxAddress;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbxNotes;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbxCategory;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panelSuggestion;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private System.Windows.Forms.DataGridView dtgOrderForm;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn Items;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private NewButton btnAddList;
        private NewButton btnSave;
        private NewButton btnPayment;
        private NewButton btnCancel;
        private System.Windows.Forms.Label lblContactValidation;
        private System.Windows.Forms.Label lblWeightValidation;
        private System.Windows.Forms.Label lblAddressValidation;
        private System.Windows.Forms.Label lblCustomerNameValidation;
        private System.Windows.Forms.Label lblTotal;
    }
}
