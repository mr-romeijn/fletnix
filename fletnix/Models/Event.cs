using System;
using System.Collections.Generic;

namespace fletnix.Models
{
    public partial class Event
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public string Location { get; set; }

        public virtual Country LocationNavigation { get; set; }
        public virtual Award NameNavigation { get; set; }
    }
}
