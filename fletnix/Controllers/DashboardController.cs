using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletnix.Models;
using fletnix.Services;
using fletnix.ViewModels;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace fletnix.Controllers
{
    [Authorize(Policy = "CustomerOnly")]
    public class DashboardController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private IFletnixRepository _repository;
        private FLETNIXContext _context;

        public DashboardController(FLETNIXContext context, IMailService MailService, IConfigurationRoot Configuration, IFletnixRepository repository){
            _mailService = MailService;
            _config = Configuration;
            _repository = repository;
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            if (User.IsAuthenticated())
            {
                var q1 = _repository.GetMostPopularMoviesOfAllTime();
                //var q2 = _repository.GetMostPopularMoviesOfLastNDays(14);
                var q3 = _repository.GetWatchHistoryUser(User.Identity.Name);

                ViewData["MostPopularOfAllTime"] = await q1;
                //ViewData["MostPopularOfLastTwoWeeks"] = await q2;
                ViewData["MostPopularOfLastTwoWeeks"] = new List<PopularMoviesViewModel>();
                ViewData["WatchHistory"] = await q3;
                
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
        
 
        [Route("/Dashboard/Movie/{id}")]
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


    }
}