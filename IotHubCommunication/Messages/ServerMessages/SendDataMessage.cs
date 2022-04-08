using System;

namespace IotHubCommunication.Messages.ServerMessages
{
    public class SendDataMessage : ServerMessage
    {
        public double Temperature { get; set; }
    }
}
