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

                    var response = new object();
                    var json = new JObject();
                    try
                    {
                        response = Web.get_("days/get_current_day");
                        json = JObject.Parse(response.ToString());
                    }
                    catch (Exception)
                    {
                        return date_;
                    }
                    date_ = JsonConvert.DeserializeObject<DateTime>(json.ToString());                   
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
