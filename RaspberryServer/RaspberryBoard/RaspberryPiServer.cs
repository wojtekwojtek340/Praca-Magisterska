using IotHubCommunication;
using IotHubCommunication.Communications;
using IotHubCommunication.Messages.ClientMessages;
using IotHubCommunication.Messages.Core.ClientMessages;
using IotHubCommunication.Messages.ServerMessages;
using RaspberryServer.Commands;
using RaspberryServer.Measures;
using RaspberryServer.Sections;
using System.Configuration;
using System.Device.Gpio;
using System.Diagnostics;

namespace RaspberryServer.RaspberryBoard
{
    public class RaspberryPiServer
    {
        public int MyProperty { get; set; }
        public CommandExecutor CommandExecutor { get; private set; }
        public Stack<ClientMessage> MessageStack { get; private set; }
        public Task WaitForMessageTask { get; private set; }
        public List<SectionBase> Sections { get; private set; }
        public RaspberryPiServer()
        {
            CommandExecutor = new();
            MessageStack = new();
            Sections = new();
            WaitForMessageTask = new Task(async () => await WaitForMessagge());
            SectionsInitialize();
            WaitForMessageTask.Start();
        }

        private void SectionsInitialize()
        {
            Sections.Add(new Section1());
            Sections.Add(new Section2());
            Sections.Add(new Section3());
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
                        Console.WriteLine("WaitForMessage Error");
                    }
                }
                Thread.Sleep(1000);
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
            }
        }

        private void ActionsExceute()
        {

        }

        private void MeasuresExecute()
        {
            Sections.ForEach(section => section.DoMeasure());
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
