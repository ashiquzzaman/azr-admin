using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AzR.Core.Config
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        DbContext Context { get; set; }

        IQueryable<TEntity> GetAll { get; }

        bool Any(Expression<Func<TEntity, bool>> predicate);
        IQueryable<string> Select(Expression<Func<TEntity, string>> predicate);
        TEntity Find(params object[] keys);
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        TEntity First(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        bool IsExist(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);
        int Count { get; }
        bool ShareContext { get; set; }
        int CounFunc(Expression<Func<TEntity, bool>> predicate);
        string MaxFunc(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where);
        string Max(Expression<Func<TEntity, string>> predicate);
        TEntity Create(TEntity item);
        int Update(TEntity item);

        int Delete(TEntity item);

        int SaveChanges();
        int Delete(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<T> ExecuteQuery<T>(string sqlQuery, params object[] parameters);
        int ExecuteCommand(string sqlCommand, params object[] parameters);

        string ToId(Expression<Func<TEntity, string>> predicate, string prefix, int returnLength = 9, char fillValue = '0');

        string CreateId(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where, string prefix,
            int returnLength = 9, char fillValue = '0');

        IEnumerable DynamicExecuteQuery(string sqlQuery, params object[] parameters);
        dynamic DynamicFirst(string sqlQuery, params object[] parameters);


        #region LINQ ASYNC

        Task<ICollection<TEntity>> GetAllAsync();
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity item);
        Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> DeleteAsync(TEntity t);
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync();
        Task<long> LongCountAsync();
        Task<int> CountFuncAsync(Expression<Func<TEntity, bool>> predicate);
        Task<long> LongCountFuncAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<string> MaxAsync(Expression<Func<TEntity, string>> predicate);
        Task<string> MaxFuncAsync(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where);
        Task<string> MinAsync(Expression<Func<TEntity, string>> predicate);
        Task<string> MinFuncAsync(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where);
        Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> SaveChangesAsync();
        #endregion

        string Create(Dictionary<string, object> model, string tableName);
        int Update(Dictionary<string, object> model, string tableName, string id);
    }
}
