using System;
using System.Linq;
using System.Threading.Tasks;
using fletnix.Models;
using fletnix.Services;
using fletnix.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace fletnix.Controllers
{
    [Authorize(Policy = "CustomerOnly")]
    public class WatchHistoryController : Controller
    {
        private IFletnixRepository _repository;
        private FLETNIXContext _context;

        public WatchHistoryController(FLETNIXContext context, IFletnixRepository repository){

            _repository = repository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetWatchHistoryUser(User.Identity.Name));
        }
    }
}