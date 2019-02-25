using GeekBurger.StoreCatalog.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.UI.Services.Interfaces
{
    public interface IUICommandService
    {
        void SendMessagesAsync();
        void AddToMessageList(IEnumerable<StoreCatalogReadyMessage> storeCatalogs);
    }
}
