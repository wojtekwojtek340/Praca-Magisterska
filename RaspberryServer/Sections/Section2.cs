using IotHubCommunication.Messages.ClientMessages;
using RaspberryServer.Commands;
using RaspberryServer.Measures.Sensors.BMP280;
using RaspberryServer.Measures.Sensors.DHT11;
using RaspberryServer.Measures.Sensors.HW390;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Sections
{
    public class Section2 : SectionBase
    {
        public Section2()
        {
            Sensors.Add(new HW390v2());            
            ElectrovalveSatusChanged += Section2_SlaveElectrovalveSatusChanged;
        }

        private void Section2_SlaveElectrovalveSatusChanged(object? sender, bool e)
        {
            var command = new SetDigitalPin
            {
                PinNumber = 26,
                PinState = e,
            };

            CommandExecutor.Execute(command);
        }
    }
}
