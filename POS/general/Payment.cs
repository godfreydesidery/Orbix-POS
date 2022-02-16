using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.general
{
    class Payment
    {
        public static double cash;
        public static double voucher;
        public static double deposit;
        public static double loyalty;
        public static double CRCard;
        public static double CAP;
        public static double invoice;
        public static double CRNote;
        public static double mobile;

        public static void setPayment(double _cash, double _voucher, double _deposit, double _loyalty, double _CRCard, double _CAP, double _invoice, double _CRNote, double _mobile)
        {
            cash = 0d;
            voucher = 0d;
            deposit = 0d;
            loyalty = 0d;
            CRCard = 0d;
            CAP = 0d;
            invoice = 0d;
            CRNote = 0d;
            mobile = 0d;
            cash = _cash;
            voucher = _voucher;
            deposit = _deposit;
            loyalty = _loyalty;
            CRCard = _CRCard;
            CAP = _CAP;
            invoice = _invoice;
            CRNote = _CRNote;
            mobile = _mobile;
            return null;
        }

        public static object commitPayment(string paymentId)
        {
            bool commited = false;
            try
            {
                var conn = new MySqlConnection(Database.conString);
                conn.Open();
                var command = new MySqlCommand();
                command.Connection = conn;
                command.CommandText = "INSERT INTO `payment`(`sale_id`, `cash`, `voucher`, `deposit`, `loyalty`, `cr_card`, `cap`, `invoice`, `cr_note`, `mobile`) VALUES (@sale_id,@cash,@voucher,@deposit,@loyalty,@cr_card,@cap,@invoice,@cr_note,@mobile)";
                command.Prepare();
                command.Parameters.AddWithValue("@sale_id", paymentId);
                command.Parameters.AddWithValue("@cash", cash);
                command.Parameters.AddWithValue("@voucher", voucher);
                command.Parameters.AddWithValue("@deposit", deposit);
                command.Parameters.AddWithValue("@loyalty", loyalty);
                command.Parameters.AddWithValue("@cr_card", CRCard);
                command.Parameters.AddWithValue("@cap", CAP);
                command.Parameters.AddWithValue("@invoice", invoice);
                command.Parameters.AddWithValue("@cr_note", CRNote);
                command.Parameters.AddWithValue("@mobile", mobile);
                command.ExecuteNonQuery();
                conn.Close();
                commited = true;
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message);
            }

            return commited;
        }
    }
}
