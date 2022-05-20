using Iot.Device.Bmxx80;

namespace RaspberryServer.Measures.Sensors.BMP280
{
    public class BMP280T : BMP280Base
    {
        protected override void DoMeasure<T>(Bmp280 bmp280, T measurementResults)
        {
            bmp280.TryReadTemperature(out var preValue);
            measurementResults.Temperature = Math.Round(preValue.DegreesCelsius, 2);
        }
    }
}
