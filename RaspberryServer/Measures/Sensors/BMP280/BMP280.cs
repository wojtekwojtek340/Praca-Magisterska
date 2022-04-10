﻿using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Measures.Sensors.BMP280
{
    public class BMP280 : Sensor
    {
        private readonly I2cConnectionSettings i2cSettings;
        public BMP280()
        {
            i2cSettings = new I2cConnectionSettings(1, Bmp280.SecondaryI2cAddress);
        }
        public override double MeasureExecute()
        {
            using var i2cDevice = I2cDevice.Create(i2cSettings);
            using var bmp280 = new Bmp280(i2cDevice);
            int measurementTime = bmp280.GetMeasurementDuration();
            bmp280.SetPowerMode(Bmx280PowerMode.Forced);
            Thread.Sleep(measurementTime);
            bmp280.TryReadTemperature(out var tempValue);
            bmp280.TryReadPressure(out var preValue);
            bmp280.TryReadAltitude(out var altValue);

            return preValue.Hectopascals;
        }
    }
}