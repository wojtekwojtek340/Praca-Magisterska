using IotHubCommunication.Messages.ClientMessages;
using RaspberryServer.Commands;
using RaspberryServer.Measures.Sensors.HW390;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Sections
{
    public class Section4 : SectionBase
    {
        public Section4()
        {
            Sensors.Add(new HW390v4());
            ElectrovalveSatusChanged += Section4_SlaveElectrovalveSatusChanged;
        }

        private async void Section4_SlaveElectrovalveSatusChanged(object? sender, bool e)
        {
            var command = new SetDigitalPin
            {
                PinNumber = 25,
                PinState = e,
            };

            //await CommandExecutor.Execute(command);
        }
    }
}
