using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.UI.Services
{
    public interface IUICommandService
    {
        void SendMessagesAsync();
        void AddToMessageList(IEnumerable<StoreCatalogReady> storeCatalogs);
    }
}
