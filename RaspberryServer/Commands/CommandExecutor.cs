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
        public static void Execute(ClientMessage message, Func<Task>? sendMessage = null)
        {
            if (message is SetDigitalPin setPinCommand)
            {
                //using var controller = new GpioController();
                //controller.OpenPin(command.PinNumber, PinMode.Output);
                //controller.Write(command.PinNumber, command.PinState);
                //controller.ClosePin(command.PinNumber);
            }
            else if(message is GetDataMessage)
            {
                sendMessage?.Invoke();
            }
        }
    }
}
