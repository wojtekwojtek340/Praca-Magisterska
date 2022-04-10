using Iot.Device.DHTxx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Measures.Sensors.BMP280
{
    public class DHT11 : Sensor
    {
        public override double MeasureExecute()
        {
            using var dht11 = new Dht11(4);
            dht11.TryReadTemperature(out var temperature);
            dht11.TryReadHumidity(out var humidity);
            return temperature.DegreesCelsius;
        }
    }
}
