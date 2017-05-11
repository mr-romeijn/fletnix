using System.Collections.Generic;
using System.Threading.Tasks;

namespace fletnix.Models
{
    public interface IFletnixRepository
    {
        IEnumerable<Movie> GetMovies();
        IEnumerable<MovieGenre> GetGenres();
        IEnumerable<MovieCast> GetMovieCast();
        MovieCast GetMovieCastByPersonId(int id);
        void UpdateMovieCast(MovieCast movieCast);
        void AddMovieCast(MovieCast movieCast);
        Task<bool> SaveChangesAsync();
        IEnumerable<Movie> SearchMoviesByTitle(string title);
        IEnumerable<Person> SearchPersonsByName(string name);
        void DeleteMovieCast(MovieCast movieCast);
        Person GetPersonById(int id);
    }
}