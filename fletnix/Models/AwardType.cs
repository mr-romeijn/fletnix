using System;
using System.Collections.Generic;

namespace fletnix.Models
{
    public partial class AwardType
    {
        public AwardType()
        {
            MovieAward = new HashSet<MovieAward>();
        }

        public string Name { get; set; }
        public string Type { get; set; }

        public virtual ICollection<MovieAward> MovieAward { get; set; }
    }
}
