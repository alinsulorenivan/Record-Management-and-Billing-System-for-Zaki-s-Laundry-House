using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;
using System.IO;

namespace ZakiLaundryHouse
{
    public static class DbConnection
    {
        //public static string ConnectionString = @"Data Source=LAPTOP-9C3VR24N\SQLEXPRESS01;Initial Catalog = ZAKI's_Laundry_House_DB; Integrated Security = True";
        //public static string ConnectionString = @"Server=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\ZAKIs_Laundry_House_DB.mdf;Integrated Security=True;";
        //public static string DatabaseFile = @"C:\ProgramData\ZakiLaundryHouse\ZAKIs_Laundry_House_DB.mdf";

        //public static string ConnectionString = $@"Server=(LocalDB)\MSSQLLocalDB;
        //                                           AttachDbFilename={DatabaseFile};
        //                                           Integrated Security=True;
        //                                           Connect Timeout=30;";
        //public static string ConnectionString =>
        //ConfigurationManager.ConnectionStrings["ZakiDB"].ConnectionString;
        public static string ConnectionString = @"Data Source=zaki.db;Version=3;";
    }
}