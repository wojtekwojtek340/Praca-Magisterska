using IotHubCommunication.Communications;
using System.Configuration;
using PCLAppConfig;
using PCLAppConfig.Infrastructure;
using IotHubCommunication.Messages.Core;

namespace IotHubCommunication
{
    public static class CommunicationFactory
    {
        public static ICommunication<T> CreateForMobileApp<T>(NameValueCollection<Setting> connectionStringSettingsCollection) where T : class, IMessageBase, new()
        {
            string clientConnectionString = connectionStringSettingsCollection["MobileAppConnectionString"];
            string serviceConnectionString = connectionStringSettingsCollection["ServiceConnectionString"];
            return new Communication<T>(serviceConnectionString, clientConnectionString, "RaspberryBoard");
        }

        public static ICommunication<T> CreateForRaspberryPi<T>(ConnectionStringSettingsCollection connectionStringSettingsCollection) where T : class, IMessageBase, new()
        {
            string clientConnectionString = connectionStringSettingsCollection["RaspberryBoardConnectionString"].ConnectionString;
            string serviceConnectionString = connectionStringSettingsCollection["ServiceConnectionString"].ConnectionString;
            return new Communication<T>(serviceConnectionString, clientConnectionString, "MobileApp");
        }
    }
}
