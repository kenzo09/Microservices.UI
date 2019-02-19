using System;

namespace GeekBurger_HTML.Services
{
    public interface IReceiveMessagesFactory
    {
        ReceiveMessagesService CreateNew(string topic, string subscription, string filterName = null, string filter = null);
    }
}