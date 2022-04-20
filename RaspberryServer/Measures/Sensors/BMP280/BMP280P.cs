using Iot.Device.Bmxx80;

namespace RaspberryServer.Measures.Sensors.BMP280
{
    public class BMP280P : BMP280Base
    {
        public BMP280P()
            :base(SensorType.Preasure)
        {
        }
        protected override double? DoMeasure(Bmp280 bmp280)
        {
            bmp280.TryReadPressure(out var preValue);
            return preValue.Hectopascals;
        }
    }
}
