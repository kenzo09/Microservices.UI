using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.UI.Contracts
{
    public class OrderRequest
    {
        public int OrderId { get; set; }

        public int StoreId { get; set; }

        public Product Products { get; set; }

        public int[] ProductionsIds { get; set; }

    }
}
