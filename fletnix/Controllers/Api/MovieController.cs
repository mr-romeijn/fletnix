using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using fletnix.Models;
using fletnix.ViewModels;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Token = fletnix.Helpers.Token;

namespace fletnix.Controllers.Api
{

    [Route("/api/movies")]
    public class MovieController : Controller
    {
        private readonly IFletnixRepository _repository;
        private FLETNIXContext _context;


        public MovieController(IFletnixRepository repository, FLETNIXContext context)
        {
            _context = context;
            _repository = repository;
            //_logger = logger;
        }

        [HttpPost]
        [Authorize(Policy = "CustomerOnly")]
        [Route("/api/movies/{id}/feedback")]
        public async Task<IActionResult> LeaveFeedback([FromBody] MovieReviewViewModel review)
        {
            var hasSeen = _context.Watchhistory.Where(r=>r.CustomerMailAddress == User.Identity.Name).FirstOrDefault(r => r.MovieId == review.MovieId);
            if (!ModelState.IsValid && hasSeen != null) return BadRequest(ModelState);
            
            var newReview = Mapper.Map<MovieReview>(review);
            newReview.CustomerMailAddress = User.Identity.Name;
            try
            {
                _repository.AddReviewToMovie(newReview);
                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/movies/{review.MovieId}/feedback",
                        Mapper.Map<MovieReviewViewModel>(newReview));
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return BadRequest("Failed to save changes to the database");
        }
        
        [HttpPatch]
        [Authorize(Policy = "CustomerOnly")]
        [Route("/api/movies/{id}/feedback")]
        public async Task<IActionResult> UpdateFeedback([FromBody] MovieReviewViewModel review)
        {
            var hasSeen = _context.Watchhistory.Where(r=>r.CustomerMailAddress == User.Identity.Name).FirstOrDefault(r => r.MovieId == review.MovieId);
            if (!ModelState.IsValid && hasSeen != null) return BadRequest(ModelState);
            var newReview = Mapper.Map<MovieReview>(review);
            newReview.CustomerMailAddress = User.Identity.Name;
            try{
                _repository.UpdateMovieReview(newReview);
                if (await _repository.SaveChangesAsync())
                {
                    return Json($"/api/movies/{review.MovieId}/feedback");
                }
                
                
            } catch (Exception e){
                return BadRequest(e.Message);
            }

            return BadRequest("Failed to save changes to the database");
        }
        
        
        [HttpDelete]
        [Authorize(Policy = "CustomerOnly")]
        [Route("/api/movies/{id}/feedback")]
        public async Task<IActionResult> DeleteFeedback([FromBody] MovieReviewViewModel review)
        {
            var hasSeen = _context.Watchhistory.Where(r=>r.CustomerMailAddress == User.Identity.Name).FirstOrDefault(r => r.MovieId == review.MovieId);
            if (!ModelState.IsValid && hasSeen != null) return BadRequest(ModelState);
            var newReview = Mapper.Map<MovieReview>(review);
            newReview.CustomerMailAddress = User.Identity.Name;
            try
            {
                _repository.DeleteMovieReview(newReview);
                if (await _repository.SaveChangesAsync())
                {
                    return Json($"/api/movies/{review.MovieId}/feedback");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return BadRequest("Failed to save changes to the database");
        }
        
        [HttpPost]
        [Authorize(Policy = "CustomerOnly")]
        [Route("/api/movies/{id}/watch")]
        public async Task<IActionResult> AddToWatchHistory(int id)
        {
            var hasSeen = _context.Watchhistory.Where(r=>r.CustomerMailAddress == User.Identity.Name).FirstOrDefault(r => r.MovieId == id);
            if (!ModelState.IsValid && hasSeen == null) return BadRequest(ModelState);
           
                _repository.AddToWatchHistory(new Watchhistory()
                {
                    CustomerMailAddress = User.Identity.Name,
                    Invoiced = true,
                    MovieId = id,
                    Price = 2,
                    WatchDate = DateTime.Now
                });
                if (await _repository.SaveChangesAsync())
                {
                    return Json($"/api/movies/{id}/watch");
                }
           

            return BadRequest("Failed to save changes to the database");
        }
        
        

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        [Route("/api/token")]
        public IActionResult GetClaims()
        {

            //Token.Set(User.Claims);
            var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            var claims = User.Claims
                .Select(a => new
                {
                    AreaId = a.Type,
                    Title = a.Value
                });

           // return new JsonResult(claims);

            var name = User.Claims.Where(c => c.Type == "name")
                .Select(c => c.Value).FirstOrDefault();

            var role = User.Claims.Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value).FirstOrDefault();

            var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value).FirstOrDefault();

            var dict = new Dictionary<String, String>();
            dict.Add("name", name);
            dict.Add("role", role);
            dict.Add("email", email);
            dict.Add("identity", User.Identity.Name);

            return new JsonResult(roles);
        }

        
        [HttpPatch]
        [Authorize(Policy = "AdminOnly")]
        [Route("/api/movie/genres")]
        public async Task<IActionResult>  UpdateGenres([FromBody] MovieGenrePostModel genres)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var models = new List<MovieGenre>();
            if (genres.Genres != null)
            {
                foreach (var g in genres.Genres)
                {
                    var m = new MovieGenreViewModel() {GenreName = g, MovieId = genres.MovieId};
                    models.Add(Mapper.Map<MovieGenre>(m));
                }
            }

            _repository.AddGenres(genres.MovieId, models);
            if (await _repository.SaveChangesAsync())
            {
                return Ok("Updated genres");
            }

            return BadRequest("Failed to save changes to the database");

        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [Route("/api/movie/director")]
        public async Task<IActionResult>  AddDirector([FromBody] MovieDirectorViewModel director)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newDirector = Mapper.Map<MovieDirector>(director);
            _repository.AddDirector(newDirector);
            if (await _repository.SaveChangesAsync())
            {
                return Created($"api/movie/director", Mapper.Map<MovieDirectorViewModel>(newDirector));
            }

            return BadRequest("Failed to save changes to the database");

        }

        [HttpDelete]
        [Authorize(Policy = "AdminOnly")]
        [Route("/api/movie/director")]
        public async Task<IActionResult>  RemoveDirector([FromBody] MovieDirectorViewModel director)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newDirector = Mapper.Map<MovieDirector>(director);
            _repository.RemoveDirector(newDirector);
            if (await _repository.SaveChangesAsync())
            {
                return Ok("Director has been removed");
            }

            return BadRequest("Failed to save changes to the database");

        }


        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [Route("/api/movie/award")]
        public async Task<IActionResult>  AddAward([FromBody] MovieAwardViewModel award)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newAward = Mapper.Map<MovieAward>(award);
            _repository.AddAward(newAward);
            if (await _repository.SaveChangesAsync())
            {
                return Created($"api/movie/award", Mapper.Map<MovieAwardViewModel>(newAward));
            }

            return BadRequest("Failed to save changes to the database");

        }

        [HttpDelete]
        [Authorize(Policy = "AdminOnly")]
        [Route("/api/movie/award")]
        public async Task<IActionResult>  RemoveAward([FromBody] MovieAwardViewModel award)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newAward = Mapper.Map<MovieAward>(award);
            _repository.RemoveAward(newAward);
            if (await _repository.SaveChangesAsync())
            {
                return Ok("Award has been removed");
            }

            return BadRequest("Failed to save changes to the database");

        }


        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        [Route("/api/movies/search/{title}")]
        public IActionResult Get(string title)
        {
            if (title.Length > 4)
            {
                try
                {
                    return Ok(_repository.SearchMoviesByTitle(title));
                }
                catch (Exception e)
                {
                    //_logger.LogError($"Failed to get all trips: {e}");
                    return BadRequest($"Error occured: {e}");
                }
            }
            else
            {
                return BadRequest("Minimum length of search query is 4 characters");
            }
        }
    }
}