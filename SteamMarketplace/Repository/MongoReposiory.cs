using MongoDB.Driver;
using SteamMarketplace.Entities;
using SteamMarketplace.Database;

using System.Linq.Expressions;

namespace SteamMarketplace.Repository
{
    public sealed class MongoRepository : IMongoRepository
    {

        private Context context { get; }

        public MongoRepository(Context _context)
        {
            context = _context;
        }


        public IMongoCollection<T> GetCollection<T>() where T : BaseEntity
        {
            return context.database.GetCollection<T>(typeof(T).Name);
        }

        public async Task CreateAsync<T>(T obj) where T : BaseEntity
        {
            await GetCollection<T>().InsertOneAsync(obj);
        }

        public async Task CreateManyAsync<T>(List<T> collection) where T : BaseEntity
        {
            await GetCollection<T>().InsertManyAsync(collection);
        }

        public async Task<T> FindAsync<T>(Expression<Func<T, bool>> funcExpression)
          where T : BaseEntity
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Where(funcExpression);

            var result = await GetCollection<T>().FindAsync(filter);

            return result.FirstOrDefault();
        }

        public async Task<T> FindAsync<T>(Expression<Func<T, bool>> funcExpression, ProjectionDefinition<T> FieldsToExclude)
            where T : BaseEntity
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Where(funcExpression);

            var result = await GetCollection<T>()
                .Find(filter).Project<T>(FieldsToExclude).FirstOrDefaultAsync();

            return result;
        }

        public async Task<K> FindAsync<T, K>(
            Expression<Func<T, IEnumerable<K>>> fields,
            Expression<Func<K, bool>> funcExpression,
            Expression<Func<T, K>> expression)
            where T : BaseEntity
            where K : BaseEntity
        {
            var filter = Builders<T>.Filter.ElemMatch(fields, funcExpression);

            return await GetCollection<T>()
                            .Aggregate()
                            .Match(filter)
                            .Project(
                                Builders<T>.Projection.Expression<K>(expression)
                        ).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>> funcExpression) where T : BaseEntity
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Where(funcExpression);

            return await GetCollection<T>()
                .Find(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>> funcExpression, int limit, int skip) where T : BaseEntity
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Where(funcExpression);

            return await GetCollection<T>()
                .Find(filter)
                .Skip(skip)
                .Limit(limit)
                .ToListAsync();
        }


        public async Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>> funcExpression, ProjectionDefinition<T> FieldsToExclude) where T : BaseEntity
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Where(funcExpression);

            return await GetCollection<T>()
                .Find(filter).Project<T>(FieldsToExclude)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>> funcExpression, ProjectionDefinition<T> FieldsToExclude, int limit, int skip) where T : BaseEntity
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Where(funcExpression);

            return await GetCollection<T>()
                .Find(filter)
                .Project<T>(FieldsToExclude)
                .Skip(skip)
                .Limit(limit)
                .ToListAsync();
        }

        public async Task<bool> ReplaceAsync<T>(T collection) where T : BaseEntity
        {

            var updatedResult = await GetCollection<T>()
                .ReplaceOneAsync(filter: x => x.Id == collection.Id, replacement: collection);

            return updatedResult.IsAcknowledged
                   && updatedResult.ModifiedCount > 0;

        }

        public async Task<bool> FindOneAndUpdateAsync<T, K>(
            Expression<Func<T, bool>> funcExpression,
            Expression<Func<T, K>> updateExpression, K objToUpdate)
            where T : BaseEntity
        {
            var filter = Builders<T>.Filter.Where(funcExpression);

            var update = Builders<T>.Update.Set(updateExpression, objToUpdate);

            var updatedResult = await GetCollection<T>().UpdateOneAsync(filter, update);

            return updatedResult.IsAcknowledged
                 && updatedResult.ModifiedCount > 0;
        }

        public async Task UpdateManyAsync<T>(List<T> collection) where T : BaseEntity
        {
            var updates = new List<WriteModel<T>>();

            var filtersBuilder = Builders<T>.Filter;

            foreach (var document in collection)
            {
                var filter = filtersBuilder.Where(x => x.Id == document.Id);

                updates.Add(new ReplaceOneModel<T>(filter, document));
            }

            await GetCollection<T>().BulkWriteAsync(updates);
        }

       

        public async Task<long> CountAsync<T>(Expression<Func<T, bool>> funcExpression) where T : BaseEntity
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Where(funcExpression);

            return await GetCollection<T>().CountDocumentsAsync(filter);
        }

        public async Task DeleteByIdAsync<T>(string id) where T : BaseEntity
        {
            await GetCollection<T>().DeleteOneAsync(t => t.Id == id);
        }

        public async Task DeleteAsync<T>(Expression<Func<T, bool>> funcExpression) where T : BaseEntity
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Where(funcExpression);

            await GetCollection<T>().DeleteOneAsync(filter);
        }

        public async Task DeleteManyAsync<T>(Expression<Func<T, bool>> funcExpression) where T : BaseEntity
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Where(funcExpression);

            await GetCollection<T>().DeleteManyAsync(filter);
        }

    }
}
