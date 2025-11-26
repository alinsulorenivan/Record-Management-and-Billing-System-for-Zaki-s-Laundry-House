using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZakiLaundryHouse
{
    public static class ActivityLogger
    {
        public static void LogAction(int userId, string actionType)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
                {
                    string query = "INSERT INTO LogTrail (UserID, ActionType, DateTime) VALUES (@UserID, @ActionType, DATETIME('now'))";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@ActionType", actionType);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving log trail: " + ex.Message);
            }
        }
    }
}