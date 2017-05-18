using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fletnix.ViewModels
{
    public class MovieGenreViewModel
    {
        [Required]
        public int MovieId;

        [Required]
        public string GenreName;
    }
}