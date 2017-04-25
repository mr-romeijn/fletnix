using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletnix.Services;
using fletnix.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace fletnix.Controllers
{
    public class HomeController : Controller
    {

        private readonly IMailService _mailService;
        private IConfigurationRoot _config;

        public HomeController(IMailService MailService, IConfigurationRoot Configuration){
            _mailService = MailService;
            _config = Configuration;
        }

        public IActionResult Index()
        {
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
