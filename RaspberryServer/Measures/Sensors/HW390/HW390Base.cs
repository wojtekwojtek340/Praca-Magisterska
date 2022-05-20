using System.IO.Ports;

namespace RaspberryServer.Measures.Sensors.HW390
{
    public class HW390Base : Sensor
    {
        protected virtual string SensorId => "";
        public override void MeasureExecute<T>(T measurementResults)
        {
            using var serialPort = new SerialPort(PinoutDictionary.UsbName, 9600, Parity.None, 8, StopBits.One)
            {
                ReadTimeout = 500,
                WriteTimeout = 500,
            };
            serialPort.Open();
            while (!serialPort.IsOpen)
            {
                Console.WriteLine("Oczekiwanie na otworzenie portu");
                Thread.Sleep(100);
            }
            serialPort.DiscardInBuffer();
            serialPort.DiscardOutBuffer();
            Thread.Sleep(1500);
            serialPort.WriteLine(SensorId);
            string anwser = serialPort.ReadLine();
            serialPort.Close();
            if (Double.TryParse(anwser, out double soil))
            {
                measurementResults.SoilMoisture = Map(soil);
            }
            else
            {
                measurementResults.SoilMoisture = null;
            }
        }
        private double Map(double value)
        {
            //linear function measure
            var result = -(1/3.3) * value + (640/3.3);

            if (result > 100)
                return 100;

            if (result < 0)
                return 0;

            return Math.Round(result, 2);
        }
    }
}
