using IotHubCommunication.Data;
using IotHubCommunication.Messages.ClientMessages;
using RaspberryServer.Commands;
using RaspberryServer.Measures.Sensors.BMP280;
using RaspberryServer.Measures.Sensors.DHT11;

namespace RaspberryServer.Sections
{
    public class GeneralSectionsSupervisor : SectionBase
    {
        public event EventHandler? SlaveElectrovalveStatusChanged;
        public List<SectionBase> Sections { get; private set; }
        public WateringPlan WateringPlan { get; set; }
        public WateringMode WateringMode { get; set; }
        public GeneralSectionsSupervisor()
        {
            Sensors.Add(new BMP280P());
            Sensors.Add(new BMP280T());
            Sensors.Add(new DHT11());
            CommandExecutor = new();
            WateringPlan = new();
            Sections = new();
            ElectrovalveSatusChanged += GeneralSectionsSupervisor_ElectrovalveSatusChanged;
            SlaveElectrovalveStatusChanged += GeneralSectionsSupervisor_SlaveElectrovalveStatusChanged1;
            SectionsInitialize();
        }

        private void GeneralSectionsSupervisor_SlaveElectrovalveStatusChanged1(object? sender, EventArgs e)
        {
            if (Sections.Any(x => x.IsElectrovalveActive == true))
            {
                IsElectrovalveActive = true;
            }
            else
            {
                IsElectrovalveActive = false;
            }
        }

        internal void ActionsExecute()
        {
            switch (WateringMode)
            {
                case WateringMode.Plan:
                    PlanModeExecute();
                    break;
                case WateringMode.Auto:
                    AutoModeExecute();
                    break;
                case WateringMode.Manual:
                    ManualModeExecute();
                    break;
                default:
                    break;
            }
        }

        private void ManualModeExecute()
        {

        }

        private void AutoModeExecute()
        {
            Sections.ForEach(x =>
            {
                if (x.MeasureProvider.MeasurementResults.SoilMoisture < 30)
                {
                    x.IsElectrovalveActive = true;
                }
                else
                {
                    x.IsElectrovalveActive = false;
                }
            });
        }

        private void PlanModeExecute()
        {
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    SetupSections(WateringPlan.Monday);
                    break;
                case DayOfWeek.Tuesday:
                    SetupSections(WateringPlan.Tuesday);
                    break;
                case DayOfWeek.Wednesday:
                    SetupSections(WateringPlan.Wednesday);
                    break;
                case DayOfWeek.Thursday:
                    SetupSections(WateringPlan.Thursday);
                    break;
                case DayOfWeek.Friday:
                    SetupSections(WateringPlan.Friday);
                    break;
                case DayOfWeek.Saturday:
                    SetupSections(WateringPlan.Saturday);
                    break;
                case DayOfWeek.Sunday:
                    SetupSections(WateringPlan.Sunday);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void SetupSections(Day day)
        {
            if (ShouldEnableWatering(day))
            {
                EnableSections();
            }
            else
            {
                DisableSections();
            }
        }

        private bool ShouldEnableWatering(Day day)
        {
            if (!day.IsChecked)
            {
                return false;
            }

            var now = DateTime.Now;
            int totalSeconds = now.Hour * 3600 + now.Minute * 60 + now.Second;
            if (totalSeconds > day.Start.TotalSeconds && totalSeconds < day.End.TotalSeconds)
            {
                return true;
            }
            return false;
        }

        private void DisableSections()
        {
            Sections.ForEach(section => section.IsElectrovalveActive = false);
        }

        private void EnableSections()
        {
            Sections.ForEach(section => section.IsElectrovalveActive = true);
        }

        public void MeasuresExecute()
        {
            DoMeasure();
            Sections.ForEach(section => section.DoMeasure());
        }

        private void SectionsInitialize()
        {
            Sections.Add(new Section1(SlaveElectrovalveStatusChanged));
            Sections.Add(new Section2(SlaveElectrovalveStatusChanged));
            Sections.Add(new Section3(SlaveElectrovalveStatusChanged));
            Sections.Add(new Section4(SlaveElectrovalveStatusChanged));
            Sections.ForEach(x => x.RaiseElectrovalveStatusChanged(false));
            RaiseElectrovalveStatusChanged(false);
        }

        internal void SetupSection(SetupSectionMessage setupSectionCommand)
        {
            switch (setupSectionCommand.SectionNumber)
            {
                case SectionNumbers.First:
                    Sections[0].IsElectrovalveActive = setupSectionCommand.SectionState;
                    break;
                case SectionNumbers.Second:
                    Sections[1].IsElectrovalveActive = setupSectionCommand.SectionState;
                    break;
                case SectionNumbers.Third:
                    Sections[2].IsElectrovalveActive = setupSectionCommand.SectionState;
                    break;
                case SectionNumbers.Fourth:
                    Sections[3].IsElectrovalveActive = setupSectionCommand.SectionState;
                    break;
                default:
                    break;
            }
        }

        private async void GeneralSectionsSupervisor_ElectrovalveSatusChanged(object? sender, bool e)
        {
            var command = new SetDigitalPin
            {
                PinNumber = Int32.Parse(PinoutDictionary.MainElectrovalve),
                PinState = !e,
            };

            await CommandExecutor.Execute(command);
        }
    }
}
