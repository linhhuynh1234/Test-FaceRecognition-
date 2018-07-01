using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;
using WebApplication2.DAL;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication2.DAL
{
    public class Data
    {
        sep21t22Entities2 db = new sep21t22Entities2();
        Utility ut = new Utility();
        SqlCommand cmd;
        //SqlDataAdapter sda;
        //SqlCommandBuilder scd;
        static string strConection = null;//"data source=125.234.238.137,8082;initial catalog=sep21t22;user id=sep21t22;password=heavyink;MultipleActiveResultSets=True;";
        public Data()
        {
            strConection = db.Database.Connection.ConnectionString;
        }
        SqlConnection conn = new SqlConnection(strConection);
        public async Task<string> InsertLesson(MonHoc lesson)
        {
            try
            {
                db.MonHocs.Add(lesson);
                await db.SaveChangesAsync();

            }
            catch (Exception )
            {

            }
            return "";



        }
        public async Task<string> InsertCourse(KhoaHoc course)
        {

            try
            {

                db.KhoaHocs.Add(course);
                await db.SaveChangesAsync();

            }
            catch (Exception )
            {

            }

            return "";

        }
        public int checkLesson(string id)
        {
            conn = ut.OpenDb();
            conn.Open();
            cmd = new SqlCommand("checkLesson", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MAMH", SqlDbType.NChar).Value = id.Trim();
            cmd.Parameters.Add("@res", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            conn.Close();
            int retval = (int)cmd.Parameters["@res"].Value;
            return retval;
        }
        public int checkCourse(string id)
        {
            conn = ut.OpenDb();
            conn.Open();
            cmd = new SqlCommand("checkCourse", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MAKH", SqlDbType.NChar).Value = id.Trim();
            cmd.Parameters.Add("@res", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            conn.Close();
            int retval = (int)cmd.Parameters["@res"].Value;
            return retval;
        }
    }
}