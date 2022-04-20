using RaspberryServer.Measures.Results;
using RaspberryServer.Measures.Sensors;
using RaspberryServer.Measures.Sensors.BMP280;
using RaspberryServer.Measures.Sensors.DHT11;
using RaspberryServer.Measures.Sensors.HW390;

namespace RaspberryServer.Measures
{
    public class MeasureProvider : IMeasureProvider
    {
        public MeasureProvider()
        {
            MeasurementResults = new();
        }
        public MeasurementResults MeasurementResults { get; private set; }

        public void MeasuresExecute(ISensor sensor)
        {
            if(sensor is BMP280P)
            {
                MeasurementResults.Preasure = sensor.MeasureExecute();
            }
            else if(sensor is BMP280T)
            {
                MeasurementResults.Temperature = sensor.MeasureExecute();
            }
            else if (sensor is DHT11)
            {
                MeasurementResults.AirHumidity = sensor.MeasureExecute();
            }
            else if (sensor is HW390Base)
            {
                MeasurementResults.SoilMoisture = sensor.MeasureExecute();
            }
        }
        private double? GetAirHumidity()
        {
            ISensor sensor = new DHT11();
            try
            {
                return sensor.MeasureExecute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private double? GetPreasure()
        {
            ISensor sensor = new BMP280Base(SensorType.Preasure);
            try
            {
                return sensor.MeasureExecute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private double? GetSoilMoisture()
        {
            ISensor sensor = new HW390Base();
            try
            {
                return sensor.MeasureExecute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private double? GetTemperature()
        {
            ISensor sensor = new BMP280Base(SensorType.Temperature);

            try
            {
                return sensor.MeasureExecute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
