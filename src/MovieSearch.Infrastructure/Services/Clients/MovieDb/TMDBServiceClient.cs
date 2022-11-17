using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BuildingBlocks.Domain;
using BuildingBlocks.Resiliency.Configs;
using Microsoft.Extensions.Options;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Core;
using MovieSearch.Core.Generals;
using MovieSearch.Core.Genres;
using MovieSearch.Core.Movies;
using MovieSearch.Core.People;
using MovieSearch.Core.Review;
using MovieSearch.Core.TV;
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using TMDbLib.Client;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.TvShows;

namespace MovieSearch.Infrastructure.Services.Clients.MovieDb;

// https://www.themoviedb.org/
// https://medium.com/@emanuele.bucarelli/improve-resilience-in-the-net-application-80adda2c7710
// https://procodeguide.com/programming/polly-in-aspnet-core
public class TMDBServiceClient : IMovieDbServiceClient
{
    private static AsyncTimeoutPolicy _timeoutPolicy;
    private static AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
    private static AsyncBulkheadPolicy _bulkheadPolicy;
    private readonly TMDbClient _client;
    private readonly IMapper _mapper;
    private readonly TMDBOptions _movieDbOptions;

    private readonly AsyncRetryPolicy _retryPolicy;

    public TMDBServiceClient(IOptions<TMDBOptions> options, IMapper mapper, IOptions<PolicyConfig> policyOptions)
    {
        _mapper = mapper;
        _movieDbOptions = options.Value;
        _client = new TMDbClient(_movieDbOptions.ApiKey);

        _retryPolicy = Policy.Handle<Exception>().RetryAsync(policyOptions.Value.RetryCount);
        _timeoutPolicy = Policy.TimeoutAsync(policyOptions.Value.TimeOutDuration, TimeoutStrategy.Pessimistic);
        _circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreakerAsync(policyOptions.Value.RetryCount + 1,
            TimeSpan.FromSeconds(policyOptions.Value.BreakDuration));
        //at any given time there will 3 parallel requests execution for specific service call and another 6 requests for other services can be in the queue. So that if the response from customer service is delayed or blocked then we don’t use too many resources
        _bulkheadPolicy = Policy.BulkheadAsync(3, 6);

        _retryPolicy.WrapAsync(_circuitBreakerPolicy).WrapAsync(_timeoutPolicy);
    }

    /// <summary>
    ///     Get a list of movies in theatres. This is a release type query that looks for all movies that have a release type
    ///     of 2 or 3 within the specified date range.
    ///     Ref: https://developers.themoviedb.org/3/movies/get-now-playing
    /// </summary>
    /// <param name="page"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<MovieInfo>> GetNowPlayingAsync(int page = 1,
        CancellationToken cancellationToken = default)
    {
        var searchResult = await _client.GetMovieNowPlayingListAsync(_movieDbOptions.Language, page,
            _movieDbOptions.Region, cancellationToken);

        return _mapper.Map<ListResultModel<MovieInfo>>(searchResult);
    }

    /// <summary>
    ///     Search for movies.
    ///     Ref: https://developers.themoviedb.org/3/search/search-movies
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="page"></param>
    /// <param name="includeAdult"></param>
    /// <param name="year"></param>
    /// <param name="primaryReleaseYear"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<MovieInfo>> SearchMovieAsync(string keyword,
        int page = 1,
        bool includeAdult = false,
        int year = 0,
        int primaryReleaseYear = 0,
        CancellationToken cancellationToken = default)
    {
        var searchResult = await _retryPolicy.ExecuteAsync(() => _client.SearchMovieAsync(keyword, page,
            includeAdult, year,
            _movieDbOptions.Region,
            primaryReleaseYear, cancellationToken));

        return _mapper.Map<ListResultModel<MovieInfo>>(searchResult);
    }

    /// <summary>
    ///     Get a list of the current popular movies on TMDB. This list updates daily.
    ///     Ref: https://developers.themoviedb.org/3/movies/get-popular-movies
    /// </summary>
    /// <param name="page"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<MovieInfo>> GetPopularMoviesAsync(int page = 1,
        CancellationToken cancellationToken = default)
    {
        var searchResult = await _retryPolicy.ExecuteAsync(() => _client.GetMoviePopularListAsync(
            _movieDbOptions.Language, page,
            _movieDbOptions.Region,
            cancellationToken));

        return _mapper.Map<ListResultModel<MovieInfo>>(searchResult);
    }

    /// <summary>
    ///     Get the most newly created movie. This is a live response and will continuously change.
    ///     Ref: https://developers.themoviedb.org/3/movies/get-latest-movie
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Movie> GetLatestMovieAsync(
        CancellationToken cancellationToken = default)
    {
        var searchResult = await _retryPolicy.ExecuteAsync(() => _client.GetMovieLatestAsync(cancellationToken));

        return _mapper.Map<Movie>(searchResult);
    }

    /// <summary>
    ///     Get a list of upcoming movies in theatres. This is a release type query that looks for all movies that have a
    ///     release type of 2 or 3 within the specified date range.
    ///     Ref: https://developers.themoviedb.org/3/movies/get-upcoming
    /// </summary>
    /// <param name="page"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<MovieInfo>> GetUpComingMoviesAsync(int page = 1,
        CancellationToken cancellationToken = default)
    {
        var searchResult = await _retryPolicy.ExecuteAsync(() => _client.GetMovieUpcomingListAsync(
            _movieDbOptions.Language, page,
            _movieDbOptions.Region,
            cancellationToken));

        return _mapper.Map<ListResultModel<MovieInfo>>(searchResult);
    }

    /// <summary>
    ///     Get the top rated movies on TMDB.
    ///     Ref: https://developers.themoviedb.org/3/movies/get-top-rated-movies
    /// </summary>
    /// <param name="page"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<MovieInfo>> GetTopRatedMoviesAsync(int page = 1,
        CancellationToken cancellationToken = default)
    {
        var searchResult = await _retryPolicy.ExecuteAsync(() => _client.GetMovieTopRatedListAsync(
            _movieDbOptions.Language, page,
            _movieDbOptions.Region,
            cancellationToken));

        return _mapper.Map<ListResultModel<MovieInfo>>(searchResult);
    }

    /// <summary>
    ///     Get the list of official genres for movies.
    ///     Ref: https://developers.themoviedb.org/3/genres/get-movie-list
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Genre>> GetMovieGenresAsync(CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() =>
            _client.GetMovieGenresAsync(_movieDbOptions.Language, cancellationToken));

        return _mapper.Map<IEnumerable<Genre>>(result);
    }

    public async Task<ListResultModel<MovieInfo>> FindMoviesByGenreAsync(IReadOnlyList<int> genreIds,
        int page = 1, CancellationToken cancellationToken = default)
    {
        var searchResult = await _retryPolicy.ExecuteAsync(() =>
            GetDiscoverMovies().IncludeWithAllOfGenre(genreIds).Query(page, cancellationToken));

        return _mapper.Map<ListResultModel<MovieInfo>>(searchResult);
    }

    public async Task<ListResultModel<TVShowInfo>> FindTvShowsByGenreAsync(IReadOnlyList<int> genreIds,
        int page = 1, CancellationToken cancellationToken = default)
    {
        var searchResult = await _retryPolicy.ExecuteAsync(() =>
            GetDiscoverTvShows().WhereGenresInclude(genreIds).Query(page, cancellationToken));

        return _mapper.Map<ListResultModel<TVShowInfo>>(searchResult);
    }

    /// <summary>
    ///     Get the list of official genres for TV shows.
    ///     Ref: https://developers.themoviedb.org/3/genres/get-tv-list
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Genre>> GetTvShowGenresAsync(CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() =>
            _client.GetTvGenresAsync(_movieDbOptions.Language, cancellationToken));

        return _mapper.Map<IEnumerable<Genre>>(result);
    }

    /// <summary>
    ///     Get the primary information about a movie.
    ///     Ref: https://developers.themoviedb.org/3/movies/get-movie-details
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Movie> GetMovieByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() => _client.GetMovieAsync(id,
            cancellationToken: cancellationToken,
            language: _movieDbOptions.Language));

        return _mapper.Map<Movie>(result);
    }

    /// <summary>
    ///     Get the primary information about a movie.
    ///     Ref: https://developers.themoviedb.org/3/movies/get-movie-details
    /// </summary>
    /// <param name="imdbId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Movie> GetMovieByImdbIdAsync(string imdbId, CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() => _client.GetMovieAsync(imdbId,
            cancellationToken: cancellationToken,
            language: _movieDbOptions.Language));

        return _mapper.Map<Movie>(result);
    }

    /// <summary>
    ///     Get the primary TV show details by id.
    ///     Ref: https://developers.themoviedb.org/3/tv/get-tv-details
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TVShow> GetTvShowByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() => _client.GetTvShowAsync(id,
            cancellationToken: cancellationToken,
            language: _movieDbOptions.Language));

        return _mapper.Map<TVShow>(result);
    }

    /// <summary>
    ///     Get the user reviews for a movie.
    ///     Ref: https://developers.themoviedb.org/3/movies/get-movie-reviews
    /// </summary>
    /// <param name="movieId"></param>
    /// <param name="page"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<ReviewInfo>> GetMovieReviewsAsync(int movieId, int page = 1,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() => _client.GetMovieReviewsAsync(movieId,
            cancellationToken: cancellationToken,
            language: _movieDbOptions.Language, page: page));

        return _mapper.Map<ListResultModel<ReviewInfo>>(result);
    }

    /// <summary>
    ///     Get the reviews for a TV show.
    ///     Ref: https://developers.themoviedb.org/3/tv/get-tv-reviews
    /// </summary>
    /// <param name="tvShowId"></param>
    /// <param name="page"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<ReviewInfo>> GetTvShowReviewsAsync(int tvShowId, int page = 1,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() => _client.GetTvShowReviewsAsync(tvShowId,
            cancellationToken: cancellationToken,
            language: _movieDbOptions.Language, page: page));

        return _mapper.Map<ListResultModel<ReviewInfo>>(result);
    }

    /// <summary>
    ///     Retrieve the details of a movie or TV show review.
    ///     Ref: https://developers.themoviedb.org/3/reviews/get-review-details
    /// </summary>
    /// <param name="reviewId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Review> GetReviewsAsync(string reviewId, CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() => _client.GetReviewAsync(reviewId, cancellationToken));

        return _mapper.Map<Review>(result);
    }

    /// <summary>
    ///     Get the cast and crew for a movie.
    ///     Ref: https://developers.themoviedb.org/3/movies/get-movie-credits
    /// </summary>
    /// <param name="movieId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<MovieCredit> GetMovieCreditsAsync(int movieId, CancellationToken cancellationToken = default)
    {
        var result =
            await _retryPolicy.ExecuteAsync(() => _client.GetMovieCreditsAsync(movieId, cancellationToken));

        return _mapper.Map<MovieCredit>(result);
    }

    /// <summary>
    ///     Get the movie credits for a person.
    ///     Ref: https://developers.themoviedb.org/3/people/get-person-movie-credits
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PersonMovieCredit> GetPersonMovieCreditsAsync(int personId,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(
            () => _client.GetPersonMovieCreditsAsync(personId, _movieDbOptions.Language, cancellationToken));

        return _mapper.Map<PersonMovieCredit>(result);
    }

    /// <summary>
    ///     Get the TV show credits for a person.
    ///     Ref: https://developers.themoviedb.org/3/people/get-person-tv-credits
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PersonTVCredit> GetPersonTvShowCreditsAsync(int personId,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() =>
            _client.GetPersonTvCreditsAsync(personId, _movieDbOptions.Language, cancellationToken));

        return _mapper.Map<PersonTVCredit>(result);
    }

    /// <summary>
    ///     Get the credits (cast and crew) that have been added to a TV show.
    ///     Ref: https://developers.themoviedb.org/3/tv/get-tv-credits
    /// </summary>
    /// <param name="tvShowId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TVShowCredit> GetTvShowCreditsAsync(int tvShowId,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() =>
            _client.GetTvShowCreditsAsync(tvShowId, _movieDbOptions.Language, cancellationToken));

        return _mapper.Map<TVShowCredit>(result);
    }

    /// <summary>
    ///     Get the images that belong to a movie.
    ///     Ref: https://developers.themoviedb.org/3/movies/get-movie-images
    /// </summary>
    /// <param name="movieId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Images> GetMovieImagesAsync(int movieId, CancellationToken cancellationToken = default)
    {
        var image = await _retryPolicy.ExecuteAsync(() => _client.GetMovieImagesAsync(movieId, cancellationToken));

        return _mapper.Map<Images>(image);
    }

    /// <summary>
    ///     Get the images that belong to a TV show.
    ///     Ref: https://developers.themoviedb.org/3/tv/get-tv-images
    /// </summary>
    /// <param name="tvShowId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Images> GetTvShowImagesAsync(int tvShowId, CancellationToken cancellationToken = default)
    {
        var image = await _retryPolicy.ExecuteAsync(() =>
            _client.GetTvShowImagesAsync(tvShowId, cancellationToken: cancellationToken));

        return _mapper.Map<Images>(image);
    }

    /// <summary>
    ///     Get the videos that have been added to a movie.
    ///     Ref: https://developers.themoviedb.org/3/movies/get-movie-videos
    /// </summary>
    /// <param name="movieId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(List<Core.Generals.Video> Videos, int MovieId)> GetMovieVideosAsync(int movieId,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() =>
            _client.GetMovieVideosAsync(movieId, cancellationToken));

        return (Videos: _mapper.Map<List<Core.Generals.Video>>(result.Results), MovieId: result.Id);
    }

    /// <summary>
    ///     Get the videos that have been added to a TV show.
    ///     Ref: https://developers.themoviedb.org/3/tv/get-tv-videos
    /// </summary>
    /// <param name="tvShowId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(List<Core.Generals.Video> Videos, int TvShowId)> GetTvShowVideosAsync(int tvShowId,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() =>
            _client.GetTvShowVideosAsync(tvShowId, cancellationToken));

        return (Videos: _mapper.Map<List<Core.Generals.Video>>(result.Results), TvShowId: result.Id);
    }

    /// <summary>
    ///     Get a list of recommended movies for a movie.
    ///     Ref: https://developers.themoviedb.org/3/movies/get-movie-recommendations
    /// </summary>
    /// <param name="movieId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<MovieInfo>> GetRecommendMoviesAsync(int movieId,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() =>
            _client.GetMovieRecommendationsAsync(movieId, cancellationToken: cancellationToken));

        return _mapper.Map<ListResultModel<MovieInfo>>(result);
    }

    /// <summary>
    ///     Get the list of TV show recommendations for this item.
    ///     Ref: https://developers.themoviedb.org/3/tv/get-tv-recommendations
    /// </summary>
    /// <param name="tvShowId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<TVShowInfo>> GetRecommendTvShowAsync(int tvShowId,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() =>
            _client.GetTvShowRecommendationsAsync(tvShowId, cancellationToken: cancellationToken));

        return _mapper.Map<ListResultModel<TVShowInfo>>(result);
    }

    /// <summary>
    ///     Get the primary person details by id.
    ///     Ref: https://developers.themoviedb.org/3/people/get-person-details
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Person> GetPersonDetailAsync(int personId, CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() =>
            _client.GetPersonAsync(personId, cancellationToken: cancellationToken));

        return _mapper.Map<Person>(result);
    }

    /// <summary>
    ///     Get the external ids for a person. We currently support the following external sources.
    ///     Ref: https://developers.themoviedb.org/3/people/get-person-external-ids
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PersonExternalIds> GetPersonExternalDataAsync(int personId,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() =>
            _client.GetPersonExternalIdsAsync(personId, cancellationToken));

        return _mapper.Map<PersonExternalIds>(result);
    }

    /// <summary>
    ///     Get a list of shows that are currently on the air. This query looks for any TV show that has an episode with an air
    ///     date in the next 7 days.
    ///     Ref: https://developers.themoviedb.org/3/tv/get-tv-on-the-air
    /// </summary>
    /// <param name="page"></param>
    /// <param name="timeZone"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<TVShowInfo>> GetTvShowOnTheAirAsync(int page = 1, string timeZone = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() =>
            _client.GetTvShowListAsync(TvShowListType.OnTheAir, page, timeZone, cancellationToken));

        return _mapper.Map<ListResultModel<TVShowInfo>>(result);
    }

    /// <summary>
    ///     Get a list of TV shows that are airing today. This query is purely day based as we do not currently support airing
    ///     times.
    ///     Ref: https://developers.themoviedb.org/3/tv/get-tv-airing-today
    /// </summary>
    /// <param name="page"></param>
    /// <param name="timeZone"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<TVShowInfo>> GetTvShowAiringTodayAsync(int page = 1,
        string timeZone = null, CancellationToken cancellationToken = default)
    {
        var result =
            await _retryPolicy.ExecuteAsync(() =>
                _client.GetTvShowListAsync(TvShowListType.AiringToday, page, timeZone, cancellationToken));

        return _mapper.Map<ListResultModel<TVShowInfo>>(result);
    }

    /// <summary>
    ///     Get a list of the current popular TV shows on TMDB. This list updates daily.
    ///     Ref: https://developers.themoviedb.org/3/tv/get-popular-tv-shows
    /// </summary>
    /// <param name="page"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<TVShowInfo>> GetPopularTvShowAsync(int page = 1,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() =>
            _client.GetTvShowPopularAsync(page, _movieDbOptions.Language, cancellationToken));

        return _mapper.Map<ListResultModel<TVShowInfo>>(result);
    }

    /// <summary>
    ///     Get a list of the top rated TV shows on TMDB.
    ///     Ref: https://developers.themoviedb.org/3/tv/get-top-rated-tv
    /// </summary>
    /// <param name="page"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<TVShowInfo>> GetTvShowTopRatedAsync(int page = 1,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() =>
            _client.GetTvShowTopRatedAsync(page, _movieDbOptions.Language, cancellationToken));

        return _mapper.Map<ListResultModel<TVShowInfo>>(result);
    }

    /// <summary>
    ///     Search for a TV show.
    ///     Ref: https://developers.themoviedb.org/3/search/search-tv-shows
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="page"></param>
    /// <param name="includeAdult"></param>
    /// <param name="firstAirDateYear"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<TVShowInfo>> SearchTvShowAsync(string keyword,
        int page = 1,
        bool includeAdult = false,
        int firstAirDateYear = 0,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() => _client.SearchTvShowAsync(keyword, page: page,
            includeAdult: includeAdult,
            cancellationToken: cancellationToken, language: _movieDbOptions.Language,
            firstAirDateYear: firstAirDateYear));

        return _mapper.Map<ListResultModel<TVShowInfo>>(result);
    }

    /// <summary>
    ///     Search multiple models in a single request. Multi search currently supports searching for movies, tv shows and
    ///     people in a single request.
    ///     Ref: https://developers.themoviedb.org/3/search/multi-search
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="page"></param>
    /// <param name="includeAdult"></param>
    /// <param name="year"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ListResultModel<dynamic>> SearchMultiAsync(string keyword, int page = 1,
        bool includeAdult = false,
        int year = 0,
        CancellationToken cancellationToken = default)
    {
        var result = await _retryPolicy.ExecuteAsync(() => _client.SearchMultiAsync(keyword, page, includeAdult,
            year,
            _movieDbOptions.Region, cancellationToken));

        return _mapper.Map<ListResultModel<dynamic>>(result);
    }

    /// <summary>
    ///     Discover movies by different types of data like average rating, number of votes, genres and certifications
    ///     Ref: https://developers.themoviedb.org/3/discover/movie-discover,
    ///     https://www.themoviedb.org/documentation/api/discover
    /// </summary>
    /// <returns></returns>
    private DiscoverMovie GetDiscoverMovies()
    {
        return _client.DiscoverMoviesAsync();
    }

    /// <summary>
    ///     Discover TV shows by different types of data like average rating, number of votes, genres, the network they aired
    ///     on and air dates.
    ///     Ref: https://developers.themoviedb.org/3/discover/tv-discover
    /// </summary>
    /// <returns></returns>
    private DiscoverTv GetDiscoverTvShows()
    {
        return _client.DiscoverTvShowsAsync();
    }
}