using System;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ZakiLaundryHouse.ucAdmin
{
    public partial class ucLogTrail : UserControl
    {
        private dashboardAdmin mainForm;
        private Form popup;

        public ucLogTrail(dashboardAdmin form, Form popupForm)
        {
            InitializeComponent();
            popup = popupForm;
            mainForm = form;
            DataGridViewStyler.ApplyNonSelectableStyle(dtgLogTrailView);
        }

        private void ucLogTrail_Load(object sender, EventArgs e)
        {
            LoadLogTrail();

        }

        private void LoadLogTrail()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
                {
                    string query = @"
WITH LogCTE AS (
    SELECT 
        lt.UserID,
        u.Name AS UserName,
        lt.ActionType,
        lt.DateTime,
        ROW_NUMBER() OVER (PARTITION BY lt.UserID ORDER BY lt.DateTime) AS rn
    FROM LogTrail lt
    INNER JOIN Users u ON lt.UserID = u.UserID
)
SELECT 
    login.UserName AS LoginUser,
    strftime('%m/%d/%Y %I:%M:%S %p', datetime(login.DateTime, 'localtime')) AS LoginTime,
    strftime('%m/%d/%Y %I:%M:%S %p', datetime(logout.DateTime, 'localtime')) AS LogoutTime
FROM 
    LogCTE login
LEFT JOIN 
    LogCTE logout
    ON login.UserID = logout.UserID
    AND login.rn + 1 = logout.rn
    AND logout.ActionType = 'Logout'
WHERE login.ActionType = 'Login'
ORDER BY login.DateTime DESC;";

                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dtgLogTrailView.DataSource = dt;
                    }
                }

                // Color code rows
                foreach (DataGridViewRow row in dtgLogTrailView.Rows)
                {
                    string login = row.Cells["LoginTime"].Value?.ToString();
                    string logout = row.Cells["LogoutTime"].Value?.ToString();

                    if (!string.IsNullOrEmpty(login) && string.IsNullOrEmpty(logout))
                        row.DefaultCellStyle.ForeColor = Color.SeaGreen; // Logged in but not logged out
                    else if (!string.IsNullOrEmpty(logout))
                        row.DefaultCellStyle.ForeColor = Color.IndianRed; // Session complete
                }
                dtgLogTrailView.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading log trail: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLogTrail();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (popup != null)
                popup.Close();
            else
                mainForm.ShowHome();
        }

        private void dtgLogTrailView_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}
