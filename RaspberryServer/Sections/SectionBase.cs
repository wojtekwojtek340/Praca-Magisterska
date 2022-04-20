using RaspberryServer.Measures;
using RaspberryServer.Measures.Sensors;
using RaspberryServer.Measures.Sensors.BMP280;

namespace RaspberryServer.Sections
{
    public class SectionBase
    {
        private readonly ManualResetEvent? isWaterOpenEvent;
        private readonly ManualResetEvent? isWaterClosedEvent;
        public MeasureProvider MeasureProvider { get; private set; }
        public List<ISensor> Sensors { get; set; }
        public bool IsWaterOpen { get; set; }

        public SectionBase()
        {
            isWaterOpenEvent = null;
            isWaterClosedEvent = null;
            MeasureProvider = new();
            Sensors = new();
        }

        internal void DoMeasure()
        {
            foreach (var sensor in Sensors)
            {
                MeasureProvider.MeasuresExecute(sensor);
            }
        }
    }
}
