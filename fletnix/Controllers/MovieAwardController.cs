using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fletnix.Models;

namespace fletnix
{
    public class MovieAwardController : Controller
    {
        private readonly FLETNIXContext _context;

        public MovieAwardController(FLETNIXContext context)
        {
            _context = context;    
        }

        // GET: MovieAward
        public async Task<IActionResult> Index()
        {
            var fLETNIXContext = _context.MovieAward.Include(m => m.AwardType).Include(m => m.Movie).Include(m => m.Person);
            return View(await fLETNIXContext.ToListAsync());
        }

        // GET: MovieAward/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieAward = await _context.MovieAward
                .Include(m => m.AwardType)
                .Include(m => m.Movie)
                .Include(m => m.Person)
                .SingleOrDefaultAsync(m => m.Name == id);
            if (movieAward == null)
            {
                return NotFound();
            }

            return View(movieAward);
        }

        // GET: MovieAward/Create
        public IActionResult Create()
        {
            ViewData["Name"] = new SelectList(_context.AwardType, "Name", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "Title");
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "Firstname");
            return View();
        }

        // POST: MovieAward/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Year,Type,MovieId,PersonId,Result")] MovieAward movieAward)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movieAward);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["Name"] = new SelectList(_context.AwardType, "Name", "Name", movieAward.Name);
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "Title", movieAward.MovieId);
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "Firstname", movieAward.PersonId);
            return View(movieAward);
        }

        // GET: MovieAward/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieAward = await _context.MovieAward.SingleOrDefaultAsync(m => m.Name == id);
            if (movieAward == null)
            {
                return NotFound();
            }
            ViewData["Name"] = new SelectList(_context.AwardType, "Name", "Name", movieAward.Name);
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "Title", movieAward.MovieId);
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "Firstname", movieAward.PersonId);
            return View(movieAward);
        }

        // POST: MovieAward/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,Year,Type,MovieId,PersonId,Result")] MovieAward movieAward)
        {
            if (id != movieAward.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieAward);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieAwardExists(movieAward.Name))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["Name"] = new SelectList(_context.AwardType, "Name", "Name", movieAward.Name);
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "Title", movieAward.MovieId);
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "Firstname", movieAward.PersonId);
            return View(movieAward);
        }

        // GET: MovieAward/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieAward = await _context.MovieAward
                .Include(m => m.AwardType)
                .Include(m => m.Movie)
                .Include(m => m.Person)
                .SingleOrDefaultAsync(m => m.Name == id);
            if (movieAward == null)
            {
                return NotFound();
            }

            return View(movieAward);
        }

        // POST: MovieAward/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var movieAward = await _context.MovieAward.SingleOrDefaultAsync(m => m.Name == id);
            _context.MovieAward.Remove(movieAward);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MovieAwardExists(string id)
        {
            return _context.MovieAward.Any(e => e.Name == id);
        }
    }
}
