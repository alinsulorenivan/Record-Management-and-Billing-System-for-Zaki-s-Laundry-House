using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZakiLaundryHouse;

namespace ZakiLaundryHouse.ucAdmin
{
    public partial class ucCustomers : UserControl
    {
        private dashboardAdmin mainForm;
        private bool isPopupOpen = false;

        public ucCustomers(dashboardAdmin form, Form popupForm)
        {
            InitializeComponent();

            this.VisibleChanged += ucCustomers_VisibleChanged;
            this.VisibleChanged += ucCustomersLBL_VisibleChanged;
            

            mainForm = form;
            LoadCustomers();
            tbxSearch.KeyDown += tbxSearch_KeyDown;
            dtgCustomers.CellClick += dtgCustomers_CellClick;
            tbxSearch.Click += tbxSearch_Click;
            DataGridViewStyler.ApplyNonSelectableStyle(dtgCustomers);
            MouseUp += (s, e) =>
            {
                containerResult.Visible = false;
            };
            foreach (Control Control in Controls)
            {
                if (Control.Name != tbxSearch.Name)
                {
                    Control.MouseUp += (s, e) =>
                    {
                        containerResult.Visible = false;
                    };
                }

            }
        }


        private void tbxSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxSearch.Text))
            {
                tbxSearch.BeginInvoke((MethodInvoker)(() => tbxSearch.SelectAll()));
            }
        }



        private void tbxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            containerResult.Visible = true;
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSearch.PerformClick();
            }
        }
        private List<Customer> allCustomers = new List<Customer>(); // Store all data in memory  // Track current page index


        public void LoadCustomers()
        {
            string query = @"
        SELECT 
            CustomerCode AS [Customer ID],
            CustomerName AS [Customer Name],
            ContactNumber AS [Contact No],
            Address
        FROM Customers";

            using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
            {
                conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                allCustomers = dt.AsEnumerable()
                    .Select(row => new Customer
                    {
                        CustomerCode = row.Field<string>("Customer ID"),
                        CustomerName = row.Field<string>("Customer Name"),
                        ContactNumber = row.Field<string>("Contact No"),
                        Address = row.Field<string>("Address")
                    })
                    .ToList();
            }

            // 🟢 Bind the data to the DataGridView
            dtgCustomers.DataSource = allCustomers;

            SetupDataGridView();

            // ✅ Add only once
            if (!dtgCustomers.Columns.Contains("OrderHistory"))
            {
                DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn
                {
                    Name = "OrderHistory",
                    HeaderText = "Order History",
                    Text = "View",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    DefaultCellStyle = { BackColor = Color.Lavender }
                };
                dtgCustomers.Columns.Add(btnCol);
            }
        }




        private void SetupDataGridView()
        {
            dtgCustomers.AllowUserToAddRows = false;
            dtgCustomers.ReadOnly = true;
            dtgCustomers.RowHeadersVisible = false;
            dtgCustomers.AllowUserToResizeRows = false;
            dtgCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgCustomers.DefaultCellStyle.SelectionBackColor = dtgCustomers.DefaultCellStyle.BackColor;
            dtgCustomers.DefaultCellStyle.SelectionForeColor = dtgCustomers.DefaultCellStyle.ForeColor;
        }


        private void ucCustomers_Load(object sender, EventArgs e)
        {
            dtgCustomers.AutoGenerateColumns = false;
            dtgCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            


        }


        private void txtsearch_TextChanged(object sender, EventArgs e)
        {

            if (tbxSearch.TextLength >= 1)
            {
                containerResult.Controls.Clear();
                lblNoResult.Visible = false;

                Data searcher = new Data();
                searcher.search(tbxSearch.Text);

                if (Data.list.Count == 0)
                {
                    lblNoResult.Visible = true;
                    containerResult.Height = 0;
                }
                else
                {
                    lblNoResult.Visible = false;
                    loadDetails();
                }
            }
            else
            {
                lblNoResult.Visible = false;
                containerResult.Controls.Clear();
                containerResult.Height = 0;
                LoadCustomers();
            }

            
        }

        private void loadDetails()
        {
            containerResult.Controls.Clear();

            int count = 0;
            foreach (Data data in Data.list)
            {
                if (count >= 8) break;

                SearchResultControl res = new SearchResultControl();
                res.details(data);
                res.CustomerClicked += SearchResultClicked;
                res.Height = 40;
                res.Margin = new Padding(0);
                containerResult.Controls.Add(res);
                containerResult.Controls.SetChildIndex(res, 0);

                count++;
            }

            containerResult.Height = containerResult.Controls.Count > 0
                ? 40 * containerResult.Controls.Count
                : 0;
        }


        private void SearchResultClicked(object sender, string customerName)
        {
            tbxSearch.Text = customerName;

            using (SQLiteConnection con = new SQLiteConnection(DbConnection.ConnectionString))
            {
                con.Open();
                string query = @"
            SELECT 
                CustomerCode AS [Customer ID],
                CustomerName AS [Customer Name],
                ContactNumber AS [Contact No],
                Address
            FROM Customers
            WHERE CustomerName = @name";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@name", customerName);

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    allCustomers = dt.AsEnumerable()
                        .Select(row => new Customer
                        {
                            CustomerCode = row.Field<string>("Customer ID"),
                            CustomerName = row.Field<string>("Customer Name"),
                            ContactNumber = row.Field<string>("Contact No"),
                            Address = row.Field<string>("Address")
                        })
                        .ToList();

                    dtgCustomers.DataSource = allCustomers;
                    SetupDataGridView();
                }
            }

            // Add Order History button if missing
            if (!dtgCustomers.Columns.Contains("OrderHistory"))
            {
                DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn
                {
                    Name = "OrderHistory",
                    HeaderText = "Order History",
                    Text = "View",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    DefaultCellStyle = { BackColor = Color.Lavender }
                };
                dtgCustomers.Columns.Add(btnCol);
            }

            containerResult.Controls.Clear();
            containerResult.Height = 0;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = tbxSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                lblNoResult.Visible = false;
                containerResult.Controls.Clear();
                containerResult.Height = 0;
                LoadCustomers();
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
            {
                conn.Open();
                string query = @"
            SELECT 
                CustomerCode AS [Customer ID],
                CustomerName AS [Customer Name],
                ContactNumber AS [Contact No],
                Address
            FROM Customers
            WHERE CustomerName LIKE @keyword 
               OR ContactNumber LIKE @keyword 
               OR Address LIKE @keyword";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        lblNoResult.Visible = true;
                        dtgCustomers.DataSource = null;
                        return;
                    }

                    lblNoResult.Visible = false;
                    allCustomers = dt.AsEnumerable()
                        .Select(row => new Customer
                        {
                            CustomerCode = row.Field<string>("Customer ID"),
                            CustomerName = row.Field<string>("Customer Name"),
                            ContactNumber = row.Field<string>("Contact No"),
                            Address = row.Field<string>("Address")
                        })
                        .ToList();

                    dtgCustomers.DataSource = allCustomers;
                    SetupDataGridView();
                }
            }

            // Ensure Order History column exists
            if (!dtgCustomers.Columns.Contains("OrderHistory"))
            {
                DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn
                {
                    Name = "OrderHistory",
                    HeaderText = "Order History",
                    Text = "View",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    DefaultCellStyle = { BackColor = Color.Lavender }
                };
                dtgCustomers.Columns.Add(btnCol);
            }
        }

        private void dtgCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (isPopupOpen || e.RowIndex < 0) return;

            if (dtgCustomers.Columns[e.ColumnIndex].Name == "OrderHistory")
            {
                isPopupOpen = true;

                string customerCode = "";
                string customerName = "";

                try
                {
                    // 🟢 Works when DataGridView is bound to List<Customer>
                    customerCode = dtgCustomers.Rows[e.RowIndex].Cells["CustomerCode"].Value.ToString();
                    customerName = dtgCustomers.Rows[e.RowIndex].Cells["CustomerName"].Value.ToString();
                }
                catch
                {
                    try
                    {
                        // 🟡 Fallback if DataGridView is bound to DataTable (SQL aliases)
                        customerCode = dtgCustomers.Rows[e.RowIndex].Cells["Customer ID"].Value.ToString();
                        customerName = dtgCustomers.Rows[e.RowIndex].Cells["Customer Name"].Value.ToString();
                    }
                    catch
                    {
                        MessageBox.Show("Unable to get customer information from this row.",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isPopupOpen = false;
                        return;
                    }
                }

                // 🟢 Create popup and load order history
                Form popupForm = new Form();
                var orderHistoryUC = new ucOrderHistory(mainForm, popupForm, "customers");
                orderHistoryUC.LoadOrderHistory(customerCode, customerName);

                ShowPopup(orderHistoryUC, popupForm, new Size(774, 574));
                isPopupOpen = false;
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

            popupForm.ShowDialog();
            dimmer.Close();
        }

        //new
        public static List<Customer> GetRecurringCustomers()
        {
            List<Customer> list = new List<Customer>();

            using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
            {
                conn.Open();
                string query = @"
            SELECT DISTINCT c.CustomerCode, c.CustomerName, c.ContactNumber, c.Address
            FROM Customers c
            INNER JOIN Orders o ON c.CustomerID = o.CustomerID";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Customer
                        {
                            CustomerCode = reader["CustomerCode"].ToString(),
                            CustomerName = reader["CustomerName"].ToString(),
                            ContactNumber = reader["ContactNumber"].ToString(),
                            Address = reader["Address"].ToString()
                        });
                    }
                }
            }

            return list;
        }

        // --- Customer class ---
        public class Customer
        {
            public string CustomerCode { get; set; }
            public string CustomerName { get; set; }
            public string ContactNumber { get; set; }
            public string Address { get; set; }
        }

        private void CbExit_Click(object sender, EventArgs e)
        {
            //formLogin loginForm = new formLogin();
            //loginForm.Show();
            //Form parentForm = this.FindForm(); 
            //if (parentForm != null)
            //{
            //    parentForm.Close(); 
            //}
        }

        private void CbMax_Click(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                if (parentForm.WindowState == FormWindowState.Normal)
                {
                    parentForm.WindowState = FormWindowState.Maximized;
                }
                else if (parentForm.WindowState == FormWindowState.Maximized)
                {
                    parentForm.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void CbMin_Click(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.WindowState = FormWindowState.Minimized;
            }
        }

        private void CbExit_Click_1(object sender, EventArgs e)
        {
            formLogin loginForm = new formLogin();
            loginForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Close();
            }
        }

        private void dtgCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void containerResult_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ucCustomers_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                containerResult.Visible = false; // Hide filter panel automatically
                containerResult.Height = 0;
                
                
            }
        }

        private void ucCustomersLBL_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                lblNoResult.Visible = false; // Hide filter panel automatically
                lblNoResult.Height = 0;


            }
        }

        private void tbxSearch_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void tbxSearch_Click_1(object sender, EventArgs e)
        {
           
        }

        public void ClearSearchBox()
        {
            tbxSearch.Text = string.Empty;
        }
    }
}
