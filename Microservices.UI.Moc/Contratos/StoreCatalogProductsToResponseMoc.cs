using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.UI.Moc.Contratos
{
    public class StoreCatalogProductsToResponseMoc
    {
        public Guid StoreId { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public List<Item> Items { get; set; }

        public decimal Price { get; set; }

    }
}
