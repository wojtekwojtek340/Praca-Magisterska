using IotHubCommunication.Messages.ClientMessages;
using IotHubCommunication.Messages.Core.ClientMessages;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Commands
{
    public class CommandExecutor
    {
        public async Task Execute(SetDigitalPin message)
        {
            await Task.Run(() =>
            {
                using var controller = new GpioController();
                controller.OpenPin(message.PinNumber, PinMode.Output);
                controller.Write(message.PinNumber, message.PinState);
                controller.ClosePin(message.PinNumber);
            });
        }
    }
}
