using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.UI.Moc.Contratos
{
    public class StoreToGetMoc
    {
        public Guid StoreId { get; set; }
        public bool Ready { get; set; }
    }
}
