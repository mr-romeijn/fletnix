using System.Collections.Generic;

namespace fletnix.Models
{
    public interface IFletnixRepository
    {
        IEnumerable<Movie> GetMovies();
        IEnumerable<MovieGenre> GetGenres();
    }
}