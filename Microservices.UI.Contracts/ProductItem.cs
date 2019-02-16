using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.UI.Contracts
{
    public class ProductItem
    {
        public Guid ItemId { get; set; }

        public string Name { get; set; }
    }
}
