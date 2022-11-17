using System;
using System.Threading.Tasks;
using MovieSearch.Core.Generals;

namespace MovieSearch.Application.Services.Clients;

public interface IVideoServiceClient
{
    Task<VideoListResultModel<Video>> GetTrailers(string movieName, int pageSize = 20,
        string page = "", DateTime? publishedAfter = null, DateTime? publishedBefore = null);
}