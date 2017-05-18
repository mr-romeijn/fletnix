using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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