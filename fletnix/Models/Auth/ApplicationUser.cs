using IdentityServer4;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace fletnix.Models.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}