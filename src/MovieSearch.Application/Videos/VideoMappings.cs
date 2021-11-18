using AutoMapper;
using MovieSearch.Application.Videos.Dtos;
using MovieSearch.Core.Generals;

namespace MovieSearch.Application.Videos
{
    public class VideoMappings : Profile
    {
        public VideoMappings()
        {
            CreateMap<Video, VideoDto>();
        }
    }
}