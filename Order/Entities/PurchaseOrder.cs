using ERPV10.SalesOrderSample;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPV10.PurchaseOrderSample
{
    /// <summary>
    /// The object with all the information of an order.
    /// </summary>
    public class PurchaseOrder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        [JsonProperty("Entidade")]
        public string Supplier { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        [JsonProperty("TipoEntidade")]
        public string EntityType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value>
        /// The type of the document.
        /// </value>
        [JsonProperty("Tipodoc")]
        public string DocumentType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value>
        /// The type of the document.
        /// </value>
        [JsonProperty("Serie")]
        public string DocumentSeries { get; set; }

        /// <summary>
        /// Gets or sets the type of the document.
        /// </summary>
        /// <value>
        /// The type of the document.
        /// </value>
        [JsonProperty("DataDoc")]
        public DateTime DocumentDate { get; set; }
        
        /// <summary>
        /// Gets or sets the document lines.
        /// </summary>
        /// <value>
        /// The document lines.
        /// </value>
        [JsonProperty("linhas")]
        public List<PurchaseOrderLine> Lines { get; set; }
    }
}
