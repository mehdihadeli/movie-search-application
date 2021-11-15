using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BuildingBlocks.Domain;
using BuildingBlocks.Web;
using Microsoft.Extensions.Options;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Application.Reviews.Dtos;
using MovieSearch.Application.TVShows.Dtos;
using MovieSearch.Core;
using MovieSearch.Core.Aggregates.Genres;
using MovieSearch.Core.Aggregates.Movies;
using TMDbLib.Client;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using TvShow = MovieSearch.Core.Aggregates.TvShows.TvShow;

namespace MovieSearch.Infrastructure.Services.Clients
{
    // https://www.themoviedb.org/
    // https://github.com/LordMike/TMDbLib
    // https://github.com/nCubed/TheMovieDbWrapper/
    public class MovieDBServiceClient : IMovieDBServiceClient
    {
        private readonly IMapper _mapper;
        private readonly TMDbClient _client;
        private readonly MovieDBOptions _movieDbOptions;

        public MovieDBServiceClient(IOptions<MovieDBOptions> options, IMapper mapper)
        {
            _mapper = mapper;
            _movieDbOptions = options.Value;
            _client = new TMDbClient(_movieDbOptions.ApiKey);
        }

        /// <summary>
        /// Get a list of movies in theatres. This is a release type query that looks for all movies that have a release type of 2 or 3 within the specified date range.
        /// Ref: https://developers.themoviedb.org/3/movies/get-now-playing
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ListResultModel<MovieLookupDto>> GetNowPlayingAsync(int page,
            CancellationToken cancellationToken = default)
        {
            var searchResult = await _client.GetMovieNowPlayingListAsync(_movieDbOptions.Language, page,
                _movieDbOptions.Region, cancellationToken);

            return _mapper.Map<ListResultModel<MovieLookupDto>>(searchResult);
        }

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
        public async Task<ListResultModel<MovieLookupDto>> SearchMoviesAsync(string keyword,
            int page = 0,
            bool includeAdult = false,
            int year = 0,
            int primaryReleaseYear = 0,
            CancellationToken cancellationToken = default)
        {
            var searchResult = await _client.SearchMovieAsync(keyword, page, includeAdult, year, _movieDbOptions.Region,
                primaryReleaseYear, cancellationToken);

            return _mapper.Map<ListResultModel<MovieLookupDto>>(searchResult);
        }

        /// <summary>
        /// Get a list of the current popular movies on TMDB. This list updates daily.
        /// Ref: https://developers.themoviedb.org/3/movies/get-popular-movies
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ListResultModel<MovieLookupDto>> GetPopularMoviesAsync(int page,
            CancellationToken cancellationToken = default)
        {
            var searchResult = await _client.GetMoviePopularListAsync(_movieDbOptions.Language, page,
                _movieDbOptions.Region,
                cancellationToken);

            return _mapper.Map<ListResultModel<MovieLookupDto>>(searchResult);
        }

        /// <summary>
        /// Get a list of upcoming movies in theatres. This is a release type query that looks for all movies that have a release type of 2 or 3 within the specified date range.
        /// Ref: https://developers.themoviedb.org/3/movies/get-upcoming
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ListResultModel<MovieLookupDto>> GetUpComingMoviesAsync(int page,
            CancellationToken cancellationToken = default)
        {
            var searchResult = await _client.GetMovieUpcomingListAsync(_movieDbOptions.Language, page,
                _movieDbOptions.Region,
                cancellationToken);

            return _mapper.Map<ListResultModel<MovieLookupDto>>(searchResult);
        }

        /// <summary>
        /// Get the top rated movies on TMDB.
        ///  Ref: https://developers.themoviedb.org/3/movies/get-top-rated-movies
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ListResultModel<MovieLookupDto>> GetTopRatedMoviesAsync(int page,
            CancellationToken cancellationToken = default)
        {
            var searchResult = await _client.GetMovieTopRatedListAsync(_movieDbOptions.Language, page,
                _movieDbOptions.Region,
                cancellationToken);

            return _mapper.Map<ListResultModel<MovieLookupDto>>(searchResult);
        }

        /// <summary>
        /// Discover movies by different types of data like average rating, number of votes, genres and certifications
        /// Ref: https://developers.themoviedb.org/3/discover/movie-discover, https://www.themoviedb.org/documentation/api/discover
        /// </summary>
        /// <returns></returns>
        private DiscoverMovie GetDiscoverMovies()
        {
            return _client.DiscoverMoviesAsync();
        }

        /// <summary>
        /// Discover TV shows by different types of data like average rating, number of votes, genres, the network they aired on and air dates.
        /// Ref: https://developers.themoviedb.org/3/discover/tv-discover
        /// </summary>
        /// <returns></returns>
        private DiscoverTv GetDiscoverTvShows()
        {
            return _client.DiscoverTvShowsAsync();
        }

        /// <summary>
        /// Get the list of official genres for movies.
        /// Ref: https://developers.themoviedb.org/3/genres/get-movie-list
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Genre>> GetMovieGenresAsync(CancellationToken cancellationToken = default)
        {
            var result = await _client.GetMovieGenresAsync(_movieDbOptions.Language, cancellationToken);

            return _mapper.Map<IEnumerable<Genre>>(result);
        }

        public async Task<ListResultModel<MovieLookupDto>> GetMoviesByGenreAsync(IReadOnlyList<int> genreIds,
            int page = 0, CancellationToken cancellationToken = default)
        {
            var searchResult = await GetDiscoverMovies().IncludeWithAllOfGenre(genreIds).Query(page, cancellationToken);

            return _mapper.Map<ListResultModel<MovieLookupDto>>(searchResult);
        }

        public async Task<ListResultModel<TvShowLookupDto>> GetTvShowsByGenreAsync(IReadOnlyList<int> genreIds,
            int page = 0, CancellationToken cancellationToken = default)
        {
            var searchResult = await GetDiscoverTvShows().WhereGenresInclude(genreIds).Query(page, cancellationToken);

            return _mapper.Map<ListResultModel<TvShowLookupDto>>(searchResult);
        }

        /// <summary>
        /// Get the list of official genres for TV shows.
        /// Ref: https://developers.themoviedb.org/3/genres/get-tv-list
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Genre>> GetTvShowGenresAsync(CancellationToken cancellationToken = default)
        {
            var result = await _client.GetTvGenresAsync(_movieDbOptions.Language, cancellationToken);

            return _mapper.Map<IEnumerable<Genre>>(result);
        }

        /// <summary>
        /// Get the primary information about a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Movie> GetMovieByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _client.GetMovieAsync(id, cancellationToken: cancellationToken,
                language: _movieDbOptions.Language);

            return _mapper.Map<Movie>(result);
        }

        /// <summary>
        /// Get the primary information about a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-details
        /// </summary>
        /// <param name="imdbId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Movie> GetMovieByImdbIdAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            var result = await _client.GetMovieAsync(imdbId: imdbId, cancellationToken: cancellationToken,
                language: _movieDbOptions.Language);

            return _mapper.Map<Movie>(result);
        }

        /// <summary>
        /// Get the primary TV show details by id.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TvShow> GetTvShowByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _client.GetTvShowAsync(id, cancellationToken: cancellationToken,
                language: _movieDbOptions.Language);

            return _mapper.Map<TvShow>(result);
        }

        /// <summary>
        /// Get the user reviews for a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-reviews
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ListResultModel<ReviewLookupDto>> GetMovieReviewsAsync(int movieId, int page = 0,
            CancellationToken cancellationToken = default)
        {
            var result = await _client.GetMovieReviewsAsync(movieId, cancellationToken: cancellationToken,
                language: _movieDbOptions.Language, page: page);

            return _mapper.Map<ListResultModel<ReviewLookupDto>>(result);
        }

        /// <summary>
        /// Get the reviews for a TV show.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-reviews
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ListResultModel<ReviewLookupDto>> GetTvShowReviewsAsync(int tvShowId, int page = 0,
            CancellationToken cancellationToken = default)
        {
            var result = await _client.GetTvShowReviewsAsync(tvShowId, cancellationToken: cancellationToken,
                language: _movieDbOptions.Language, page: page);

            return _mapper.Map<ListResultModel<ReviewLookupDto>>(result);
        }

        /// <summary>
        /// Retrieve the details of a movie or TV show review.
        /// Ref: https://developers.themoviedb.org/3/reviews/get-review-details
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Core.Aggregates.Reviews.Review> GetReviewsAsync(string reviewId,
            CancellationToken cancellationToken = default)
        {
            var result = await _client.GetReviewAsync(reviewId, cancellationToken);

            return _mapper.Map<Core.Aggregates.Reviews.Review>(result);
        }

        /// <summary>
        /// Get the cast and crew for a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-credits
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetMovieCreditsAsync(int movieId, CancellationToken cancellationToken = default)
        {
            return await _client.GetMovieCreditsAsync(movieId, cancellationToken);
        }

        /// <summary>
        /// Get the credits (cast and crew) that have been added to a TV show.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-credits
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetTvShowCreditsAsync(int tvShowId, CancellationToken cancellationToken = default)
        {
            return await _client.GetTvShowCreditsAsync(tvShowId, _movieDbOptions.Language, cancellationToken);
        }

        /// <summary>
        /// Get the images that belong to a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-images
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetMovieBackdropsImagesAsync(int movieId,
            CancellationToken cancellationToken = default)
        {
            return await _client.GetMovieImagesAsync(movieId, cancellationToken);
        }

        /// <summary>
        /// Get the images that belong to a TV show.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-images
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetTvShowBackdropsImagesAsync(int tvShowId,
            CancellationToken cancellationToken = default)
        {
            return await _client.GetTvShowImagesAsync(tvShowId, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Get the videos that have been added to a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-videos
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetMovieVideosAsync(int movieId, CancellationToken cancellationToken = default)
        {
            return await _client.GetMovieVideosAsync(movieId, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Get the videos that have been added to a TV show.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-videos
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetTvSHowVideosAsync(int tvShowId, CancellationToken cancellationToken = default)
        {
            return await _client.GetTvShowVideosAsync(tvShowId, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Get a list of recommended movies for a movie.
        /// Ref: https://developers.themoviedb.org/3/movies/get-movie-recommendations
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetRecommendMoviesAsync(int movieId, CancellationToken cancellationToken = default)
        {
            return await _client.GetMovieRecommendationsAsync(movieId, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Get the list of TV show recommendations for this item.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-recommendations
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetRecommendTvShowAsync(int tvShowId, CancellationToken cancellationToken = default)
        {
            return await _client.GetTvShowRecommendationsAsync(tvShowId, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Get the primary person details by id.
        /// Ref: https://developers.themoviedb.org/3/people/get-person-details
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetPersonDetailAsync(int personId, CancellationToken cancellationToken = default)
        {
            return await _client.GetPersonAsync(personId, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Get the external ids for a person. We currently support the following external sources.
        /// Ref: https://developers.themoviedb.org/3/people/get-person-external-ids
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetPersonExternalDataAsync(int personId,
            CancellationToken cancellationToken = default)
        {
            return await _client.GetPersonExternalIdsAsync(personId, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Get the movie credits for a person.
        /// Ref: https://developers.themoviedb.org/3/people/get-person-movie-credits
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetPersonMovieCastAsync(int personId, CancellationToken cancellationToken = default)
        {
            return await _client.GetPersonMovieCreditsAsync(personId, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Get the TV show credits for a person.
        /// Ref: https://developers.themoviedb.org/3/people/get-person-tv-credits
        /// </summary>
        /// <param name="tvShowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetPersonTvShowCastAsync(int tvShowId, CancellationToken cancellationToken = default)
        {
            return await _client.GetPersonTvCreditsAsync(tvShowId, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Get a list of shows that are currently on the air. This query looks for any TV show that has an episode with an air date in the next 7 days.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-on-the-air
        /// </summary>
        /// <param name="page"></param>
        /// <param name="timeZone"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetTvShowOnTheAirAsync(int page = 0,
            string timeZone = null, CancellationToken cancellationToken = default)
        {
            return await _client.GetTvShowListAsync(TvShowListType.OnTheAir, page, timeZone, cancellationToken);
        }

        /// <summary>
        /// Get a list of TV shows that are airing today. This query is purely day based as we do not currently support airing times.
        /// Ref: https://developers.themoviedb.org/3/tv/get-tv-airing-today
        /// </summary>
        /// <param name="page"></param>
        /// <param name="timeZone"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetTvShowAiringTodayAsync(int page = 0,
            string timeZone = null, CancellationToken cancellationToken = default)
        {
            return await _client.GetTvShowListAsync(TvShowListType.AiringToday, page, timeZone, cancellationToken);
        }

        /// <summary>
        /// Get a list of the current popular TV shows on TMDB. This list updates daily.
        /// Ref: https://developers.themoviedb.org/3/tv/get-popular-tv-shows
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetPopularTvShow(int page = 0, CancellationToken cancellationToken = default)
        {
            return await _client.GetTvShowPopularAsync(page, _movieDbOptions.Language, cancellationToken);
        }

        /// <summary>
        /// Get a list of the top rated TV shows on TMDB.
        /// Ref: https://developers.themoviedb.org/3/tv/get-top-rated-tv
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<dynamic> GetTvShowTopRatedAsync(int page = 0, CancellationToken cancellationToken = default)
        {
            return await _client.GetTvShowTopRatedAsync(page, _movieDbOptions.Language, cancellationToken);
        }

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
        public async Task<dynamic> SearchTvShowsAsync(string keyword,
            int page = 0,
            bool includeAdult = false,
            int firstAirDateYear = 0,
            CancellationToken cancellationToken = default)
        {
            return await _client.SearchTvShowAsync(query: keyword, page: page, includeAdult: includeAdult,
                cancellationToken: cancellationToken, language: _movieDbOptions.Language,
                firstAirDateYear: firstAirDateYear);
        }

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
        public async Task<dynamic> SearchMultiAsync(string keyword, int page = 0, bool includeAdult = false,
            int year = 0,
            CancellationToken cancellationToken = default)
        {
            return await _client.SearchMultiAsync(keyword, page, includeAdult, year, region: _movieDbOptions.Region,
                cancellationToken);
        }
    }
}