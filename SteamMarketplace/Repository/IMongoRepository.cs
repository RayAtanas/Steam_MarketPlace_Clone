using SteamMarketplace.Entities;
using MongoDB.Driver;
using NPOI.SS.Formula.Functions;
using System.Linq.Expressions;

namespace SteamMarketplace.Repository
{
    public interface IMongoRepository
    {

        public IMongoCollection<T> GetCollection<T>() where T : BaseEntity;

        public Task CreateAsync<T>(T obj) where T : BaseEntity;

        public Task CreateManyAsync<T>(List<T> collection) where T : BaseEntity;

        public Task<T> FindAsync<T>(Expression<Func<T, bool>> funcExpression)
            where T : BaseEntity;

        public Task<T> FindAsync<T>(Expression<Func<T, bool>> funcExpression, ProjectionDefinition<T> FieldsToExclude)
            where T : BaseEntity;

        public Task<K> FindAsync<T, K>(
            Expression<Func<T, IEnumerable<K>>> fields,
            Expression<Func<K, bool>> funcExpression,
            Expression<Func<T, K>> expression)
            where T : BaseEntity
            where K : BaseEntity;

        public Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>> funcExpression) where T : BaseEntity;

        public Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>> funcExpression, int limit, int skip)
            where T : BaseEntity;

        public Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>> funcExpression,
            ProjectionDefinition<T> FieldsToExclude) where T : BaseEntity;

        public Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>> funcExpression,
            ProjectionDefinition<T> FieldsToExclude, int limit, int skip) where T : BaseEntity;

        public Task<bool> ReplaceAsync<T>(T collection) where T : BaseEntity;

        public Task<bool> FindOneAndUpdateAsync<T, K>(
            Expression<Func<T, bool>> funcExpression,
            Expression<Func<T, K>> updateExpression, K objToUpdate)
            where T : BaseEntity;

        public Task UpdateManyAsync<T>(List<T> collection) where T : BaseEntity;

        public Task<long> CountAsync<T>(Expression<Func<T, bool>> funcExpression) where T : BaseEntity;

        public Task DeleteByIdAsync<T>(string id) where T : BaseEntity;

        public Task DeleteAsync<T>(Expression<Func<T, bool>> funcExpression) where T : BaseEntity;

        public Task DeleteManyAsync<T>(Expression<Func<T, bool>> funcExpression) where T : BaseEntity;
    }

 
    
}
