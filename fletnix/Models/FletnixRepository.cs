using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<MovieGenre> GetGenres()
        {
            return _context.MovieGenre.ToList();
        }


    }
}