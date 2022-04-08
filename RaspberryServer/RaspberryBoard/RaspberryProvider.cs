using IotHubCommunication;
using IotHubCommunication.Communications;
using IotHubCommunication.Messages.ClientMessages;
using IotHubCommunication.Messages.Core.ClientMessages;
using IotHubCommunication.Messages.ServerMessages;
using RaspberryServer.Commands;
using System.Configuration;
using System.Device.Gpio;

namespace RaspberryServer.RaspberryBoard
{
    public class RaspberryProvider
    {
        private readonly CommandExecutor _commandExecutor;
        public Stack<ClientMessage> MessageStack { get; private set; }
        public Task WaitForMessageTask { get; private set; }

        public GpioController GpioController { get; set; }
        public RaspberryProvider()
        {
            MessageStack = new();
            WaitForMessageTask = new Task(async () => await WaitForMessagge());
            WaitForMessageTask.Start();
            _commandExecutor = new();
        }
        private async Task WaitForMessagge()
        {
            while (true)
            {
                using (var communication = CommunicationFactory.CreateForRaspberryPi<ClientMessage>(ConfigurationManager.ConnectionStrings))
                {
                    communication.MessageReceived += CommunicationMessageReceived;
                    try
                    {
                        await communication.ReceiveAsync();
                    }
                    catch (Exception)
                    {
                        //TODO : logowanie błędów
                        Console.WriteLine("ERROR");
                    }
                }
                Thread.Sleep(500);
            }
        }
        private void CommunicationMessageReceived(object? sender, ClientMessage message)
        {
            if (message == null) return;
            MessageStack.Push(message);
        }
        public void Start()
        {
            while (true)
            {
                if (MessageStack.Count > 0)
                {
                    var message = MessageStack.Pop();
                    _commandExecutor.Execute(message);
                }
                Thread.Sleep(100);
            }
        }
    }
}
