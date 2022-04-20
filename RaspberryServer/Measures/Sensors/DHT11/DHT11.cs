using Iot.Device.DHTxx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Measures.Sensors.DHT11
{
    public class DHT11 : Sensor
    {
        public override double? MeasureExecute()
        {
            using var dht11 = new Dht11(4);
            dht11.TryReadHumidity(out var humidity);
            while(humidity.Percent == 0 || humidity.Percent > 100)
            {
                dht11.TryReadHumidity(out humidity);
            };
            return humidity.Percent;
        }
    }
}
