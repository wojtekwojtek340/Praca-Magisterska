using Iot.Device.DHTxx;

namespace RaspberryServer.Measures.Sensors.DHT11
{
    public class DHT11 : Sensor
    {
        public override void MeasureExecute<T>(T measurementResults)
        {
            using var dht11 = new Dht11(Int32.Parse(PinoutDictionary.DHT11))
            {
                MinTimeBetweenReads = TimeSpan.FromMilliseconds(100),
            };
            dht11.TryReadHumidity(out var humidity);
            while (humidity.Percent == 0 || humidity.Percent > 100)
            {
                dht11.TryReadHumidity(out humidity);
            };
            measurementResults.AirHumidity = humidity.Percent;
        }
    }
}
