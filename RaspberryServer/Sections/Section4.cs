using RaspberryServer.Commands;
using RaspberryServer.Measures.Sensors.HW390;

namespace RaspberryServer.Sections
{
    public class Section4 : SectionBase
    {
        private readonly EventHandler? SlaveElectrovalveStatusChanged;
        public Section4(EventHandler? slaveElectrovalveStatusChanged)
        {
            Sensors.Add(new HW390v4());
            ElectrovalveSatusChanged += Section4_SlaveElectrovalveSatusChanged;
            SlaveElectrovalveStatusChanged = slaveElectrovalveStatusChanged;
        }
        private async void Section4_SlaveElectrovalveSatusChanged(object? sender, bool e)
        {
            SlaveElectrovalveStatusChanged?.Invoke(this, EventArgs.Empty);

            var command = new SetDigitalPin
            {
                PinNumber = Int32.Parse(PinoutDictionary.Section4Electrovalve),
                PinState = e,
            };

            await CommandExecutor.Execute(command);
        }
    }
}
