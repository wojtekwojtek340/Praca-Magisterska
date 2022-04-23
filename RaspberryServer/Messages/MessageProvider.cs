using IotHubCommunication;
using IotHubCommunication.Messages.Core.ClientMessages;
using IotHubCommunication.Messages.ServerMessages;
using RaspberryServer.Commands;
using RaspberryServer.Sections;
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
        private readonly GeneralSectionsSupervisor generalSectionsSupervisor;
        public Stack<ClientMessage> MessageStack { get; private set; }
        public MessageProvider(GeneralSectionsSupervisor generalSectionsSupervisor)
        {
            this.generalSectionsSupervisor = generalSectionsSupervisor;
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
        private async Task SendMessage()
        {
            List<double?> soilMoisture = new List<double?>();
            generalSectionsSupervisor.Sections.ForEach(x => soilMoisture.Add(x.MeasureProvider.MeasurementResults.SoilMoisture));

            var message = new SendDataMessage()
            {
                Id = 10,
                Temperature = generalSectionsSupervisor.MeasureProvider.MeasurementResults.Temperature,
                Preasure = generalSectionsSupervisor.MeasureProvider.MeasurementResults.Preasure,
                AirHumidity = generalSectionsSupervisor.MeasureProvider.MeasurementResults.AirHumidity,
                SoilMoisture = soilMoisture,
            };

            using (var communication = CommunicationFactory.CreateForRaspberryPi<ClientMessage>(ConfigurationManager.ConnectionStrings))
            {
                await communication.SendAsync(message);
            }
        }

        public void MessagesExecute()
        {
            if (MessageStack.Count > 0)
            {
                var message = MessageStack.Pop();
                CommandExecutor.Execute(message, SendMessage);
            }
        }

        
    }
}
