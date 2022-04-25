using IotHubCommunication.Messages.ClientMessages;
using RaspberryServer.Commands;
using RaspberryServer.Measures.Sensors.BMP280;
using RaspberryServer.Measures.Sensors.DHT11;
using RaspberryServer.Measures.Sensors.HW390;

namespace RaspberryServer.Sections
{
    public class Section3 : SectionBase
    {
        public Section3()
        {
            Sensors.Add(new HW390v3());
            ElectrovalveSatusChanged += Section3_SlaveElectrovalveSatusChanged;
        }

        private async void Section3_SlaveElectrovalveSatusChanged(object? sender, bool e)
        {
            var command = new SetDigitalPin
            {
                PinNumber = 5,
                PinState = e,
            };

            //await CommandExecutor.Execute(command);
        }
    }
}
