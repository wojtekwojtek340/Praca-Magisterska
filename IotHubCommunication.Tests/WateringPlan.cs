using IotHubCommunication.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Models
{
    public class WateringPlan
    {
        public bool IsChecked { get; set; }
        public Day Monday { get; set; }
        public Day Tuesday { get; set; }
        public Day Wednesday { get; set; }
        public Day Thursday { get; set; }
        public Day Friday { get; set; }
        public Day Saturday { get; set; }
        public Day Sunday { get; set; }

        public WateringPlan()
        {
            Monday = new Day();
            Tuesday = new Day();
            Wednesday = new Day();
            Thursday = new Day();
            Friday = new Day();
            Saturday = new Day();
            Sunday = new Day();
        }

    }
}
