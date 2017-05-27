using System.IO;
using System.Threading.Tasks;
using fletnix.Models;
using fletnix.Services;
using fletnix.ViewModels;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using fletnix.Helpers;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace fletnix.Controllers
{
    public class HomeController : Controller
    {

        private readonly IMailService _mailService;
        private IConfigurationRoot _config;
        private IFletnixRepository _repository;
        private readonly FLETNIXContext _context;

        public HomeController(FLETNIXContext context, IMailService MailService, IConfigurationRoot Configuration, IFletnixRepository repository){
            _mailService = MailService;
            _config = Configuration;
            _repository = repository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsAuthenticated())
            {
                return RedirectToAction("index", "Dashboard");
            }

            return View();
        }

      
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model){

            if (ModelState.IsValid)
            {
                _mailService.sendMail(_config["MailSettings:ToAddress"], model.Email, "Fletnix contact form",
                    model.Message);

                ViewBag.FormNotification = "Your message has been received, we will respond as soon as possible.";

                ModelState.Clear();
            }

            return View();
        }

		public IActionResult Login()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

        public IActionResult Error()
        {
            return View();
        }
    }
}
