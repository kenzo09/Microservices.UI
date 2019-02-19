using System;
using System.Collections.Generic;

namespace GeekBurger_HTML.Models
{
    public class OrderToPost
    {
        public List<ProductToGetFormat> Products { get; set; }
        public Guid RequesterId { get; set; }
    }
}