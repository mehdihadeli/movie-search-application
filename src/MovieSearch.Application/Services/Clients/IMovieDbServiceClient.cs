using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using MovieSearch.Core.Generals;
using MovieSearch.Core.Genres;
using MovieSearch.Core.Movies;
using MovieSearch.Core.People;
using MovieSearch.Core.Review;
using MovieSearch.Core.TV;

namespace MovieSearch.Application.Services.Clients
{
    //https://deviq.com/domain-driven-design/anti-corruption-layer
    //https://docs.microsoft.com/en-us/azure/architecture/patterns/anti-corruption-layer
    //https://www.markhneedham.com/blog/2009/07/07/domain-driven-design-anti-corruption-layer/
    //https://dev.to/asarnaout/the-anti-corruption-layer-pattern-pcd

    /// <summary>
    /// Anti-Corruption Layer: A highly defensive strategy to isolate our model from corruption by legacy models or external model.
    /// </summary>
    public interface IMovieDbServiceClient
    {
        /// <summary>
        /// Get a list of movies in theatres. This is a release type query that looks for all movies that have a release type of 2 or 3 within the specified date range.
        /// Ref: https://developers.themoviedb.org/3/movies/get-now-playing
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<MovieInfo>> GetNowPlayingAsync(int page,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Search for movies.
        /// Ref: https://developers.themoviedb.org/3/search/search-movies
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="page"></param>
        /// <param name="includeAdult"></param>
        /// <param name="year"></param>
        /// <param name="primaryReleaseYear"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<MovieInfo>> SearchByTitleMoviesAsync(string keyword,
            int page = 0,
            bool includeAdult = false,
            int year = 0,
            int primaryReleaseYear = 0,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of the current popular movies on TMDB. This list updates daily.
        /// Ref: https://developers.themoviedb.org/3/movies/get-popular-movies
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<MovieInfo>> GetPopularMoviesAsync(int page,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of upcoming movies in theatres. This is a release type query that looks for all movies that have a release type of 2 or 3 within the specified date range.
        /// Ref: https://developers.themoviedb.org/3/movies/get-upcoming
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<MovieInfo>> GetUpComingMoviesAsync(int page,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the top rated movies on TMDB.
        ///  Ref: https://developers.themoviedb.org/3/movies/get-top-rated-movies
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<MovieInfo>> GetTopRatedMoviesAsync(int page,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the list of official genres for movies.
        /// Ref: https://developers.themoviedb.org/3/genres/get-movie-list
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<Genre>> GetMovieGenresAsync(CancellationToken cancellationToken = default);

        Task<ListResultModel<MovieInfo>> FindMoviesByGenreAsync(IReadOnlyList<int> genreIds,
            int page = 0, CancellationToken cancellationToken = default);

        Task<ListResultModel<TVShowInfo>> FindTvShowsByGenreAsync(IReadOnlyList<int> genreIds,
            int page = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the list of official genres for TV shows.
        /// Ref: https://developers.themoviedb.org/3/genres/get-tv-list
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<Genre>> GetTvShowGenresAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the primary information about a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Movie> GetMovieByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the primary information about a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-details
        /// </summary>
        /// <param name="imdbId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Movie> GetMovieByImdbIdAsync(string imdbId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the primary TV show details by id.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TVShow> GetTvShowByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the user reviews for a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-reviews
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<ReviewInfo>> GetMovieReviewsAsync(int movieId, int page = 1,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the reviews for a TV show.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-reviews
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<ReviewInfo>> GetTvShowReviewsAsync(int tvShowId, int page = 1,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the details of a movie or TV show review.
        /// Ref: https://developers.themoviedb.org/3/reviews/get-review-details
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Review> GetReviewsAsync(string reviewId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the cast and crew for a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-credits
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MovieCredit> GetMovieCreditsAsync(int movieId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the movie credits for a person.
        /// Ref: https://developers.themoviedb.org/3/people/get-person-movie-credits
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PersonMovieCredit> GetPersonMovieCreditsAsync(int personId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the TV show credits for a person.
        /// Ref: https://developers.themoviedb.org/3/people/get-person-tv-credits
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PersonTVCredit> GetPersonTvShowCreditsAsync(int personId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the credits (cast and crew) that have been added to a TV show.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-credits
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TVShowCredit> GetTvShowCreditsAsync(int tvShowId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the images that belong to a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-images
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Images> GetMovieImagesAsync(int movieId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the images that belong to a TV show.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-images
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Images> GetTvShowImagesAsync(int tvShowId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the videos that have been added to a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-videos
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<(List<Video> Videos, int MovieId)> GetMovieVideosAsync(int movieId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the videos that have been added to a TV show.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-videos
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<(List<Video> Videos, int TvShowId)> GetTvShowVideosAsync(int tvShowId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of recommended movies for a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-recommendations
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<MovieInfo>> GetRecommendMoviesAsync(int movieId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the list of TV show recommendations for this item.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-recommendations
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<TVShowInfo>> GetRecommendTvShowAsync(int tvShowId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the primary person details by id.
        /// Ref: https://developers.themoviedb.org/3/people/get-person-details
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Person> GetPersonDetailAsync(int personId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the external ids for a person. We currently support the following external sources.
        /// Ref: https://developers.themoviedb.org/3/people/get-person-external-ids
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PersonExternalIds> GetPersonExternalDataAsync(int personId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of shows that are currently on the air. This query looks for any TV show that has an episode with an air date in the next 7 days.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-on-the-air
        /// </summary>
        /// <param name="page"></param>
        /// <param name="timeZone"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<TVShowInfo>> GetTvShowOnTheAirAsync(int page = 1, string timeZone = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of TV shows that are airing today. This query is purely day based as we do not currently support airing times.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-airing-today
        /// </summary>
        /// <param name="page"></param>
        /// <param name="timeZone"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<TVShowInfo>> GetTvShowAiringTodayAsync(int page = 1,
            string timeZone = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of the current popular TV shows on TMDB. This list updates daily.
        /// Ref: https://developers.themoviedb.org/3/tv/get-popular-tv-shows
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<TVShowInfo>> GetPopularTvShowAsync(int page = 1,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of the top rated TV shows on TMDB.
        /// Ref: https://developers.themoviedb.org/3/tv/get-top-rated-tv
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<TVShowInfo>> GetTvShowTopRatedAsync(int page = 1,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Search for a TV show.
        /// Ref: https://developers.themoviedb.org/3/search/search-tv-shows
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="page"></param>
        /// <param name="includeAdult"></param>
        /// <param name="firstAirDateYear"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<TVShowInfo>> SearchTvShowsByTitleAsync(string keyword,
            int page = 1,
            bool includeAdult = false,
            int firstAirDateYear = 0,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Search multiple models in a single request. Multi search currently supports searching for movies, tv shows and people in a single request.
        /// Ref: https://developers.themoviedb.org/3/search/multi-search
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="page"></param>
        /// <param name="includeAdult"></param>
        /// <param name="year"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<dynamic>> SearchMultiAsync(string keyword, int page = 1,
            bool includeAdult = false,
            int year = 0,
            CancellationToken cancellationToken = default);
    }
}