using Iot.Device.Bmxx80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Measures.Sensors.BMP280
{
    public class BMP280T : BMP280Base
    {
        public BMP280T()
            : base(SensorType.Temperature)
        {
        }
        protected override double? DoMeasure(Bmp280 bmp280)
        {
            bmp280.TryReadTemperature(out var preValue);
            return preValue.DegreesCelsius;
        }
    }
}
