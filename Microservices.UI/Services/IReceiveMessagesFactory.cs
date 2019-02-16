namespace Microservices.UI.Services
{
    public interface IReceiveMessagesFactory
    {
        ReceiveMessagesService CreateNew(string topic, string subscription, string filterName = null, string filter = null);
    }
}
