using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using fletnix.Models;
using fletnix.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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