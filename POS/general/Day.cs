using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.general
{
    class Day
    {
        public static string CURRENT_DATE = "";

        public static string bussinessDate  = "";

        // Install-Package Microsoft.VisualBasic

        internal partial class SurroundingClass
        {
            public static int zNo = 0;
            public static string systemDate = "";

            public static DateTime getCurrentDay()
            {
                var date_ = DateTime.Parse("0001-01-01");
                try
                {
                    var conn = new MySqlConnection(Database.conString);
                    var command = new MySqlCommand();
                    string codeQuery = "SELECT `day_no`, `date`, `start_at`, `end_at`, `open_closed` FROM `day_log` ORDER BY `day_no` DESC LIMIT 1";
                    conn.Open();
                    command.CommandText = codeQuery;
                    command.Connection = conn;
                    command.CommandType = CommandType.Text;
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read)
                    {
                        date_ = reader.GetDateTime("date");
                        break;
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                return date_;
            }
        }
    }
}
