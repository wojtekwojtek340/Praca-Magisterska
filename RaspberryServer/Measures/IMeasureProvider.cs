using RaspberryServer.Measures.Results;
using RaspberryServer.Measures.Sensors;

namespace RaspberryServer.Measures
{
    public interface IMeasureProvider<T> where T : class, IMeasurementResults, new()
    {
        T MeasurementResults { get; }
        void MeasuresExecute(ISensor sensor);
    }
}
