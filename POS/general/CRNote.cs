using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.general
{
    class CRNote
    {
        public double getCreditNoteNo(string cRNo)
        {
            double no = 0d;
            try
            {
                var conn = new MySqlConnection(Database.conString);
                var command = new MySqlCommand();
                string query = "";
                query = "SELECT  `cr_no`, `cr_amount`, `cr_bill_no`, `cr_date`, `cr_status`, `cr_details` FROM `cr_note` WHERE `cr_no`='" + cRNo + "'";
                conn.Open();
                command.CommandText = query;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read)
                {
                    no = Val(reader.GetString("cr_amount"));
                    break;
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message);
            }

            return no;
        }
    }
}
