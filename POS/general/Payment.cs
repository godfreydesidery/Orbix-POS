using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        }

        public static bool commitPayment(string paymentId)
        {
            bool commited = false;
            try
            {
                var response = new object();
                var json = new JObject();
                try
                {
                    response = Web.get_("credit_notes/get_crnoteno?code=");
                    json = JObject.Parse(response.ToString());
                }
                catch (Exception)
                {
                    return false;
                }
                return JsonConvert.DeserializeObject<bool>(json.ToString());               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return commited;
        }
    }
}
