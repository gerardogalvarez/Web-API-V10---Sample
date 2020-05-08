using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPV10.PurchaseOrderSample
{
    /// <summary>
    /// The object with the information of an order line.
    /// </summary>
    public class PurchaseOrderLine
    {     
        /// <summary>
        /// 
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        [JsonProperty("artigo")]
        public string Item { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        [JsonProperty("quantidade")]
        public double Quantity { get; set; }

    }
}
