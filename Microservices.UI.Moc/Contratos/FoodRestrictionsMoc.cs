using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.UI.Moc.Contratos
{
    public class FoodRestrictionsMoc
    {
        public List<string> Restrictions { get; set; }

        public string Others { get; set; }

        public Guid UserId { get; set; }

        public Guid RequesterId { get; set; }

    }
}
