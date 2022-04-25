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
    public class Section1 : SectionBase
    {
        public Section1()
        {            
            Sensors.Add(new HW390v1());
            ElectrovalveSatusChanged += Section1_SlaveElectrovalveSatusChanged;
        }
        private async void Section1_SlaveElectrovalveSatusChanged(object? sender, bool e)
        {
            var command = new SetDigitalPin
            {
                PinNumber = 16,
                PinState = e,
            };

            //await CommandExecutor.Execute(command);
        }
    }
}
