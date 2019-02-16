using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.UI.Moc.Contratos
{
    public class UserToResponse
    {
        public bool Processing { get; set; } = true;

        public Guid UserId { get; set; } = new Guid();
    }
}
