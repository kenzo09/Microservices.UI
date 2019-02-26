using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.UI.Services.Interfaces
{
    public interface IConfigurationService
    {
        object GetConfigValue(Type type, string key);
    }
}
