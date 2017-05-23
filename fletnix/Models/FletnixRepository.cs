using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletnix.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace fletnix.Models
{
    public class FletnixRepository : IFletnixRepository
    {
        private FLETNIXContext _context;
        private ILogger<FLETNIXContext> _logger;

        public FletnixRepository(FLETNIXContext context, ILogger<FLETNIXContext> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Movie> GetMovies()
        {
            _logger.LogInformation("Retrieving all movies from the database");
            return _context.Movie.ToList();
        }

        public IEnumerable<Movie> SearchMoviesByTitle(string title)
        {
            return _context.Movie.AsNoTracking().Where(m => (m.Title.ToLower().Contains(title.ToLower())))
                .OrderByDescending(m => m.PublicationYear);
        }

        public IEnumerable<Person> SearchPersonsByName(string name)
        {
            return _context.Person.AsNoTracking().Where(p=>( p.Firstname.ToLower().Contains(name.ToLower()) || p.Lastname.ToLower().Contains(name.ToLower())) ).OrderByDescending(p=>p.PersonId);
        }

        public IEnumerable<MovieGenre> GetGenres()
        {
            return _context.MovieGenre.ToList();
        }


        public IEnumerable<MovieCast> GetMovieCast()
        {
            return _context.MovieCast.ToList();
        }

        public MovieCast GetMovieCastByPersonId(int id)
        {
            return _context.MovieCast.FirstOrDefault(e => e.PersonId == id);
        }

        public Person GetPersonById(int id)
        {
            return _context.Person.FirstOrDefault(e => e.PersonId == id);
        }

        public void AddDirector(MovieDirector director)
        {
            _context.MovieDirector.Add(director);
        }

        public void RemoveDirector(MovieDirector director)
        {
            _context.MovieDirector.Remove(director);
        }

        public void AddAward(MovieAward newAward)
        {
            _context.MovieAward.Add(newAward);
        }

        public void RemoveAward(MovieAward newAward)
        {
            _context.MovieAward.Remove(newAward);
        }

        public void AddGenres(int movieId, List<MovieGenre> models)
        {

            var listOfItemsToBeRemoved = _context.MovieGenre.AsNoTracking().Where(genre => genre.MovieId == movieId).ToList();

            foreach (var l in listOfItemsToBeRemoved)
            {
                _context.MovieGenre.Remove(l);
            }

            _context.SaveChanges();

            _context.MovieGenre.AddRange(models);
        }

        public void UpdateMovieCast(MovieCast movieCast)
        {

            _context.MovieCast.Remove(_context.MovieCast.FirstOrDefault(m => (m.MovieId == movieCast.MovieId && m.PersonId == movieCast.PersonId && movieCast.Role == m.Role)));
            _context.MovieCast.Add(movieCast);

            /* var cast = _context.MovieCast
                 .Where(e => e.PersonId == movieCast.PersonId)
                 .First(e => e.MovieId == movieCast.MovieId)

             cast.Role = movieCast.Role; */
        }

        public List<PopularMoviesViewModel> GetMostPopularMoviesOfLastNDays(int nDays, int nAmount = 50)
        {
            var MostPopularMoviesOfLastNDays = (from w in _context.Watchhistory
                where w.WatchDate >= Convert.ToDateTime(DateTime.Now).AddDays(-nDays)
                join m in _context.Movie on w.MovieId equals m.MovieId
                group m by new { m } into g
                orderby g.Count() descending
                select new PopularMoviesViewModel
                {
                    Movie = g.Key.m,
                    TimesViewed = g.Count()
                }).AsNoTracking().Take(nAmount);

            return MostPopularMoviesOfLastNDays.ToList();
        }

        public List<PopularMoviesViewModel> GetMostPopularMoviesOfAllTime(int nAmount = 50)
        {
            var MostPopularOfAllTime = (from w in _context.Watchhistory
                join m in _context.Movie on w.MovieId equals m.MovieId
                group m by new {m}
                into g
                orderby g.Count() descending
                select new PopularMoviesViewModel
                {
                    Movie = g.Key.m,
                    TimesViewed = g.Count()
                }).AsNoTracking();

            return MostPopularOfAllTime.Take(nAmount).ToList();
        }

        public List<PopularMoviesViewModel> GetWatchHistoryUser(string email, int nAmount = 50)
        {
            var watchHistoryUser = (from w in _context.Watchhistory
                where w.CustomerMailAddress == email
                join m in _context.Movie on w.MovieId equals m.MovieId
                group m by new { m } into g
                orderby g.Count() descending
                select new PopularMoviesViewModel
                {
                    Movie = g.Key.m,
                    TimesViewed = g.Count()
                }).AsNoTracking().Take(nAmount);

            /* _context.Watchhistory.Where(h => h.CustomerMailAddress == User.Identity.Name)
             .Include(m => m.MovieId).ToList();*/

            return watchHistoryUser.ToList();
        }

        public Movie GetMovieById(int? id)
        {
            var movie = _context.Movie
                .Include(director => director.MovieDirector).ThenInclude(person => person.Person)
                .Include(cast => cast.MovieCast).ThenInclude(person => person.Person)
                .Include(award => award.MovieAward)
                .Include(genre => genre.MovieGenre).First(m => m.MovieId == id);
            movie.PreviousPartNavigation = _context.Movie.FirstOrDefault(m => movie.PreviousPart == m.MovieId);

            return movie;
        }

        public bool CheckIfSeenByUser(int? id, string email)
        {
            var seen = _context.Watchhistory
                .Where(wh => wh.CustomerMailAddress == email)
                .FirstOrDefault(wh => wh.MovieId == id);

            return seen != null;
        }

        public void DeleteMovieCast(MovieCast movieCast)
        {
            //_context.MovieCast.Remove(movieCast);
            _context.MovieCast.Remove(_context.MovieCast.FirstOrDefault(m => (m.MovieId == movieCast.MovieId && m.PersonId == movieCast.PersonId)));
        }

        public void AddMovieCast(MovieCast movieCast)
        {
            _context.MovieCast.Add(movieCast);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}