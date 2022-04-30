using Iot.Device.Bmxx80;

namespace RaspberryServer.Measures.Sensors.BMP280
{
    public class BMP280P : BMP280Base
    {
        public BMP280P()
        {
        }
        protected override void DoMeasure<T>(Bmp280 bmp280, T measurementResults)
        {
            bmp280.TryReadPressure(out var preValue);
            measurementResults.Preasure = Math.Round(preValue.Hectopascals, 2);
        }
    }
}
