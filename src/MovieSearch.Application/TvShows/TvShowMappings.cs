using AutoMapper;
using MovieSearch.Application.TvShows.Dtos;
using MovieSearch.Core.TV;

namespace MovieSearch.Application.TvShows
{
    public class TvShowMappings : Profile
    {
        public TvShowMappings()
        {
            CreateMap<TVShow, TVShowDto>();
            CreateMap<TVShowInfo, TVShowInfoDto>();
            CreateMap<TVShowCredit, TVShowCreditDto>();
            CreateMap<TVShowCastMember, TVShowCastMemberDto>();
            CreateMap<TVShowCrewMember, TVShowCrewMemberDto>();
            CreateMap<TVShowCreator, TVShowCreatorDto>();
        }
    }
}