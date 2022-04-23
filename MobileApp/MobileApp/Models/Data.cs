using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Models
{
    public class Data
    {
        public double? Temperature { get; set; }
        public double? Preasure { get; set; }
        public double? AirHumidity { get; set; }
        public List<double?> SoilMoisture { get; set; }
    }
}
