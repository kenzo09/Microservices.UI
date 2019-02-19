using Microservices.UI.Contracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace GeekBurger_HTML.Services
{
    public class ReceiveMessagesFactory : IReceiveMessagesFactory
    {
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly ILogger<ReceiveMessagesService> _logger;
        public ReceiveMessagesFactory(IHubContext<MessageHub> hubContext, ILogger<ReceiveMessagesService> logger)
        {

            _hubContext = hubContext;
            _logger = logger;

            //var a = new Microservices.UI.Contracts.ShowProductsList();



            //By default, creates this receivemessageservice
            CreateNew("uicommand", "html");
            CreateNew("orderpaid", "html", "filter-store", "8048e9ec-80fe-4bad-bc2a-e4f4a75c834e");
        }

        public ReceiveMessagesService CreateNew(string topic, string subscription, string filterName = null, string filter = null)
        {
            return new ReceiveMessagesService(_hubContext, _logger, topic, subscription, filterName, filter);
        }
    }
}
