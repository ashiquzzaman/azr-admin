using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AzR.Core.Config;

namespace AzR.Core.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Members

        protected DbContext Context;
        private bool disposed = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of repository
        /// </summary>
        /// <param name="context"></param>
        public Repository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            Context = context;

        }

        #endregion

        #region PROPERTY
        // Entity corresponding Database Table
        protected DbSet<T> DbSet
        {
            get { return Context.Set<T>(); }
        }

        #endregion

        #region LINQ QUERY

        /// <summary>
        /// add a item in a table. item never be added untill call savechanges method.
        /// </summary>
        /// <param name="item">object of a class which will be added into corresponding DB table.</param>
        public virtual void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            DbSet.Add(item); // add new item in this set
        }

        /// <summary>
        /// Remove a item in a table. item never be Removed untill call savechanges method.
        /// </summary>
        /// <param name="item">object of a class which will be Removed into corresponding DB table.</param>
        public virtual void Remove(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            Attach(item); //attach item if not exist
            DbSet.Remove(item); //set as "removed"

        }

        /// <summary>
        /// Modify a item in a table. item never be Modified untill call savechanges method.
        /// </summary>
        /// <param name="item">object of a class which will be Modified into corresponding DB table.</param>
        public virtual void Modify(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var entry = Context.Entry(item);
            DbSet.Attach(item);
            entry.State = EntityState.Modified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return DbSet.SingleOrDefault(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> All()
        {
            return DbSet.AsQueryable();
        }

        public T Create(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            DbSet.Add(item);
            Context.SaveChanges();
            return item;
        }

        public int Update(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var entry = Context.Entry(item);
            DbSet.Attach(item);
            entry.State = EntityState.Modified;
            return Context.SaveChanges();
        }

        public int Update(Expression<Func<T, bool>> predicate)
        {
            var records = FindAll(predicate);
            if (!records.Any())
            {
                throw new ObjectNotFoundException();
            }
            foreach (var record in records)
            {
                var entry = Context.Entry(record);

                DbSet.Attach(record);

                entry.State = EntityState.Modified;
            }
            return Context.SaveChanges();
        }

        public int Delete(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            DbSet.Remove(item);

            return Context.SaveChanges();
        }

        public int Delete(Expression<Func<T, bool>> predicate)
        {
            var records = FindAll(predicate);
            if (!records.Any())
            {
                throw new ObjectNotFoundException();
            }
            foreach (var record in records)
            {
                DbSet.Remove(record);
            }
            return Context.SaveChanges();
        }

        /// <summary>
        /// Count all item in a DB table.
        /// </summary>
        public int Count
        {
            get { return DbSet.Count(); }
        }
        public long LongCount
        {
            get { return DbSet.LongCount(); }
        }
        public int CountFunc(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Count(predicate);
        }

        public long LongCountFunc(Expression<Func<T, bool>> predicate)
        {
            return DbSet.LongCount(predicate);
        }

        public bool IsExist(Expression<Func<T, bool>> predicate)
        {
            var count = DbSet.Count(predicate);
            return count > 0;
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            return DbSet.First(predicate);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return DbSet.FirstOrDefault(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string Max(Expression<Func<T, string>> predicate)
        {
            return DbSet.Max(predicate);
        }

        public string MaxFunc(Expression<Func<T, string>> predicate, Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).Max(predicate);
        }

        public string Min(Expression<Func<T, string>> predicate)
        {
            return DbSet.Min(predicate);
        }

        public string MinFunc(Expression<Func<T, string>> predicate, Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).Min(predicate);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable();
        }

        #endregion

        #region SQL RAW QUERY

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return Context.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery)
        {
            return Context.Database.SqlQuery<TEntity>(sqlQuery);
        }
        public TEntity ExecuteSingleQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return Context.Database.SqlQuery<TEntity>(sqlQuery, parameters).FirstOrDefault();
        }
        public TEntity ExecuteSingleQuery<TEntity>(string sqlQuery)
        {
            return Context.Database.SqlQuery<TEntity>(sqlQuery).Single();
        }
        public IEnumerable DynamicExecuteQuery(string sqlQuery, params object[] parameters)
        {
            return Context.Database.DynamicSqlQuery(sqlQuery, parameters);
        }

        public dynamic DynamicFirst(string sqlQuery, params object[] parameters)
        {
            return Context.Database.DynamicSqlQuery(sqlQuery, parameters).Cast<dynamic>().FirstOrDefault(o => o != null);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return Context.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }
        public string ExecuteScalar(string sqlCommand, params object[] parameters)
        {
            //decimal data = base.Database.SqlQuery<decimal>(sqlCommand, parameters).Single();
            return Context.Database.SqlQuery<string>(sqlCommand, parameters).Single().ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="parameters"></param>
        public TEntity ExecuteScalar<TEntity>(string sqlCommand, params object[] parameters)
        {
            return Context.Database.SqlQuery<TEntity>(sqlCommand, parameters).Single();
        }

        #endregion

        #region DATABSE TRANSACTION

        public void Attach<TEntity>(TEntity item) where TEntity : class
        {
            Context.Entry(item).State = EntityState.Unchanged;
        }

        public void SetModified<TEntity>(TEntity item) where TEntity : class
        {
            //this operation also attach item in object state manager
            Context.Entry(item).State = EntityState.Modified;
        }

        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current)
            where TEntity : class
        {
            //if it is not attached, attach original and set current values
            Context.Entry(original).CurrentValues.SetValues(current);
        }

        public int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed;

            do
            {
                try
                {
                    Context.SaveChanges();

                    saveFailed = false;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                        .ForEach(entry =>
                        {
                            entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                        });

                }
            } while (saveFailed);

        }

        public void RollbackChanges()
        {
            Context.ChangeTracker.Entries()
                .ToList()
                .ForEach(entry => entry.State = EntityState.Unchanged);
        }

        #endregion

        #region Dictionary

        public string Create(Dictionary<string, object> model, string tableName)
        {
            string result;
            if (model.Count != 0)
            {
                var dictonaryParams = model.Select(o => new SqlParameter("@" + o.Key, o.Value)).ToList();
                var dictonaryField = string.Join(",", model.Keys.ToArray());
                var dictonaryValue = model.Aggregate(string.Empty, (current, o) => current + ("@" + o.Key + ","));
                if (dictonaryValue != "")
                    dictonaryValue = dictonaryValue.Remove(dictonaryValue.LastIndexOf(",", StringComparison.Ordinal));
                var dictonaryQuery =
                    string.Format(
                        "INSERT INTO {0} ({1}) VALUES ( {2}); SELECT CAST(SCOPE_IDENTITY() AS VARCHAR(50)) AS LAST_IDENTITY;",
                        tableName, dictonaryField, dictonaryValue);
                result = ExecuteScalar(dictonaryQuery, dictonaryParams.ToArray());
            }
            else
            {
                result = ExecuteScalar(string.Format("INSERT INTO {0} DEFAULT VALUES; SELECT CAST(SCOPE_IDENTITY() AS VARCHAR(50)) AS LAST_IDENTITY;", tableName));
            }
            return result;
        }

        public int Update(Dictionary<string, object> model, string tableName, string where)
        {
            var dictonaryParams = model.Select(o => new SqlParameter("@" + o.Key, o.Value)).ToList();
            var dictonary = model.Aggregate(string.Empty, (current, o) => current + o.Key + "=" + "@" + o.Key + ",");
            var dictonaryQuery = string.Format("UPDATE {2} SET {0} WHERE {1}",
                dictonary.Remove(dictonary.LastIndexOf(",", StringComparison.Ordinal)), where, tableName);
            var result = ExecuteCommand(dictonaryQuery, dictonaryParams.ToArray());
            return result;
        }

        #endregion

        #region IDisposable Members

        ~Repository()
        {
            Dispose(false);
        }

        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (Context != null)
                    {
                        Context.Dispose();
                        Context = null;
                    }
                }
            }
            disposed = true;
        }


        #endregion
        #region LINQ ASYNC

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.SingleOrDefaultAsync(predicate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> CreateAsync(T entity)
        {
            DbSet.Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var entry = Context.Entry(item);
            DbSet.Attach(item);
            entry.State = EntityState.Modified;
            return await Context.SaveChangesAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Expression<Func<T, bool>> predicate)
        {
            var records = FindAll(predicate);
            if (!records.Any())
            {
                throw new ObjectNotFoundException();
            }
            foreach (var record in records)
            {
                var entry = Context.Entry(record);

                DbSet.Attach(record);

                entry.State = EntityState.Modified;
            }
            return await Context.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(T t)
        {
            DbSet.Remove(t);
            return await Context.SaveChangesAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var records = await DbSet.Where(predicate).ToListAsync();
            if (!records.Any())
            {
                throw new ObjectNotFoundException();
            }
            foreach (var record in records)
            {
                DbSet.Remove(record);
            }
            return await Context.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<long> LongCountAsync()
        {
            return await DbSet.LongCountAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> CountFuncAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.CountAsync(predicate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<long> LongCountFuncAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.LongCountAsync(predicate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> FirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.FirstAsync(predicate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<string> MaxAsync(Expression<Func<T, string>> predicate)
        {
            return await DbSet.MaxAsync(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<string> MaxFuncAsync(Expression<Func<T, string>> predicate, Expression<Func<T, bool>> where)
        {
            return await DbSet.Where(where).MaxAsync(predicate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<string> MinAsync(Expression<Func<T, string>> predicate)
        {
            return await DbSet.MinAsync(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<string> MinFuncAsync(Expression<Func<T, string>> predicate, Expression<Func<T, bool>> where)
        {
            return await DbSet.Where(where).MinAsync(predicate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate)
        {
            var count = await DbSet.CountAsync(predicate);
            return count > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}
