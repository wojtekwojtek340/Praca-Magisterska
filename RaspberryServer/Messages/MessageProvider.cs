using IotHubCommunication;
using IotHubCommunication.Messages.Core.ClientMessages;
using RaspberryServer.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Messages
{
    public class MessageProvider : IMessageProvider
    {
        private readonly Task waitForMessageTask;
        public Stack<ClientMessage> MessageStack { get; private set; }

        public MessageProvider()
        {
            MessageStack = new();
            waitForMessageTask = new Task(async () => await WaitForMessagge());
            waitForMessageTask.Start();
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
        public void MessagesExecute()
        {
            if (MessageStack.Count > 0)
            {
                var message = MessageStack.Pop();
                CommandExecutor.Execute(message);
            }
        }
    }
}
