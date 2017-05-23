using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletnix.Models;
using fletnix.Services;
using fletnix.ViewModels;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using fletnix.Helpers;
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
            ViewData["MostPopularOfAllTime"] = _repository.GetMostPopularMoviesOfAllTime();

            ViewData["MostPopularOfLastTwoWeeks"] = _repository.GetMostPopularMoviesOfLastNDays(14);

            ViewData["WatchHistory"] = _repository.GetWatchHistoryUser(User.Identity.Name);

            return View();
        }

        [Authorize(Policy = "CustomerOnly")]
        [Route("/Movie/{id}")]
        public async Task<IActionResult> Movie(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _repository.GetMovieById(id);
            var seen = _repository.CheckIfSeenByUser(id, User.Identity.Name);

            if (movie == null)
            {
                return NotFound();
            }

            ViewData["seen"] = seen;

            return View("~/Views/Home/MovieDetail.cshtml", movie);
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
