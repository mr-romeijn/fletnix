using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fletnix.ViewModels
{
    public class MovieGenrePostModel
    {
        public MovieGenrePostModel()
        {
            Genres = new List<string>();
        }

        [Required]
        public int MovieId;

        [Required]
        public List<string> Genres;
    }
}