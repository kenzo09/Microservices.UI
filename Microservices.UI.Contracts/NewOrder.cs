using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.UI.Contracts
{
    public class NewOrder
    {
        public Guid OrderId { get; set; }
        public int StoreId { get; set; }

        public decimal Total { get; set; }

        public Product Products { get; set; }

        public int[] ProductionIds { get; set; }

    }
}
