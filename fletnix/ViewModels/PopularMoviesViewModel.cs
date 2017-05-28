using System;
using fletnix.Models;

namespace fletnix.ViewModels
{
    public class PopularMoviesViewModel
    {
        public Movie Movie;
        public int TimesViewed;
        public DateTime WatchDate { get; set; }
        public Customer Customer { get; set; }
        public MovieReview Review { get; set; }
    }
}