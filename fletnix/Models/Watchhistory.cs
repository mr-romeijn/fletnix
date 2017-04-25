using System;
using System.Collections.Generic;

namespace fletnix.Models
{
    public partial class Watchhistory
    {
        public int MovieId { get; set; }
        public string CustomerMailAddress { get; set; }
        public DateTime WatchDate { get; set; }
        public decimal Price { get; set; }
        public bool Invoiced { get; set; }

        public virtual Customer CustomerMailAddressNavigation { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
