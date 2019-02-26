using Microservices.UI.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.UI.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public object GetConfigValue(Type type, string key)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return config.GetValue(type, key);
        }
    }
}
