using System;
using System.Collections.Generic;

namespace fletnix.Models
{
    public partial class Country
    {
        public Country()
        {
            Customer = new HashSet<Customer>();
            Event = new HashSet<Event>();
        }

        public string CountryName { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<Event> Event { get; set; }
    }
}
