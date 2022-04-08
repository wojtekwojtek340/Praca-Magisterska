using IotHubCommunication.Messages.Core.ClientMessages;

namespace IotHubCommunication.Messages.ClientMessages
{
    public class TurnOnWaterMessage : ClientMessage
    {
        public int Time { get; set; }
    }
}
