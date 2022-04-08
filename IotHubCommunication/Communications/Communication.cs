using IotHubCommunication.Messages.Core;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IotHubCommunication.Communications
{
    public class Communication<T> : ICommunication<T> where T : class, IMessageBase, new()
    {
        private readonly string _targetDevice;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        public event EventHandler<object> MessageSend;
        public event EventHandler<T> MessageReceived;
        public EventHandler MessageSent { get; set; }
        public ServiceClient Service { get; private set; }
        public DeviceClient Client { get; private set; }
        public Communication(string serviceConnectionString, string clientConnectionString, string targetDevice)
        {
            _targetDevice = targetDevice;
            _jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects };
            Service = ServiceClient.CreateFromConnectionString(serviceConnectionString);
            Client = DeviceClient.CreateFromConnectionString(clientConnectionString);
        }
        public async Task ReceiveAsync()
        {
            while (true)
            {
                var receivedMessage = await Client.ReceiveAsync(TimeSpan.FromSeconds(5));
                if (receivedMessage == null) continue;
                await Client.CompleteAsync(receivedMessage);
                T response = JsonConvert.DeserializeObject<T>(Encoding.ASCII.GetString(receivedMessage.GetBytes()), _jsonSerializerSettings);
                if (receivedMessage != null)
                {
                    MessageReceived?.Invoke(this, response);
                    return;
                }
            }
        }

        public async Task SendAsync<TSend>(TSend item) where TSend : class, IMessageBase, new()
        {
            var commandMessage = JsonConvert.SerializeObject(item, _jsonSerializerSettings);
            Microsoft.Azure.Devices.Message message = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(commandMessage));
            await Service.SendAsync(_targetDevice, message);
            MessageSend?.Invoke(this, message);
        }

        #region Dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Service.Dispose();
                    Client.Dispose();

                    if(MessageSend != null)
                    {
                        foreach (var item in MessageSend?.GetInvocationList())
                        {
                            MessageSend -= item as EventHandler<object>;
                        }
                    }

                    if(MessageReceived != null)
                    {
                        foreach (var item in MessageReceived?.GetInvocationList())
                        {
                            MessageReceived -= item as EventHandler<T>;
                        }
                    }             
                }
                disposedValue = true;
            }
        }
        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
