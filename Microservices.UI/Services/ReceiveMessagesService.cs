using GeekBurger.StoreCatalog.Contract;
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

        public ReceiveMessagesService(IUICommandService uiCommandService, string topic, string subscription, string filterName = null, string filter = null)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _serviceBusConfiguration = configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            _uiCommandService = uiCommandService;

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

            List<StoreCatalogReadyMessage> storeCatalog = null;

            if (message.Label.ToLowerInvariant() == nameof(StoreCatalogReadyMessage).ToLowerInvariant())
                storeCatalog = JsonConvert.DeserializeObject<List<StoreCatalogReadyMessage>>(messageString);

            Console.WriteLine("Message Received");
            Console.WriteLine($"message Label: {message.Label}");
            Console.WriteLine($"message CorrelationId: {message.CorrelationId}");

            _uiCommandService.AddToMessageList(storeCatalog);

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
