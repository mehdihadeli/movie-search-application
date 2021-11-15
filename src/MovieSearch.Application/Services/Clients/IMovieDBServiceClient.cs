using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using MovieSearch.Application.Movies.Dtos;
using TMDbLib.Objects.Discover;

namespace MovieSearch.Infrastructure.Services.Clients
{
    public interface IMovieDBServiceClient
    {
        /// <summary>
        /// Get a list of movies in theatres. This is a release type query that looks for all movies that have a release type of 2 or 3 within the specified date range.
        /// Ref: https://developers.themoviedb.org/3/movies/get-now-playing
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListResultModel<MovieLookupDto>> GetNowPlayingAsync(int page,
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
        Task<ListResultModel<MovieLookupDto>> SearchMoviesAsync(string keyword,
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
        Task<object> GetPopularMoviesAsync(int page, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of upcoming movies in theatres. This is a release type query that looks for all movies that have a release type of 2 or 3 within the specified date range.
        /// Ref: https://developers.themoviedb.org/3/movies/get-upcoming
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetUpComingMoviesAsync(int page, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the top rated movies on TMDB.
        ///  Ref: https://developers.themoviedb.org/3/movies/get-top-rated-movies
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetTopRatedMoviesAsync(int page, CancellationToken cancellationToken = default);

        /// <summary>
        /// Discover movies by different types of data like average rating, number of votes, genres and certifications
        /// Ref: https://developers.themoviedb.org/3/discover/movie-discover, https://www.themoviedb.org/documentation/api/discover
        /// </summary>
        /// <returns></returns>
        DiscoverMovie GetDiscoverMovies();

        /// <summary>
        /// Discover TV shows by different types of data like average rating, number of votes, genres, the network they aired on and air dates.
        /// Ref: https://developers.themoviedb.org/3/discover/tv-discover
        /// </summary>
        /// <returns></returns>
        DiscoverTv GetDiscoverTvShows();

        /// <summary>
        /// Get the list of official genres for movies.
        /// Ref: https://developers.themoviedb.org/3/genres/get-movie-list
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetMovieGenresAsync(CancellationToken cancellationToken = default);

        Task<object> GetMoviesByGenreAsync(IReadOnlyList<int> genreIds, int page = 0, CancellationToken
            cancellationToken = default);

        Task<object> GetTvShowsByGenreAsync(IReadOnlyList<int> genreIds, int page = 0, CancellationToken
            cancellationToken = default);

        /// <summary>
        /// Get the list of official genres for TV shows.
        /// Ref: https://developers.themoviedb.org/3/genres/get-tv-list
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetTvShowGenresAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the primary information about a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetMovieByIdAsync(int id, CancellationToken
            cancellationToken = default);

        /// <summary>
        /// Get the primary information about a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-details
        /// </summary>
        /// <param name="imdbId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetMovieByImdbIdAsync(string imdbId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the primary TV show details by id.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetTvShowByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the user reviews for a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-reviews
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetMovieReviewsAsync(int movieId, int page = 0,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the reviews for a TV show.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-reviews
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetTvShowReviewsAsync(int tvShowId, int page = 0,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the details of a movie or TV show review.
        /// Ref: https://developers.themoviedb.org/3/reviews/get-review-details
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetReviewsAsync(string reviewId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the cast and crew for a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-credits
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetMovieCreditsAsync(int movieId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the credits (cast and crew) that have been added to a TV show.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-credits
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetTvShowCreditsAsync(int tvShowId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the images that belong to a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-images
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetMovieBackdropsImagesAsync(int movieId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the images that belong to a TV show.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-images
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetTvShowBackdropsImagesAsync(int tvShowId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the videos that have been added to a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-videos
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetMovieVideosAsync(int movieId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the videos that have been added to a TV show.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-videos
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetTvSHowVideosAsync(int tvShowId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of recommended movies for a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-recommendations
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetRecommendMoviesAsync(int movieId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the list of TV show recommendations for this item.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-recommendations
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetRecommendTvShowAsync(int tvShowId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the primary person details by id.
        /// Ref: https://developers.themoviedb.org/3/people/get-person-details
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetPersonDetailAsync(int personId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the external ids for a person. We currently support the following external sources.
        /// Ref: https://developers.themoviedb.org/3/people/get-person-external-ids
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetPersonExternalDataAsync(int personId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the movie credits for a person.
        /// Ref: https://developers.themoviedb.org/3/people/get-person-movie-credits
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetPersonMovieCastAsync(int personId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the TV show credits for a person.
        /// Ref: https://developers.themoviedb.org/3/people/get-person-tv-credits
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetPersonTvShowCastAsync(int tvShowId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of shows that are currently on the air. This query looks for any TV show that has an episode with an air date in the next 7 days.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-on-the-air
        /// </summary>
        /// <param name="page"></param>
        /// <param name="timeZone"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetTvShowOnTheAirAsync(int page = 0,
            string timeZone = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of TV shows that are airing today. This query is purely day based as we do not currently support airing times.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-airing-today
        /// </summary>
        /// <param name="page"></param>
        /// <param name="timeZone"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetTvShowAiringTodayAsync(int page = 0,
            string timeZone = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of the current popular TV shows on TMDB. This list updates daily.
        /// Ref: https://developers.themoviedb.org/3/tv/get-popular-tv-shows
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetPopularTvShow(int page = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of the top rated TV shows on TMDB.
        /// Ref: https://developers.themoviedb.org/3/tv/get-top-rated-tv
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> GetTvShowTopRatedAsync(int page = 0, CancellationToken cancellationToken = default);

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
        Task<object> SearchTvShowsAsync(string keyword,
            int page = 0,
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
        Task<object> SearchMultiAsync(string keyword, int page = 0, bool includeAdult = false, int year = 0,
            CancellationToken cancellationToken = default);
    }
}