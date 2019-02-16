using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.UI.Contracts
{
    public class ShowFoodRestrictionsForm
    {
        public int UserId { get; set; }

        public int RequesterId { get; set; }
    }
}
