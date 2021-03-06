﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microservices.UI.Services.Interfaces
{
    public interface IRequisicaoService
    {
        Task<HttpResponseMessage> PostAsync(dynamic data, Uri uri, string api);
        Task<HttpResponseMessage> GetAsync(Uri uri, string api, string parametros = null);
    }
}
