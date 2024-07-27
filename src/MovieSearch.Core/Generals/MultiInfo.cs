namespace MovieSearch.Core.Generals;

public class MultiInfo
{
    public int Id { get; init; }
    public virtual MediaType MediaType { get; init; }
    public double Popularity { get; init; }
}
