using System.Threading.Tasks;
using MovieSearch.Core.Generals;

namespace MovieSearch.Application.Services.Clients
{
    public interface IVideoServiceClient
    {
        Task<VideoListResultModel<MovieSearch.Core.Generals.Video>> GetTrailers(string movieName, int pageSize = 20,
            string page = "");
    }
}