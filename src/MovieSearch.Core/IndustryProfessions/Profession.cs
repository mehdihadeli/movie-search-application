using System.Collections.Generic;

namespace MovieSearch.Core.IndustryProfessions
{
    public class Profession
    {
        public string Department { get; init; }
        public IReadOnlyList<string> Jobs { get; init; }

        public override string ToString()
            => $"{Department} {Jobs.Count} jobs";
    }
}
