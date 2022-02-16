using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.general
{
    class Product
    {

        internal partial class SurroundingClass
        {
            public string id { get; set; }
            public string primaryBarcode { get; set; }
            public string code { get; set; }
            public string description { get; set; }
            public string shortDescription { get; set; }
            public string commonDescription { get; set; }
            public string standardUom { get; set; }
            public double packSize { get; set; }
            public string ingredients { get; set; }
            public double costPriceVatIncl { get; set; }
            public double costPriceVatExcl { get; set; }
            public double sellingPriceVatIncl { get; set; }
            public double sellingPriceVatExcl { get; set; }
            public double profitMargin { get; set; }
            public double vat { get; set; }
            public double discountRatio { get; set; }
            public double stock { get; set; }
            public double minimumStock { get; set; }
            public double maximumStock { get; set; }
            public double defaultReorderLevel { get; set; }
            public double defaultReorderQty { get; set; }
            public string status { get; set; }
            public int sellable { get; set; }
            public int returnable { get; set; }

            public List<string> getDescriptions()
            {
                // Returns a list of descriptions of all the products
                var list = new List<string>();
                try
                {
                    var response = new object();
                    var json = new Newtonsoft.Json.Linq.JObject();
                    response = Web.get_("products/descriptions");
                    list = JsonConvert.DeserializeObject<List<string>>(response.ToString());
                    return list;
                }
                catch (Exception ex)
                {
                    return list;
                }              
            }
        }
    }
}
