using System;

namespace IotHubCommunication.Messages.Core
{
    public interface IMessageBase
    {
        int Id { get; set; }
        DateTime Date { get; }
    }
}
