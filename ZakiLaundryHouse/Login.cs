using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;

namespace ZakiLaundryHouse
{
    public partial class formLogin : Form
    {
        public formLogin()
        {
            InitializeComponent();
            tbxPassword.KeyDown += TbxPassword_KeyDown;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            this.FormBorderStyle = FormBorderStyle.None;
            ucPanel11.BackColor = Color.FromArgb(80, 245, 245, 245);
        }

        private void TbxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevents ding sound
                btnLogin.PerformClick();   // Triggers the login button click event
            }
        }

        // 🔹 Authenticate the user from the database
        private (int? userId, string role) AuthenticateUser(string username, string password)
        {
            string connStr = DbConnection.ConnectionString;
            int? userId = null;
            string userRole = null;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connStr))
                {
                    conn.Open();

                    string sql = @"SELECT UserID, Role 
                           FROM Users 
                           WHERE Username = @Username 
                           AND Password = @Password";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = Convert.ToInt32(reader["UserID"]);
                                userRole = reader["Role"].ToString().Trim().ToLower();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return (userId, userRole);
        }

        private void formLogin_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnLogin;
            tbxPassword.PasswordChar = '*';


            btnShowpass.Visible = false;
            btnHidepass.Visible = true;
        }

        private void btnHidepass_Click(object sender, EventArgs e)
        {

            tbxPassword.PasswordChar = '\0';
            btnHidepass.Visible = false;
            btnShowpass.Visible = true;
        }

        private void btnShowpass_Click(object sender, EventArgs e)
        {

            tbxPassword.PasswordChar = '*';
            btnShowpass.Visible = false;
            btnHidepass.Visible = true;
        }





        private void xbtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = tbxUsername.Text.Trim();
            string password = tbxPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔹 Authenticate user through database
            var result = AuthenticateUser(username, password);
            int? userId = result.userId;
            string role = result.role;

            // 🔹 If valid login
            if (!string.IsNullOrEmpty(role) && userId.HasValue)
            {
                try
                {
                    // ✅ Record the login in LogTrail table
                    using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
                    {
                        conn.Open();

                        string insertLog = "INSERT INTO LogTrail (UserID, ActionType, DateTime) VALUES (@UserID, @ActionType, DATETIME('now'))";

                        using (SQLiteCommand cmd = new SQLiteCommand(insertLog, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userId.Value);
                            cmd.Parameters.AddWithValue("@ActionType", "Login");
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // ✅ Open dashboard and pass user info
                    dashboardAdmin dashboardForm = new dashboardAdmin(userId.Value, role);
                    dashboardForm.Show();
                    dashboardForm.ShowHome();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving login log: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
