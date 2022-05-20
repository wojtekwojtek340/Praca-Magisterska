using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;
using RaspberryServer.Measures.Results;
using System.Device.I2c;

namespace RaspberryServer.Measures.Sensors.BMP280
{
    public abstract class BMP280Base : Sensor
    {
        private readonly I2cConnectionSettings i2cSettings;
        public BMP280Base()
        {
            i2cSettings = new I2cConnectionSettings(1, Bmp280.SecondaryI2cAddress);
        }
        public override void MeasureExecute<T>(T measurementResults)
        {
            using var i2cDevice = I2cDevice.Create(i2cSettings);
            using var bmp280 = new Bmp280(i2cDevice);
            int measurementTime = bmp280.GetMeasurementDuration();
            bmp280.SetPowerMode(Bmx280PowerMode.Forced);
            Thread.Sleep(measurementTime);
            DoMeasure(bmp280, measurementResults);
        }
        protected abstract void DoMeasure<T>(Bmp280 bmp280, T measurementResults) where T : IMeasurementResults;
    }
}
