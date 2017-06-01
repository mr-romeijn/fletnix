using System.Collections.Generic;
using System.Threading.Tasks;
using fletnix.Helpers;
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
        Task<List<PopularMoviesViewModel>> GetWatchHistoryUser(string email, int nAmount = 50);
        Task<List<PopularMoviesViewModel>> GetMostPopularMoviesOfLastNDays(int nDays, int nAmount = 50);
        Task<List<PopularMoviesViewModel>> GetMostPopularMoviesOfAllTime(int nAmount = 50);
        Movie GetMovieById(int? id);
        bool CheckIfSeenByUser(int? id, string email);
        void AddReviewToMovie(MovieReview review);
        void UpdateMovieReview(MovieReview review);
        void DeleteMovieReview(MovieReview review);
        Task<List<PopularMoviesViewModel>> GetLatestMoviesAdded();
        void AddToWatchHistory(Watchhistory wh);
        Task<PaginatedList<AwardReportViewModel>> GetAwardReport(int? fromYear, int? tillYear, int pageSize = 15, int? page = 1);
        List<PriceRatingIndexViewModel> HighestAverageRatingReport();
        List<PriceRatingIndexViewModel> LowestAverageRatingReport();
        List<PriceRatingIndexViewModel> HighestAveragePriceIndexRatingReport();
        List<PriceRatingIndexViewModel> LowestAveragePriceIndexRatingReport();
    }
}