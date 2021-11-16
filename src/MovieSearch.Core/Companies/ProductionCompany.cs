using System.Collections.Generic;

namespace MovieSearch.Core.Companies
{
    public class ProductionCompany : IEqualityComparer<ProductionCompany>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string LogoPath { get; init; }
        public string OriginCountry { get; init; }

        public override bool Equals(object obj)
        {
            if (obj is not ProductionCompany info)
            {
                return false;
            }

            return Equals(this, info);
        }

        public bool Equals(ProductionCompany x, ProductionCompany y)
            => x != null && y != null && x.Id == y.Id && x.Name == y.Name && x.LogoPath == y.LogoPath &&
               x.OriginCountry == y.OriginCountry;

        public override int GetHashCode()
            => GetHashCode(this);

        public int GetHashCode(ProductionCompany obj)
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + obj.Id.GetHashCode();
                hash = hash * 23 + obj.Name.GetHashCode();
                return hash;
            }
        }
    }
}