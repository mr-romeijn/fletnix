using System;
using System.Collections.Generic;

namespace fletnix.Models
{
    public partial class MovieCast
    {
        public int MovieId { get; set; }
        public int PersonId { get; set; }
        public string Role { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Person Person { get; set; }
    }
}
