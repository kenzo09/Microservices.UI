﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.UI.Contracts
{
    public class FaceToPost
    {
        public string Face { get; set; }
        public Guid RequestId { get; set; }

    }
}
