using IotHubCommunication.Messages.ClientMessages;
using RaspberryServer.Commands;
using RaspberryServer.Measures.Sensors.BMP280;
using RaspberryServer.Measures.Sensors.DHT11;
using RaspberryServer.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Sections
{
    public class GeneralSectionsSupervisor : SectionBase
    {
        public List<SectionBase> Sections { get; private set; }
        public GeneralSectionsSupervisor()
        {
            Sensors.Add(new BMP280P());
            Sensors.Add(new BMP280T());
            Sensors.Add(new DHT11());
            Sections = new();
            ElectrovalveSatusChanged += GeneralSectionsSupervisor_ElectrovalveSatusChanged;
            SectionsInitialize();
        }        

        public void MeasuresExecute()
        {
            DoMeasure();
            Sections.ForEach(section => section.DoMeasure());
        }

        private void SectionsInitialize()
        {
            Sections.Add(new Section1());
            Sections.Add(new Section2());
            Sections.Add(new Section3());
        }
        private void GeneralSectionsSupervisor_ElectrovalveSatusChanged(object? sender, bool e)
        {
            var command = new SetDigitalPin
            {
                PinNumber = 6,
                PinState = e,
            };

            CommandExecutor.Execute(command);
        }

    }
}
