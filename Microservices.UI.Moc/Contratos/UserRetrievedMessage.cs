using System;

namespace Microservices.UI.Moc.Contratos
{
    public class UserRetrievedMessage
    {
        public Guid UserId { get; set; }
        public bool AreRestrictionsSet { get; set; }
    }
}
