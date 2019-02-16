using System;
using System.Collections.Generic;

namespace Microservices.UI.Contracts
{
    public class ShowProductsList
    {
        public Guid StoreId { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public List<ProductItem> Items { get; set; }

        public decimal Price { get; set; }

        public Guid RequesterId { get; set; }
    }
}
