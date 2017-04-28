using System.ComponentModel.DataAnnotations;

namespace fletnix.ViewModels
{
    public class MovieCastViewModel
    {

        [Required]
        public int MovieId;

        [Required]
        public int PersonId;

        [Required]
        [StringLength(255,MinimumLength = 3)]
        public string Role;
    }
}