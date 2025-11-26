using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Management; // ✅ Add this at the top of your file

namespace ZakiLaundryHouse.ucAdmin
{

    public partial class ucPaymentForm : UserControl
    {
        private dashboardAdmin mainForm;
        private Form popup;
        private string previousForm;
        public int CustomerID { get; set; }
        public int OrderID { get; set; }
        private string invoiceNo;
        private decimal subtotal;
        private decimal total;
        private decimal cash;
        private decimal change;
        private System.Windows.Forms.Timer amountValidationTimer;
        private bool _hasUserTyped = false;
        private bool _isUpdatingText = false;


        public event Action PaymentCompleted;
        public ucPaymentForm(dashboardAdmin form, Form popupForm, string cameFrom)
        {
            InitializeComponent();
            amountValidationTimer = new System.Windows.Forms.Timer();
            amountValidationTimer.Interval = 300; // 1.3 seconds delay
            amountValidationTimer.Tick += AmountValidationTimer_Tick;

            mainForm = form;
            popup = popupForm;
            previousForm = cameFrom;
            cbxPaymentMethod.SelectedIndexChanged += CbxPaymentMethod_SelectedIndexChanged;
            tbxAmountReceived.TextChanged += TbxAmountReceived_TextChanged;
            tbxAmountReceived.KeyPress += TbxAmountReceived_KeyPress;

            tbxCustName.ReadOnly = true;
            tbxCustName.BackColor = Color.White;
            tbxCustName.TabStop = false;
            tbxCustName.Enter += (s, e) => tbxAmountReceived.Focus();

            tbxTotAmount.ReadOnly = true;
            tbxTotAmount.BackColor = Color.White;
            tbxTotAmount.TabStop = false;
            tbxTotAmount.Enter += (s, e) => tbxAmountReceived.Focus();

            tbxChange.ReadOnly = true;
            tbxChange.BackColor = Color.White;
            tbxChange.TabStop = false;
            tbxChange.Enter += (s, e) => tbxAmountReceived.Focus();

            this.Load += (s, e) =>
            {
                tbxAmountReceived.Focus();
                tbxAmountReceived.SelectionStart = tbxAmountReceived.Text.Length;
                tbxAmountReceived.SelectionLength = 0;
            };
        }

        public TextBox CustomerNameTextBox => tbxCustName;
        public TextBox TotalAmountTextBox => tbxTotAmount;
        public ComboBox PaymentMethodComboBox => cbxPaymentMethod;
        public TextBox AmountReceivedTextBox => tbxAmountReceived;
        public TextBox ChangeTextBox => tbxChange;

        private void CbxPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMethod = cbxPaymentMethod.SelectedItem?.ToString();
            _hasUserTyped = false;

            // 🔹 Reset amount fields and validation every time payment method changes
            tbxAmountReceived.Text = "0.00";
            tbxChange.Text = "";
            lblAmountReceivedValidation.Visible = false;

            if (selectedMethod == "Cash")
            {
                // allow typing in Amount Received
                tbxAmountReceived.Enabled = true;
                tbxAmountReceived.ReadOnly = false;
                tbxAmountReceived.BackColor = Color.White;

                tbxChange.ReadOnly = false;
                tbxChange.BackColor = Color.White;

                tbxAmountReceived.Text = "0.00";
                tbxAmountReceived.Focus();
                CalculateChange();
            }
            else
            {
                // online payment → disable typing
                tbxAmountReceived.Enabled = false;
                tbxAmountReceived.ReadOnly = true;
                tbxChange.Enabled = false;
                tbxAmountReceived.BackColor = Color.WhiteSmoke;

                // auto set AmountReceived = Total Amount
                tbxAmountReceived.Text = tbxTotAmount.Text;

                // no change for non-cash
                tbxChange.ReadOnly = true;
                tbxChange.BackColor = Color.WhiteSmoke;
                tbxChange.Text = "0.00";
            }

            // Refocus on amount received textbox for smoother workflow
            tbxAmountReceived.Focus();
            tbxAmountReceived.SelectionStart = tbxAmountReceived.Text.Length;
            tbxAmountReceived.SelectionLength = 0;

            // Always clear the Reference Number when payment method changes
            tbxReferenceNumber.Clear();

            // Enable Reference Number for Gcash, Maya, and Others
            if (selectedMethod == "Gcash" || selectedMethod == "Maya" || selectedMethod == "BPI")
            {
                tbxReferenceNumber.Enabled = true;
                tbxReferenceNumber.MaxLength = 25; // limit input to 20 characters
            }
            else
            {
                tbxReferenceNumber.Enabled = false;
                tbxReferenceNumber.Clear(); // optional: clear when disabled
            }
        }

        private void TbxAmountReceived_TextChanged(object sender, EventArgs e)
        {
            if (!_hasUserTyped && tbxAmountReceived.Focused)
                _hasUserTyped = true;

            lblAmountReceivedValidation.Visible = false;

            if (_isUpdatingText) return;
            _isUpdatingText = true;

            TextBox tbx = sender as TextBox;
            string text = tbx.Text;

            if (text.EndsWith(".00"))
                text = text.Substring(0, text.Length - 3);

            text = new string(text.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(text))
                text = "0";
            else
            {
                text = text.TrimStart('0');
                if (string.IsNullOrEmpty(text))
                    text = "0";
            }

            tbx.Text = text + ".00";
            tbx.SelectionStart = tbx.Text.Length - 3;
            tbx.SelectionLength = 0;

            _isUpdatingText = false;

            // 🔹 Only start validation if user has typed (and textbox focused)
            if (_hasUserTyped)
            {
                amountValidationTimer.Stop();
                amountValidationTimer.Start();
            }

            // 🔹 Update change live for Cash
            string paymentMethod = cbxPaymentMethod.SelectedItem?.ToString() ?? "";
            if (paymentMethod == "Cash")
                CalculateChange();
            else
                tbxChange.Text = "";
        }







        private void CalculateChange()
        {
            decimal totalAmount, amountReceived;

            // Check if both fields are successfully parsed as decimals
            if (decimal.TryParse(tbxTotAmount.Text, out totalAmount) &&
                decimal.TryParse(tbxAmountReceived.Text, out amountReceived))
            {
                decimal change = amountReceived - totalAmount;

                // If the change is negative, set it to 0
                if (change < 0)
                {
                    tbxChange.Text = "0.00";  // Set change as 0 if negative
                }
                else
                {
                    tbxChange.Text = change.ToString("0.00");  // Show the calculated change
                }

                // Update the change class variable
                this.change = change;
            }
            else
            {
                tbxChange.Text = "";  // Clear the text if parsing fails
                this.change = 0;  // Reset change
            }
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This order has not been paid yet. It will remain as 'Pending'.\n\nDo you want to go back?", "Unpaid Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                return;
            }
            if (popup != null)
            {
                popup.Close();
            }
            else
            {
                if (previousForm == "billing")
                {
                    mainForm.addUserControl(new ucBilling(mainForm, null));
                }
                else if (previousForm == "order")
                {
                    mainForm.addUserControl(new ucOrderForm(mainForm, null));
                }
                else
                {
                    mainForm.addUserControl(new ucBilling(mainForm, null));
                }
            }
        }


        private void ucPaymentForm_Load(object sender, EventArgs e)
        {
            // Make read-only fields appear white (not gray)
            tbxCustName.ReadOnly = true;
            tbxCustName.BackColor = Color.White;

            tbxTotAmount.ReadOnly = true;
            tbxTotAmount.BackColor = Color.White;

            tbxChange.ReadOnly = true;
            tbxChange.BackColor = Color.White;

            // Delay setting focus so no text is selected
            this.BeginInvoke(new Action(() =>
            {
                tbxAmountReceived.Focus();

            }));
        }

        private void SavePayment()
        {
            string connectionString = DbConnection.ConnectionString;
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(connectionString))
            {
                conn.Open();

                // Insert payment record (SQLite uses CURRENT_TIMESTAMP instead of GETDATE())
                string insertQuery = @"
            INSERT INTO Payments (OrderID, CustomerID, AmountPaid, PaymentDate, PaymentMethod)
            VALUES (@OrderID, @CustomerID, @AmountPaid, CURRENT_TIMESTAMP, @PaymentMethod)";

                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters.AddWithValue("@AmountPaid", Convert.ToDecimal(tbxTotAmount.Text));

                    string selectedMethod = cbxPaymentMethod.SelectedItem.ToString();
                    string paymentMethodFormatted;

                    // If not Cash, include reference number
                    if (!selectedMethod.Equals("Cash", StringComparison.OrdinalIgnoreCase))
                    {
                        paymentMethodFormatted = $"{selectedMethod} (Ref no. {tbxReferenceNumber.Text})";
                    }
                    else
                    {
                        paymentMethodFormatted = "Cash";
                    }

                    cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethodFormatted);

                    cmd.ExecuteNonQuery();
                }

                // Update order status to Completed
                string updateOrderStatus = "UPDATE Orders SET Status = 'Completed' WHERE OrderID = @OrderID";
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(updateOrderStatus, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private string GetInvoiceNoByOrderID(int orderId)
        {
            string invoiceNo = null;
            string connectionString = DbConnection.ConnectionString;

            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT InvoiceNo FROM Orders WHERE OrderID = @OrderID";
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        invoiceNo = result.ToString();
                    }
                }
            }

            return invoiceNo; // Always returns a string or null
        }


        private void TbxAmountReceived_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tbx = sender as TextBox;

            // Disallow '.' completely
            if (e.KeyChar == '.')
            {
                e.Handled = true;
                return;
            }

            // Allow only digits and control keys (backspace etc)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            // Handle replacing leading zero on digit input (except backspace)
            if (!char.IsControl(e.KeyChar))
            {
                if (tbx.Text == "0.00" || tbx.Text == "0")
                {
                    // Replace with the new digit immediately (handled in TextChanged)
                    // Let TextChanged handle this logic
                }
            }

        }

        private void btnViewSummary_Click(object sender, EventArgs e)
        {
            // Get the total amount from the textbox
            decimal totalAmount = decimal.TryParse(tbxTotAmount.Text, out decimal amount) ? amount : 0;

            // Get the amount received from the textbox
            decimal amountReceived = decimal.TryParse(tbxAmountReceived.Text, out decimal receivedAmount) ? receivedAmount : 0;

            // Call CalculateChange() explicitly to update the change before showing the summary
            // But ensure it's called only for cash payments
            if (cbxPaymentMethod.SelectedItem?.ToString() == "Cash")
            {
                CalculateChange();  // Ensure change is recalculated before displaying the summary
            }

            // Now pass the latest values into the summary
            string invoiceNo = GetInvoiceNoByOrderID(this.OrderID);
            decimal changeDue = (cbxPaymentMethod.SelectedItem?.ToString() == "Cash") ? this.change : 0;  // Use the updated change variable for cash payments, else set to 0

            // Show the summary in a new form
            ucOrderSummary summaryUC = new ucOrderSummary
            {
                InvoiceNo = invoiceNo,
                //Subtotal = totalAmount,
                Total = totalAmount,
                Cash = amountReceived, // This will show the amount paid
                Change = changeDue,     // This will show the change due, or 0 for non-cash payments
                PaymentMethod = cbxPaymentMethod.SelectedItem.ToString()
            };

            // Show the summary in a new form
            Form receiptForm = new Form
            {
                Text = "Order Summary",
                Size = new Size(500, 700),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog, // prevents resizing
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Wrap the UserControl in a scrollable panel
            Panel scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            summaryUC.Dock = DockStyle.Top; // keep original layout
            scrollPanel.Controls.Add(summaryUC);

            // Add the scrollable panel to the form
            receiptForm.Controls.Add(scrollPanel);

            receiptForm.ShowDialog();
        }
        private void newButton1_Click(object sender, EventArgs e)
        {
            try
            {
                // ---- VALIDATION FIRST ----

                if (!decimal.TryParse(tbxTotAmount.Text, out decimal totalAmount))
                    return;

                if (!decimal.TryParse(tbxAmountReceived.Text, out decimal amountReceived))
                    return;

                string paymentMethod = cbxPaymentMethod.SelectedItem?.ToString() ?? "";

                // ✅ Validation Logic BEFORE confirmation
                if (paymentMethod == "Cash" && amountReceived < totalAmount)
                {
                    lblAmountReceivedValidation.Text = "Insufficient amount for cash payment.";
                    lblAmountReceivedValidation.ForeColor = Color.Red;
                    lblAmountReceivedValidation.Visible = true;
                    return;
                }

                if (amountReceived < totalAmount)
                {
                    lblAmountReceivedValidation.Text = "Insufficient amount.";
                    lblAmountReceivedValidation.ForeColor = Color.Red;
                    lblAmountReceivedValidation.Visible = true;
                    return;
                }

                // ---- CONFIRMATION AFTER VALIDATION ----

                DialogResult dialogResult = MessageBox.Show(
                    "Are you sure you want to confirm the payment and print the receipt?",
                    "Confirm Payment",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (dialogResult == DialogResult.No)
                    return;

                // 3️⃣ Check printer BEFORE saving OR printing
                PrinterSettings xp58 = GetReceiptPrinter();
                if (xp58 == null)
                    return;

                if (!IsPrinterOnline(xp58.PrinterName))
                {
                    MessageBox.Show(
                        $"The printer \"{xp58.PrinterName}\" is offline or not responding.\n" +
                        $"Please check the connection and try again.",
                        "Printer Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // ✅ Now safe to save and print
                SavePayment(); // Make sure SavePayment() is also converted to SQLite
                PaymentCompleted?.Invoke();

                invoiceNo = GetInvoiceNoByOrderID(this.OrderID); // Already converted to SQLite
                subtotal = totalAmount;
                total = totalAmount;
                cash = amountReceived;
                change = amountReceived - totalAmount;

                string contact = "";
                using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(DbConnection.ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT ContactNumber FROM Customers WHERE CustomerID = @CustomerID";
                    using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            contact = result.ToString();
                    }
                }

                // CREATE RECEIPT DATA
                ucOrderSummary summaryUC = new ucOrderSummary
                {
                    InvoiceNo = invoiceNo,
                    Subtotal = subtotal,
                    Total = total,
                    Cash = cash,
                    Change = change,
                    CustomerName = tbxCustName.Text,
                    CustomerContact = contact,
                    ReferenceNumber = tbxReferenceNumber.Text,
                    PaymentMethod = cbxPaymentMethod.SelectedItem?.ToString() ?? ""
                };

                summaryUC.PrintReceipt();
                popup?.Close();
                mainForm?.addUserControl(new ucBilling(mainForm, null));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving payment: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void AmountValidationTimer_Tick(object sender, EventArgs e)
        {
            amountValidationTimer.Stop();
            if (!_hasUserTyped)
                return;

            string paymentMethod = cbxPaymentMethod.SelectedItem?.ToString() ?? "";

            if (!decimal.TryParse(tbxTotAmount.Text, out decimal totalAmount) ||
                !decimal.TryParse(tbxAmountReceived.Text, out decimal amountReceived))
                return;

            // 🔹 Online payments: exact match required
            if (paymentMethod == "Gcash" || paymentMethod == "Maya" || paymentMethod == "BPI")
            {
                
                if (amountReceived != totalAmount)
                {
                    lblAmountReceivedValidation.Text = "Please input exact amount.";
                    lblAmountReceivedValidation.ForeColor = Color.Red;
                    lblAmountReceivedValidation.Visible = true;
                }
                else
                {
                    lblAmountReceivedValidation.Visible = false;
                }
            }
            // 🔹 Cash payments: insufficient amount check (after delay)
            else if (paymentMethod == "Cash")
            {
                if (amountReceived < totalAmount)
                {
                    lblAmountReceivedValidation.Text = "Insufficient amount.";
                    lblAmountReceivedValidation.ForeColor = Color.Red;
                    lblAmountReceivedValidation.Visible = true;
                }
                else
                {
                    lblAmountReceivedValidation.Visible = false;
                }
            }
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

        private void tbxReferenceNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) &&
                !char.IsControl(e.KeyChar) &&
                e.KeyChar != ' ' &&
                e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }

        private void tbxAmountReceived_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}