using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using BuildingBlocks.Persistence;
using MicroBootstrap.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BuildingBlocks.Mongo
{
    public class MongoRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IAggregate
    {
        private readonly IMongoDbContext _context;
        private readonly IMongoCollection<TEntity> DbSet;

        public MongoRepository(IMongoDbContext context)
        {
            _context = context;
            DbSet = _context.GetCollection<TEntity>();
        }

        public Task<TEntity> GetAsync(Guid id)
            => GetAsync(e => e.Id.Equals(id));

        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
            => DbSet.Find(predicate).SingleOrDefaultAsync();


        public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
            => await DbSet.Find(predicate).ToListAsync();

        public IAsyncEnumerable<TEntity> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return DbSet.AsQueryable().ToAsyncEnumerable();
        }

        public Task<ListResultModel<TEntity>> BrowseAsync<TQuery>(Expression<Func<TEntity, bool>> predicate,
            TQuery query) where TQuery : IPageList
            => DbSet.AsQueryable().Where(predicate).PaginateAsync(query);

        public Task AddAsync(TEntity entity)
            => DbSet.InsertOneAsync(entity);

        public Task UpdateAsync(TEntity entity)
            => UpdateAsync(entity, e => e.Id.Equals(entity.Id));

        public Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate)
            => DbSet.ReplaceOneAsync(predicate, entity);

        public Task DeleteAsync(Guid id)
            => DeleteAsync(e => e.Id.Equals(id));

        public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
            => DbSet.DeleteOneAsync(predicate);

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
            => DbSet.Find(predicate).AnyAsync();

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}