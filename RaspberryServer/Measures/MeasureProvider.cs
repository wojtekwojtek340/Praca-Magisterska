using RaspberryServer.Measures.Results;
using RaspberryServer.Measures.Sensors;
using RaspberryServer.Measures.Sensors.BMP280;
using RaspberryServer.Measures.Sensors.DHT11;
using RaspberryServer.Measures.Sensors.HW390;
using System.Diagnostics;

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
            if(sensor is BMP280P)
            {
                try
                {
                    MeasurementResults.Preasure = sensor.MeasureExecute();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if(sensor is BMP280T)
            {
                try
                {
                    MeasurementResults.Temperature = sensor.MeasureExecute();                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (sensor is DHT11)
            {
                try
                { 
                    MeasurementResults.AirHumidity = sensor.MeasureExecute();              
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (sensor is HW390Base)
            {
                try
                {
                    MeasurementResults.SoilMoisture = sensor.MeasureExecute();          
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }       
    }
}
