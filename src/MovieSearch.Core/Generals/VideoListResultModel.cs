using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieSearch.Core.Generals;

public class VideoListResultModel<T>
    where T : notnull
{
    public VideoListResultModel(
        List<T> items,
        long totalItems,
        string pageToken,
        string nextPageToken,
        string previousPageToken,
        int pageSize = 20
    )
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

    public static VideoListResultModel<T> Create(
        List<T> items,
        long totalItems,
        string pageToken,
        string nextPageToken,
        string previousPageToken,
        int pageSize = 20
    )
    {
        return new VideoListResultModel<T>(items, totalItems, pageToken, nextPageToken, previousPageToken, pageSize);
    }

    public VideoListResultModel<U> Map<U>(Func<T, U> map)
    {
        return VideoListResultModel<U>.Create(
            Items.Select(map).ToList(),
            TotalItems,
            PageToken,
            NextPageToken,
            PreviousPageToken,
            PageSize
        );
    }
}
