using System;
using System.Collections.Generic;

namespace fletnix.Models
{
    public partial class Person
    {
        public Person()
        {
            MovieAward = new HashSet<MovieAward>();
            MovieCast = new HashSet<MovieCast>();
            MovieDirector = new HashSet<MovieDirector>();
        }

        public int PersonId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Gender { get; set; }

        public virtual ICollection<MovieAward> MovieAward { get; set; }
        public virtual ICollection<MovieCast> MovieCast { get; set; }
        public virtual ICollection<MovieDirector> MovieDirector { get; set; }
    }
}
