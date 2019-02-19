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
        private const string Subscription = "ui";

        public ReceiveMessagesFactory(IRequisicaoService requisicaoService, IUICommandService uiCommandService)
        {
            _requisicaoService = requisicaoService;
            _uiCommandService = uiCommandService;
            CreateNew("StoreCatalogReady", Subscription);
            CreateNew("UserRetrieved", Subscription);

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var url = config.GetValue(typeof(string), "StoreCatalogUri").ToString();
            Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri);
            _requisicaoService.GetAsync(uri, "store", $"?StoreId={Guid.NewGuid()}&Ready=true");
        }

        public ReceiveMessagesService CreateNew(string topic, string subscription, string filterName = null, string filter = null)
        {
            return new ReceiveMessagesService(_uiCommandService, topic, subscription, filterName, filter);
        }
    }
}
