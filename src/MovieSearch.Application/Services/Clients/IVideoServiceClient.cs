using System.Threading.Tasks;
using MovieSearch.Core.Generals;

namespace MovieSearch.Application.Services.Clients
{
    public interface IVideoServiceClient
    {
        Task<VideoListResultModel<MovieSearch.Core.Generals.Video>> GetVideos(string movieName, int pageSize = 20,
            string page = "");
    }
}