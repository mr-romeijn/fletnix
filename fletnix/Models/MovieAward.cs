using System;
using System.Collections.Generic;

namespace fletnix.Models
{
    public partial class MovieAward
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public int MovieId { get; set; }
        public int PersonId { get; set; }
        public string Result { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Person Person { get; set; }
        public virtual AwardType AwardType { get; set; }
    }
}
