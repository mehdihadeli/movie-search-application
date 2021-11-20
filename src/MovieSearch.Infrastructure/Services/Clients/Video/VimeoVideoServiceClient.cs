using System;
using System.Threading.Tasks;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Core.Generals;

namespace MovieSearch.Infrastructure.Services.Clients.Video
{
    public class VimeoVideoServiceClient : IVideoServiceClient
    {
        public Task<VideoListResultModel<Core.Generals.Video>> GetTrailers(string movieName, int pageSize = 20, string page = "", DateTime? publishedAfter = null,
            DateTime? publishedBefore = null)
        {
            throw new NotImplementedException();
        }
    }
}