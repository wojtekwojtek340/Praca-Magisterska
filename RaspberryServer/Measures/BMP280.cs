using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Measures
{
    public class BMP280 : Iot.Device.Bmxx80.Bmx280Base
    {
        public BMP280(byte deviceId, I2cDevice i2cDevice) : base(deviceId, i2cDevice)
        {
        }
    }
}
