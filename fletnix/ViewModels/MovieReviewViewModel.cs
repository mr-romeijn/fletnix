using System;
using System.ComponentModel.DataAnnotations;

namespace fletnix.ViewModels
{
    public class MovieReviewViewModel
    {
        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Review;
        
        [Required]
        [Range(1,10)]
        public int Rating;

        [Required]
        public int MovieId;

        [Required] public string CustomerMailAddress;

        [Required] public DateTime ReviewDate = DateTime.Now;
    }
}