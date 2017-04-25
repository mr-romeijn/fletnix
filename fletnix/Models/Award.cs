using System;
using System.Collections.Generic;

namespace fletnix.Models
{
    public partial class Award
    {
        public Award()
        {
            Event = new HashSet<Event>();
        }

        public string Name { get; set; }

        public virtual ICollection<Event> Event { get; set; }
    }
}
