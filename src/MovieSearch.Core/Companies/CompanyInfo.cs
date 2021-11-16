using System.Collections.Generic;
using MovieSearch.Core.Generals;

namespace MovieSearch.Core.Companies
{
    public class CompanyInfo : MultiInfo, IEqualityComparer<CompanyInfo>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string LogoPath { get; init; }

        public CompanyInfo(int id, string name, string logoPath = null)
        {
            Id = id;
            Name = name;
            LogoPath = logoPath;
        }

        public override bool Equals(object obj)
        {
            if (obj is not CompanyInfo info)
            {
                return false;
            }

            return Equals(this, info);
        }

        public bool Equals(CompanyInfo x, CompanyInfo y)
            => x != null && y != null && x.Id == y.Id && x.Name == y.Name && x.LogoPath == y.LogoPath;

        public override int GetHashCode()
            => GetHashCode(this);

        public int GetHashCode(CompanyInfo obj)
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + obj.Id.GetHashCode();
                hash = hash * 23 + obj.Name.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return "n/a";
            }

            return $"{Name} ({Id})";
        }
    }
}