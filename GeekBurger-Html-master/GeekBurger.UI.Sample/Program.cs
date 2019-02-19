using System;
using System.IO;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace GeekBurger.UI.Sample
{
    class Program
    {
        private static IConfiguration _configuration;

        private static void Main(string[] args)
        {
            Console.WriteLine("Type the topic|label|filepath and hit enter");
            while (true)
            {
                var command = Console.ReadLine();

                if(command == "exit") break;

                var message = command?.Split('|');
                if(message?.Length > 1)
                    SendMessageAsync(message[0], message[1], message.Length > 2 ? message[2] : null);
            }

        }

        public static async void SendMessageAsync(string topic, string label, string filePath)
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var config = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            
            var topicClient = new TopicClient(config.ConnectionString, topic);

            Message message;
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                var content = File.ReadAllBytes(filePath);
                message = new Message(content) {Label = label};
            }
            else
                message = new Message() { Label = label };

            await topicClient.SendAsync(message);

            await topicClient.CloseAsync();

            Console.WriteLine("Message successfully sent!");
        }
    }
}
