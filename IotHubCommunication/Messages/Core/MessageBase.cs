using System;

namespace IotHubCommunication.Messages.Core
{
    public class MessageBase : IMessageBase
    {
        public int Id { get; set; }
        public DateTime Date => DateTime.Now;
    }
}
