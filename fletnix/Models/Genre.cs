using System;
using System.Collections.Generic;

namespace fletnix.Models
{
    public partial class Genre
    {
        public Genre()
        {
            MovieGenre = new HashSet<MovieGenre>();
        }

        public string GenreName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MovieGenre> MovieGenre { get; set; }
    }
}
