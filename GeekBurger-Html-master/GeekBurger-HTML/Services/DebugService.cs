using System.IO;
using System.Threading.Tasks;
using GeekBurger_HTML.Configuration;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace GeekBurger_HTML.Services
{
    public class DebugService : IDebugService
    {
        private readonly IConfiguration _configuration;
        public DebugService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendMessageAsync(string topic, string label, string filePath)
        {

            var config = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();

            var topicClient = new TopicClient(config.ConnectionString, topic);

            Message message;
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                var content = File.ReadAllBytes(filePath);
                message = new Message(content) { Label = label };
            }
            else
                message = new Message() { Label = label, CorrelationId = label };

            await topicClient.SendAsync(message);

            await topicClient.CloseAsync();

            return true;
        }
    }
}