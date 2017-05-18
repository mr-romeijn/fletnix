using System.ComponentModel.DataAnnotations;

namespace fletnix.ViewModels
{
    public class MovieDirectorViewModel
    {
        [Required]
        public int MovieId;

        [Required]
        public int PersonId;

    }
}