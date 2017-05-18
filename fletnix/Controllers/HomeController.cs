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
            var movies = _context.Movie
                .AsNoTracking()
                .Join(
                    _context.Watchhistory.Where(
                            w => w.WatchDate >= Convert.ToDateTime(DateTime.Now).AddDays(-30000))
                        .GroupBy(w => w.MovieId)
                        .Select(m => new { amount = m.Count(), mID = m.Key })
                        .OrderByDescending(a => a.amount)
                        .Select(a => new { movieid = a.mID }),
                    m => m.MovieId, w => w.movieid, (m, w) => m).Take(25).ToList();


            ViewData["Movies"] = movies;

            var watchHistory = _context.Movie
                .AsNoTracking()
                .Join(
                    _context.Watchhistory.Where(
                            w => w.CustomerMailAddress == User.Identity.Name)
                        .GroupBy(w => w.MovieId)
                        .Select(m => new { amount = m.Count(), mID = m.Key })
                        .OrderByDescending(a => a.amount)
                        .Select(a => new { movieid = a.mID }),
                    m => m.MovieId, w => w.movieid, (m, w) => m).Take(25).ToList();

               /* _context.Watchhistory.Where(h => h.CustomerMailAddress == User.Identity.Name)
                .Include(m => m.MovieId).ToList();*/

            ViewData["WatchHistory"] = watchHistory;

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

            /*var movie = await _context.Movie
                .Include(m => m.PreviousPartNavigation)
                .SingleOrDefaultAsync(m => m.MovieId == id);*/

            var movie = _context.Movie
                .Include(director => director.MovieDirector).ThenInclude(person => person.Person)
                .Include(cast => cast.MovieCast).ThenInclude(person => person.Person)
                .Include(award => award.MovieAward)
                .Include(genre => genre.MovieGenre).First(m => m.MovieId == id);
            movie.PreviousPartNavigation = _context.Movie.FirstOrDefault(m => movie.PreviousPart == m.MovieId);

            if (movie == null)
            {
                return NotFound();
            }

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
