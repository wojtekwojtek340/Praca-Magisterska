using RaspberryServer.Commands;
using RaspberryServer.Measures.Sensors.HW390;

namespace RaspberryServer.Sections
{
    public class Section1 : SectionBase
    {
        private readonly EventHandler? SlaveElectrovalveStatusChanged;
        public Section1(EventHandler? slaveElectrovalveStatusChanged) : base()
        {
            Sensors.Add(new HW390v1());
            ElectrovalveSatusChanged += Section1_SlaveElectrovalveSatusChanged;
            SlaveElectrovalveStatusChanged = slaveElectrovalveStatusChanged;
        }
        private async void Section1_SlaveElectrovalveSatusChanged(object? sender, bool e)
        {
            SlaveElectrovalveStatusChanged?.Invoke(this, EventArgs.Empty);

            var command = new SetDigitalPin
            {
                PinNumber = Int32.Parse(PinoutDictionary.Section1Electrovalve),
                PinState = !e,
            };

            await CommandExecutor.Execute(command);
        }
    }
}
