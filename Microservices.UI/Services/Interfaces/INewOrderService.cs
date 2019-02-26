using Microservices.UI.Contracts;

namespace Microservices.UI.Services.Interfaces
{
    public interface INewOrderService
    {
        void SendMessagesAsync();
        void AddToMessageList(string label, OrderResponse order);
    }
}
