using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ZakiLaundryHouse.ucAdmin
{
   
    public partial class ucAddAccount : UserControl
    {
        private dashboardAdmin mainForm;
        private Form popup;
        public string Mode { get; set; } = "create";
        private string editingUserCode = "";
        public event Action OnUserSaved;
        public ucAddAccount(dashboardAdmin form, Form popupForm)
        {
            InitializeComponent();
            mainForm = form;
            popup = popupForm;
            
        }

        public void SetUserData(Dictionary<string, string> userData)
        {
            if (userData == null) return;

            tbxName.Text = userData["Name"];
            cbxRole.Text = userData["Role"];
            tbxUsername.Text = userData["Username"];
            tbxPassword.Text = userData["Password"];
            editingUserCode = userData["UserCode"];

            lblTitle.Text = "Edit Account";
            btnAction.Text = "Save";
        }

        private void ucAddAccount_Load(object sender, EventArgs e)
        {
            cbxRole.Items.Clear();
            cbxRole.Items.AddRange(new string[] { "Admin", "Staff"});
            if (Mode == "create")
            {
                cbxRole.SelectedIndex = 1;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (popup != null)
            {
                popup.Close();
            }
        }
        private void btnAction_Click(object sender, EventArgs e)
        {
            string name = tbxName.Text.Trim();
            string role = cbxRole.Text.Trim();
            string username = tbxUsername.Text.Trim();
            string password = tbxPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(role) ||
                string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill all fields.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (NameExists(name, Mode == "edit" ? editingUserCode : null))
            {
                MessageBox.Show("This name already exists. Please use a different name.",
                                "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidatePassword(password))
                return;

            using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
            {
                conn.Open();

                if (Mode == "create")
                {
                    if (UsernameExists(username))
                    {
                        MessageBox.Show("Username already exists. Please choose a different one.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string insertQuery = @"INSERT INTO Users (Name, Role, Username, Password)
                                           VALUES (@Name, @Role, @Username, @Password)";
                    using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("The account has been created!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (Mode == "edit" && !string.IsNullOrEmpty(editingUserCode))
                {
                    if (UsernameExists(username, editingUserCode))
                    {
                        MessageBox.Show("Another user already uses this username.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string updateQuery = @"UPDATE Users
                                           SET Name = @Name, Role = @Role, Username = @Username, Password = @Password
                                           WHERE UserCode = @UserCode";
                    using (SQLiteCommand cmd = new SQLiteCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@UserCode", editingUserCode);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Account updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            OnUserSaved?.Invoke();
        }

        private bool UsernameExists(string username, string excludeUserCode = null)
        {
            using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                if (!string.IsNullOrEmpty(excludeUserCode))
                    query += " AND UserCode <> @UserCode";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    if (!string.IsNullOrEmpty(excludeUserCode))
                        cmd.Parameters.AddWithValue("@UserCode", excludeUserCode);

                    conn.Open();
                    long count = (long)cmd.ExecuteScalar(); // SQLite returns long
                    return count > 0;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            popup.Close();
        }
        private bool ValidatePassword(string password)
        {
            // Length validation
            if (password.Length < 6 || password.Length > 10)
            {
                MessageBox.Show("Password must be between 6 and 10 characters long.",
                                "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Must contain at least one uppercase letter
            if (!password.Any(char.IsUpper))
            {
                MessageBox.Show("Password must contain at least one uppercase letter.",
                                "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true; // ✅ Passed all checks
        }
        private bool NameExists(string name, string excludeUserCode = null)
        {
            using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Name = @Name";
                if (!string.IsNullOrEmpty(excludeUserCode))
                    query += " AND UserCode <> @UserCode";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    if (!string.IsNullOrEmpty(excludeUserCode))
                        cmd.Parameters.AddWithValue("@UserCode", excludeUserCode);

                    conn.Open();
                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void tbxName_Leave(object sender, EventArgs e)
        {
            string input = tbxName.Text.Trim();

            if (!string.IsNullOrWhiteSpace(input))
            {
                input = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ");
                // Format capitalization (Title Case)
                System.Globalization.TextInfo textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                tbxName.Text = textInfo.ToTitleCase(input.ToLower());
            }
        }

        private void tbxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            // Allow letters, space, dot, hyphen, apostrophe, and control keys (like backspace)
            if (!char.IsLetter(ch) && ch != ' ' && ch != '.' && ch != '-' && ch != '\'' && !char.IsControl(ch))
            {
                e.Handled = true; // Ignore invalid key
            }
            if (tbxName.SelectionStart == 0 && e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void tbxUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tbxUsername.SelectionStart == 0 && e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void tbxUsername_Leave(object sender, EventArgs e)
        {
            string input = tbxUsername.Text.Trim();

            if (!string.IsNullOrWhiteSpace(input))
            {
                // Replace multiple spaces with a single space
                input = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ");
                tbxUsername.Text = input; // Keep original capitalization
            }
        }

        private void tbxUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
