using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.UI.Contracts
{
    public class FoodRestrictions
    {
        public string[] Restrictions { get; set; }

        public string Others { get; set; }

        public int UserId { get; set; }

        public int RequesterId { get; set; }

    }
}
