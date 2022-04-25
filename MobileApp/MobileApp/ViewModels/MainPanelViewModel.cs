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
        private event EventHandler<WateringMode> _wateringModeChanged;

        private WateringMode wateringMode;
        public WateringMode WateringMode 
        {
            get => wateringMode;
            set
            {
                if(wateringMode != value)
                {
                    SetProperty(ref wateringMode, value);
                    _wateringModeChanged?.Invoke(this, value);
                }
            }
        }      

        public Stack<ServerMessage> MessageStack { get; private set; }
        public SensorsData SensorsData { get; private set; }
        public ICommand RefreshCommand { get; }
        public ICommand EnableSectionCommand { get; }
        public ICommand DisableSectionCommand { get; }
        public ICommand SetWateringPlanCommand { get; }
        public WateringPlan WateringPlan { get; set; }
        public MainPanelViewModel()
        {
            Title = "Panel Główny";
            MessageStack = new Stack<ServerMessage>();
            SensorsData = new SensorsData();
            WateringPlan = new WateringPlan();

            _wateringModeChanged += MainPanelViewModel__wateringModeChanged;
            _waitForMessageTask = new Task(async () => await WaitForMessagge());
            _waitForMessageTask.Start();


            RefreshCommand = new Command(async () => await Refresh());
            SetWateringPlanCommand = new Command(async () => await SetWateringPlan());
            EnableSectionCommand = new Command<SectionNumbers>(async (sectionNumber) => await EnableSection(sectionNumber));
            DisableSectionCommand = new Command<SectionNumbers>(async (sectionNumber) => await DisableSection(sectionNumber));
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
            MessageStack.Push(message);

            if(message is SendDataMessage data)
            {
                SensorsData.Temperature = data.SensorsData.Temperature;
                SensorsData.AirHumidity = data.SensorsData.AirHumidity;
                SensorsData.Preasure = data.SensorsData.Preasure;
                SensorsData.SoilMoisture = data.SensorsData.SoilMoisture;
                OnPropertyChanged(nameof(SensorsData));
            }
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