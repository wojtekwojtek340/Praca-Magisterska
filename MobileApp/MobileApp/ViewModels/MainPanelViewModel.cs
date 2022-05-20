using IotHubCommunication;
using IotHubCommunication.Communications;
using IotHubCommunication.Data;
using IotHubCommunication.Messages.ClientMessages;
using IotHubCommunication.Messages.ServerMessages;
using MobileApp.Models;
using PCLAppConfig;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class MainPanelViewModel : BaseViewModel
    {
        private readonly Task _waitForMessageTask;
        private readonly Task _messagesExecuteTask;
        private event EventHandler<WateringMode> WateringModeChanged;

        private WateringMode wateringMode;
        public WateringMode WateringMode 
        {
            get => wateringMode;
            set
            {
                if(wateringMode != value)
                {
                    SetProperty(ref wateringMode, value);
                    WateringModeChanged?.Invoke(this, value);
                }
            }
        }      

        public Queue<ServerMessage> MessageQueue { get; private set; }
        public SensorsData SensorsData { get; private set; }
        public ICommand RefreshCommand { get; }
        public ICommand EnableSectionCommand { get; }
        public ICommand DisableSectionCommand { get; }
        public ICommand SetWateringPlanCommand { get; }
        public WateringPlan WateringPlan { get; set; }
        public MainPanelViewModel()
        {
            Title = "Panel Główny";
            MessageQueue = new Queue<ServerMessage>();
            SensorsData = new SensorsData();
            WateringPlan = new WateringPlan();

            WateringModeChanged += MainPanelViewModel__wateringModeChanged;

            _waitForMessageTask = new Task(async () => await WaitForMessagge());
            _waitForMessageTask.Start();

            _messagesExecuteTask = new Task(async () => await MessagesExecute());
            _messagesExecuteTask.Start();


            RefreshCommand = new Command(async () => await Refresh());
            SetWateringPlanCommand = new Command(async () => await SetWateringPlan());
            EnableSectionCommand = new Command<SectionNumbers>(async (sectionNumber) => await EnableSection(sectionNumber));
            DisableSectionCommand = new Command<SectionNumbers>(async (sectionNumber) => await DisableSection(sectionNumber));

            Task.Run(async () => await GetServerMode());
            Task.Run(async() => await Refresh());
        }

        private async Task GetServerMode()
        {
            var message = new GetModeMessage()
            {
                Id = 6,
            };

            using (var communication = CommunicationFactory.CreateForMobileApp<ServerMessage>(ConfigurationManager.AppSettings))
            {
                await communication.SendAsync(message);
            }
        }

        private async Task MessagesExecute()
        {
            while (true)
            {
                if(MessageQueue.Count > 0)
                {
                    var message = MessageQueue.Dequeue();

                    if (message is SendDataMessage data)
                    {
                        await SetSensorsData(data);
                    }
                    else if(message is SendModeMessage mode)
                    {
                        WateringMode = mode.WateringMode;                        
                    }
                }
                Thread.Sleep(1000);
            }
        }

        private Task SetSensorsData(SendDataMessage data)
        {
            SensorsData.Temperature = data.SensorsData.Temperature;
            SensorsData.AirHumidity = data.SensorsData.AirHumidity;
            SensorsData.Preasure = data.SensorsData.Preasure;
            SensorsData.SoilMoisture = data.SensorsData.SoilMoisture;
            OnPropertyChanged(nameof(SensorsData));
            return Task.CompletedTask;
        }

        private async void MainPanelViewModel__wateringModeChanged(object sender, WateringMode e)
        {
            var message = new SetModeMessage()
            {
                Id = 10,
                WateringMode = WateringMode
            };

            using (var communication = CommunicationFactory.CreateForMobileApp<ServerMessage>(ConfigurationManager.AppSettings))
            {
                await communication.SendAsync(message);
            }
        }

        private async Task SetWateringPlan()
        {
            var message = new SetWateringPlan()
            {
                Id = 6,
                WateringPlan = WateringPlan
            };

            using (var communication = CommunicationFactory.CreateForMobileApp<ServerMessage>(ConfigurationManager.AppSettings))
            {
                await communication.SendAsync(message);
            }
        }

        private async Task WaitForMessagge()
        {
            while (true)
            {
                using (var communication = CommunicationFactory.CreateForMobileApp<ServerMessage>(ConfigurationManager.AppSettings))
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
        private void CommunicationMessageReceived(object sender, ServerMessage message)
        {
            if (message == null) return;
            MessageQueue.Enqueue(message);            
        }
        private async Task EnableSection(SectionNumbers sectionNumber)
        {
            var message = new SetupSectionMessage()
            {
                Id = 6,
                SectionNumber = sectionNumber,
                SectionState = true,
            };

            using (var communication = CommunicationFactory.CreateForMobileApp<ServerMessage>(ConfigurationManager.AppSettings))
            {
                await communication.SendAsync(message);
            }
        }
        private async Task DisableSection(SectionNumbers sectionNumber)
        {
            var message = new SetupSectionMessage()
            {
                Id = 10,
                SectionNumber = sectionNumber,
                SectionState = false,
            };

            using (var communication = CommunicationFactory.CreateForMobileApp<ServerMessage>(ConfigurationManager.AppSettings))
            {
                await communication.SendAsync(message);
            }
        }
        private async Task Refresh()
        {
            var message = new GetDataMessage()
            {
                Id = 10,
            };

            using (var communication = CommunicationFactory.CreateForMobileApp<ServerMessage>(ConfigurationManager.AppSettings))
            {
                await communication.SendAsync(message);
            }
        }        
    }
}