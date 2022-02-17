using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

                var response = new object();
                var json = new JObject();
                try
                {
                    response = Web.get_("company/get_etails");
                    json = JObject.Parse(response.ToString());
                }
                catch (Exception)
                {
                    return false;
                }

                NAME = json.SelectToken("name");
                CONTACT_NAME = json.SelectToken("name");
                TIN = json.SelectToken("name");
                VRN = json.SelectToken("name");
                BANK_ACC_NAME = json.SelectToken("name");
                BANK_ACC_ADDRESS = json.SelectToken("name");
                BANK_POST_CODE = json.SelectToken("name");
                BANK_NAME = json.SelectToken("name");
                BANK_ACC_NO = json.SelectToken("name");
                ADDRESS = json.SelectToken("name");
                POST_CODE = json.SelectToken("name");
                PHYSICAL_ADDRESS = json.SelectToken("name");
                TELEPHONE = json.SelectToken("name");
                MOBILE = json.SelectToken("name");
                EMAIL = json.SelectToken("name");
                FAX = json.SelectToken("name");
                loaded = true;               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return loaded;
        }
    }
}
