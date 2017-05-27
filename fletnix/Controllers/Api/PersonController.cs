using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using fletnix.Models;
using fletnix.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fletnix.Controllers.Api
{

    [Route("/api/persons")]
    public class PersonController : WalledGarden
    {
        private readonly IFletnixRepository _repository;


        public PersonController(IFletnixRepository repository)
        {
            _repository = repository;
            //_logger = logger;
        }

        [HttpGet]
        [Route("/api/persons/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_repository.GetPersonById(id));
            }
            catch (Exception e)
            {
                //_logger.LogError($"Failed to get all trips: {e}");
                return BadRequest($"Error occured: {e}");
            }
        }

        [HttpGet]
        [Route("/api/persons/search/{name}")]
        public IActionResult Search(string name)
        {
            if (name.Length > 3)
            {
                try
                {
                    return Ok(_repository.SearchPersonsByName(name).OrderBy(p=>p.Firstname));
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