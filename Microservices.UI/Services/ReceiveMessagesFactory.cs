using Microservices.UI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Microservices.UI.Services
{
    public class ReceiveMessagesFactory : IReceiveMessagesFactory
    {
        private readonly IRequisicaoService _requisicaoService;
        private readonly IUICommandService _uiCommandService;
        private readonly IConfigurationService _configurationService;
        private const string Subscription = "ui";

        public ReceiveMessagesFactory(IRequisicaoService requisicaoService, IUICommandService uiCommandService,
            IConfigurationService configurationService)
        {
            _requisicaoService = requisicaoService;
            _uiCommandService = uiCommandService;
            _configurationService = configurationService;
            CreateNew("StoreCatalogReadyMessage", Subscription);
            CreateNew("UserRetrieved", Subscription);
            
            var url = _configurationService.GetConfigValue(typeof(string), "StoreCatalogUri").ToString();
            Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri);
            _requisicaoService.GetAsync(uri, "storeMoc", $"?StoreId={Guid.NewGuid()}&Ready=true");
        }

        public ReceiveMessagesService CreateNew(string topic, string subscription, string filterName = null, string filter = null)
        {
            return new ReceiveMessagesService(_uiCommandService, _requisicaoService, _configurationService, topic, subscription, filterName, filter);
        }
    }
}
