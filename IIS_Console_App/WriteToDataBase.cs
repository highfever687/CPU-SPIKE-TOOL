using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIS_Console_App
{
    class WriteToDataBase
    {
        public static void SendToDataBase(string message, string elapsedTime)
        {
            string theRecycleMessage = message;
            DateTime pollTime = DateTime.Parse(elapsedTime);
            bool success = true;
            //Open and query the database currently this class and the database is unused
            //Reserved for future changes
            SqlConnection db = new SqlConnection("Data Source=192.168.1.79;Initial Catalog=EmailVerification;User Id=emailver;Password=password;");
            db.Open();
            try
            {
            SqlCommand cmd = new SqlCommand("INSERT INTO EmailLog(TimeStamp, Message, Successful, system) VALUES(@time,@msg, @tmp, @sys)", db);
            cmd.Parameters.AddWithValue("@time", pollTime);
            cmd.Parameters.AddWithValue("@msg", theRecycleMessage);
            cmd.Parameters.AddWithValue("@tmp", success);
            cmd.Parameters.AddWithValue("@sys", "email");
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = null;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                db.Close();
            }
        
        }


    }
}
