using AutoMapper;
using MovieSearch.Application.People.Dtos;
using MovieSearch.Core.People;

namespace MovieSearch.Application.People
{
    public class PeopleMappings : Profile
    {
        public PeopleMappings()
        {
            CreateMap<Person, PersonDto>();
            CreateMap<PersonMovieCredit, PersonMovieCreditDto>();
            CreateMap<PersonMovieCastMember, PersonMovieCastMemberDto>();
            CreateMap<PersonMovieCrewMember, PersonMovieCrewMemberDto>();

            CreateMap<PersonTVCredit, PersonTVShowCreditDto>();
            CreateMap<PersonTVCastMember, PersonTVShowCastMemberDto>();
            CreateMap<PersonTVCrewMember, PersonTVShowCrewMemberDto>();
        }
    }
}