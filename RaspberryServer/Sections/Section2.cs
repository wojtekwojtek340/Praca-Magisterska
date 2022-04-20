using RaspberryServer.Measures.Sensors.BMP280;
using RaspberryServer.Measures.Sensors.DHT11;
using RaspberryServer.Measures.Sensors.HW390;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Sections
{
    public class Section2 : SectionBase
    {
        public Section2()
        {
            Sensors.Add(new BMP280P());
            Sensors.Add(new BMP280T());
            Sensors.Add(new DHT11());
            Sensors.Add(new HW390v2());
        }
    }
}
