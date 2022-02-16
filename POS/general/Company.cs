using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.general
{
    class Company
    {
        public static object NAME = "";
        public static object CONTACT_NAME = "";
        public static object TIN = "";
        public static object VRN = "";
        public static object BANK_ACC_NAME = "";
        public static object BANK_ACC_ADDRESS = "";
        public static object BANK_POST_CODE = "";
        public static object BANK_NAME = "";
        public static object BANK_ACC_NO = "";
        public static object ADDRESS = "";
        public static object POST_CODE = "";
        public static object PHYSICAL_ADDRESS = "";
        public static object TELEPHONE = "";
        public static object MOBILE = "";
        public static object EMAIL = "";
        public static object FAX = "";

        public static bool loadCompanyDetails()
        {
            bool loaded = false;
            try
            {
                var conn = new SqlConnection(Database.conString);
                var command = new MySqlCommand();
                // create bar code
                string codeQuery = "SELECT `company_name`, `contact_name`, `tin`, `vrn`, `bank_acc_name`, `bank_acc_address`, `bank_post_code`, `bank_name`, `bank_acc_no`, `address`, `post_code`, `physical_address`, `telephone`, `mobile`, `email`, `fax` FROM `company`";
                conn.Open();
                command.CommandText = codeQuery;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read)
                {
                    NAME = reader.GetString("company_name");
                    CONTACT_NAME = reader.GetString("contact_name");
                    TIN = reader.GetString("tin");
                    VRN = reader.GetString("vrn");
                    BANK_ACC_NAME = reader.GetString("bank_acc_name");
                    BANK_ACC_ADDRESS = reader.GetString("bank_acc_address");
                    BANK_POST_CODE = reader.GetString("bank_post_code");
                    BANK_NAME = reader.GetString("bank_name");
                    BANK_ACC_NO = reader.GetString("bank_acc_no");
                    ADDRESS = reader.GetString("address");
                    POST_CODE = reader.GetString("post_code");
                    PHYSICAL_ADDRESS = reader.GetString("physical_address");
                    TELEPHONE = reader.GetString("telephone");
                    MOBILE = reader.GetString("mobile");
                    EMAIL = reader.GetString("email");
                    FAX = reader.GetString("fax");
                    loaded = true;
                    break;
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return loaded;
        }
    }
}
