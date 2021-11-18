using System.Collections.Generic;
using BuildingBlocks.Domain;
using BuildingBlocks.Web;

namespace Thesaurus.Api.Words.ViewModels
{
    public class SearchMoviesByTitleRequest
    {
        public string SearchKeywords { get; set; }
        public int Page { get; set; } = 1;
    }
}