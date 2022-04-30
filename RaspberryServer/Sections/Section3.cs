using RaspberryServer.Commands;
using RaspberryServer.Measures.Sensors.HW390;

namespace RaspberryServer.Sections
{
    public class Section3 : SectionBase
    {
        private readonly EventHandler? SlaveElectrovalveStatusChanged;
        public Section3(EventHandler? slaveElectrovalveStatusChanged)
        {
            Sensors.Add(new HW390v3());
            ElectrovalveSatusChanged += Section3_SlaveElectrovalveSatusChanged;
            SlaveElectrovalveStatusChanged = slaveElectrovalveStatusChanged;
        }
        private async void Section3_SlaveElectrovalveSatusChanged(object? sender, bool e)
        {
            SlaveElectrovalveStatusChanged?.Invoke(this, EventArgs.Empty);

            var command = new SetDigitalPin
            {
                PinNumber = Int32.Parse(PinoutDictionary.Section3Electrovalve),
                PinState = e,
            };

            await CommandExecutor.Execute(command);
        }
    }
}
