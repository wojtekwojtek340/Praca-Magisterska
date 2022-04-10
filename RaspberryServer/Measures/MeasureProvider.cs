using RaspberryServer.Measures.Results;
using RaspberryServer.Measures.Sensors;
using RaspberryServer.Measures.Sensors.BMP280;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Measures
{
    public class MeasureProvider : IMeasureProvider
    {
        public MeasureProvider()
        {
            MeasurementResults = new();
        }
        public MeasurementResults MeasurementResults { get; private set; }

        public void MeasuresExecute()
        {
            MeasurementResults.Preasure = GetPreasure();
            MeasurementResults.Temperature = GetTemperature();
            MeasurementResults.SoilMoisture = GetSoilMoisture();
            MeasurementResults.AirHumidity = GetAirHumidity();
        }
        private double? GetAirHumidity()
        {
            throw new NotImplementedException();
        }

        private double? GetPreasure()
        {
            //ISensor sensor = new BMP280();
            //return sensor.MeasureExecute();
            return null;
        }

        private double[]? GetSoilMoisture()
        {
            throw new NotImplementedException();
        }

        private double? GetTemperature()
        {
            ISensor sensor = new DHT11();
            return sensor.MeasureExecute();
        }
    }
}
