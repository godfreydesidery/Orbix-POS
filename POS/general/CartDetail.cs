using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.general
{
    class CartDetail
    {
        public string id { get; set; }
        public string code { get; set; }
        public string barcode { get; set; }
        public string description { get; set; }
        public double qty { get; set; }
        public double vat { get; set; }
        public double costPriceVatIncl { get; set; }
        public double costPriceVatExcl { get; set; }
        public double sellingPriceVatIncl { get; set; }
        public double sellingPriceVatExcl { get; set; }
        public double discountRatio { get; set; }
        public int voided { get; set; }
        public Cart cart { get; set; } = new Cart();
    }
}
