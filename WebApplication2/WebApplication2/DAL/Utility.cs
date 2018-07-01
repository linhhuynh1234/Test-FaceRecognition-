using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication2.DAL
{
    class Utility
    {
        #region Project
        public static SqlConnection conn;
        public static SqlCommand cmd;
        public static SqlDataAdapter da;

        static string strConection = null;//"data source=125.234.238.137,8082;initial catalog=sep21t22;user id=sep21t22;password=heavyink;MultipleActiveResultSets=True;";
        public Utility()
        {
            strConection = new WebApplication2.Models.sep21t22Entities2().Database.Connection.ConnectionString;
        }
        public SqlConnection OpenDb()
        {
            conn = new SqlConnection(strConection);
            return conn;
        }
        public static void OpenConnection()
        {
            // sever name
            // chuoi xac thuc
            try
            {
                // mo ket noi
                conn = new SqlConnection(strConection);
                conn.Open();
            }
            catch (SqlException )
            {

            }
        }
        // dong vs ngat ket noi voi database
        public static void DisConnection()
        {
            // dong ket noi
            conn.Close();
            conn.Dispose();
            conn = null;
        }
        // Tao bang de luu co sao du lieu
        public static DataTable getDataTable(string sql)
        {
            // khoi tao 1 sqlcommand de tro toi du lieu trogn database
            cmd = new SqlCommand(sql, conn);
            // khoi tao 1 sqladapter de luu du lieu tu tren database
            da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            // tao 1 datatable de hien thi du lieu
            DataTable table = new DataTable();
            da.Fill(table);
            da.Dispose();
            return table;
        }
        // Tao ham Excute de co the tao tac voi co so du lieu
        public void Excute(string sql)
        {
            cmd = new SqlCommand(sql, conn);
            // goi ham ExcuteNonQuery de co the thuc hien cac thao tac Delete, Insert, Update cho database
            cmd.ExecuteNonQuery();
        }
        #endregion
    }
}