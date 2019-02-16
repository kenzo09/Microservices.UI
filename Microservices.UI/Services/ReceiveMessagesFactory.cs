namespace Microservices.UI.Services
{
    public class ReceiveMessagesFactory : IReceiveMessagesFactory
    {
        private const string Subscription = "ui";

        public ReceiveMessagesFactory()
        {
            CreateNew("StoreCatalogReady", Subscription);
            CreateNew("UserRetrieved", Subscription);
        }

        public ReceiveMessagesService CreateNew(string topic, string subscription, string filterName = null, string filter = null)
        {
            return new ReceiveMessagesService(topic, subscription, filterName, filter);
        }
    }
}
