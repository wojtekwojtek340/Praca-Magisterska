using RaspberryServer.Commands;
using RaspberryServer.Measures.Sensors.HW390;

namespace RaspberryServer.Sections
{
    public class Section2 : SectionBase
    {
        private readonly EventHandler? SlaveElectrovalveStatusChanged;
        public Section2(EventHandler? slaveElectrovalveStatusChanged)
        {
            Sensors.Add(new HW390v2());
            ElectrovalveSatusChanged += Section2_SlaveElectrovalveSatusChanged;
            SlaveElectrovalveStatusChanged = slaveElectrovalveStatusChanged;
        }
        private async void Section2_SlaveElectrovalveSatusChanged(object? sender, bool e)
        {
            SlaveElectrovalveStatusChanged?.Invoke(this, EventArgs.Empty);

            var command = new SetDigitalPin
            {
                PinNumber = Int32.Parse(PinoutDictionary.Section2Electrovalve),
                PinState = !e,
            };

            await CommandExecutor.Execute(command);
        }
    }
}
