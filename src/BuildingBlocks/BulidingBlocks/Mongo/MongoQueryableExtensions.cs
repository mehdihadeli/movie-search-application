using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Linq; // Ensure this is included for IMongoQueryable

namespace MicroBootstrap.Mongo
{
    public static class MongoQueryableExtensions
    {
        public static async Task<ListResultModel<T>> PaginateAsync<T>(this IQueryable<T> collection, IPageList query)
        {
            return await collection.PaginateAsync(query.Page, query.PageSize);
        }

        public static async Task<ListResultModel<T>> PaginateAsync<T>(
            this IQueryable<T> collection,
            int page = 1,
            int pageSize = 10
        )
        {
            if (page <= 0)
                page = 1;

            if (pageSize <= 0)
                pageSize = 10;

            // Use CountDocumentsAsync instead of CountAsync for MongoDB.Driver.Linq changes
            var isEmpty = await collection.AnyAsync() == false;
            if (isEmpty)
                return ListResultModel<T>.Empty;

            var totalItems = await collection.CountAsync();
            var totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
            var data = await collection.Limit(page, pageSize).ToListAsync();

            return ListResultModel<T>.Create(data, totalItems, page, pageSize);
        }

        public static IQueryable<T> Limit<T>(this IQueryable<T> collection, IPageList query)
        {
            return collection.Limit(query.Page, query.PageSize);
        }

        public static IQueryable<T> Limit<T>(this IQueryable<T> collection, int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
                page = 1;

            if (resultsPerPage <= 0)
                resultsPerPage = 10;

            var skip = (page - 1) * resultsPerPage;
            var data = collection.Skip(skip).Take(resultsPerPage);

            return data;
        }
    }
}
