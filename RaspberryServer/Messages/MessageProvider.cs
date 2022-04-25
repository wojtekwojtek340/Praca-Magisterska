using IotHubCommunication;
using IotHubCommunication.Data;
using IotHubCommunication.Messages.ClientMessages;
using IotHubCommunication.Messages.Core.ClientMessages;
using IotHubCommunication.Messages.ServerMessages;
using RaspberryServer.Commands;
using RaspberryServer.Sections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Device.Gpio;
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
        public CommandExecutor CommandExecutor { get; set; }
        public MessageProvider(GeneralSectionsSupervisor generalSectionsSupervisor)
        {
            this.generalSectionsSupervisor = generalSectionsSupervisor;
            MessageStack = new();
            CommandExecutor = new();
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
        private async Task SendDataMessage()
        {
            List<double?> soilMoisture = new();
            generalSectionsSupervisor.Sections.ForEach(x => soilMoisture.Add(x.MeasureProvider.MeasurementResults.SoilMoisture));

            var message = new SendDataMessage()
            {
                Id = 10,
                SensorsData = new SensorsData
                {
                    Temperature = generalSectionsSupervisor.MeasureProvider.MeasurementResults.Temperature,
                    Preasure = generalSectionsSupervisor.MeasureProvider.MeasurementResults.Preasure,
                    AirHumidity = generalSectionsSupervisor.MeasureProvider.MeasurementResults.AirHumidity,
                    SoilMoisture = soilMoisture,
                }
            };

            using var communication = CommunicationFactory.CreateForRaspberryPi<ClientMessage>(ConfigurationManager.ConnectionStrings);
            await communication.SendAsync(message);
        }

        public async void MessagesExecute()
        {
            if (MessageStack.Count > 0)
            {
                var message = MessageStack.Pop();

                if (message is SetupSectionMessage setupSectionCommand)
                {
                    generalSectionsSupervisor.SetupSection(setupSectionCommand);            
                }
                else if (message is GetDataMessage)
                {
                    await SendDataMessage();
                }
                else if(message is SetWateringPlan setWateringPlanCommand)
                {
                    generalSectionsSupervisor.WateringPlan = setWateringPlanCommand.WateringPlan;
                }   
                else if(message is SetModeMessage setModeCommand)
                {
                    generalSectionsSupervisor.WateringMode = setModeCommand.WateringMode;
                }
            }
        }        
    }
}
