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

namespace ZakiLaundryHouse.ucAdmin
{
    public partial class ucAdminOrders : UserControl
    {
        private dashboardAdmin mainForm;
        private string originalStatusBeforeEdit = null;
        private bool suppressMessageBox = false;
        public event Action OnOrderSaved;

        public ucAdminOrders(dashboardAdmin form)
        {
            InitializeComponent();
            mainForm = form;
            LoadOrders();
            dtgOrders.CellValueChanged += dtgOrders_CellValueChanged;
            dtgOrders.CurrentCellDirtyStateChanged += dtgOrders_CurrentCellDirtyStateChanged;
            dtgOrders.CellBeginEdit += dtgOrders_CellBeginEdit;
            DataGridViewStyler.ApplyNonSelectableStyle(dtgOrders);
        }

        public void LoadOrders()
        {
            string connectionString = DbConnection.ConnectionString;
            string query = @"
            SELECT 
                strftime('%Y-%m-%d', o.OrderDate) AS Date,
                o.OrderCode AS [Order No.],
                c.CustomerName AS [Customer Name],
                c.ContactNumber AS [Contact No.],
                o.Remarks,
                o.TotalAmount AS [Total Amount],
                o.Status
            FROM Orders o
            INNER JOIN Customers c ON o.CustomerID = c.CustomerID
            WHERE DATE(o.OrderDate) = DATE('now') OR o.Status = 'Pending'
            ORDER BY o.OrderDate DESC";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dtgOrders.DataSource = dt;

                SetupDataGridView();

                if (dtgOrders.Columns.Contains("Status"))
                    dtgOrders.Columns["Status"].ReadOnly = true;

                if (dtgOrders.Columns.Contains("Total Amount"))
                {
                    dtgOrders.Columns["Total Amount"].DefaultCellStyle.Format = "C2"; // e.g., ₱1,234.00
                }
            }
        }

        private void dtgOrders_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dtgOrders.IsCurrentCellDirty)
                dtgOrders.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dtgOrders_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0 && dtgOrders.Columns[e.ColumnIndex].Name == "Status")
            {
                // Block editing for the Status column
                e.Cancel = true;
            }
        }

        private void dtgOrders_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dtgOrders.Columns[e.ColumnIndex].Name == "Status")
            {
                string orderNo = dtgOrders.Rows[e.RowIndex].Cells["Order No."].Value.ToString();
                string newStatus = dtgOrders.Rows[e.RowIndex].Cells["Status"].Value.ToString();
                string currentStatus = GetOrderStatusFromDB(orderNo);

                if (currentStatus == "Completed" && newStatus == "Pending")
                {
                    MessageBox.Show("You cannot revert a completed order back to pending.",
                        "Invalid Action", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    dtgOrders.CellValueChanged -= dtgOrders_CellValueChanged;
                    dtgOrders.Rows[e.RowIndex].Cells["Status"].Value = "Completed";
                    dtgOrders.CellValueChanged += dtgOrders_CellValueChanged;
                    originalStatusBeforeEdit = null;

                    this.BeginInvoke(new Action(() =>
                    {
                        RefreshOrders();
                    }));

                    return;
                }

                string query = "UPDATE Orders SET Status = @Status WHERE OrderCode = @OrderNo";
                using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@OrderNo", orderNo);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private string GetOrderStatusFromDB(string orderNo)
        {
            string status = "";
            string query = "SELECT Status FROM Orders WHERE OrderCode = @OrderNo";
            using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@OrderNo", orderNo);
                conn.Open();
                status = cmd.ExecuteScalar()?.ToString();
            }
            return status;
        }

        private void SetupDataGridView()
        {
            dtgOrders.AllowUserToAddRows = false;
            dtgOrders.ReadOnly = false;
            dtgOrders.RowHeadersVisible = false;
            dtgOrders.AllowUserToResizeRows = false;
            dtgOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn col in dtgOrders.Columns)
            {
                col.ReadOnly = true; // All columns readonly by default
            }
        }

        public void RefreshOrders()
        {
            LoadOrders();
        }

        private void CbExit_Click(object sender, EventArgs e)
        {
            formLogin loginForm = new formLogin();
            loginForm.Show();
            Form parentForm = this.FindForm();
            parentForm?.Close();
        }

        private void CbMax_Click(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.WindowState = parentForm.WindowState == FormWindowState.Normal
                    ? FormWindowState.Maximized
                    : FormWindowState.Normal;
            }
        }

        private void CbMin_Click(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null)
                parentForm.WindowState = FormWindowState.Minimized;
        }

        private void btnAddNewOrder_Click(object sender, EventArgs e)
        {
            FormDimmer dimmer = new FormDimmer
            {
                Size = mainForm.Size,
                Location = mainForm.Location,
                Owner = mainForm
            };
            dimmer.Show();

            Form popupForm = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(774, 574),
                BackColor = Color.White,
                ShowInTaskbar = false
            };

            ucOrderForm orderForm = new ucOrderForm(mainForm, popupForm)
            {
                Dock = DockStyle.Fill
            };

            orderForm.OnOrderSaved += () =>
            {
                RefreshOrders();
                popupForm.Close();
                dimmer.Close();
                this.BringToFront();
                this.Visible = true;
                OnOrderSaved?.Invoke();
            };

            popupForm.Controls.Add(orderForm);
            popupForm.ShowDialog(mainForm);
            dimmer.Close();
            this.BringToFront();
            this.Visible = true;
        }

        private void dtgOrders_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}
