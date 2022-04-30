using RaspberryServer.Measures.Results;
using RaspberryServer.Measures.Sensors;

namespace RaspberryServer.Measures
{
    public class MeasureProvider<T> : IMeasureProvider<T> where T : class, IMeasurementResults, new()
    {
        public MeasureProvider()
        {
            MeasurementResults = new();
        }
        public T MeasurementResults { get; private set; }

        public void MeasuresExecute(ISensor sensor)
        {
            try
            {
                sensor.MeasureExecute(MeasurementResults);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
