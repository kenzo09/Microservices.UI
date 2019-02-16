using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.UI.Contracts
{
    public class OrderRequest
    {
        public Guid OrderId { get; set; }

        public Guid StoreId { get; set; }

        public Product Products { get; set; }

        public Guid[] ProductionsIds { get; set; }

    }
}
