using IotHubCommunication.Messages.ClientMessages;
using RaspberryServer.Commands;
using RaspberryServer.Measures;
using RaspberryServer.Measures.Results;
using RaspberryServer.Measures.Sensors;

namespace RaspberryServer.Sections
{
    public class SectionBase
    { 
        protected event EventHandler<bool>? ElectrovalveSatusChanged;
        public IMeasureProvider<MeasurementResults> MeasureProvider { get; private set; }
        public List<ISensor> Sensors { get; set; }       

        private bool isElectrovalveActive;
        public bool IsElectrovalveActive
        {
            get { return isElectrovalveActive; }
            set 
            { 
                isElectrovalveActive = value;
                ElectrovalveSatusChanged?.Invoke(this, value);
            }
        }
        public SectionBase()
        {
            MeasureProvider = new MeasureProvider<MeasurementResults>();
            Sensors = new();
        }
        public void DoMeasure()
        {
            foreach (var sensor in Sensors)
            {
                MeasureProvider.MeasuresExecute(sensor);
            }
        }
    }
}
