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


        public MovieController(IFletnixRepository repository)
        {
            _repository = repository;
            //_logger = logger;
        }

        [HttpGet]
        [Route("/api/token")]
        public IActionResult GetClaims()
        {

            //Token.Set(User.Claims);

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

            return new JsonResult(dict);
        }

        [HttpGet]
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