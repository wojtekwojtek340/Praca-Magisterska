using RaspberryServer.Measures.Sensors.BMP280;
using RaspberryServer.Measures.Sensors.DHT11;
using RaspberryServer.Measures.Sensors.HW390;

namespace RaspberryServer.Sections
{
    public class Section3 : SectionBase
    {
        public Section3()
        {
            Sensors.Add(new BMP280P());
            Sensors.Add(new BMP280T());
            Sensors.Add(new DHT11());
            Sensors.Add(new HW390v3());
        }
    }
}
