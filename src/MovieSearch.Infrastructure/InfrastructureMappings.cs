using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BuildingBlocks.Domain;
using MovieSearch.Core.Collections;
using MovieSearch.Core.Companies;
using MovieSearch.Core.Generals;
using MovieSearch.Core.Movies;
using MovieSearch.Core.People;
using MovieSearch.Core.Review;
using MovieSearch.Core.TV;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using Cast = TMDbLib.Objects.Movies.Cast;
using Country = MovieSearch.Core.Generals.Country;
using Credits = TMDbLib.Objects.TvShows.Credits;
using ImageData = TMDbLib.Objects.General.ImageData;
using Images = MovieSearch.Core.Generals.Images;
using Movie = MovieSearch.Core.Movies.Movie;
using Network = MovieSearch.Core.TV.Network;
using Person = TMDbLib.Objects.People.Person;
using ProductionCompany = TMDbLib.Objects.General.ProductionCompany;
using Review = TMDbLib.Objects.Reviews.Review;
using Video = TMDbLib.Objects.General.Video;

namespace MovieSearch.Infrastructure
{
    public class InfrastructureMappings : Profile
    {
        public InfrastructureMappings()
        {
            CreateMap(typeof(SearchContainerWithDates<>), typeof(ListResultModel<>))
                .ConvertUsing(typeof(SearchContainerWithDatesToListResultModelConverter<,>));
            CreateMap(typeof(SearchContainer<>), typeof(ListResultModel<>))
                .ConvertUsing(typeof(SearchContainerToListResultModelConverter<,>));

            CreateMap<SearchMovie, MovieInfo>();

            CreateMap<SearchTv, TVShowInfo>();
            CreateMap<SearchCompany, CompanyInfo>();
            CreateMap<Genre, Core.Genres.Genre>();
            CreateMap<ProductionCompany, MovieSearch.Core.Companies.ProductionCompany>();
            CreateMap<ProductionCountry, Country>()
                .ForMember(x => x.Iso3166Code, opt => opt.MapFrom(s => s.Iso_3166_1));
            CreateMap<SpokenLanguage, Language>()
                .ForMember(x => x.Iso639Code, opt => opt.MapFrom(s => s.Iso_639_1))
                .ForMember(x => x.EnglishName, opt => opt.Ignore());
            CreateMap<TMDbLib.Objects.Languages.Language, Language>()
                .ForMember(x => x.Iso639Code, opt => opt.MapFrom(s => s.Iso_639_1));
            CreateMap<SearchCollection, CollectionInfo>();
            CreateMap<Keyword, Core.Keywords.Keyword>();
            CreateMap<TMDbLib.Objects.Movies.Movie, Movie>()
                .ForMember(x => x.MovieCollectionInfo, opt => opt.MapFrom(s => s.BelongsToCollection))
                .ForMember(x => x.IsVideo, opt => opt.MapFrom(s => s.Video))
                .ForMember(x => x.Keywords, opt => opt.Ignore());

            CreateMap<TvShow, TVShow>()
                .ForMember(x => x.Keywords, opt => opt.Ignore());
            CreateMap<SearchTvSeason, Season>();
            CreateMap<NetworkWithLogo, Network>();
            CreateMap<CreatedBy, TVShowCreator>();
            CreateMap<PersonGender, Gender>();
            CreateMap<ReviewBase, ReviewInfo>();
            CreateMap<Review, MovieSearch.Core.Review.Review>();
            CreateMap<TMDbLib.Objects.Movies.Credits, MovieCredit>()
                .ForMember(x => x.MovieId, opt => opt.MapFrom(s => s.Id))
                .ForMember(x => x.CastMembers, opt => opt.MapFrom(s => s.Cast))
                .ForMember(x => x.CrewMembers, opt => opt.MapFrom(s => s.Crew));
            CreateMap<TMDbLib.Objects.TvShows.Credits, TVShowCredit>()
                .ForMember(x => x.TvShowId, opt => opt.MapFrom(s => s.Id))
                .ForMember(x => x.CastMembers, opt => opt.MapFrom(s => s.Cast))
                .ForMember(x => x.CrewMembers, opt => opt.MapFrom(s => s.Crew));
            CreateMap<TMDbLib.Objects.TvShows.Cast, TVShowCastMember>();
            CreateMap<TMDbLib.Objects.Movies.Cast, MovieCastMember>();
            CreateMap<Crew, MovieCrewMember>();
            CreateMap<Crew, TVShowCrewMember>();

            CreateMap<MovieCredits, PersonMovieCredit>()
                .ForMember(x => x.PersonId, opt => opt.MapFrom(s => s.Id))
                .ForMember(x => x.CastRoles, opt => opt.MapFrom(s => s.Cast))
                .ForMember(x => x.CrewRoles, opt => opt.MapFrom(s => s.Crew));
            CreateMap<MovieRole, PersonMovieCastMember>();
            CreateMap<MovieJob, PersonMovieCrewMember>();

            CreateMap<TvCredits, PersonTVCredit>()
                .ForMember(x => x.PersonId, opt => opt.MapFrom(s => s.Id))
                .ForMember(x => x.CastRoles, opt => opt.MapFrom(s => s.Cast))
                .ForMember(x => x.CrewRoles, opt => opt.MapFrom(s => s.Crew));
            CreateMap<TvRole, PersonTVCastMember>();
            CreateMap<TvJob, PersonTVCrewMember>();

            CreateMap<ImagesWithId, Images>();
            CreateMap<ImageData, Core.Generals.ImageData>();

            CreateMap<Video, Core.Generals.Video>()
                .ForMember(x=>x.PublishedAt,opt=>opt.Ignore());
            CreateMap<Person, Core.People.Person>();

            CreateMap<SearchBase, MultiInfo>();
            CreateMap<SearchMovie, MultiInfo>();
            CreateMap<SearchTv, MultiInfo>();
            CreateMap<SearchPerson, MultiInfo>();
        }
    }

    public class SearchContainerWithDatesToListResultModelConverter<TSource, TDestination> : ITypeConverter<
        SearchContainerWithDates<TSource>, ListResultModel<TDestination>>
    {
        public ListResultModel<TDestination> Convert(SearchContainerWithDates<TSource> source,
            ListResultModel<TDestination> destination, ResolutionContext context)
        {
            var destinationItems = context.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source.Results);
            return new ListResultModel<TDestination>(destinationItems?.ToList(), source.TotalResults, source.Page,
                source.Results.Count);
        }
    }

    public class SearchContainerToListResultModelConverter<TSource, TDestination> : ITypeConverter<
        SearchContainer<TSource>, ListResultModel<TDestination>>
    {
        public ListResultModel<TDestination> Convert(SearchContainer<TSource> source,
            ListResultModel<TDestination> destination, ResolutionContext context)
        {
            var destinationItems = context.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source.Results);
            return new ListResultModel<TDestination>(destinationItems?.ToList(), source.TotalResults, source.Page,
                source.Results.Count);
        }
    }
}