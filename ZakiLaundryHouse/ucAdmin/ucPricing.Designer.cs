namespace ZakiLaundryHouse.ucAdmin
{
    partial class ucPricing
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.dtgPricing = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbxPrice = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbxMaxWeight = new System.Windows.Forms.TextBox();
            this.cbxCategory = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelPricingForm = new System.Windows.Forms.Panel();
            this.btnCancel = new ZakiLaundryHouse.ucAdmin.NewButton();
            this.tbxMin = new System.Windows.Forms.TextBox();
            this.btnSave = new ZakiLaundryHouse.ucAdmin.NewButton();
            this.cbxItem = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxPricingType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.pnlDropdownPricing = new System.Windows.Forms.Panel();
            this.rdbAll = new System.Windows.Forms.RadioButton();
            this.rdbOthers = new System.Windows.Forms.RadioButton();
            this.rdbAddOn = new System.Windows.Forms.RadioButton();
            this.rdbSelfServ = new System.Windows.Forms.RadioButton();
            this.rdbFullServ = new System.Windows.Forms.RadioButton();
            this.timerSlideDownPricing = new System.Windows.Forms.Timer(this.components);
            this.btnFilterPricing = new FontAwesome.Sharp.IconButton();
            ((System.ComponentModel.ISupportInitialize)(this.dtgPricing)).BeginInit();
            this.panelPricingForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            this.pnlDropdownPricing.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(44, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 35);
            this.label1.TabIndex = 32;
            this.label1.Text = "Pricing";
            // 
            // dtgPricing
            // 
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgPricing.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtgPricing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgPricing.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgPricing.BackgroundColor = System.Drawing.Color.White;
            this.dtgPricing.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgPricing.Location = new System.Drawing.Point(235, 71);
            this.dtgPricing.Name = "dtgPricing";
            this.dtgPricing.RowHeadersWidth = 51;
            this.dtgPricing.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dtgPricing.Size = new System.Drawing.Size(359, 386);
            this.dtgPricing.TabIndex = 33;
            this.dtgPricing.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgPricing_CellContentClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(0, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(232, 1);
            this.panel1.TabIndex = 60;
            // 
            // tbxPrice
            // 
            this.tbxPrice.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxPrice.Location = new System.Drawing.Point(14, 297);
            this.tbxPrice.Multiline = true;
            this.tbxPrice.Name = "tbxPrice";
            this.tbxPrice.Size = new System.Drawing.Size(196, 26);
            this.tbxPrice.TabIndex = 59;
            this.tbxPrice.TextChanged += new System.EventHandler(this.tbxPrice_TextChanged);
            this.tbxPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxPrice_KeyPress);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.Window;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(106, 213);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 21);
            this.label6.TabIndex = 58;
            this.label6.Text = "Max Wght";
            // 
            // tbxMaxWeight
            // 
            this.tbxMaxWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxMaxWeight.Location = new System.Drawing.Point(109, 237);
            this.tbxMaxWeight.Multiline = true;
            this.tbxMaxWeight.Name = "tbxMaxWeight";
            this.tbxMaxWeight.Size = new System.Drawing.Size(90, 26);
            this.tbxMaxWeight.TabIndex = 57;
            this.tbxMaxWeight.TextChanged += new System.EventHandler(this.tbxQtyWeight_TextChanged);
            this.tbxMaxWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxMaxWeight_KeyPress);
            this.tbxMaxWeight.Leave += new System.EventHandler(this.tbxMaxWeight_Leave);
            // 
            // cbxCategory
            // 
            this.cbxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCategory.FormattingEnabled = true;
            this.cbxCategory.Location = new System.Drawing.Point(14, 75);
            this.cbxCategory.Name = "cbxCategory";
            this.cbxCategory.Size = new System.Drawing.Size(196, 25);
            this.cbxCategory.TabIndex = 55;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.Window;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(11, 273);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 21);
            this.label5.TabIndex = 52;
            this.label5.Text = "Price";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.Window;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 21);
            this.label4.TabIndex = 51;
            this.label4.Text = "Item Name";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.Window;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 21);
            this.label3.TabIndex = 50;
            this.label3.Text = "Category";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 29);
            this.label2.TabIndex = 49;
            this.label2.Text = "Pricing Form";
            // 
            // panelPricingForm
            // 
            this.panelPricingForm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelPricingForm.BackColor = System.Drawing.Color.White;
            this.panelPricingForm.Controls.Add(this.btnCancel);
            this.panelPricingForm.Controls.Add(this.tbxMin);
            this.panelPricingForm.Controls.Add(this.btnSave);
            this.panelPricingForm.Controls.Add(this.cbxItem);
            this.panelPricingForm.Controls.Add(this.label2);
            this.panelPricingForm.Controls.Add(this.label8);
            this.panelPricingForm.Controls.Add(this.cbxPricingType);
            this.panelPricingForm.Controls.Add(this.label7);
            this.panelPricingForm.Controls.Add(this.tbxMaxWeight);
            this.panelPricingForm.Controls.Add(this.label6);
            this.panelPricingForm.Controls.Add(this.panel1);
            this.panelPricingForm.Controls.Add(this.tbxPrice);
            this.panelPricingForm.Controls.Add(this.label5);
            this.panelPricingForm.Controls.Add(this.label4);
            this.panelPricingForm.Controls.Add(this.cbxCategory);
            this.panelPricingForm.Controls.Add(this.label3);
            this.panelPricingForm.Location = new System.Drawing.Point(3, 71);
            this.panelPricingForm.Name = "panelPricingForm";
            this.panelPricingForm.Size = new System.Drawing.Size(232, 385);
            this.panelPricingForm.TabIndex = 61;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCancel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCancel.BorderRadius = 10;
            this.btnCancel.BorderSize = 0;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(146, 338);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 27);
            this.btnCancel.TabIndex = 120;
            this.btnCancel.Text = "Clear";
            this.btnCancel.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tbxMin
            // 
            this.tbxMin.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxMin.Location = new System.Drawing.Point(14, 237);
            this.tbxMin.Multiline = true;
            this.tbxMin.Name = "tbxMin";
            this.tbxMin.Size = new System.Drawing.Size(78, 26);
            this.tbxMin.TabIndex = 124;
            this.tbxMin.TextChanged += new System.EventHandler(this.tbxMin_TextChanged);
            this.tbxMin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxMin_KeyPress);
            this.tbxMin.Leave += new System.EventHandler(this.tbxMin_Leave);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(101)))), ((int)(((byte)(159)))));
            this.btnSave.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(101)))), ((int)(((byte)(159)))));
            this.btnSave.BorderRadius = 10;
            this.btnSave.BorderSize = 0;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(76, 338);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(64, 27);
            this.btnSave.TabIndex = 119;
            this.btnSave.Text = "Save";
            this.btnSave.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(101)))), ((int)(((byte)(159)))));
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbxItem
            // 
            this.cbxItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxItem.FormattingEnabled = true;
            this.cbxItem.Location = new System.Drawing.Point(14, 123);
            this.cbxItem.Name = "cbxItem";
            this.cbxItem.Size = new System.Drawing.Size(196, 25);
            this.cbxItem.TabIndex = 117;
            this.cbxItem.SelectedIndexChanged += new System.EventHandler(this.cbxItem_SelectedIndexChanged);
            this.cbxItem.TextChanged += new System.EventHandler(this.cbxItem_TextChanged);
            this.cbxItem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbxItem_KeyPress);
            this.cbxItem.Leave += new System.EventHandler(this.cbxItem_Leave);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.Window;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(11, 213);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 21);
            this.label8.TabIndex = 125;
            this.label8.Text = "Min Wght";
            // 
            // cbxPricingType
            // 
            this.cbxPricingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPricingType.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxPricingType.FormattingEnabled = true;
            this.cbxPricingType.Items.AddRange(new object[] {
            "Per Minimum",
            "Per KG",
            "Per Load",
            "Fixed"});
            this.cbxPricingType.Location = new System.Drawing.Point(14, 180);
            this.cbxPricingType.Name = "cbxPricingType";
            this.cbxPricingType.Size = new System.Drawing.Size(196, 25);
            this.cbxPricingType.TabIndex = 123;
            this.cbxPricingType.SelectedIndexChanged += new System.EventHandler(this.cbxPricingType_SelectedIndexChanged_1);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.Window;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(11, 159);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 21);
            this.label7.TabIndex = 122;
            this.label7.Text = "Price Type";
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.Tags;
            this.iconPictureBox1.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.Location = new System.Drawing.Point(12, 24);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(32, 32);
            this.iconPictureBox1.TabIndex = 63;
            this.iconPictureBox1.TabStop = false;
            // 
            // pnlDropdownPricing
            // 
            this.pnlDropdownPricing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDropdownPricing.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlDropdownPricing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDropdownPricing.Controls.Add(this.rdbAll);
            this.pnlDropdownPricing.Controls.Add(this.rdbOthers);
            this.pnlDropdownPricing.Controls.Add(this.rdbAddOn);
            this.pnlDropdownPricing.Controls.Add(this.rdbSelfServ);
            this.pnlDropdownPricing.Controls.Add(this.rdbFullServ);
            this.pnlDropdownPricing.Location = new System.Drawing.Point(382, 42);
            this.pnlDropdownPricing.Name = "pnlDropdownPricing";
            this.pnlDropdownPricing.Size = new System.Drawing.Size(121, 155);
            this.pnlDropdownPricing.TabIndex = 117;
            this.pnlDropdownPricing.Visible = false;
            this.pnlDropdownPricing.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDropdownPricing_Paint);
            // 
            // rdbAll
            // 
            this.rdbAll.AutoSize = true;
            this.rdbAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdbAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbAll.ForeColor = System.Drawing.Color.Black;
            this.rdbAll.Location = new System.Drawing.Point(15, 122);
            this.rdbAll.Name = "rdbAll";
            this.rdbAll.Size = new System.Drawing.Size(45, 20);
            this.rdbAll.TabIndex = 6;
            this.rdbAll.TabStop = true;
            this.rdbAll.Text = "All";
            this.rdbAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbAll.UseVisualStyleBackColor = true;
            this.rdbAll.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // rdbOthers
            // 
            this.rdbOthers.AutoSize = true;
            this.rdbOthers.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdbOthers.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbOthers.ForeColor = System.Drawing.Color.Black;
            this.rdbOthers.Location = new System.Drawing.Point(15, 96);
            this.rdbOthers.Name = "rdbOthers";
            this.rdbOthers.Size = new System.Drawing.Size(66, 20);
            this.rdbOthers.TabIndex = 5;
            this.rdbOthers.TabStop = true;
            this.rdbOthers.Text = "Others";
            this.rdbOthers.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbOthers.UseVisualStyleBackColor = true;
            // 
            // rdbAddOn
            // 
            this.rdbAddOn.AutoSize = true;
            this.rdbAddOn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdbAddOn.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbAddOn.ForeColor = System.Drawing.Color.Black;
            this.rdbAddOn.Location = new System.Drawing.Point(15, 70);
            this.rdbAddOn.Name = "rdbAddOn";
            this.rdbAddOn.Size = new System.Drawing.Size(77, 20);
            this.rdbAddOn.TabIndex = 2;
            this.rdbAddOn.TabStop = true;
            this.rdbAddOn.Text = "Add Ons";
            this.rdbAddOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbAddOn.UseVisualStyleBackColor = true;
            // 
            // rdbSelfServ
            // 
            this.rdbSelfServ.AutoSize = true;
            this.rdbSelfServ.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdbSelfServ.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbSelfServ.ForeColor = System.Drawing.Color.Black;
            this.rdbSelfServ.Location = new System.Drawing.Point(15, 44);
            this.rdbSelfServ.Name = "rdbSelfServ";
            this.rdbSelfServ.Size = new System.Drawing.Size(92, 20);
            this.rdbSelfServ.TabIndex = 1;
            this.rdbSelfServ.TabStop = true;
            this.rdbSelfServ.Text = "Self Service";
            this.rdbSelfServ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbSelfServ.UseVisualStyleBackColor = true;
            // 
            // rdbFullServ
            // 
            this.rdbFullServ.AutoSize = true;
            this.rdbFullServ.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdbFullServ.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbFullServ.ForeColor = System.Drawing.Color.Black;
            this.rdbFullServ.Location = new System.Drawing.Point(15, 17);
            this.rdbFullServ.Name = "rdbFullServ";
            this.rdbFullServ.Size = new System.Drawing.Size(91, 20);
            this.rdbFullServ.TabIndex = 0;
            this.rdbFullServ.TabStop = true;
            this.rdbFullServ.Text = "Full Service";
            this.rdbFullServ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbFullServ.UseVisualStyleBackColor = true;
            this.rdbFullServ.CheckedChanged += new System.EventHandler(this.rdbFullServ_CheckedChanged);
            // 
            // timerSlideDownPricing
            // 
            this.timerSlideDownPricing.Tick += new System.EventHandler(this.timerSlideDownPricing_Tick);
            // 
            // btnFilterPricing
            // 
            this.btnFilterPricing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilterPricing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.btnFilterPricing.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFilterPricing.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnFilterPricing.ForeColor = System.Drawing.Color.White;
            this.btnFilterPricing.IconChar = FontAwesome.Sharp.IconChar.Filter;
            this.btnFilterPricing.IconColor = System.Drawing.Color.White;
            this.btnFilterPricing.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnFilterPricing.IconSize = 15;
            this.btnFilterPricing.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFilterPricing.Location = new System.Drawing.Point(503, 42);
            this.btnFilterPricing.Name = "btnFilterPricing";
            this.btnFilterPricing.Size = new System.Drawing.Size(75, 23);
            this.btnFilterPricing.TabIndex = 118;
            this.btnFilterPricing.Text = "Filter";
            this.btnFilterPricing.UseVisualStyleBackColor = false;
            this.btnFilterPricing.Click += new System.EventHandler(this.iconButton1_Click);
            // 
            // ucPricing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.btnFilterPricing);
            this.Controls.Add(this.pnlDropdownPricing);
            this.Controls.Add(this.iconPictureBox1);
            this.Controls.Add(this.panelPricingForm);
            this.Controls.Add(this.dtgPricing);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this.Name = "ucPricing";
            this.Size = new System.Drawing.Size(598, 490);
            this.Load += new System.EventHandler(this.ucPricing_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgPricing)).EndInit();
            this.panelPricingForm.ResumeLayout(false);
            this.panelPricingForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            this.pnlDropdownPricing.ResumeLayout(false);
            this.pnlDropdownPricing.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dtgPricing;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbxPrice;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbxMaxWeight;
        private System.Windows.Forms.ComboBox cbxCategory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelPricingForm;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private System.Windows.Forms.Panel pnlDropdownPricing;
        private System.Windows.Forms.RadioButton rdbSelfServ;
        private System.Windows.Forms.RadioButton rdbFullServ;
        private System.Windows.Forms.RadioButton rdbOthers;
        private System.Windows.Forms.RadioButton rdbAddOn;
        private System.Windows.Forms.Timer timerSlideDownPricing;
        private FontAwesome.Sharp.IconButton btnFilterPricing;
        private System.Windows.Forms.RadioButton rdbAll;
        private System.Windows.Forms.ComboBox cbxItem;
        private System.Windows.Forms.TextBox tbxMin;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbxPricingType;
        private System.Windows.Forms.Label label7;
        private NewButton btnSave;
        private NewButton btnCancel;
    }
}
