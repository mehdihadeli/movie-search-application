using System.Collections.Generic;

namespace MovieSearch.Core.Generals
{
    public class VideoListResultModel<T> where T : notnull
    {
        public VideoListResultModel(List<T> items, long totalItems, string pageToken, string nextPageToken,
            string previousPageToken, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            PageToken = pageToken;
            NextPageToken = nextPageToken;
            PreviousPageToken = previousPageToken;
            PageSize = pageSize;
        }

        public List<T> Items { get; init; }
        public long TotalItems { get; init; }
        public string PageToken { get; init; }
        public string NextPageToken { get; init; }
        public string PreviousPageToken { get; init; }
        public int PageSize { get; init; }
    }
}