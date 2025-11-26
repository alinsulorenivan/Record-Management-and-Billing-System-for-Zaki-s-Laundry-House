using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakiLaundryHouse.ucAdmin;

namespace ZakiLaundryHouse.ucAdmin
{
    public partial class ucBilling : UserControl
    {
        private dashboardAdmin mainForm;
        private Form popup;
        private bool isPopupOpen = false;
        public event Action OnPaymentCompleted;


        public ucBilling(dashboardAdmin form, Form popupForm)
        {
            InitializeComponent();
            mainForm = form;
            popup = popupForm;
            LoadPendingPayments();
            dtgBilling.CellClick += dtgBilling_CellClick;
            DataGridViewStyler.ApplyNonSelectableStyle(dtgBilling);
            mainForm.Resize += MainForm_Resize;
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (mainForm.WindowState == FormWindowState.Normal)
            {
                // Force DataGridView to redraw properly
                dtgBilling.AutoResizeColumns();
                dtgBilling.Refresh();
            }
        }

        public void LoadPendingPayments()
        {
            string connectionString = DbConnection.ConnectionString;
            string query = @"
        SELECT
            o.OrderID,
            o.OrderDate AS [Date],
            o.InvoiceNo AS [Invoice No],
            o.OrderCode AS [Order No],
            c.CustomerID,
            c.CustomerName AS [Customer Name],
            o.TotalAmount AS [Amount Due],
            o.Status
        FROM Orders o
        INNER JOIN Customers c ON o.CustomerID = c.CustomerID
        WHERE o.Status = 'Pending'
        ORDER BY o.OrderDate DESC";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dtgBilling.DataSource = dt;
                SetupDataGridView();
            }

            // Hide internal ID columns (not visible to users)
            if (dtgBilling.Columns.Contains("OrderID"))
                dtgBilling.Columns["OrderID"].Visible = false;
            if (dtgBilling.Columns.Contains("CustomerID"))
                dtgBilling.Columns["CustomerID"].Visible = false;
            if (dtgBilling.Columns.Contains("Amount Due"))
            {
                dtgBilling.Columns["Amount Due"].DefaultCellStyle.Format = "C2"; // e.g., ₱1,234.00
            }

            // Add Payment button column once
            if (!dtgBilling.Columns.Contains("Payment"))
            {
                DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn
                {
                    Name = "Payment",
                    HeaderText = "Payment",
                    Text = "Proceed to Payment",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat
                };
                btnColumn.DefaultCellStyle.BackColor = Color.Lavender;
                dtgBilling.Columns.Add(btnColumn);
            }

            this.Show();       // ensure control is visible
            this.BringToFront(); // force display
            this.Refresh();
        }

        private void SetupDataGridView()
        {
            dtgBilling.AllowUserToAddRows = false;
            dtgBilling.ReadOnly = true;
            dtgBilling.RowHeadersVisible = false;
            dtgBilling.AllowUserToResizeRows = false;
            dtgBilling.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgBilling.DefaultCellStyle.SelectionBackColor = dtgBilling.DefaultCellStyle.BackColor;
            dtgBilling.DefaultCellStyle.SelectionForeColor = dtgBilling.DefaultCellStyle.ForeColor;
        }

        private void ucBilling_Load(object sender, EventArgs e)
        {
            // Optional: auto-load again on control load
            // LoadPendingPayments();
        }

        private void dtgBilling_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (isPopupOpen || e.RowIndex < 0) return;

            if (dtgBilling.Columns[e.ColumnIndex].Name == "Payment")
            {
                isPopupOpen = true;

                string customerName = dtgBilling.Rows[e.RowIndex].Cells["Customer Name"].Value.ToString();
                string amountDue = dtgBilling.Rows[e.RowIndex].Cells["Amount Due"].Value.ToString();
                int customerID = Convert.ToInt32(dtgBilling.Rows[e.RowIndex].Cells["CustomerID"].Value);
                int orderID = Convert.ToInt32(dtgBilling.Rows[e.RowIndex].Cells["OrderID"].Value);

                Form popupForm = new Form();
                var paymentUC = new ucPaymentForm(mainForm, popupForm, "billing");

                // Fill payment form details
                paymentUC.CustomerNameTextBox.Text = customerName;
                paymentUC.TotalAmountTextBox.Text = amountDue;
                paymentUC.CustomerID = customerID;
                paymentUC.OrderID = orderID;

                // Payment method options
                paymentUC.PaymentMethodComboBox.Items.Clear();
                paymentUC.PaymentMethodComboBox.Items.AddRange(new string[] { "Cash", "Gcash", "Maya", "BPI" });
                paymentUC.PaymentMethodComboBox.SelectedIndex = 0;

                paymentUC.AmountReceivedTextBox.Text = "";
                paymentUC.ChangeTextBox.Text = "";

                // Refresh after payment completion
                paymentUC.PaymentCompleted += () =>
                {
                    LoadPendingPayments();
                    mainForm.AdminHomeUserControl?.LoadTodayStats();
                    isPopupOpen = false;
                    OnPaymentCompleted?.Invoke();

                    mainForm.AdminOrdersUserControl?.LoadOrders();


                };

                ShowPopup(paymentUC, popupForm, new Size(774, 574));
            }

        }

        private void ShowPopup(UserControl control, Form popupForm, Size size)
        {
            FormDimmer dimmer = new FormDimmer
            {
                Size = mainForm.Size,
                Location = mainForm.Location,
                Owner = mainForm
            };
            dimmer.Show();

            popupForm.FormBorderStyle = FormBorderStyle.None;
            popupForm.StartPosition = FormStartPosition.CenterParent;
            popupForm.Size = size;
            popupForm.BackColor = Color.White;
            popupForm.ShowInTaskbar = false;

            control.Dock = DockStyle.Fill;
            popupForm.Controls.Add(control);

            popupForm.FormClosed += (s, args) =>
            {
                isPopupOpen = false;       // Release lock
                LoadPendingPayments();     // Reload grid in case no payment action happened
            };

            popupForm.ShowDialog();
            dimmer.Close();
        }
        private void ucBilling_Resize(object sender, EventArgs e)
        {
            dtgBilling.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgBilling.Refresh();  // Re-adjust column widths on resize
        }
        //private void ucBilling_VisibleChanged(object sender, EventArgs e)
        //{
        //    if (this.Visible)
        //    {
        //        LoadPendingPayments();
        //    }
        //}

    }
}
//eto