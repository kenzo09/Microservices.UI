using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.UI.Moc.Contratos
{
    public class StoreCatalogProductsToRequestMoc
    {
        public string StoreName { get; set; }

        public Guid UserId { get; set; }

        public string[] Restrictions { get; set; }

    }
}
