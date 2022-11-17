using MovieSearch.Core.Generals;

namespace MovieSearch.Application.Generals.Dtos;

public class MultiInfoDto
{
    public int Id { get; init; }
    public virtual MediaType MediaType { get; init; }
    public double Popularity { get; init; }
}