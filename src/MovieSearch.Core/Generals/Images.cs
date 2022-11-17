using System.Collections.Generic;

namespace MovieSearch.Core.Generals;

public class Images
{
    public int Id { get; set; }
    public List<ImageData> Backdrops { get; set; }
    public List<ImageData> Posters { get; set; }
}