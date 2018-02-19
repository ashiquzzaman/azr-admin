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

namespace AzR.Core.AppContexts
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Members

        private DbContext _context;
        private bool _disposed;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of repository
        /// </summary>
        /// <param name="context"></param>
        protected Repository(DbContext context)
        {
            _context = context;
        }

        #endregion

        #region PROPERTY
        // Entity corresponding Database Table
        private DbSet<T> DbSet
        {
            get { return _context.Set<T>(); }
        }

        #endregion

        #region LINQ QUERY

        /// <summary>
        /// add a item in a table. item never be added until call savechanges method.
        /// </summary>
        /// <param name="item">object of a class which will be added into corresponding DB table.</param>
        public virtual void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            DbSet.Add(item); // add new item in this set
        }

        /// <summary>
        /// Remove a item in a table. item never be Removed until call savechanges method.
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
        /// Modify a item in a table. item never be Modified until call savechanges method.
        /// </summary>
        /// <param name="item">object of a class which will be Modified into corresponding DB table.</param>
        public virtual void Modify(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var entry = _context.Entry(item);
            DbSet.Attach(item);
            entry.State = EntityState.Modified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return DbSet.SingleOrDefault(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Single(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Single(predicate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> All()
        {
            return DbSet.AsNoTracking().AsQueryable();
        }

        public T Create(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            DbSet.Add(item);
            _context.SaveChanges();
            return item;
        }

        public int Update(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var entry = _context.Entry(item);
            DbSet.Attach(item);
            entry.State = EntityState.Modified;
            return _context.SaveChanges();
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
                var entry = _context.Entry(record);

                DbSet.Attach(record);

                entry.State = EntityState.Modified;
            }
            return _context.SaveChanges();
        }

        public int Delete(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            DbSet.Remove(item);

            return _context.SaveChanges();
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
            return _context.SaveChanges();
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
            return DbSet.AsNoTracking().SingleOrDefault(predicate);
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
            return DbSet.Where(predicate).AsNoTracking().AsQueryable();
        }

        #endregion

        #region SQL RAW QUERY

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return _context.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery)
        {
            return _context.Database.SqlQuery<TEntity>(sqlQuery);
        }
        public TEntity ExecuteSingleQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return _context.Database.SqlQuery<TEntity>(sqlQuery, parameters).FirstOrDefault();
        }

        public IEnumerable DynamicExecuteQuery(string sqlQuery, params object[] parameters)
        {
            return _context.Database.DynamicSqlQuery(sqlQuery, parameters);
        }

        public dynamic DynamicFirst(string sqlQuery, params object[] parameters)
        {
            return _context.Database.DynamicSqlQuery(sqlQuery, parameters).Cast<dynamic>().FirstOrDefault(o => o != null);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }
        public string ExecuteScalar(string sqlCommand, params object[] parameters)
        {
            return _context.Database.SqlQuery<string>(sqlCommand, parameters).Single();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="parameters"></param>
        public TEntity ExecuteScalar<TEntity>(string sqlCommand, params object[] parameters)
        {
            return _context.Database.SqlQuery<TEntity>(sqlCommand, parameters).Single();
        }

        #endregion

        #region DATABSE TRANSACTION

        public void Attach<TEntity>(TEntity item) where TEntity : class
        {
            _context.Entry(item).State = EntityState.Unchanged;
        }

        public void SetModified<TEntity>(TEntity item) where TEntity : class
        {
            //this operation also attach item in object state manager
            _context.Entry(item).State = EntityState.Modified;
        }

        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current)
            where TEntity : class
        {
            //if it is not attached, attach original and set current values
            _context.Entry(original).CurrentValues.SetValues(current);
        }

        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed;

            do
            {
                try
                {
                    _context.SaveChanges();

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
            _context.ChangeTracker.Entries()
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
                result = ExecuteScalar(dictonaryQuery, dictonaryParams);
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
            var result = ExecuteCommand(dictonaryQuery, dictonaryParams);
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

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                        _context = null;
                    }
                }
            }
            _disposed = true;
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
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> CreateAsync(T entity)
        {
            DbSet.Add(entity);
            await _context.SaveChangesAsync();
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
            var entry = _context.Entry(item);
            DbSet.Attach(item);
            entry.State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Expression<Func<T, bool>> predicate)
        {
            var records = await DbSet.Where(predicate).ToListAsync();
            if (!records.Any())
            {
                throw new ObjectNotFoundException();
            }
            foreach (var record in records)
            {
                var entry = _context.Entry(record);

                DbSet.Attach(record);

                entry.State = EntityState.Modified;
            }
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(T t)
        {
            DbSet.Remove(t);
            return await _context.SaveChangesAsync();
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
            return await _context.SaveChangesAsync();
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
        /// <returns></returns>
        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.SingleOrDefaultAsync(predicate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> SingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.SingleAsync(predicate);
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
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}
