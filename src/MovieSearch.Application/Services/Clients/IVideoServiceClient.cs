using System.Threading.Tasks;
using MovieSearch.Core.Generals;

namespace MovieSearch.Application.Services.Clients
{
    public interface IVideoServiceClient
    {
        Task<VideoListResultModel<string>> GetVideos(string movieName, string page = "");
    }
}