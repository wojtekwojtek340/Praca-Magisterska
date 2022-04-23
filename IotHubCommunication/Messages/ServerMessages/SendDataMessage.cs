using System;
using System.Collections.Generic;

namespace IotHubCommunication.Messages.ServerMessages
{
    public class SendDataMessage : ServerMessage
    {
        public double? Temperature { get; set; }
        public double? Preasure { get; set; }
        public double? AirHumidity { get; set; }
        public List<double?> SoilMoisture { get; set; }
    }
}
