using System;
using System.Collections.Generic;
using System.Data.SQLite;
using ZakiLaundryHouse;

namespace ZakiLaundryHouse.ucAdmin
{
    public class Data
    {
        public string name { get; set; }
        public string contact { get; set; }

        public static List<Data> list = new List<Data>();

        public void search(string key)
        {
            using (SQLiteConnection con = new SQLiteConnection(DbConnection.ConnectionString))
            {
                con.Open();
                string query = "SELECT CustomerName, ContactNumber FROM Customers WHERE CustomerName LIKE @key";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@key", $"%{key}%");

                    using (SQLiteDataReader reader = cmd.ExecuteReader()) // ✅ SQLiteDataReader
                    {
                        list.Clear();
                        while (reader.Read())
                        {
                            Data data = new Data()
                            {
                                name = reader["CustomerName"].ToString(),
                                contact = reader["ContactNumber"].ToString()
                            };
                            list.Add(data);
                        }
                    }
                }
            }
        }
    }
}
