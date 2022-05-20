using System.Device.Gpio;

namespace RaspberryServer.Commands
{
    public class CommandExecutor
    {
        public Task Execute(SetDigitalPin message)
        {
            using var controller = new GpioController();
            controller.OpenPin(message.PinNumber, PinMode.Output);
            controller.Write(message.PinNumber, message.PinState);
            controller.ClosePin(message.PinNumber);
            return Task.CompletedTask;
        }
    }
}
