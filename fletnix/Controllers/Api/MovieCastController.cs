using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fletnix.Models;
using fletnix.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fletnix.Controllers.Api
{

    [Route("/api/moviecast")]
    public class MovieCastController : Controller
    {
        private readonly IFletnixRepository _repository;


        public MovieCastController(IFletnixRepository repository)
        {
            _repository = repository;
            //_logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(Mapper.Map<IEnumerable<MovieCastViewModel>>(_repository.GetMovieCast()));
            }
            catch (Exception e)
            {
                //_logger.LogError($"Failed to get all trips: {e}");
                return BadRequest("Error occured");
            }
        }

        [HttpGet]
        [Route("/api/moviecast/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(Mapper.Map<MovieCastViewModel>(_repository.GetMovieCastByPersonId(id)));
            }
            catch (Exception e)
            {
                //_logger.LogError($"Failed to get all trips: {e}");
                return BadRequest($"Error occured: {e}");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] MovieCastViewModel castMember)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newCastMember = Mapper.Map<MovieCast>(castMember);

            _repository.UpdateMovieCast(newCastMember);
            if (await _repository.SaveChangesAsync())
            {
                return Created($"api/moviecast/", Mapper.Map<MovieCastViewModel>(newCastMember));
            }

            return BadRequest("Failed to save changes to the database");

        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] MovieCastViewModel castMember)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newCastMember = Mapper.Map<MovieCast>(castMember);
            _repository.AddMovieCast(newCastMember);
            if (await _repository.SaveChangesAsync())
            {
                return Created($"api/moviecast/", Mapper.Map<MovieCastViewModel>(newCastMember));
            }

            return BadRequest("Failed to save changes to the database");

        }

    }
}