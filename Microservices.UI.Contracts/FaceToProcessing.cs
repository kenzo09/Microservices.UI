using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.UI.Contracts
{
    public class FaceToProcessing
    {
        public bool Processing { get; set; } = true;
        public Guid UserId { get; set; } = Guid.NewGuid();
    }
}
