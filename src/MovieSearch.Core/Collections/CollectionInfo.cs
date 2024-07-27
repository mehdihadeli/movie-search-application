namespace MovieSearch.Core.Collections;

public class CollectionInfo
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string PosterPath { get; init; }
    public string BackdropPath { get; init; }

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(Name))
            return "n/a";

        return $"{Name} ({Id})";
    }
}
