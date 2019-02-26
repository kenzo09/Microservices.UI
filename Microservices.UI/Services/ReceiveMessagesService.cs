using GeekBurger.StoreCatalog.Contract;
using Microservices.UI.Moc.Contratos;
using Microservices.UI.Services.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.UI.Services
{
    public class ReceiveMessagesService
    {
        private readonly string _topicName;
        private static ServiceBusConfiguration _serviceBusConfiguration;
        private readonly string _subscriptionName;
        private readonly IUICommandService _uiCommandService;
        private readonly IRequisicaoService _requisicaoService;
        private readonly IConfiguration _configuration;

        public ReceiveMessagesService(IUICommandService uiCommandService, IRequisicaoService requisicaoService,
            string topic, string subscription, string filterName = null, string filter = null)
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _serviceBusConfiguration = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            _uiCommandService = uiCommandService;
            _requisicaoService = requisicaoService;
            _topicName = topic;
            _subscriptionName = subscription;
            ReceiveMessages(filterName, filter);
        }

        private async void ReceiveMessages(string filterName = null, string filter = null)
        {
            var subscriptionClient = new SubscriptionClient(_serviceBusConfiguration.ConnectionString, _topicName, _subscriptionName);

            if (filterName != null && filter != null)
            {
                const string defaultRule = "$default";

                if (subscriptionClient.GetRulesAsync().Result.Any(x => x.Name == defaultRule))
                    subscriptionClient.RemoveRuleAsync(defaultRule).Wait();

                if (subscriptionClient.GetRulesAsync().Result.All(x => x.Name != filterName))
                    await subscriptionClient.AddRuleAsync(new RuleDescription
                    {
                        Filter = new CorrelationFilter { Label = filter },
                        Name = filterName
                    });
            }

            var mo = new MessageHandlerOptions(ExceptionHandle) { AutoComplete = true };

            subscriptionClient.RegisterMessageHandler(Handle, mo);
        }

        private Task Handle(Message message, CancellationToken arg2)
        {
            var messageString = "";
            if (message.Body != null)
                messageString = Encoding.UTF8.GetString(message.Body);

            Console.WriteLine("Message Received");
            Console.WriteLine($"message Label: {message.Label}");
            Console.WriteLine($"message CorrelationId: {message.CorrelationId}");

            if (message.Label.ToLowerInvariant() == nameof(StoreCatalogReadyMessage).ToLowerInvariant())
            {
                var storeCatalogs = JsonConvert.DeserializeObject<List<StoreCatalogReadyMessage>>(messageString);
                _uiCommandService.AddToMessageList("ShowWelcomePage", storeCatalogs);
                _uiCommandService.SendMessagesAsync();
            }

            if (message.Label.ToLowerInvariant() == "noRestriction".ToLowerInvariant())
            {
                _uiCommandService.AddToMessageList("ShowFoodRestrictionsForm");
            }

            if (message.Label.ToLowerInvariant() == "restriction".ToLowerInvariant())
            {
                var url = _configuration.GetValue(typeof(string), "StoreCatalogUri").ToString();
                Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri);
                _requisicaoService.GetAsync(uri, "products");
            }

            return Task.CompletedTask;
        }

        private static Task ExceptionHandle(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Message handler encountered an exception {arg.Exception}.");
            var context = arg.ExceptionReceivedContext;
            Console.WriteLine($"- Endpoint: {context.Endpoint}, Path: {context.EntityPath}, Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
