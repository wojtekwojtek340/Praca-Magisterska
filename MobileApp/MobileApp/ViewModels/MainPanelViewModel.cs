using IotHubCommunication;
using IotHubCommunication.Communications;
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

        public WateringMode WateringMode 
        {
            get; 
            set;
        }
        public Stack<ServerMessage> MessageStack { get; private set; }
        public Data Data { get; private set; }
        public ICommand RefreshCommand { get; }
        public ICommand EnableSectionCommand { get; }
        public ICommand DisableSectionCommand { get; }
        public WateringPlan WateringPlan { get; set; }
        public MainPanelViewModel()
        {
            Title = "Panel Główny";
            MessageStack = new Stack<ServerMessage>();
            Data = new Data();
            WateringPlan = new WateringPlan();
            
            _waitForMessageTask = new Task(async () => await WaitForMessagge());
            _waitForMessageTask.Start();


            RefreshCommand = new Command(async () => await Refresh());
            EnableSectionCommand = new Command<int>(async (parameter) => await EnableSection(parameter));
            DisableSectionCommand = new Command<int>(async (parameter) => await DisableSection(parameter));
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
                Data.Temperature = data.Temperature;
                Data.AirHumidity = data.AirHumidity;
                Data.Preasure = data.Preasure;
                Data.SoilMoisture = data.SoilMoisture;
                OnPropertyChanged(nameof(Data));
            }
        }
        private async Task EnableSection(int parameter)
        {
            var message = new SetDigitalPin()
            {
                Id = 6,
                PinNumber = 6,
                PinState = true,
            };

            using (var communication = CommunicationFactory.CreateForMobileApp<ServerMessage>(ConfigurationManager.AppSettings))
            {
                await communication.SendAsync(message);
            }
        }
        private async Task DisableSection(int parameter)
        {
            var message = new SetDigitalPin()
            {
                Id = 10,
                PinNumber = 6,
                PinState = false,
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