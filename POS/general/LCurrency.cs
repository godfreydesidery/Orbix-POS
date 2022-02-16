using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.general
{
    class LCurrency
    {
        public static object displayValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = "0";
            }

            return String.Format("{0:0.00}", value);
        }

        public static object getValue(string value)
        {
            return value.Replace(",", "");
        }

    }
}
