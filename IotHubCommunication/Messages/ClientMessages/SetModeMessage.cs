using IotHubCommunication.Data;
using IotHubCommunication.Messages.Core.ClientMessages;
namespace IotHubCommunication.Messages.ClientMessages
{
    public class SetModeMessage : ClientMessage
    {
        public WateringMode WateringMode { get; set; }
    }
}
