using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.UI.Moc.Contratos
{
    public class UserFoodRestrictionsMoc
    {
        public bool Processing { get; set; } = true;
        public Guid UserId { get; set; } = new Guid();
    }
}
