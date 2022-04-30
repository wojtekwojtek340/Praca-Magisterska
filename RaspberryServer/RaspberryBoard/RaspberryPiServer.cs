using RaspberryServer.Messages;
using RaspberryServer.Sections;

namespace RaspberryServer.RaspberryBoard
{
    public class RaspberryPiServer
    {
        public IMessageProvider MessageProvider { get; private set; }
        public GeneralSectionsSupervisor GeneralSectionsSupervisor { get; private set; }
        public RaspberryPiServer()
        {
            GeneralSectionsSupervisor = new();
            MessageProvider = new MessageProvider(GeneralSectionsSupervisor);
        }
        public void Start()
        {
            while (true)
            {
                MessagesExecute();
                MeasuresExecute();
                ActionsExceute();
                Thread.Sleep(1000);
            }
        }
        private void MessagesExecute()
        {
            MessageProvider.MessagesExecute();
        }

        private void ActionsExceute()
        {
            GeneralSectionsSupervisor.ActionsExecute();
        }

        private void MeasuresExecute()
        {
            GeneralSectionsSupervisor.MeasuresExecute();
        }
    }
}
