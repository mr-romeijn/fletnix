using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletnix.Helpers;
using fletnix.Models;
using fletnix.Services;
using fletnix.ViewModels;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace fletnix.Controllers
{
    [Authorize(Policy = "CustomerOnly")]
    public class DashboardController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private IFletnixRepository _repository;
        private FLETNIXContext _context;
        private IRedisCache _cache;

        public DashboardController(FLETNIXContext context, IMailService MailService, IConfigurationRoot Configuration, IFletnixRepository repository, IRedisCache cache){
            _mailService = MailService;
            _config = Configuration;
            _repository = repository;
            _context = context;
            _cache = cache;
        }
        
        public async Task<IActionResult> Index()
        {
            if (User.IsAuthenticated())
            {
                
                var q3 = _repository.GetWatchHistoryUser(User.Identity.Name);
                
                if (!_cache.Exists("MostPopularOfLastTwoWeeks"))
                {
                    var q2 = _repository.GetMostPopularMoviesOfLastNDays(14);
                    ViewData["MostPopularOfLastTwoWeeks"] = await q2;
                    _cache.Add("MostPopularOfLastTwoWeeks", JsonConvert.SerializeObject(ViewData["MostPopularOfLastTwoWeeks"]));
                }
                else
                {
                    ViewData["MostPopularOfLastTwoWeeks"] = JsonConvert.DeserializeObject<List<PopularMoviesViewModel>>(_cache.Retrieve("MostPopularOfLastTwoWeeks"));
                }
                
                if (!_cache.Exists("MostPopularOfAllTime"))
                {
                    var q1 = _repository.GetMostPopularMoviesOfAllTime(14);
                    ViewData["MostPopularOfAllTime"] = await q1;
                    _cache.Add("MostPopularOfAllTime", JsonConvert.SerializeObject(ViewData["MostPopularOfAllTime"]));
                }
                else
                {
                    ViewData["MostPopularOfAllTime"] = JsonConvert.DeserializeObject<List<PopularMoviesViewModel>>(_cache.Retrieve("MostPopularOfAllTime"));
                }
                
                if (!_cache.Exists("LatestMovies"))
                {
                    var q4 = _repository.GetLatestMoviesAdded();
                    ViewData["LatestMovies"] = await q4;
                    _cache.Add("LatestMovies", JsonConvert.SerializeObject(ViewData["LatestMovies"]));
                }
                else
                {
                    ViewData["LatestMovies"] = JsonConvert.DeserializeObject<List<PopularMoviesViewModel>>(_cache.Retrieve("LatestMovies"));
                }
                
                
                ViewData["WatchHistory"] = await q3;
                //ViewData["LatestMovies"] = await q4;
                
                //ViewData["MostPopularOfAllTime"] = new List<PopularMoviesViewModel>();
                //ViewData["MostPopularOfLastTwoWeeks"] = new List<PopularMoviesViewModel>();
                //ViewData["WatchHistory"] = new List<PopularMoviesViewModel>();;
                //ViewData["LatestMovies"] = new List<PopularMoviesViewModel>();;
                
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