using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.UI.Contracts
{
    public class ShowFoodRestrictionsForm
    {
        public Guid UserId { get; set; }

        public Guid RequesterId { get; set; }
    }
}
