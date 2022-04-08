﻿using IotHubCommunication;
using IotHubCommunication.Communications;
using IotHubCommunication.Messages.ClientMessages;
using IotHubCommunication.Messages.ServerMessages;
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
        public Stack<ServerMessage> MessageStack { get; private set; }
        public ICommand RefreshCommand { get; }
        public ICommand SetPin1HighCommand { get; }
        public ICommand SetPin1LowCommand { get; }
        public MainPanelViewModel()
        {
            Title = "Panel Główny";
            MessageStack = new Stack<ServerMessage>();
            
            _waitForMessageTask = new Task(async () => await WaitForMessagge());
            _waitForMessageTask.Start();

            RefreshCommand = new Command(async () => await Refresh());
            SetPin1HighCommand = new Command(async () => await SetPin1High());
            SetPin1LowCommand = new Command(async () => await SetPin1Low());
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
        }
        private async Task SetPin1High()
        {
            var message = new SetDigitalPin()
            {
                Id = 10,
                PinNumber = 2,
                PinState = true,
            };

            using (var communication = CommunicationFactory.CreateForMobileApp<ServerMessage>(ConfigurationManager.AppSettings))
            {
                await communication.SendAsync(message);
            }
        }
        private async Task SetPin1Low()
        {
            var message = new SetDigitalPin()
            {
                Id = 10,
                PinNumber = 2,
                PinState = false,
            };

            using (var communication = CommunicationFactory.CreateForMobileApp<ServerMessage>(ConfigurationManager.AppSettings))
            {
                await communication.SendAsync(message);
            }
        }
        private async Task Refresh()
        {

        }        
    }
}