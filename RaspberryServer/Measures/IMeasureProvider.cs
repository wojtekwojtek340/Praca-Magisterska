using RaspberryServer.Measures.Results;
using RaspberryServer.Measures.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Measures
{
    public interface IMeasureProvider<T> where T : class, IMeasurementResults, new()
    {
        T MeasurementResults { get; }
        void MeasuresExecute(ISensor sensor);
    }
}
