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
        public CommandExecutor CommandExecutor { get; set; }

        private bool isElectrovalveActive;
        public bool IsElectrovalveActive
        {
            get { return isElectrovalveActive; }
            set
            {
                if (IsElectrovalveActive != value)
                {
                    isElectrovalveActive = value;
                    ElectrovalveSatusChanged?.Invoke(this, value);
                }
            }
        }
        public SectionBase()
        {
            CommandExecutor = new();
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
        public void RaiseElectrovalveStatusChanged(bool state)
        {
            ElectrovalveSatusChanged?.Invoke(this, state);
        }
    }
}
