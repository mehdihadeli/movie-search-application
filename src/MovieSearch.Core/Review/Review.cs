using MovieSearch.Core.Generals;

namespace MovieSearch.Core.Review;

public class Review
{
    public string Id { get; init; }
    public string Author { get; init; }
    public string Content { get; init; }
    public string Url { get; init; }

    /// <summary>
    ///     A language code, e.g. en
    /// </summary>
    public string Iso_639_1 { get; init; }

    public int MediaId { get; init; }
    public string MediaTitle { get; init; }
    public MediaType MediaType { get; init; }
}