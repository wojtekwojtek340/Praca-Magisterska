using System.Device.Gpio;

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
