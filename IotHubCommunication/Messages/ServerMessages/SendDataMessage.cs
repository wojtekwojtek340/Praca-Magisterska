using IotHubCommunication.Data;
using System;
using System.Collections.Generic;

namespace IotHubCommunication.Messages.ServerMessages
{
    public class SendDataMessage : ServerMessage
    {
        public SensorsData SensorsData { get; set; }
    }
}
