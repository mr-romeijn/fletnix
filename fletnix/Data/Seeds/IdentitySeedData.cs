using System.Linq;
using System.Threading.Tasks;
using fletnix.Models;
using fletnix.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace fletnix.Data.Seeds
{
    public class IdentitySeedData
    {
        private FLETNIXContext _context;
        private UserManager<ApplicationUser> _userManager;

        public IdentitySeedData(FLETNIXContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task EnsureSeedData()
        {

            foreach (var customer in _context.Customer.ToList())
            {
                if (await _userManager.FindByEmailAsync(customer.CustomerMailAddress) == null)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = customer.CustomerMailAddress,
                        Email = customer.CustomerMailAddress
                    };

                    await _userManager.CreateAsync(user, "Welkom123");
                }
            }


            if (await _userManager.FindByEmailAsync("nicky.romeijn@gmail.com") == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "nicky.romeijn@gmail.com",
                    Email = "nicky.romeijn@gmail.com"
                };

                await _userManager.CreateAsync(user, "Welkom123");
            }


        }
    }
}