using System;
using System.Collections.Generic;
using System.Text;

namespace IotHubCommunication.Data
{ 
    public class SensorsData
    {
        public double? Temperature { get; set; }
        public double? Preasure { get; set; }
        public double? AirHumidity { get; set; }
        public List<double?> SoilMoisture { get; set; }
    }
}
