using System.Collections.Generic;
using System.Threading.Tasks;
using fletnix.ViewModels;

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
        void AddDirector(MovieDirector director);
        void RemoveDirector(MovieDirector director);
        void AddAward(MovieAward newAward);
        void RemoveAward(MovieAward newAward);
        void AddGenres(int movieId, List<MovieGenre> models);
        List<PopularMoviesViewModel> GetWatchHistoryUser(string email, int nAmount = 50);
        List<PopularMoviesViewModel> GetMostPopularMoviesOfLastNDays(int nDays, int nAmount = 50);
        List<PopularMoviesViewModel> GetMostPopularMoviesOfAllTime(int nAmount = 50);
        Movie GetMovieById(int? id);
        bool CheckIfSeenByUser(int? id, string email);
    }
}