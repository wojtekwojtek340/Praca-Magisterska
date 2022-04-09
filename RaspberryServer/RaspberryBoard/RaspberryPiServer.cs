using IotHubCommunication;
using IotHubCommunication.Communications;
using IotHubCommunication.Messages.ClientMessages;
using IotHubCommunication.Messages.Core.ClientMessages;
using IotHubCommunication.Messages.ServerMessages;
using RaspberryServer.Commands;
using RaspberryServer.Measures;
using System.Configuration;
using System.Device.Gpio;
using System.Diagnostics;

namespace RaspberryServer.RaspberryBoard
{
    public class RaspberryPiServer
    {
        public CommandExecutor CommandExecutor { get; private set; }
        public MeasureProvider MeasureProvider { get; private set; }
        public Stack<ClientMessage> MessageStack { get; private set; }
        public Task WaitForMessageTask { get; private set; }
        public RaspberryPiServer()
        {
            CommandExecutor = new();
            MeasureProvider = new();
            MessageStack = new();
            WaitForMessageTask = new Task(async () => await WaitForMessagge());
            WaitForMessageTask.Start();
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
                        Trace.WriteLine("WaitForMessage Error");
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
                MessagesExecute();
                MeasuresExecute();
                ActionsExceute();
                Thread.Sleep(100);
            }
        }

        private void ActionsExceute()
        {

        }

        private void MeasuresExecute()
        {

        }

        private void MessagesExecute()
        {
            if (MessageStack.Count > 0)
            {
                var message = MessageStack.Pop();
                CommandExecutor.Execute(message);
            }
        }
    }
}
