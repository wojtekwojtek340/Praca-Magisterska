using IotHubCommunication.Messages.Core;
using System;
using System.Threading.Tasks;

namespace IotHubCommunication.Communications
{
    public interface ICommunication<T> : IDisposable where T : class, IMessageBase, new()
    {
        event EventHandler<object> MessageSend;
        event EventHandler<T> MessageReceived;
        Task SendAsync<Tfunc>(Tfunc item) where Tfunc : class, IMessageBase, new();
        Task ReceiveAsync();
    }
}
