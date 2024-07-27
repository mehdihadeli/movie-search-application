using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BuildingBlocks.Domain;
using BuildingBlocks.Resiliency.Configs;
using BuildingBlocks.Test.Fixtures;
using DM.MovieApi.IntegrationTests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MovieSearch.Api;
using MovieSearch.Core;
using MovieSearch.Core.Genres;
using MovieSearch.Core.Movies;
using MovieSearch.Core.Review;
using MovieSearch.Core.TV;
using MovieSearch.Infrastructure.Services.Clients.MovieDb;
using MovieSearch.IntegrationTests.Mocks;
using Xunit;

namespace MovieSearch.IntegrationTests.Infrastructure.Services.Clients.MovieDb;

public class TMDBServiceClientTests : IntegrationTestFixture<Program>
{
    private readonly TMDBServiceClient _sut;

    public TMDBServiceClientTests()
    {
        //setup the swaps for our tests
        RegisterTestServices(services => { });

        _sut = new TMDBServiceClient(
            ServiceProvider.GetRequiredService<IOptions<TMDBOptions>>(),
            ServiceProvider.GetRequiredService<IMapper>(),
            ServiceProvider.GetRequiredService<IOptions<PolicyConfig>>()
        );
    }

    [Fact]
    public async Task get_now_playing_should_returns_valid_data()
    {
        //Act
        var result = await _sut.GetNowPlayingAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ListResultModel<MovieInfo>>();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(result.Items.Count);
        TMDBTestUtil.AssertMovieInformationStructure(result.Items);
    }

    [Fact]
    public async Task search_movie_by_title_should_return_valid_data()
    {
        //Act
        var result = await _sut.SearchMovieAsync(MovieMocks.Data.Title);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ListResultModel<MovieInfo>>();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(result.Items.Count);
        result.Items.Any().Should().BeTrue();
        TMDBTestUtil.AssertMovieInformationStructure(result.Items);
    }

    [Fact]
    public async Task get_popular_movies_should_returns_valid_data()
    {
        //Act
        var result = await _sut.GetPopularMoviesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ListResultModel<MovieInfo>>();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(result.Items.Count);
        TMDBTestUtil.AssertMovieInformationStructure(result.Items);
    }

    [Fact]
    public async Task get_latest_movie_should_returns_valid_data()
    {
        //Act
        var result = await _sut.GetLatestMovieAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Movie>();
    }

    [Fact]
    public async Task get_up_coming_movies_should_returns_valid_data()
    {
        //Act
        var result = await _sut.GetUpComingMoviesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ListResultModel<MovieInfo>>();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(result.Items.Count);
        TMDBTestUtil.AssertMovieInformationStructure(result.Items);
    }

    [Fact]
    public async Task get_top_rated_movies_should_returns_valid_data()
    {
        //Act
        var result = await _sut.GetTopRatedMoviesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ListResultModel<MovieInfo>>();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(result.Items.Count);
        TMDBTestUtil.AssertMovieInformationStructure(result.Items);
    }

    [Fact]
    public async Task get_movie_genres_should_returns_valid_data()
    {
        //Act
        var result = (await _sut.GetMovieGenresAsync()).ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<Genre>>();
        result.Any().Should().Be(true);
    }

    [Fact]
    public async Task find_movies_by_genre_should_returns_valid_data()
    {
        var genreId = GenreFactory.Comedy().Id;

        //Act
        var result = await _sut.FindMoviesByGenreAsync(new List<int>(genreId));

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ListResultModel<MovieInfo>>();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(result.Items.Count);
        TMDBTestUtil.AssertMovieInformationStructure(result.Items);
    }

    [Fact]
    public async Task find_tv_shows_by_genre_should_returns_valid_data()
    {
        var genreId = GenreFactory.Comedy().Id;

        //Act
        var result = await _sut.FindTvShowsByGenreAsync(new List<int>(genreId));

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ListResultModel<TVShowInfo>>();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(result.Items.Count);
        TMDBTestUtil.AssertTvShowInformationStructure(result.Items);
    }

    [Fact]
    public async Task get_tv_show_genres_should_returns_valid_data()
    {
        //Act
        var result = (await _sut.GetTvShowGenresAsync()).ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<Genre>>();
    }

    [Fact]
    public async Task get_movie_by_id_should_returns_valid_data()
    {
        //Act
        var movie = await _sut.GetMovieByIdAsync(MovieMocks.Data.Id);

        // Assert
        movie.Id.Should().Be(MovieMocks.Data.Id);
        movie.ImdbId.Should().Be(MovieMocks.Data.ImdbId);
        movie.Title.Should().Be(MovieMocks.Data.Title);
        movie.OriginalTitle.Should().Be(MovieMocks.Data.OriginalTitle);
        movie.Tagline.Should().Be(MovieMocks.Data.Tagline);
        movie.OriginalLanguage.Should().Be(MovieMocks.Data.OriginalLanguage);
        movie.Homepage.Should().Be(MovieMocks.Data.Homepage);
        movie.Status.Should().Be(MovieMocks.Data.Status);
        movie.Budget.Should().Be(MovieMocks.Data.Budget);
        movie.Runtime.Should().Be(MovieMocks.Data.Runtime);
        movie.ReleaseDate.Should().Be(MovieMocks.Data.ReleaseDate);
        movie.Overview.StartsWith(MovieMocks.Data.Overview).Should().BeTrue();
        (movie.Popularity > 7).Should().BeTrue();
        (movie.VoteAverage > 5).Should().BeTrue();
        (movie.VoteCount > 1500).Should().BeTrue();
        TMDBTestUtil.AssertImagePath(movie.BackdropPath);
        TMDBTestUtil.AssertImagePath(movie.PosterPath);

        MovieMocks.Data.SpokenLanguages.Should().BeEquivalentTo(movie.SpokenLanguages);
        MovieMocks.Data.ProductionCompanies.Should().BeEquivalentTo(movie.ProductionCompanies);
        MovieMocks.Data.ProductionCountries.Should().BeEquivalentTo(movie.ProductionCountries);

        movie.MovieCollectionInfo.Should().NotBeNull();
        movie.MovieCollectionInfo.Id.Should().Be(10);
        movie.MovieCollectionInfo.Name.Should().Be("Star Wars Collection");
        TMDBTestUtil.AssertImagePath(movie.MovieCollectionInfo.BackdropPath);
        TMDBTestUtil.AssertImagePath(movie.MovieCollectionInfo.PosterPath);

        MovieMocks.Data.Genres.Should().BeEquivalentTo(movie.Genres);
        MovieMocks.Data.Keywords.Should().BeEquivalentTo(movie.Keywords);
    }

    [Fact]
    public async Task get_movie_by_imdbid_should_returns_valid_data()
    {
        //Act
        var movie = await _sut.GetMovieByImdbIdAsync(MovieMocks.Data.ImdbId);

        // Assert
        movie.Should().NotBeNull();
        movie.Id.Should().Be(MovieMocks.Data.Id);
        movie.ImdbId.Should().Be(MovieMocks.Data.ImdbId);
        movie.Title.Should().Be(MovieMocks.Data.Title);
        movie.OriginalTitle.Should().Be(MovieMocks.Data.OriginalTitle);
        movie.Tagline.Should().Be(MovieMocks.Data.Tagline);
        movie.OriginalLanguage.Should().Be(MovieMocks.Data.OriginalLanguage);
        movie.Homepage.Should().Be(MovieMocks.Data.Homepage);
        movie.Status.Should().Be(MovieMocks.Data.Status);
        movie.Budget.Should().Be(MovieMocks.Data.Budget);
        movie.Runtime.Should().Be(MovieMocks.Data.Runtime);
        movie.ReleaseDate.Should().Be(MovieMocks.Data.ReleaseDate);
        movie.Overview.StartsWith(MovieMocks.Data.Overview).Should().BeTrue();
        (movie.Popularity > 7).Should().BeTrue();
        (movie.VoteAverage > 5).Should().BeTrue();
        (movie.VoteCount > 1500).Should().BeTrue();
        TMDBTestUtil.AssertImagePath(movie.BackdropPath);
        TMDBTestUtil.AssertImagePath(movie.PosterPath);

        MovieMocks.Data.SpokenLanguages.Should().BeEquivalentTo(movie.SpokenLanguages);
        MovieMocks.Data.ProductionCompanies.Should().BeEquivalentTo(movie.ProductionCompanies);
        MovieMocks.Data.ProductionCountries.Should().BeEquivalentTo(movie.ProductionCountries);

        movie.MovieCollectionInfo.Should().NotBeNull();
        movie.MovieCollectionInfo.Id.Should().Be(10);
        movie.MovieCollectionInfo.Name.Should().Be("Star Wars Collection");
        TMDBTestUtil.AssertImagePath(movie.MovieCollectionInfo.BackdropPath);
        TMDBTestUtil.AssertImagePath(movie.MovieCollectionInfo.PosterPath);

        MovieMocks.Data.Genres.Should().BeEquivalentTo(movie.Genres);
        MovieMocks.Data.Keywords.Should().BeEquivalentTo(movie.Keywords);
    }

    [Fact]
    public async Task get_tv_show_by_id_should_returns_valid_data()
    {
        //Act
        var tvShow = await _sut.GetTvShowByIdAsync(TvShowMock.Data.Id);

        // Assert
        tvShow.Should().NotBeNull();

        tvShow.CreatedBy.Should().BeEquivalentTo(TvShowMock.Data.CreatedBy);
        tvShow.EpisodeRunTime.Should().BeEquivalentTo(TvShowMock.Data.EpisodeRunTime);
        tvShow.FirstAirDate.Date.Should().Be(TvShowMock.Data.FirstAirDate);

        tvShow.Genres.Should().BeEquivalentTo(TvShowMock.Data.Genres);

        tvShow.Homepage.Should().Be(TvShowMock.Data.Homepage);

        tvShow.Languages.Should().BeEquivalentTo(TvShowMock.Data.Languages);

        tvShow.Name.Should().Be(TvShowMock.Data.Name);

        tvShow.Networks.Should().BeEquivalentTo(TvShowMock.Data.Networks);
        tvShow.OriginCountry.Should().BeEquivalentTo(TvShowMock.Data.OriginCountry);
        tvShow.OriginalLanguage.Should().Be(TvShowMock.Data.OriginalLanguage);

        TMDBTestUtil.AssertImagePath(tvShow.BackdropPath);
        TMDBTestUtil.AssertImagePath(tvShow.PosterPath);
        tvShow.ProductionCompanies.Should().BeEquivalentTo(TvShowMock.Data.ProductionCompanies);
    }

    [Fact]
    public async Task get_movie_reviews_should_returns_valid_data()
    {
        //Act
        var reviews = await _sut.GetMovieReviewsAsync(MovieMocks.Data.Id);

        // Assert
        reviews.Should().NotBeNull();
        reviews.Should().BeOfType<ListResultModel<ReviewInfo>>();
        reviews.Items.Should().BeOfType<List<ReviewInfo>>();
        reviews.TotalItems.Should().Be(8);
    }

    [Fact]
    public async Task get_tv_show_reviews_should_returns_valid_data()
    {
        //Act
        var reviews = await _sut.GetTvShowReviewsAsync(TvShowMock.Data.Id);

        // Assert
        reviews.Should().NotBeNull();
        reviews.Should().BeOfType<ListResultModel<ReviewInfo>>();
        reviews.Items.Should().BeOfType<List<ReviewInfo>>();
        reviews.TotalItems.Should().Be(9);
    }

    [Fact]
    public async Task get_reviews_should_returns_valid_data()
    {
        //Act
        var review = await _sut.GetReviewsAsync("5913e02fc3a3683a93004984");

        // Assert
        review.Should().NotBeNull();
    }

    [Fact]
    public async Task get_movie_credits_should_returns_valid_data()
    {
        // Act
        var credits = await _sut.GetMovieCreditsAsync(MovieMocks.Data.Id);

        // Assert
        credits.Should().NotBeNull();
        credits.CastMembers.Count.Should().BeGreaterThan(0);
        credits.CrewMembers.Count.Should().BeGreaterThan(0);
        credits.MovieId.Should().Be(MovieMocks.Data.Id);
    }

    [Fact]
    public async Task get_person_movie_credits_should_returns_valid_data()
    {
        // Act
        var credits = await _sut.GetPersonMovieCreditsAsync(PersonMock.Data.Id);

        // Assert
        credits.Should().NotBeNull();
        credits.CastRoles.Count.Should().BeGreaterThan(0);
        credits.CrewRoles.Count.Should().BeGreaterThan(0);
        credits.PersonId.Should().Be(PersonMock.Data.Id);
    }

    [Fact]
    public async Task get_person_tv_show_credits_should_returns_valid_data()
    {
        // Act
        var credits = await _sut.GetPersonTvShowCreditsAsync(PersonMock.Data.Id);

        // Assert
        credits.Should().NotBeNull();
        credits.CastRoles.Count.Should().BeGreaterThan(0);
        credits.CrewRoles.Count.Should().Be(0);
        credits.PersonId.Should().Be(PersonMock.Data.Id);
    }

    [Fact]
    public async Task get_tv_show_credits_should_returns_valid_data()
    {
        // Act
        var credits = await _sut.GetTvShowCreditsAsync(TvShowMock.Data.Id);

        // Assert
        credits.Should().NotBeNull();
        credits.CastMembers.Count.Should().BeGreaterThan(0);
        credits.CrewMembers.Count.Should().BeGreaterThan(0);
        credits.TvShowId.Should().Be(TvShowMock.Data.Id);
    }

    [Fact]
    public async Task get_movie_images_should_returns_valid_data()
    {
        // Act
        var images = await _sut.GetMovieImagesAsync(MovieMocks.Data.Id);

        // Assert
        images.Should().NotBeNull();
        images.Id.Should().Be(MovieMocks.Data.Id);
        images.Backdrops.Count.Should().BeGreaterThan(0);
        images.Posters.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task get_tv_show_images_should_returns_valid_data()
    {
        // Act
        var images = await _sut.GetTvShowImagesAsync(TvShowMock.Data.Id);

        // Assert
        images.Should().NotBeNull();
        images.Id.Should().Be(TvShowMock.Data.Id);
        images.Backdrops.Count.Should().BeGreaterThan(0);
        images.Posters.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task get_movie_videos_should_returns_valid_data()
    {
        // Act
        var videos = await _sut.GetMovieVideosAsync(MovieMocks.Data.Id);

        // Assert
        videos.Should().NotBeNull();
        videos.Videos.Should().NotBeNull();
        videos.Videos.Should().BeOfType<List<Core.Generals.Video>>();
        videos.Videos.Count.Should().BeGreaterThan(0);
        videos.MovieId.Should().Be(MovieMocks.Data.Id);
    }

    [Fact]
    public async Task get_tv_show_videos_should_returns_valid_data()
    {
        // Act
        var videos = await _sut.GetTvShowVideosAsync(TvShowMock.Data.Id);

        // Assert
        videos.Should().NotBeNull();
        videos.Videos.Should().NotBeNull();
        videos.Videos.Should().BeOfType<List<Core.Generals.Video>>();
        videos.Videos.Count.Should().BeGreaterThan(0);
        videos.TvShowId.Should().Be(TvShowMock.Data.Id);
    }

    [Fact]
    public async Task get_person_detail_should_returns_valid_data()
    {
        // Act
        var person = await _sut.GetPersonDetailAsync(PersonMock.Data.Id);

        // Assert
        person.Should().NotBeNull();
        person.Id.Should().Be(PersonMock.Data.Id);
    }

    [Fact]
    public async Task get_tv_show_on_the_air_should_returns_valid_data()
    {
        // Act
        var result = await _sut.GetTvShowOnTheAirAsync();

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.Items.Should().BeOfType<List<TVShowInfo>>();
        result.Items.Any().Should().BeTrue();
        result.Items.Count.Should().Be(result.PageSize);
        result.Page.Should().Be(1);
    }

    [Fact]
    public async Task get_tv_show_airing_today_should_returns_valid_data()
    {
        // Act
        var result = await _sut.GetTvShowAiringTodayAsync();

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.Items.Should().BeOfType<List<TVShowInfo>>();
        result.Items.Any().Should().BeTrue();
        result.Items.Count.Should().Be(result.PageSize);
        result.Page.Should().Be(1);
    }

    [Fact]
    public async Task get_popular_tv_show_should_returns_valid_data()
    {
        // Act
        var result = await _sut.GetPopularTvShowAsync();

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.Items.Should().BeOfType<List<TVShowInfo>>();
        result.Items.Any().Should().BeTrue();
        result.Items.Count.Should().Be(result.PageSize);
        result.Page.Should().Be(1);
    }

    [Fact]
    public async Task get_tv_show_top_rated_should_returns_valid_data()
    {
        // Act
        var result = await _sut.GetTvShowTopRatedAsync();

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.Items.Should().BeOfType<List<TVShowInfo>>();
        result.Items.Any().Should().BeTrue();
        result.Items.Count.Should().Be(result.PageSize);
        result.Page.Should().Be(1);
    }

    [Fact]
    public async Task search_tv_shows_by_title_should_returns_valid_data()
    {
        // Act
        var result = await _sut.SearchTvShowAsync(TvShowMock.Data.Name);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.Items.Should().BeOfType<List<TVShowInfo>>();
        result.Items.Any().Should().BeTrue();
        result.Items.Count.Should().Be(result.PageSize);
        result.Page.Should().Be(1);
    }

    [Fact]
    public async Task search_multi_should_returns_valid_data()
    {
        // Act
        var result = await _sut.SearchMultiAsync(TvShowMock.Data.Name);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.Items.Should().BeOfType<List<dynamic>>();
        result.Items.Any().Should().BeTrue();
        result.Items.Count.Should().Be(result.PageSize);
        result.Page.Should().Be(1);
    }
}
