using System;
using System.ComponentModel.DataAnnotations;

namespace fletnix.ViewModels
{
    public class ContactViewModel
    {

        [Required]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(4096, MinimumLength = 10)]
        public string Message { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        public string LastName { get; set; }

        public ContactViewModel()
        {
            
        }
    }
}
