namespace ZakiLaundryHouse.ucAdmin
{
    partial class ucSales
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
            this.dateFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtgSales = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.dateTo = new System.Windows.Forms.DateTimePicker();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.panelExportDropdown = new System.Windows.Forms.FlowLayoutPanel();
            this.panelExport = new System.Windows.Forms.Panel();
            this.btnExportAs = new ZakiLaundryHouse.ucAdmin.NewButton();
            this.btnExcel = new ZakiLaundryHouse.ucAdmin.NewButton();
            this.btnExportPDF = new ZakiLaundryHouse.ucAdmin.NewButton();
            this.ExportDroprdownTimer = new System.Windows.Forms.Timer(this.components);
            this.btnFilter = new ZakiLaundryHouse.ucAdmin.NewButton();
            this.Totallbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtgSales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            this.panelExportDropdown.SuspendLayout();
            this.panelExport.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateFrom
            // 
            this.dateFrom.CustomFormat = "MMMM dd, yyyy";
            this.dateFrom.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateFrom.Location = new System.Drawing.Point(78, 63);
            this.dateFrom.Name = "dateFrom";
            this.dateFrom.Size = new System.Drawing.Size(148, 23);
            this.dateFrom.TabIndex = 16;
            this.dateFrom.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 23);
            this.label1.TabIndex = 17;
            this.label1.Text = "Date From";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(44, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 35);
            this.label2.TabIndex = 18;
            this.label2.Text = "Sales";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 23);
            this.label3.TabIndex = 20;
            this.label3.Text = "Date To";
            // 
            // dtgSales
            // 
            this.dtgSales.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgSales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgSales.BackgroundColor = System.Drawing.Color.White;
            this.dtgSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgSales.Location = new System.Drawing.Point(2, 124);
            this.dtgSales.Name = "dtgSales";
            this.dtgSales.Size = new System.Drawing.Size(592, 314);
            this.dtgSales.TabIndex = 21;
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(427, 441);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(44, 23);
            this.lblTotal.TabIndex = 22;
            this.lblTotal.Text = "Total:";
            // 
            // dateTo
            // 
            this.dateTo.CustomFormat = "MMMM dd, yyyy";
            this.dateTo.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTo.Location = new System.Drawing.Point(78, 95);
            this.dateTo.MinDate = new System.DateTime(2025, 1, 1, 0, 0, 0, 0);
            this.dateTo.Name = "dateTo";
            this.dateTo.Size = new System.Drawing.Size(148, 23);
            this.dateTo.TabIndex = 19;
            this.dateTo.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.ArrowTrendUp;
            this.iconPictureBox1.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(77)))), ((int)(((byte)(113)))));
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.Location = new System.Drawing.Point(12, 24);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(32, 32);
            this.iconPictureBox1.TabIndex = 106;
            this.iconPictureBox1.TabStop = false;
            // 
            // panelExportDropdown
            // 
            this.panelExportDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelExportDropdown.Controls.Add(this.panelExport);
            this.panelExportDropdown.Controls.Add(this.btnExcel);
            this.panelExportDropdown.Controls.Add(this.btnExportPDF);
            this.panelExportDropdown.Location = new System.Drawing.Point(494, 82);
            this.panelExportDropdown.MaximumSize = new System.Drawing.Size(100, 109);
            this.panelExportDropdown.MinimumSize = new System.Drawing.Size(100, 36);
            this.panelExportDropdown.Name = "panelExportDropdown";
            this.panelExportDropdown.Size = new System.Drawing.Size(100, 36);
            this.panelExportDropdown.TabIndex = 112;
            // 
            // panelExport
            // 
            this.panelExport.Controls.Add(this.btnExportAs);
            this.panelExport.Location = new System.Drawing.Point(0, 0);
            this.panelExport.Margin = new System.Windows.Forms.Padding(0);
            this.panelExport.Name = "panelExport";
            this.panelExport.Size = new System.Drawing.Size(100, 36);
            this.panelExport.TabIndex = 112;
            // 
            // btnExportAs
            // 
            this.btnExportAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportAs.BackColor = System.Drawing.Color.Red;
            this.btnExportAs.BackgroundColor = System.Drawing.Color.Red;
            this.btnExportAs.BorderRadius = 10;
            this.btnExportAs.BorderSize = 0;
            this.btnExportAs.FlatAppearance.BorderSize = 0;
            this.btnExportAs.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExportAs.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExportAs.ForeColor = System.Drawing.Color.White;
            this.btnExportAs.Location = new System.Drawing.Point(3, 3);
            this.btnExportAs.Name = "btnExportAs";
            this.btnExportAs.Size = new System.Drawing.Size(94, 29);
            this.btnExportAs.TabIndex = 111;
            this.btnExportAs.Text = "EXPORT";
            this.btnExportAs.TextColor = System.Drawing.Color.Red;
            this.btnExportAs.UseVisualStyleBackColor = false;
            this.btnExportAs.Click += new System.EventHandler(this.btnExportAs_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnExcel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnExcel.BorderRadius = 10;
            this.btnExcel.BorderSize = 0;
            this.btnExcel.FlatAppearance.BorderSize = 0;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExcel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExcel.ForeColor = System.Drawing.Color.White;
            this.btnExcel.Location = new System.Drawing.Point(3, 39);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(94, 29);
            this.btnExcel.TabIndex = 108;
            this.btnExcel.Text = "EXCEL";
            this.btnExcel.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnExcel.UseVisualStyleBackColor = false;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnExportPDF
            // 
            this.btnExportPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnExportPDF.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnExportPDF.BorderRadius = 10;
            this.btnExportPDF.BorderSize = 0;
            this.btnExportPDF.FlatAppearance.BorderSize = 0;
            this.btnExportPDF.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExportPDF.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExportPDF.ForeColor = System.Drawing.Color.White;
            this.btnExportPDF.Location = new System.Drawing.Point(3, 74);
            this.btnExportPDF.Name = "btnExportPDF";
            this.btnExportPDF.Size = new System.Drawing.Size(94, 29);
            this.btnExportPDF.TabIndex = 109;
            this.btnExportPDF.Text = "PDF";
            this.btnExportPDF.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnExportPDF.UseVisualStyleBackColor = false;
            this.btnExportPDF.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ExportDroprdownTimer
            // 
            this.ExportDroprdownTimer.Interval = 10;
            this.ExportDroprdownTimer.Tick += new System.EventHandler(this.ExportDroprdownTimer_Tick);
            // 
            // btnFilter
            // 
            this.btnFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.btnFilter.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.btnFilter.BorderRadius = 10;
            this.btnFilter.BorderSize = 0;
            this.btnFilter.FlatAppearance.BorderSize = 0;
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFilter.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnFilter.ForeColor = System.Drawing.Color.White;
            this.btnFilter.Location = new System.Drawing.Point(437, 85);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(54, 29);
            this.btnFilter.TabIndex = 110;
            this.btnFilter.Text = "Filter";
            this.btnFilter.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.btnFilter.UseVisualStyleBackColor = false;
            // 
            // Totallbl
            // 
            this.Totallbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Totallbl.AutoSize = true;
            this.Totallbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.Totallbl.Location = new System.Drawing.Point(471, 441);
            this.Totallbl.Name = "Totallbl";
            this.Totallbl.Size = new System.Drawing.Size(0, 17);
            this.Totallbl.TabIndex = 113;
            // 
            // ucSales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.Totallbl);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.panelExportDropdown);
            this.Controls.Add(this.iconPictureBox1);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.dtgSales);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateFrom);
            this.Name = "ucSales";
            this.Size = new System.Drawing.Size(598, 490);
            ((System.ComponentModel.ISupportInitialize)(this.dtgSales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            this.panelExportDropdown.ResumeLayout(false);
            this.panelExport.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DateTimePicker dateFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dtgSales;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.DateTimePicker dateTo;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private NewButton btnExcel;
        private NewButton btnExportPDF;
        private NewButton btnFilter;
        private NewButton btnExportAs;
        private System.Windows.Forms.FlowLayoutPanel panelExportDropdown;
        private System.Windows.Forms.Panel panelExport;
        private System.Windows.Forms.Timer ExportDroprdownTimer;
        private System.Windows.Forms.Label Totallbl;
    }
}
