using GeekBurger.Orders.Contract;
using System;

namespace GeekBurger_HTML.Models
{
    public class FaceToPost
    {
        public byte[] Face { get; set; }
        public Guid RequesterId { get; set; }
    }
}
