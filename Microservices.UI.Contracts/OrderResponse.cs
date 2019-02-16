using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.UI.Contracts
{
    public class OrderResponse
    {
        public int OrderId { get; set; }

        public int StoreId { get; set; }

        public decimal Total { get; set; }
    }
}
