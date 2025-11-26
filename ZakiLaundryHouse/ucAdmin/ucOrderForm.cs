using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;        // SQLite
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;   // Printing support
using System.Linq;
using System.Management;         // For WMI/Hardware info
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZakiLaundryHouse.ucAdmin
{
    public partial class ucOrderForm : UserControl
    {
        private dashboardAdmin mainForm;
        private Form popup;
        private List<ucCustomers.Customer> recurringCustomers;
        public event Action OnOrderSaved;
        private string originalCustomerName = null;
        private string originalContactNum = null;
        public int CustomerID { get; set; }
        public int OrderID { get; set; }
        private Timer contactTypingTimer = new Timer();
        private bool isTyping = false;
        private bool isValidating = false;





        public ucOrderForm(dashboardAdmin form, Form popupForm)
        {
            InitializeComponent();
            contactTypingTimer.Interval = 800; // wait 0.8s after typing stops
            contactTypingTimer.Tick += ContactTypingTimer_Tick;
            LoadCategories();
            mainForm = form;
            popup = popupForm;
            SetupCustomerAutoComplete();
            tbxContactNum.KeyPress += tbxContactNum_KeyPress;
            SetupDataGridView();
            tbxCustomerName.PreviewKeyDown += tbxCustomerName_PreviewKeyDown;
            tbxWeightItems.PreviewKeyDown += tbxWeightItems_PreviewKeyDown;
            tbxWeightItems.Leave += tbxWeightItems_Leave;
            btnAddList.PreviewKeyDown += btnAddList_PreviewKeyDown;
            tbxQtyOthers.PreviewKeyDown += tbxQtyOthers_PreviewKeyDown;
            dtgOrderForm.CellContentClick += dtgOrderForm_CellContentClick;

            DataGridViewStyler.ApplyNonSelectableStyle(dtgOrderForm);
            tbxCustomerName.TextAlign = HorizontalAlignment.Left; // also fixes cursor weirdness
            tbxCustomerName.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Tab)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
            };


        }
        private void btnAddList_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                btnPayment.Focus();
            }
        }
        private void tbxQtyOthers_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                btnAddList.Focus();
            }
        }

        private void tbxWeightItems_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                tbxQtyOthers.Focus();
                tbxQtyOthers.SelectionStart = tbxQtyOthers.Text.Length;
                tbxQtyOthers.SelectionLength = 0;
            }
        }

        private void tbxCustomerName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                if (panelSuggestion.Visible && panelSuggestion.Controls.Count > 0)
                {
                    Label firstSuggestion = panelSuggestion.Controls[0] as Label;
                    if (firstSuggestion != null)
                    {
                        string[] parts = firstSuggestion.Text.Split('-');
                        if (parts.Length >= 3)
                        {
                            tbxCustomerName.TextAlign = HorizontalAlignment.Left;
                            tbxCustomerName.Text = parts[0].Trim();
                            tbxContactNum.Text = parts[1].Trim();
                            tbxAddress.Text = parts[2].Trim();

                            tbxCustomerName.SelectionStart = tbxCustomerName.Text.Length;
                            tbxCustomerName.SelectionLength = 0;
                        }
                    }
                }


                panelSuggestion.Visible = false;
                panelSuggestion.Controls.Clear();

                e.IsInputKey = true;
                tbxWeightItems.Focus();
            }
        }

        private void SetupCustomerAutoComplete()
        {
            recurringCustomers = ucCustomers.GetRecurringCustomers();

            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();
            foreach (var c in recurringCustomers)
            {
                if (!string.IsNullOrWhiteSpace(c.CustomerName))
                    coll.Add(c.CustomerName);

                if (!string.IsNullOrWhiteSpace(c.CustomerCode))
                    coll.Add(c.CustomerCode);
            }
            //foreach (var c in recurringCustomers)
            //{
            //    coll.Add($"{c.CustomerCode} - {c.CustomerName}");
            //    coll.Add($"{c.CustomerName} - {c.CustomerCode}");
            //    coll.Add(c.ContactNumber);
            //}

            tbxCustomerName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbxCustomerName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tbxCustomerName.AutoCompleteCustomSource = coll;

            tbxCustomerName.TextChanged -= tbxCustomerName_TextChanged;
            tbxCustomerName.TextChanged += tbxCustomerName_TextChanged;

        }
        private void SetupDataGridView()
        {
            dtgOrderForm.AllowUserToAddRows = false;
            dtgOrderForm.ReadOnly = false;
            dtgOrderForm.RowHeadersVisible = false;
            dtgOrderForm.AllowUserToResizeRows = false;
            dtgOrderForm.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgOrderForm.DefaultCellStyle.SelectionBackColor = dtgOrderForm.DefaultCellStyle.BackColor;
            dtgOrderForm.DefaultCellStyle.SelectionForeColor = dtgOrderForm.DefaultCellStyle.ForeColor;

            AddRemoveButtonColumn();

            // ✅ Auto apply style to all "Remove" buttons
            dtgOrderForm.RowsAdded += (s, e) =>
            {
                foreach (DataGridViewRow row in dtgOrderForm.Rows)
                {
                    if (row.Cells["Remove"] is DataGridViewButtonCell cell)
                    {
                        cell.Style.BackColor = Color.LightCoral;
                        cell.Style.ForeColor = Color.White;
                        cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            };
        }


        private void AddRemoveButtonColumn()
        {
            if (!dtgOrderForm.Columns.Contains("Remove"))
            {
                DataGridViewButtonColumn btnRemove = new DataGridViewButtonColumn();
                btnRemove.Name = "Remove";
                btnRemove.HeaderText = "";
                btnRemove.Text = "Remove";
                btnRemove.UseColumnTextForButtonValue = true;
                btnRemove.Width = 80;
                btnRemove.DefaultCellStyle.BackColor = Color.LightCoral;
                btnRemove.DefaultCellStyle.ForeColor = Color.White;
                btnRemove.FlatStyle = FlatStyle.Flat;
                btnRemove.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgOrderForm.Columns.Add(btnRemove);
            }
        }
        private void dtgOrderForm_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgOrderForm.Columns[e.ColumnIndex].Name == "Remove" && e.RowIndex >= 0)
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to remove this item?",
                    "Confirm Remove",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    dtgOrderForm.Rows.RemoveAt(e.RowIndex);
                    UpdateTotalAmount();
                }
            }
        }


        private void ucOrderForm_Load(object sender, EventArgs e)
        {


            if (cbxCategory.SelectedValue != null)
            {
                string selectedCategory = cbxCategory.SelectedValue.ToString();
                LoadItemNamesByCategory(selectedCategory);

                // Also trigger weight enabling logic here
                if (selectedCategory == "Full Service" || selectedCategory == "Self Service")
                {
                    tbxWeightItems.Enabled = true;
                }
                else
                {
                    tbxWeightItems.Enabled = false;
                    tbxWeightItems.Text = string.Empty;
                }
            }
            this.ActiveControl = tbxCustomerName;
            tbxCustomerName.Focus();

        }



        private bool SaveOrder()
        {
            if (!ValidateRequiredFieldsOnSave())
            {
                MessageBox.Show("Please fill in all required fields before saving.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtgOrderForm.Rows.Count == 0)
            {
                MessageBox.Show("No items added. Please add at least one item before saving.", "Missing Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                string connStr = DbConnection.ConnectionString; // Make sure this is a valid SQLite connection string
                using (SQLiteConnection conn = new SQLiteConnection(connStr))
                {
                    conn.Open();
                    using (SQLiteTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            int customerId;

                            // 🔹 Ensure customer exists
                            string checkCustomer = @"SELECT CustomerID FROM Customers WHERE CustomerName = @Name AND ContactNumber = @Contact;";
                            using (SQLiteCommand cmdCheck = new SQLiteCommand(checkCustomer, conn, transaction))
                            {
                                cmdCheck.Parameters.AddWithValue("@Name", tbxCustomerName.Text);
                                cmdCheck.Parameters.AddWithValue("@Contact", tbxContactNum.Text);

                                object existingCustomer = cmdCheck.ExecuteScalar();
                                if (existingCustomer != null)
                                {
                                    customerId = Convert.ToInt32(existingCustomer);
                                }
                                else
                                {
                                    string insertCustomer = @"
                                INSERT INTO Customers (CustomerName, ContactNumber, Address)
                                VALUES (@Name, @Contact, @Address);
                                SELECT last_insert_rowid();";

                                    using (SQLiteCommand cmdInsert = new SQLiteCommand(insertCustomer, conn, transaction))
                                    {
                                        cmdInsert.Parameters.AddWithValue("@Name", tbxCustomerName.Text);
                                        cmdInsert.Parameters.AddWithValue("@Contact", tbxContactNum.Text);
                                        cmdInsert.Parameters.AddWithValue("@Address", tbxAddress.Text);
                                        customerId = Convert.ToInt32(cmdInsert.ExecuteScalar());
                                    }
                                }
                            }

                            // 🔹 Insert order
                            string insertOrder = @"
                        INSERT INTO Orders (CustomerID, OrderDate, TotalAmount, Status, Remarks, OrderCode, InvoiceNo)
                        VALUES (@CustomerID, @OrderDate, @TotalAmount, @Status, @Remarks, @OrderCode, @InvoiceNo);
                        SELECT last_insert_rowid();";

                            int orderId;
                            using (SQLiteCommand cmdOrder = new SQLiteCommand(insertOrder, conn, transaction))
                            {
                                cmdOrder.Parameters.AddWithValue("@CustomerID", customerId);
                                cmdOrder.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                                decimal.TryParse(lblTotal.Text, out decimal totalAmount);
                                cmdOrder.Parameters.AddWithValue("@TotalAmount", totalAmount);
                                cmdOrder.Parameters.AddWithValue("@Status", "Pending");
                                cmdOrder.Parameters.AddWithValue("@Remarks", tbxNotes.Text);
                                cmdOrder.Parameters.AddWithValue("@OrderCode", "ORD-" + Guid.NewGuid().ToString().Substring(0, 6));
                                cmdOrder.Parameters.AddWithValue("@InvoiceNo", "INV-" + Guid.NewGuid().ToString().Substring(0, 6));
                                orderId = Convert.ToInt32(cmdOrder.ExecuteScalar());
                            }

                            this.CustomerID = customerId;
                            this.OrderID = orderId;

                            // 🔹 Prepare for bulk inserts
                            StringBuilder insertDetailsBatch = new StringBuilder();
                            StringBuilder updateStockBatch = new StringBuilder();

                            foreach (DataGridViewRow row in dtgOrderForm.Rows)
                            {
                                if (row.IsNewRow) continue;

                                int itemId = GetItemIdByName(row.Cells["Items"].Value.ToString());
                                string itemName = row.Cells["Items"].Value?.ToString()?.Trim() ?? "";
                                string category = row.Cells["Category"].Value?.ToString()?.Trim() ?? "";

                                // Convert quantity
                                int quantity = 0;
                                if (row.Cells["Qty"].Value != null)
                                {
                                    decimal qtyDecimal;
                                    if (decimal.TryParse(row.Cells["Qty"].Value.ToString(), out qtyDecimal))
                                        quantity = (int)Math.Round(qtyDecimal);
                                }

                                // Convert weight
                                decimal weight = 0;
                                if (row.Cells["Weight"].Value != null)
                                {
                                    string raw = row.Cells["Weight"].Value.ToString().Replace("kg", "").Trim();
                                    decimal.TryParse(raw, out weight);
                                }

                                decimal price = Convert.ToDecimal(row.Cells["Price"].Value);

                                // Append to batch inserts
                                insertDetailsBatch.AppendLine($@"
                            INSERT INTO OrderDetails (OrderID, ItemID, Quantity, Weight, Price, OrderDetailCode)
                            VALUES ({orderId}, {itemId}, {quantity}, {weight}, {price}, 'DTL-{Guid.NewGuid().ToString().Substring(0, 6)}');");

                                // Append to batch stock updates (only for applicable items)
                                if ((category != "Full Service" && category != "Self Service") &&
                                    !(category == "Add On" && (itemName.Equals("Fold", StringComparison.OrdinalIgnoreCase) ||
                                                               itemName.Equals("Service Charge", StringComparison.OrdinalIgnoreCase))))
                                {
                                    updateStockBatch.AppendLine($@"
                                UPDATE Items SET StockAvailable = 
                                    CASE WHEN StockAvailable >= {quantity} THEN StockAvailable - {quantity} ELSE 0 END
                                WHERE ItemID = {itemId};");
                                }
                            }

                            // 🔹 Execute all detail inserts
                            if (insertDetailsBatch.Length > 0)
                            {
                                using (SQLiteCommand cmdBulkInsert = new SQLiteCommand(insertDetailsBatch.ToString(), conn, transaction))
                                {
                                    cmdBulkInsert.ExecuteNonQuery();
                                }
                            }

                            // 🔹 Execute all stock updates
                            if (updateStockBatch.Length > 0)
                            {
                                using (SQLiteCommand cmdBulkUpdate = new SQLiteCommand(updateStockBatch.ToString(), conn, transaction))
                                {
                                    cmdBulkUpdate.ExecuteNonQuery();
                                }
                            }

                            // 🔹 Commit transaction
                            transaction.Commit();

                            MessageBox.Show("Order saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // 🔹 Check low stock items
                            CheckLowStockWarning(conn, OrderID);

                            OnOrderSaved?.Invoke();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Error saving order: " + ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }





        private void LoadCategories()
        {
            string connStr = DbConnection.ConnectionString;
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                string query = @"
            SELECT Category
            FROM Items
            GROUP BY Category
            ORDER BY
                CASE 
                    WHEN Category = 'Full Service' THEN 1
                    WHEN Category = 'Self Service' THEN 2
                    ELSE 3
                END, Category;";

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cbxCategory.DataSource = dt;
                cbxCategory.DisplayMember = "Category";
                cbxCategory.ValueMember = "Category";

                cbxCategory.SelectedIndex = -1;
            }
        }

        public void LoadItemNamesByCategory(string category)
        {
            string connStr = DbConnection.ConnectionString;
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                string query = @"
            SELECT DISTINCT i.ItemID, i.ItemName
            FROM Items i
            INNER JOIN Pricing p ON i.ItemID = p.ItemID
            WHERE i.Category = @Category
              AND (i.IsArchived = 0 OR i.IsArchived IS NULL)
              AND (p.IsArchived = 0 OR p.IsArchived IS NULL)
            ORDER BY i.ItemName;";

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@Category", category);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cbxItemName.DataSource = dt;
                cbxItemName.DisplayMember = "ItemName";
                cbxItemName.ValueMember = "ItemID";
                cbxItemName.SelectedIndex = -1;
            }
        }



        private void UpdateItemDropdownAndWeightControl(string category)
        {
            LoadItemNamesByCategory(category);

            if (category == "Full Service" || category == "Self Service")
                tbxWeightItems.Enabled = true;
            else
            {
                tbxWeightItems.Enabled = false;
                tbxWeightItems.Text = string.Empty;
            }
        }

        private void cbxCategory_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string category = cbxCategory.Text;

            if (string.IsNullOrEmpty(category))
                return;

            // Load items for the selected category
            LoadItemNamesByCategory(category);

            // Enable/disable weight textbox
            if (category == "Full Service" || category == "Self Service")
                tbxWeightItems.Enabled = true;
            else
            {
                tbxWeightItems.Enabled = false;
                tbxWeightItems.Text = string.Empty;
            }
        }


        private void tbxCustomerName_TextChanged(object sender, EventArgs e)
        {
            string input = tbxCustomerName.Text.Trim();

            // Validation
            if (customerNameTouched)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    lblCustomerNameValidation.ForeColor = Color.Red;
                    lblCustomerNameValidation.Text = "Customer name cannot be empty.";
                    lblCustomerNameValidation.Visible = true;
                }
                else if (!IsValidCustomerName(input))
                {
                    lblCustomerNameValidation.ForeColor = Color.Red;
                    lblCustomerNameValidation.Text = "Please enter a valid customer name.";
                    lblCustomerNameValidation.Visible = true;
                }
                else
                {
                    lblCustomerNameValidation.Visible = false;
                }
            }


            // Suggestion dropdown
            if (input.Length < 3 || recurringCustomers == null)
            {
                panelSuggestion.Visible = false;
                panelSuggestion.Controls.Clear();
                return;
            }
            //this
            var matches = recurringCustomers
       .Where(c =>
           (!string.IsNullOrEmpty(c.CustomerName) &&
            c.CustomerName.IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0)
           ||
           (!string.IsNullOrEmpty(c.CustomerCode) &&
            c.CustomerCode.IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0)
       )
       .ToList();


            if (matches.Count == 0)
            {
                panelSuggestion.Visible = false;
                panelSuggestion.Controls.Clear();
                return;
            }
            panelSuggestion.Controls.Clear(); // Clear old results, this

            foreach (var c in matches)
            {
                Label lbl = new Label
                {
                    Text = $"{c.CustomerName} - {c.ContactNumber} - {c.Address}",
                    AutoSize = false,
                    Height = 24,
                    Dock = DockStyle.Top,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Cursor = Cursors.Hand,
                    Padding = new Padding(5)
                };
                lbl.MouseEnter += (s, ev) =>
                {
                    lbl.BackColor = Color.MediumPurple; // Highlight on hover
                    lbl.ForeColor = Color.White; // Optional: better contrast
                };

                lbl.MouseLeave += (s, ev) =>
                {
                    lbl.BackColor = Color.Lavender; // Reset to default
                    lbl.ForeColor = Color.Black; // Restore text color
                };

                lbl.Click += (s, ev) =>
                {
                    tbxCustomerName.Text = c.CustomerName;
                    tbxContactNum.Text = c.ContactNumber;
                    tbxAddress.Text = c.Address;


                    panelSuggestion.Visible = false;
                };

                panelSuggestion.Controls.Add(lbl);

            }

            panelSuggestion.Height = Math.Min(matches.Count * 24, 96);
            panelSuggestion.Visible = matches.Count > 0;
            panelSuggestion.BringToFront();
        }


        private int GetItemIdByName(string itemName)
        {
            string connStr = DbConnection.ConnectionString;
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                string query = "SELECT ItemID FROM Items WHERE ItemName = @ItemName";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ItemName", itemName);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0; // return 0 if not found
                }
            }
        }



        private void UpdateTotalAmount()
        {
            lblTotal.Text = dtgOrderForm.Rows.Cast<DataGridViewRow>()
       .Sum(r => Convert.ToDecimal(r.Cells["Amount"].Value))
       .ToString("N2");
        }



        private void tbxContactNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Allow only digits, backspace, and one '+' at the start
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                if (!(e.KeyChar == '+' && tb.SelectionStart == 0 && !tb.Text.Contains("+")))
                {
                    e.Handled = true;
                    return;
                }
            }

            // 🔹 Restrict length based on prefix
            if (!char.IsControl(e.KeyChar))
            {
                string text = tb.Text;

                if (text.StartsWith("09"))
                {
                    if (text.Length >= 11)
                        e.Handled = true;
                }
                else if (text.StartsWith("+63"))
                {
                    if (text.Length >= 13)
                        e.Handled = true;
                }
                else
                {
                    // Invalid prefix — still cap at 13 for safety
                    if (text.Length >= 13)
                        e.Handled = true;
                }
            }
        }






        private bool IsValidPHMobileNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Clean input — remove spaces, dashes, parentheses
            string cleaned = new string(input.Where(c => char.IsDigit(c) || c == '+').ToArray());

            // Must start with +63 or 09
            if (cleaned.StartsWith("+63"))
            {
                // +63 + 10 digits = 13 chars total
                string digits = cleaned.Substring(3);
                return digits.Length == 10 && digits.All(char.IsDigit);
            }
            else if (cleaned.StartsWith("09"))
            {
                // 11 digits total, all numeric
                return cleaned.Length == 11 && cleaned.All(char.IsDigit);
            }

            return false;
        }

        private void panelSuggestion_Paint(object sender, PaintEventArgs e)
        {
        }
        private const decimal MIN_WEIGHT = 1m;
        private const decimal MAX_WEIGHT = 9.99m;

        private void tbxWeightItems_Leave(object sender, EventArgs e)
        {
            // Remove any existing "kg" before parsing
            string raw = tbxWeightItems.Text.Replace("kg", "").Trim();

            if (decimal.TryParse(raw, out decimal w) && w > 0)
            {
                if (w > MAX_WEIGHT)
                {
                    // Show validation instead of MessageBox
                    lblWeightValidation.Visible = true;
                    lblWeightValidation.ForeColor = Color.Red;
                    lblWeightValidation.Text = $"Maximum allowed weight is {MAX_WEIGHT} kg.";
                    tbxWeightItems.Text = $"{MAX_WEIGHT:0.00} kg";
                }
                else
                {
                    // Clear validation and format normally
                    lblWeightValidation.Visible = false;
                    lblWeightValidation.Text = "";
                    tbxWeightItems.Text = w.ToString("0.00") + " kg";
                }
            }
            else
            {
                tbxWeightItems.Text = ""; // clear invalid input
            }

            // Move cursor to end
            tbxWeightItems.SelectionStart = tbxWeightItems.Text.Length;
            tbxWeightItems.SelectionLength = 0;
        }





        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void btnAddList_Click(object sender, EventArgs e)
        {
            try
            {
                // ===== Validation =====
                if (string.IsNullOrWhiteSpace(tbxCustomerName.Text) ||
                    string.IsNullOrWhiteSpace(tbxContactNum.Text) ||
                    string.IsNullOrWhiteSpace(tbxAddress.Text))
                {
                    MessageBox.Show("Customer information is required.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!IsValidPHMobileNumber(tbxContactNum.Text))
                {
                    MessageBox.Show("Please enter a valid mobile number starting with '09' or '+63'.", "Invalid Mobile Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbxItemName.SelectedValue == null)
                {
                    MessageBox.Show("Please select an item.", "Missing Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal qty = 0;
                decimal weight = 0;

                // Parse Quantity
                if (!string.IsNullOrEmpty(tbxQtyOthers.Text))
                {
                    decimal.TryParse(tbxQtyOthers.Text, out qty);
                }

                //Parse Weight
                if (!string.IsNullOrEmpty(tbxWeightItems.Text))
                {
                    string rawWeight = tbxWeightItems.Text.Replace("kg", "").Trim();
                    decimal.TryParse(rawWeight, out weight);
                }

                // ===== Fetch pricing =====
                int itemId = Convert.ToInt32(cbxItemName.SelectedValue);
                decimal price = 0;
                decimal subtotal = 0;
                string pricingType = "";
                decimal minWeight = 0, maxWeight = 9999;

                string connStr = DbConnection.ConnectionString;
                using (SQLiteConnection conn = new SQLiteConnection(connStr))
                {
                    conn.Open();
                    string priceQuery = @"
            SELECT Price, PricingType, MinWeight, MaxWeight
            FROM Pricing
            WHERE ItemID = @ItemID
            ORDER BY 
                CASE 
                    WHEN PricingType = 'Per KG' THEN 1
                    WHEN PricingType = 'Per Load' THEN 2
                    WHEN PricingType = 'Per Minimum' THEN 3
                    WHEN PricingType = 'Fixed' THEN 4
                    ELSE 5
                END
            LIMIT 1;"; // SQLite uses LIMIT instead of TOP 1

                    using (SQLiteCommand cmd = new SQLiteCommand(priceQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ItemID", itemId);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                price = Convert.ToDecimal(reader["Price"]);
                                pricingType = reader["PricingType"].ToString();
                                if (reader["MinWeight"] != DBNull.Value) minWeight = Convert.ToDecimal(reader["MinWeight"]);
                                if (reader["MaxWeight"] != DBNull.Value) maxWeight = Convert.ToDecimal(reader["MaxWeight"]);
                                lblWeightValidation.Visible = false;
                                lblWeightValidation.Text = "";
                            }
                            else
                            {
                                MessageBox.Show("No pricing found for this item.", "Pricing Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }
                // Category-specific requirement
                string category = cbxCategory.Text;


                // Weight is required ONLY for "Per KG" or "Per Minimum"
                bool weightRequired = pricingType == "Per KG" || pricingType == "Per Minimum";

                // QTY required for all items
                if (qty == 0)
                {
                    MessageBox.Show("Quantity is required for this item.", "Missing Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Weight required only for specific pricing modes
                if (weightRequired && weight == 0)
                {
                    MessageBox.Show("Weight is required for this item.", "Missing Weight", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                else
                {
                    if (qty == 0)
                    {
                        MessageBox.Show("Quantity is required for this item.",
                                        "Missing Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (category != "Full Service" && category != "Self Service" &&
    !(category == "Add On" &&
      (cbxItemName.Text.Equals("Fold", StringComparison.OrdinalIgnoreCase) ||
       cbxItemName.Text.Equals("Service Charge", StringComparison.OrdinalIgnoreCase))))
                {
                    using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
                    {
                        conn.Open();
                        string stockQuery = "SELECT StockAvailable FROM Items WHERE ItemID = @ItemID";
                        using (SQLiteCommand cmdStock = new SQLiteCommand(stockQuery, conn))
                        {
                            cmdStock.Parameters.AddWithValue("@ItemID", itemId);
                            object stockObj = cmdStock.ExecuteScalar();
                            decimal availableStock = (stockObj == DBNull.Value) ? 0 : Convert.ToDecimal(stockObj);

                            if (qty > availableStock)
                            {
                                MessageBox.Show(
                                    $"'{cbxItemName.Text}' is out of stock.\nAvailable: {availableStock}, Requested: {qty}",
                                    "Insufficient Stock",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
                                return; // stop before adding to list
                            }
                        }
                    }
                }



                // ===== Compute subtotal =====
                switch (pricingType)
                {
                    case "Per KG":
                        subtotal = (price * weight) * qty;
                        break;

                    case "Per Load":
                        subtotal = price * qty;
                        break;

                    case "Per Minimum":
                        if (weight < minWeight || weight > maxWeight)
                        {
                            MessageBox.Show("Please Input Valid Weight", "Weight Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        subtotal = price * qty;
                        break;

                    case "Fixed":
                        subtotal = price * qty;
                        break;

                    default:
                        MessageBox.Show("Invalid pricing type for this item.", "Pricing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }

                // ===== Merge or add row =====
                string[] mergeCategories = { "Add On", "Others" };
                bool canMerge = mergeCategories.Contains(category);
                if (canMerge)
                {
                    foreach (DataGridViewRow row in dtgOrderForm.Rows)
                    {
                        if (row.Cells["Category"].Value?.ToString() == category &&
                            row.Cells["Items"].Value?.ToString() == cbxItemName.Text)
                        {
                            // Merge quantity
                            decimal oldQty = 0;
                            decimal.TryParse(row.Cells["Qty"].Value?.ToString(), out oldQty);
                            row.Cells["Qty"].Value = oldQty + qty;

                            // Merge weight
                            decimal oldWeight = 0;
                            decimal.TryParse(row.Cells["Weight"].Value?.ToString().Replace("kg", ""), out oldWeight);
                            if (weight > 0)
                                row.Cells["Weight"].Value = (oldWeight + weight).ToString("N2") + " kg";

                            // Merge subtotal
                            decimal oldAmount = 0;
                            decimal.TryParse(row.Cells["Amount"].Value?.ToString(), out oldAmount);
                            row.Cells["Amount"].Value = (oldAmount + subtotal).ToString("N2");

                            UpdateTotalAmount();
                            return; // merged
                        }
                    }
                }

                // Add new row
                dtgOrderForm.Rows.Add(
                    category,
                    cbxItemName.Text,
                    qty > 0 ? qty.ToString("N2") : "",
                    weight > 0 ? weight.ToString("N2") + " kg" : "",
                    price.ToString("N2"),
                    subtotal.ToString("N2")
                );

                // Lock customer info
                //if (originalCustomerName == null && originalContactNum == null)
                //{
                //    originalCustomerName = tbxCustomerName.Text;
                //    originalContactNum = tbxContactNum.Text;
                //}
                //tbxCustomerName.ReadOnly = true;
                //tbxContactNum.ReadOnly = true;
                //tbxAddress.ReadOnly = true;
                panelSuggestion.Visible = false;

                UpdateTotalAmount();
                // ===== Clear input fields after adding to list =====
                cbxCategory.SelectedIndex = -1;
                cbxItemName.SelectedIndex = -1;
                tbxWeightItems.Text = "";
                tbxQtyOthers.Text = "";
                lblWeightValidation.Visible = false;
                lblWeightValidation.Text = "";
                cbxCategory.Focus();

            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid number format. Please enter numeric values for quantity or weight.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
        "Are you sure you want to cancel?",
        "Confirmation",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

            if (result == DialogResult.No)
                return;

            if (popup != null)
            {
                popup.Close();
            }
            originalCustomerName = null;
            originalContactNum = null;

            // Unlock on cancel
            tbxCustomerName.ReadOnly = false;
            tbxContactNum.ReadOnly = false;
            tbxAddress.ReadOnly = false;

            panelSuggestion.Visible = true;
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (this.OrderID == 0) // not saved yet
            {
                bool saved = SaveOrder();
                if (!saved)
                {
                    // if save failed, don't proceed to payment
                    return;
                }

                // ✅ After saving, check for low stock items using SQLite
                using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
                {
                    conn.Open();
                    // CheckLowStockWarning(conn, this.OrderID); // Make sure CheckLowStockWarning accepts SQLiteConnection
                }
            }

            // now safe to open payment form with valid OrderID and CustomerID
            try
            {
                FormDimmer dimmer = new FormDimmer
                {
                    Size = mainForm.Size,
                    Location = mainForm.Location,
                    Owner = mainForm
                };
                dimmer.Show();

                Form paymentPopup = new Form
                {
                    FormBorderStyle = FormBorderStyle.None,
                    StartPosition = FormStartPosition.CenterParent,
                    Size = new Size(774, 574),
                    BackColor = Color.White,
                    ShowInTaskbar = false
                };

                ucPaymentForm paymentForm = new ucPaymentForm(mainForm, paymentPopup, "order");
                paymentForm.PaymentCompleted += () =>
                {
                    OnOrderSaved?.Invoke();
                };

                paymentForm.CustomerNameTextBox.Text = tbxCustomerName.Text;
                paymentForm.TotalAmountTextBox.Text = lblTotal.Text;

                paymentForm.CustomerID = this.CustomerID;
                paymentForm.OrderID = this.OrderID;

                paymentForm.PaymentMethodComboBox.Items.Clear();
                paymentForm.PaymentMethodComboBox.Items.AddRange(new string[] { "Cash", "Gcash", "Maya", "Others" });
                paymentForm.PaymentMethodComboBox.SelectedIndex = 0;

                paymentForm.AmountReceivedTextBox.Text = "";
                paymentForm.ChangeTextBox.Text = "";

                paymentForm.Dock = DockStyle.Fill;
                paymentPopup.Controls.Add(paymentForm);

                paymentPopup.ShowDialog();
                dimmer.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error while navigating to payment: " + ex.Message,
                    "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1️⃣ Required fields check
            if (!ValidateRequiredFieldsOnSave())
            {
                MessageBox.Show("Please fill in all required fields before saving.",
                                "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2️⃣ Must have at least one item
            if (dtgOrderForm.Rows.Count == 0)
            {
                MessageBox.Show("Please add at least one item before saving the order.",
                                "No Items Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3️⃣ Check printer BEFORE saving OR printing
            PrinterSettings xp58 = GetReceiptPrinter();
            if (xp58 == null)
                return; // User declined or no printer found

            if (!IsPrinterOnline(xp58.PrinterName))
            {
                MessageBox.Show(
                    $"The printer \"{xp58.PrinterName}\" is offline or not responding.\n" +
                    $"Please check the connection and try again.",
                    "Printer Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return; // ❌ STOP — do not save, do not print
            }

            // 4️⃣ Save the order (ONLY if printer is OK)
            SaveOrder();

            // 5️⃣ Parse total amount
            decimal totalAmount = decimal.TryParse(lblTotal.Text, out decimal amount) ? amount : 0;

            // 6️⃣ Get invoice number
            string invoiceNo = GetInvoiceNoByOrderID(this.OrderID);

            // 7️⃣ Create summary UC & print receipt
            ucUnpaidOrderSummary unpaidSummaryUC = new ucUnpaidOrderSummary
            {
                InvoiceNo = invoiceNo,
                Total = totalAmount,
                CustomerName = tbxCustomerName.Text,
                CustomerContact = tbxContactNum.Text
            };

            unpaidSummaryUC.PrintReceipt(); // ✅ SAFE: printer is confirmed working

            popup.Close();
        }


        private string GetInvoiceNoByOrderID(int orderId)
        {
            string invoiceNo = string.Empty;
            string connectionString = DbConnection.ConnectionString;

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT InvoiceNo FROM Orders WHERE OrderID = @OrderID";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        invoiceNo = result.ToString();
                    }
                }
            }

            return invoiceNo; // Always returns a string (never null)
        }
        private void tbxAddress_TextChanged(object sender, EventArgs e)
        {
            if (!addressTouched)
                return;

            // If they deleted all text after typing
            if (string.IsNullOrWhiteSpace(tbxAddress.Text))
            {
                lblAddressValidation.ForeColor = Color.Red;
                lblAddressValidation.Text = "Address cannot be empty.";
                lblAddressValidation.Visible = true;
            }
            else if (!IsValidAddress(tbxAddress.Text)) // <-- your validation logic
            {
                lblAddressValidation.ForeColor = Color.Red;
                lblAddressValidation.Text = "Address is not valid.";
                lblAddressValidation.Visible = true;
            }
            else
            {
                lblAddressValidation.Visible = false;
            }
        }




        private void tbxWeightItems_TextChanged(object sender, EventArgs e)
        {
            lblWeightValidation.Visible = false;
            lblWeightValidation.Text = "";

            string rawWeight = tbxWeightItems.Text.Replace("kg", "").Trim();

            // Live validation for max weight
            if (decimal.TryParse(rawWeight, out decimal currentWeight))
            {
                if (currentWeight > MAX_WEIGHT)
                {
                    lblWeightValidation.Visible = true;
                    lblWeightValidation.ForeColor = Color.Red;
                    lblWeightValidation.Text = $"Maximum allowed weight is {MAX_WEIGHT} kg.";
                }
            }

            if ((cbxCategory.Text == "Full Service" || cbxCategory.Text == "Self Service") &&
                cbxItemName.SelectedValue != null)
            {
                if (decimal.TryParse(rawWeight, out decimal weight))
                {
                    int itemId = Convert.ToInt32(cbxItemName.SelectedValue);

                    // Fetch min and max weight for the selected item
                    decimal minWeight = 0, maxWeight = 9999;
                    string connStr = DbConnection.ConnectionString;
                    using (SQLiteConnection conn = new SQLiteConnection(connStr))
                    {
                        conn.Open();
                        string priceQuery = @"SELECT MinWeight, MaxWeight 
                                      FROM Pricing 
                                      WHERE ItemID = @ItemID 
                                      ORDER BY 
                                        CASE WHEN PricingType = 'Per KG' THEN 1 
                                             WHEN PricingType = 'Per Load' THEN 2 
                                             WHEN PricingType = 'Per Minimum' THEN 3 
                                             WHEN PricingType = 'Fixed' THEN 4 
                                             ELSE 5 END
                                      LIMIT 1";

                        using (SQLiteCommand cmd = new SQLiteCommand(priceQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@ItemID", itemId);
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (reader["MinWeight"] != DBNull.Value)
                                        minWeight = Convert.ToDecimal(reader["MinWeight"]);
                                    if (reader["MaxWeight"] != DBNull.Value)
                                        maxWeight = Convert.ToDecimal(reader["MaxWeight"]);
                                }
                            }
                        }
                    }

                    // Validate against DB min/max
                    if (weight < minWeight || weight > maxWeight)
                    {
                        lblWeightValidation.Visible = true;
                        lblWeightValidation.ForeColor = Color.Red;
                        lblWeightValidation.Text = $"Allowed weight: {minWeight}–{maxWeight} kg.";
                    }
                }
            }
        }

        private void tbxWeightItems_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Allow control keys (Backspace, Delete, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            // Allow only digits and one decimal point
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && tb.Text.Contains("."))
            {
                e.Handled = true;
                return;
            }

            // Simulate the text after this keypress
            string newText = tb.Text.Remove(tb.SelectionStart, tb.SelectionLength)
                                    .Insert(tb.SelectionStart, e.KeyChar.ToString());

            // ✅ Allow typing any number up to 99.99, but show validation label if > 9.99
            if (decimal.TryParse(newText, out decimal testValue))
            {
                if (testValue > MAX_WEIGHT)
                {
                    // Do NOT block typing
                    lblWeightValidation.Visible = true;
                    lblWeightValidation.ForeColor = Color.Red;
                    lblWeightValidation.Text = $"Maximum allowed weight is {MAX_WEIGHT} kg.";
                }
                else
                {
                    lblWeightValidation.Visible = false;
                    lblWeightValidation.Text = "";
                }
            }

            // Prevent more than 2 digits before decimal if no decimal exists
            if (!tb.Text.Contains("."))
            {
                if (tb.Text.Length >= 2 && e.KeyChar != '.' && tb.SelectionLength == 0)
                {
                    e.Handled = true;
                    return;
                }
            }

            // Prevent more than 2 decimal places
            if (tb.Text.Contains("."))
            {
                int index = tb.Text.IndexOf(".");
                int decimals = tb.Text.Length - index - 1;

                if (tb.SelectionStart > index && decimals >= 2)
                {
                    e.Handled = true;
                    return;
                }
            }
        }





        private void CheckLowStockWarning(SQLiteConnection existingConnection, int orderId)
        {
            try
            {
                string query = @"
            SELECT 
                ItemName, 
                StockAvailable
            FROM Items
            WHERE 
                LowStockThreshold IS NOT NULL 
                AND StockAvailable <= LowStockThreshold
                AND Category NOT IN ('Full Service', 'Self Service', 'Extra Charges')
                AND ItemName NOT IN ('Fold', 'Service Charge')
                AND (IsArchived = 0 OR IsArchived IS NULL)";

                using (SQLiteCommand cmd = new SQLiteCommand(query, existingConnection))
                {
                    // No need for OrderID parameter since it's not used in the query
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        StringBuilder lowStockItems = new StringBuilder();
                        StringBuilder outOfStockItems = new StringBuilder();

                        while (reader.Read())
                        {
                            string itemName = reader["ItemName"].ToString();
                            int stock = Convert.ToInt32(reader["StockAvailable"]);

                            if (stock <= 0)
                                outOfStockItems.AppendLine($"- {itemName} (Out of stock)");
                            else
                                lowStockItems.AppendLine($"- {itemName} ({stock} left)");
                        }

                        if (outOfStockItems.Length > 0)
                        {
                            string message =
                                "The following item/s are out of stock:\n\n" +
                                outOfStockItems.ToString() +
                                "\nPlease restock.";

                            MessageBox.Show(
                                message,
                                "Out of Stock Warning",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                        }
                        else if (lowStockItems.Length > 0)
                        {
                            string message =
                                "The following item/s are low on stock:\n\n" +
                                lowStockItems.ToString() +
                                "\nPlease restock them soon.";

                            MessageBox.Show(
                                message,
                                "Low Stock Warning",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Something went wrong while checking stock levels:\n\n" + ex.Message,
                    "System Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }





        private void tbxQtyOthers_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Remove non-digit characters
            string digitsOnly = new string(tb.Text.Where(char.IsDigit).ToArray());

            // Limit to 3 digits
            if (digitsOnly.Length > 3)
                digitsOnly = digitsOnly.Substring(0, 3);

            if (tb.Text != digitsOnly)
            {
                int cursorPos = tb.SelectionStart - (tb.Text.Length - digitsOnly.Length);
                tb.Text = digitsOnly;
                tb.SelectionStart = Math.Max(0, cursorPos);
            }
        }



        private void tbxContactNum_TextChanged(object sender, EventArgs e)
        {
            // If we're running validation due to timer, skip typing flag
            if (!isValidating)
            {
                //lblContactValidation.Visible = false; // hide while typing
                isTyping = true;

                // restart timer
                contactTypingTimer.Stop();
                contactTypingTimer.Start();

                return; // don’t validate yet while typing
            }

            // 🔹 Validation logic runs only when timer triggers
            string input = tbxContactNum.Text.Trim();
            lblContactValidation.ForeColor = Color.Red;

            if (string.IsNullOrEmpty(input))
            {
                lblContactValidation.Visible = false;
                return;
            }
            if (string.IsNullOrEmpty(input))
            {
                lblContactValidation.Text = "Contact number cannot be empty.";
                lblContactValidation.Visible = true;
                return;
            }

            if (input.StartsWith("09"))
            {
                // ✅ Prevent typing more than 11 digits
                if (input.Length > 11)
                {
                    tbxContactNum.Text = input.Substring(0, 11);
                    tbxContactNum.SelectionStart = tbxContactNum.Text.Length; // ✅ added
                }
                if (input.Length < 11)
                {
                    lblContactValidation.Text = "Mobile number must be 11 digits long.";
                    lblContactValidation.Visible = true;
                }
                else if (input.Length > 11)
                {
                    lblContactValidation.Text = "Mobile number must only have 11 digits.";
                    lblContactValidation.Visible = true;
                }
                else
                {
                    lblContactValidation.Visible = false; // ✅ valid
                }
            }
            else if (input.StartsWith("+63"))
            {
                // ✅ Prevent typing more than 13 chars
                if (input.Length > 13)
                {
                    tbxContactNum.Text = input.Substring(0, 13);
                    tbxContactNum.SelectionStart = tbxContactNum.Text.Length; // ✅ added
                }
                if (input.Length < 13)
                {
                    lblContactValidation.Text = "Mobile number must be 13 digits including '+63'.";
                    lblContactValidation.Visible = true;
                }
                else if (input.Length > 13)
                {
                    lblContactValidation.Text = "Mobile number must only have 13 characters.";
                    lblContactValidation.Visible = true;
                }
                else
                {
                    lblContactValidation.Visible = false; // ✅ valid
                }
            }
            else
            {
                lblContactValidation.Text = "Number must start with '09' or '+63'.";
                lblContactValidation.Visible = true;
            }
        }


        private void ContactTypingTimer_Tick(object sender, EventArgs e)
        {
            contactTypingTimer.Stop();
            isTyping = false;
            isValidating = true; // ✅ tell TextChanged this is from timer

            tbxContactNum_TextChanged(tbxContactNum, EventArgs.Empty); // trigger validation

            isValidating = false; // reset flag
        }

        private void tbxAddress_Leave(object sender, EventArgs e)
        {
            addressTouched = true;

            // Trim extra spaces and replace multiple spaces with a single space
            tbxAddress.Text = System.Text.RegularExpressions.Regex.Replace(tbxAddress.Text.Trim(), @"\s+", " ");

            // Convert to Title Case
            System.Globalization.TextInfo textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
            tbxAddress.Text = textInfo.ToTitleCase(tbxAddress.Text.ToLower());

            if (string.IsNullOrWhiteSpace(tbxAddress.Text))
            {
                lblAddressValidation.ForeColor = Color.Red;
                lblAddressValidation.Text = "Address cannot be empty.";
                lblAddressValidation.Visible = true;
                return;
            }

            // Regex: must start with a letter, number, or #, then letters, numbers, spaces, #, -, ,, ., /
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbxAddress.Text, @"^[A-Za-z0-9#][A-Za-z0-9\s#\-,./]*$"))
            {
                lblAddressValidation.ForeColor = Color.Red;
                lblAddressValidation.Text = "Address is not valid.";
                lblAddressValidation.Visible = true;
                return;
            }

            lblAddressValidation.Visible = false;
        }


        bool addressTouched = false;
        private void tbxAddress_Enter(object sender, EventArgs e)
        {
            addressTouched = true;
        }
        bool customerNameTouched = false;

        private void tbxCustomerName_Enter(object sender, EventArgs e)
        {
            customerNameTouched = true;
        }

        private void tbxCustomerName_Leave(object sender, EventArgs e)
        {
            customerNameTouched = true; // ensure flag is true

            string input = tbxCustomerName.Text.Trim();
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ");
            if (string.IsNullOrWhiteSpace(input))
            {
                lblCustomerNameValidation.ForeColor = Color.Red;
                lblCustomerNameValidation.Text = "Customer name cannot be empty.";
                lblCustomerNameValidation.Visible = true;
            }
            else if (!IsValidCustomerName(input))
            {
                lblCustomerNameValidation.ForeColor = Color.Red;
                lblCustomerNameValidation.Text = "Please enter a valid customer name.";
                lblCustomerNameValidation.Visible = true;
            }
            else
            {
                System.Globalization.TextInfo textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                tbxCustomerName.Text = textInfo.ToTitleCase(input.ToLower());
                lblCustomerNameValidation.Visible = false;
            }

            panelSuggestion.Visible = false;
        }
        bool contactTouched = false;
        private void tbxContactNum_Enter(object sender, EventArgs e)
        {
            contactTouched = true;
        }

        private void tbxContactNum_Leave(object sender, EventArgs e)
        {
            string input = tbxContactNum.Text.Trim();

            if (string.IsNullOrWhiteSpace(input) && contactTouched)
            {

                lblContactValidation.ForeColor = Color.Red;
                lblContactValidation.Text = "Contact number cannot be empty.";
                lblContactValidation.Visible = true;
            }
            else
            {
                isValidating = true;
                tbxContactNum_TextChanged(tbxContactNum, EventArgs.Empty);
                isValidating = false;
            }
        }

        private void tbxCustomerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            // Allow control keys (Backspace, Delete, etc.)
            if (char.IsControl(ch))
                return;

            string text = tbxCustomerName.Text;
            int cursorPos = tbxCustomerName.SelectionStart;

            // Prevent leading space
            if (cursorPos == 0 && ch == ' ')
            {
                e.Handled = true;
                return;
            }

            // ✅ Allow letters, digits, and spaces freely
            if (char.IsLetterOrDigit(ch) || ch == ' ')
                return;

            // Allow special characters: '.', '-', and '\''
            if (ch == '.' || ch == '-' || ch == '\'')
            {
                // Prevent placing at the beginning
                if (cursorPos == 0)
                {
                    e.Handled = true;
                    return;
                }

                // Prevent consecutive special chars or space before/after
                char prevChar = cursorPos > 0 ? text[cursorPos - 1] : '\0';
                if (prevChar == '.' || prevChar == '-' || prevChar == '\'' || prevChar == ' ')
                {
                    e.Handled = true;
                    return;
                }

                return; // valid special char
            }

            // Block all other characters
            e.Handled = true;
        }




        private void tbxAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tbxAddress.SelectionStart == 0 && e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void tbxNotes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tbxNotes.SelectionStart == 0 && e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void tbxNotes_Leave(object sender, EventArgs e)
        {
            string input = tbxNotes.Text.Trim();

            if (!string.IsNullOrWhiteSpace(input))
            {
                // Replace multiple spaces with a single space
                input = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ");

                
            }
        }
        private bool ValidateRequiredFieldsOnSave()
        {
            bool hasError = false;

            // 🔹 Customer Name
            if (string.IsNullOrWhiteSpace(tbxCustomerName.Text))
            {
                lblCustomerNameValidation.ForeColor = Color.Red;
                lblCustomerNameValidation.Text = "Customer name cannot be empty.";
                lblCustomerNameValidation.Visible = true;
                hasError = true;
            }
            else
            {
                lblCustomerNameValidation.Visible = false;
            }

            // 🔹 Contact Number
            if (string.IsNullOrWhiteSpace(tbxContactNum.Text))
            {
                lblContactValidation.ForeColor = Color.Red;
                lblContactValidation.Text = "Contact number cannot be empty.";
                lblContactValidation.Visible = true;
                hasError = true;
            }
            else
            {
                lblContactValidation.Visible = false;
            }

            // 🔹 Address
            if (string.IsNullOrWhiteSpace(tbxAddress.Text))
            {
                lblAddressValidation.ForeColor = Color.Red;
                lblAddressValidation.Text = "Address cannot be empty.";
                lblAddressValidation.Visible = true;
                hasError = true;
            }
            else
            {
                lblAddressValidation.Visible = false;
            }

            return !hasError; // ✅ Return true only if all fields are filled
        }
        // Helper method to validate customer name
        private bool IsValidCustomerName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            // Trim extra spaces
            name = System.Text.RegularExpressions.Regex.Replace(name, @"\s+", " ");

            // Check for invalid characters
            if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[A-Za-z\s\.\-']+$"))
                return false;

            // Must contain at least one letter
            if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"[A-Za-z]"))
                return false;

            // Optional: ensure it's at least 2 characters long overall
            if (name.Length < 3)
                return false;

            return true;

        }
        private bool IsValidAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return false;

            // Trim extra spaces
            address = System.Text.RegularExpressions.Regex.Replace(address, @"\s+", " ");

            // ❌ Reject if starts with invalid character
            if (System.Text.RegularExpressions.Regex.IsMatch(address, @"^[\.\-\/,']"))
                return false;

            // ✅ Allow letters, numbers, spaces, commas, periods, hyphens, slashes, and apostrophes
            if (!System.Text.RegularExpressions.Regex.IsMatch(address, @"^[A-Za-z0-9\s,\.\-\/']+$"))
                return false;

            // Must contain at least one letter or digit
            if (!System.Text.RegularExpressions.Regex.IsMatch(address, @"[A-Za-z0-9]"))
                return false;

            // Optional: ensure it's at least 3 characters long
            if (address.Length < 3)
                return false;

            return true;
        }

        private PrinterSettings GetReceiptPrinter()
        {
            PrinterSettings xp58 = null;

            foreach (string printerName in PrinterSettings.InstalledPrinters)
            {
                if (printerName.ToLower().Contains("xp-58") ||
                    printerName.ToLower().Contains("xprinter") ||
                    printerName.ToLower().Contains("pos"))
                {
                    xp58 = new PrinterSettings { PrinterName = printerName };
                    break;
                }
            }

            if (xp58 == null)
            {
                string defaultPrinter = new PrinterSettings().PrinterName;

                DialogResult useDefault = MessageBox.Show(
                    $"XP-58 not found.\nUse default printer instead?\n({defaultPrinter})",
                    "Printer Not Found",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (useDefault == DialogResult.Yes)
                    xp58 = new PrinterSettings { PrinterName = defaultPrinter };
                else
                    return null;
            }

            return xp58;
        }


        private bool IsPrinterOnline(string printerName)
        {
            string query = $"SELECT * FROM Win32_Printer WHERE Name LIKE '%{printerName.Replace("\\", "\\\\")}%'";

            using (var searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject printer in searcher.Get())
                {
                    bool isOnline = !(bool)printer["WorkOffline"];
                    return isOnline;
                }
            }

            return false; // not found
        }

        private void cbxItemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxItemName.SelectedIndex == -1)
                return;

            int itemId = Convert.ToInt32(cbxItemName.SelectedValue);

            string connStr = DbConnection.ConnectionString;
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                string priceQuery = @"
            SELECT PricingType
            FROM Pricing
            WHERE ItemID = @ItemID
            LIMIT 1"; // SQLite uses LIMIT instead of TOP

                using (SQLiteCommand cmd = new SQLiteCommand(priceQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ItemID", itemId);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string pricingType = reader["PricingType"].ToString();

                            // ===== ENABLE/DISABLE WEIGHT FIELD =====
                            if (pricingType == "Fixed")
                            {
                                tbxWeightItems.Enabled = false;
                                tbxWeightItems.Text = "";
                            }
                            else
                            {
                                tbxWeightItems.Enabled = true;
                            }
                        }
                    }
                }
            }
        }
    }
}

