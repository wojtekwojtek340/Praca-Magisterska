using IotHubCommunication.Data;
using IotHubCommunication.Messages.Core.ClientMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace IotHubCommunication.Messages.ClientMessages
{
    public class SetWateringPlan : ClientMessage
    {
        public WateringPlan WateringPlan { get; set; }
    }
}
