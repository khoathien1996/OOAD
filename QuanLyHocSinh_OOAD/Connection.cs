﻿using System.Data.SqlClient;

namespace QuanLyHocSinh_OOAD
{
    public class Connection
    {
        public static string strConnectionString = "Data Source=HONGPHUC;Initial Catalog = QLHS; Integrated Security = True";
        public static SqlConnection KetNoi()
        {
            SqlConnection conn = new SqlConnection(strConnectionString);
            return conn;
        }
    }
}
