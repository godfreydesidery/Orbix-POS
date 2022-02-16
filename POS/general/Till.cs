using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.general
{
    class Till
    {
        public static string TILLNO = "";

        // Till basic information
        public string id { get; set; }
        public string no { get; set; }
        public string name { get; set; }
        public string computerName { get; set; }
        public int active { get; set; }

        // Till postiotion
        public double cash { get; set; }
        public double voucher { get; set; }
        public double deposit { get; set; }
        public double loyalty { get; set; }
        public double crCard { get; set; }
        public double cheque { get; set; }
        public double cap { get; set; }
        public double invoice { get; set; }
        public double crNote { get; set; }
        public double mobile { get; set; }
        public double other { get; set; }

        // Till float balance
        public double floatBalance { get; set; }

        public static object tillTotalRegister(string tillNo, double cash, double voucher, double cheque, double deposit, double loyalty, double CRCard, double CAP, double invoice, double CRNote, double mobile)
        {
            bool commited = false;
            var conn = new MySqlConnection(Database.conString);
            string query = "";
            var command = new MySqlCommand();
            conn.Open();
            command.Connection = conn;
            query = "INSERT IGNORE INTO `till_total`(`till_no`) VALUES ('" + Till.TILLNO + "')";
            command.CommandText = query;
            command.Prepare();
            command.ExecuteNonQuery();
            query = "UPDATE `till_total` SET `cash`=`cash`+'" + cash.ToString() + "',`voucher`=`voucher`+'" + voucher.ToString() + "',`cheque`=`cheque`+' " + cheque.ToString() + "',`deposit`=`deposit`+'" + deposit.ToString() + "',`loyalty`=`loyalty`+'" + loyalty.ToString() + "',`cr_card`=`cr_card`+'" + CRCard.ToString() + "',`cap`=`cap`+'" + CAP.ToString() + "',`invoice`=`invoice`+'" + invoice.ToString() + "',`cr_note`=`cr_note`+'" + CRNote.ToString() + "',`mobile`=`mobile`+'" + mobile.ToString() + "' WHERE `till_no`='" + tillNo.ToString() + "'";
            command.CommandText = query;
            command.Prepare();
            command.ExecuteNonQuery();
            conn.Close();
            commited = true;
            return commited;
        }
    }
}
