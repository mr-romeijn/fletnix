using System;

namespace fletnix.Models
{
    public class MovieReview
    {
        public int MovieId { get; set; }
        public string CustomerMailAddress { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
       
        public virtual Customer CustomerMailAddressNavigation { get; set; }
        public virtual Movie Movie { get; set; }
    }
}