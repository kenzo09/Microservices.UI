﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.UI.Moc.Contratos
{
    public class UserToResponseMoc
    {
        public bool Processing { get; set; } = true;

        public Guid UserId { get; set; } = Guid.NewGuid();
    }
}
