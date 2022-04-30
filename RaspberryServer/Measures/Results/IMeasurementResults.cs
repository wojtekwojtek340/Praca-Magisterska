namespace RaspberryServer.Measures.Results
{
    public interface IMeasurementResults
    {
        public double? Temperature { get; set; }
        public double? Preasure { get; set; }
        public double? AirHumidity { get; set; }
        public double? SoilMoisture { get; set; }
    }
}
