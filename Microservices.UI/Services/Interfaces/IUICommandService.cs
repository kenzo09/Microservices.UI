﻿using GeekBurger.StoreCatalog.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.UI.Services.Interfaces
{
    public interface IUICommandService
    {
        void SendMessagesAsync();
        void AddToMessageList(string label, IEnumerable<StoreCatalogReadyMessage> storeCatalogs);
        void AddToMessageList(string label, IEnumerable<ProductByStoreToGet> products);
        void AddToMessageList(string label);
    }
}
