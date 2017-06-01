using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletnix.Attributes;
using fletnix.Controllers;
using fletnix.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fletnix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace fletnix
{
    public class MovieController : WalledGarden
    {
        private readonly FLETNIXContext _context;
        private ICache _cache;

        public MovieController(FLETNIXContext context, ICache cache)
        {
            _cache = cache;
            _context = context;    
        }

        // GET: Movie
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["TitleSortParam"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["YearSortParam"] = sortOrder == "Year" ? "year_desc" : "Year";

            IQueryable<Movie> fLETNIXContext;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            System.Net.WebUtility.HtmlDecode(searchString);

            //var searchParams = System.Net.WebUtility.HtmlDecode(searchString).Split(null).ToList();

            var movies = _context.Movie.AsNoTracking();
           
            switch (sortOrder)
            {
                case "title_desc":
                    movies = movies.OrderByDescending(m => m.Title);
                    break;
                case "Year":
                    movies = movies.OrderBy(m => m.PublicationYear);
                    break;
                case "year_desc":
                    movies = movies.OrderByDescending(m => m.PublicationYear);
                    break;
                default:
                    movies = movies.OrderBy(m => m.Title);
                    break;
            }

            ViewData["CurrentSort"] = sortOrder;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                int movieid = 0;
                if (Int32.TryParse(searchString, out movieid))
                {
                    movies = movies.Where(m => m.MovieId == movieid);
                }
                else
                {
                    movies = movies.Where(m => (m.Title.ToLower().Contains(searchString.ToLower())));
                }
            }

            int pageSize = 15;
            return View(await PaginatedList<Movie>.CreateAsync(movies.AsNoTracking().Include(m => m.PreviousPartNavigation), page ?? 1, pageSize));

            return View(await fLETNIXContext.ToListAsync());
        }

        // GET: Movie/Details/5
        [Authorize(Policy = "CustomerOnly")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.PreviousPartNavigation)
                .SingleOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movie/Create
        public IActionResult Create()
        {
            ViewData["PreviousPart"] = new SelectList(_context.Movie, "MovieId", "Title");
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,Title,Duration,Description,PublicationYear,CoverImage,PreviousPart,Price,Url")] Movie movie, List<IFormFile> files)
        {

            var t = files;

            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                _cache.Remove("LatestMovies");
                return RedirectToAction("Edit", new {id = movie.MovieId});
            }
           
            return RedirectToAction("Edit", new {id = movie.MovieId});
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }


            //ViewData["PreviousPart"] = new SelectList(_context.Movie, "MovieId", "Title", movie.PreviousPart);
            //ViewData["Persons"] = _context.Person.ToList();

            movie = _context.Movie
                .Include(director => director.MovieDirector).ThenInclude(person => person.Person)
                .Include(cast => cast.MovieCast).ThenInclude(person => person.Person)
                .Include(award => award.MovieAward)
                .Include(genre => genre.MovieGenre).First(m => m.MovieId == id);

            movie.PreviousPartNavigation = _context.Movie.FirstOrDefault(m => movie.PreviousPart == m.MovieId);

            ViewData["AwardTypes"] = _context.AwardType.ToList();
            ViewData["Genres"] = _context.Genre.ToList();
            ViewData["AwardResults"] = new List<String>{"nominated","won"};

            return View(movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,Duration,Description,PublicationYear,CoverImage,PreviousPart,Price,Url")] Movie movie)
        {

            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit",new {id=movie.MovieId, save="success" });
            }

            //ViewData["PreviousPart"] = new SelectList(_context.Movie, "MovieId", "Title", movie.PreviousPart);
            return View(movie);
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.PreviousPartNavigation)
                .SingleOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.MovieId == id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.MovieId == id);
        }

    }
}
