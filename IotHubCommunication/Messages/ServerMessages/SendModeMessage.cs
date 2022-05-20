using IotHubCommunication.Data;

namespace IotHubCommunication.Messages.ServerMessages
{
    public class SendModeMessage : ServerMessage
    {
        public WateringMode WateringMode { get; set; }
    }
}
