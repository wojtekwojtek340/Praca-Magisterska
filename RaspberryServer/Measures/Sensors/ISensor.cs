using RaspberryServer.Measures.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Measures.Sensors
{
    public interface ISensor
    {
        void MeasureExecute<T>(T measurementResults) where T : IMeasurementResults;
    }
}
