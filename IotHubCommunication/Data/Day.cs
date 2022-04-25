using System;
using System.Collections.Generic;
using System.Text;

namespace IotHubCommunication.Data
{
    public class Day
    {
        public bool IsChecked { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}
