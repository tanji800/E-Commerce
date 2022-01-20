using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Models
{
    public class cart
    {
        public int pro_id { get; set; }

        public string pro_name { get; set; }
        public Nullable<int> pro_price { get; set; }

        public Nullable<int> o_qty { get; set; }
        public Nullable<double> o_bill { get; set; }

    }
}