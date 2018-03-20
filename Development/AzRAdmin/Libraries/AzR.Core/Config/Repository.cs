using AzR.Core.AuditLogs;
using AzR.Core.Enumerations;
using AzR.Core.Notifications;
using AzR.Utilities.Attributes;
using AzR.Utilities.Exentions;
using AzR.Utilities.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;

// By: Ashiquzzaman;

namespace AzR.Core.Config
{

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbContext _context;//IAppDbContext
        private bool _shareContext;
        private bool disposed = false;
        public bool ShareContext
        {
            get { return _shareContext; }
            set { _shareContext = value; }
        }

        public DbContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;

            }
        }

        public Repository(DbContext context)
        {
            Context = context;
            _context.Database.CommandTimeout = 100000;
        }

        protected DbSet<TEntity> DbSet
        {
            get
            {
                return _context.Set<TEntity>();
            }
        }

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
            if (ShareContext || _context == null) return;
            if (!disposed)
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
            disposed = true;
        }



        public IQueryable<TEntity> GetAll
        {
            get
            {
                return DbSet.AsNoTracking().AsQueryable();
            }
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }
        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.First(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public int Count
        {
            get { return DbSet.AsNoTracking().Count(); }
        }

        public int Counting(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().Count(predicate);
        }

        public string MaxValue(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where)
        {
            return DbSet.Where(where).AsNoTracking().Max(predicate);
        }

        public string Max(Expression<Func<TEntity, string>> predicate)
        {
            return DbSet.AsNoTracking().Max(predicate);
        }

        public TEntity Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().FirstOrDefault(predicate);
        }


        public IQueryable<string> Select(Expression<Func<TEntity, string>> predicate)
        {
            return DbSet.Select(predicate);
        }


        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).AsNoTracking().AsQueryable();
        }

        public TEntity Create(TEntity item)
        {
            DbSet.Add(item);

            if (!ShareContext)
            {
                SaveChanges();
            }

            return item;
        }

        public int Update(TEntity item)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var entry = _context.Entry(item);
                    var entityValue = new EntityValue();

                    DbSet.Attach(item);

                    entry.State = EntityState.Modified;
                    var oldObject = entityValue.OldObject(entry);


                    var properties = typeof(TEntity).GetProperties()
                        .Where(property =>
                            property != null && Attribute.IsDefined(property, typeof(IgnoreUpdateAttribute)))
                        .Select(p => p.Name);
                    foreach (var property in properties)
                    {
                        entry.Property(property).IsModified = false;
                    }


                    var keyValue = entry.OriginalValues.PropertyNames.Any(s => s == "Id")
                        ? entry.OriginalValues.GetValue<object>("Id").ToString()
                        : "0";


                    var audit = WriteLog.Create<TEntity>(ActionType.Update, keyValue, (TEntity)oldObject, item);
                    if (audit == null) return 0;
                    var auditlog = _context.Set<AuditLog>();
                    auditlog.Add(audit);

                    var result = !ShareContext ? _context.SaveChanges() : 0;
                    scope.Complete();
                    return result;
                }
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                throw;
            }
        }

        public int Delete(TEntity item)
        {
            DbSet.Remove(item);

            return !ShareContext ? SaveChanges() : 0;
        }

        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var records = FindAll(predicate);

            foreach (var record in records)
            {
                DbSet.Remove(record);
            }
            return !ShareContext ? SaveChanges() : 0;
        }

        public bool IsExist(Expression<Func<TEntity, bool>> predicate)
        {
            var count = DbSet.Count(predicate);
            return count > 0;
        }
        public IEnumerable<T> ExecuteQuery<T>(string sqlQuery, params object[] parameters)
        {
            return _context.Database.SqlQuery<T>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }
        public IEnumerable DynamicExecuteQuery(string sqlQuery, params object[] parameters)
        {
            return _context.Database.DynamicSqlQuery(sqlQuery, parameters);
        }

        public dynamic DynamicFirst(string sqlQuery, params object[] parameters)
        {
            return _context.Database.DynamicSqlQuery(sqlQuery, parameters).Cast<dynamic>().FirstOrDefault(o => o != null);
        }
        public string ToId(Expression<Func<TEntity, string>> predicate, string prefix, int returnLength = 9, char fillValue = '0')
        {
            return DbSet.Max(predicate).MakeId(prefix, returnLength, fillValue);
        }

        public string CreateId(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where, string prefix,
            int returnLength = 9, char fillValue = '0')
        {
            return DbSet.Where(where).AsNoTracking().Max(predicate).MakeId(prefix, returnLength, fillValue);
        }
        #endregion

        #region LINQ ASYNC

        public async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }
        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).AsNoTracking().ToListAsync();
        }
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            DbSet.Add(entity);


            await SaveChangesAsync();
            return entity;
        }
        public async Task<int> UpdateAsync(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var entry = _context.Entry(item);
            DbSet.Attach(item);
            entry.State = EntityState.Modified;
            return await SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var records = Where(predicate);
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
            return await SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(TEntity t)
        {
            DbSet.Remove(t);
            return await SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
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
            return await SaveChangesAsync();
        }
        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }
        public async Task<long> LongCountAsync()
        {
            return await DbSet.LongCountAsync();
        }
        public async Task<int> CountFuncAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.CountAsync(predicate);
        }
        public async Task<long> LongCountFuncAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.LongCountAsync(predicate);
        }
        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.FirstAsync(predicate);
        }
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }
        public async Task<string> MaxAsync(Expression<Func<TEntity, string>> predicate)
        {
            return await DbSet.MaxAsync(predicate);
        }
        public async Task<string> MaxFuncAsync(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where)
        {
            return await DbSet.Where(where).AsNoTracking().MaxAsync(predicate);
        }
        public async Task<string> MinAsync(Expression<Func<TEntity, string>> predicate)
        {
            return await DbSet.AsNoTracking().MinAsync(predicate);
        }
        public async Task<string> MinFuncAsync(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where)
        {
            return await DbSet.Where(where).AsNoTracking().MinAsync(predicate);
        }
        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var count = await DbSet.CountAsync(predicate);
            return count > 0;
        }
        #endregion

        #region Dictionary

        public string Create(Dictionary<string, object> model, string tableName)
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
            var result = ExecuteQuery<string>(dictonaryQuery, dictonaryParams.ToArray()).FirstOrDefault();
            return result;
        }

        public int Update(Dictionary<string, object> model, string tableName, string id)
        {
            var dictonaryParams = model.Select(o => new SqlParameter("@" + o.Key, o.Value)).ToList();
            var dictonary = model.Aggregate(string.Empty, (current, o) => current + o.Key + "=" + "@" + o.Key + ",");
            var dictonaryQuery = string.Format("UPDATE {2} SET {0} WHERE Id={1}",
                dictonary.Remove(dictonary.LastIndexOf(",", StringComparison.Ordinal)), id, tableName);
            var result = ExecuteCommand(dictonaryQuery, dictonaryParams.ToArray());
            return result;
        }

        #endregion



        public int SaveChanges()
        {
            try
            {
                var ignorClass = typeof(TEntity).IsDefined(typeof(IgnoreLogAttribute), false);
                return ignorClass ? _context.SaveChanges() : CreateLog();
            }
            catch (DbEntityValidationException ex)
            {
                var outputLines = new List<string>();
                foreach (var eve in ex.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                        DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    outputLines.AddRange(eve.ValidationErrors.Select(ve => string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage)));
                }
                GeneralHelper.WriteValue(string.Join("\n", outputLines));

                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                GeneralHelper.WriteValue(string.Join("\n", outputLines));
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                throw new Exception(ex.Message);
            }
        }



        private int CreateLog()
        {
            using (var scope = new TransactionScope())
            {
                var changes = 0;
                var logList = new List<AuditLog>();



                var addedEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();

                if (addedEntries.Count > 0)
                {
                    _context.SaveChanges();
                    foreach (var entry in addedEntries)
                    {
                        var audit = WriteLog.Create(entry, 1);
                        if (audit == null) continue;
                        logList.Add(audit);
                    }
                }

                var modifiedEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();

                if (modifiedEntries.Count > 0)
                {
                    foreach (var entry in modifiedEntries)
                    {
                        var properties = typeof(TEntity).GetProperties()
                            .Where(property =>
                                property != null && Attribute.IsDefined(property, typeof(IgnoreUpdateAttribute)))
                            .Select(p => p.Name);
                        foreach (var property in properties)
                        {
                            entry.Property(property).IsModified = false;
                        }

                        var audit = WriteLog.Create(entry, 2);
                        if (audit == null) continue;
                        logList.Add(audit);
                    }
                }

                var deleteEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();
                if (deleteEntries.Count > 0)
                {
                    foreach (var entry in deleteEntries)
                    {
                        var audit = WriteLog.Create(entry, 3);
                        if (audit == null) continue;
                        logList.Add(audit);
                    }
                }
                var auditlog = _context.Set(typeof(AuditLog));
                auditlog.AddRange(logList);
                changes = _context.SaveChanges();

                scope.Complete();
                return changes;
            }
        }

        public async Task<int> SaveChangesAsync()
        {

            try
            {
                var ignorClass = typeof(TEntity).IsDefined(typeof(IgnoreLogAttribute), false);
                return ignorClass ? await _context.SaveChangesAsync() : await CreateLogAsync();
            }
            catch (DbEntityValidationException ex)
            {
                var outputLines = new List<string>();
                foreach (var eve in ex.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                        DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    outputLines.AddRange(eve.ValidationErrors.Select(ve => string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage)));
                }
                GeneralHelper.WriteValue(string.Join("\n", outputLines));

                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                GeneralHelper.WriteValue(string.Join("\n", outputLines));
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex)
            {
                ex.WriteLog();
                throw new Exception(ex.Message);
            }
        }



        private Task<int> CreateLogAsync()
        {
            using (var scope = new TransactionScope())
            {
                var changes = Task.FromResult(0);
                var logList = new List<AuditLog>();
                var notifies = new List<Notification>();
                var addedEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();

                if (addedEntries.Count > 0)
                {
                    _context.SaveChanges();
                    foreach (var entry in addedEntries)
                    {
                        var audit = WriteLog.Create(entry, 1);
                        if (audit == null) continue;
                        logList.Add(audit);
                        var notify = Notification.ActionNotifyForGroup(entry, 1, audit.Id);
                        if (notify == null) continue;
                        notifies.Add(notify);
                    }
                }


                var modifiedEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();

                if (modifiedEntries.Count > 0)
                {
                    foreach (var entry in modifiedEntries)
                    {
                        var properties = typeof(TEntity).GetProperties()
                            .Where(property =>
                                property != null && Attribute.IsDefined(property, typeof(IgnoreUpdateAttribute)))
                            .Select(p => p.Name);
                        foreach (var property in properties)
                        {
                            entry.Property(property).IsModified = false;
                        }

                        var audit = WriteLog.Create(entry, 2);
                        if (audit == null) continue;
                        logList.Add(audit);

                        var notify = Notification.ActionNotifyForGroup(entry, 1, audit.Id);
                        if (notify == null) continue;
                        notifies.Add(notify);
                    }
                }

                var deleteEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();
                if (deleteEntries.Count > 0)
                {
                    foreach (var entry in deleteEntries)
                    {
                        var audit = WriteLog.Create(entry, 3);
                        if (audit == null) continue;
                        logList.Add(audit);

                        var notify = Notification.ActionNotifyForGroup(entry, 1, audit.Id);
                        if (notify == null) continue;
                        notifies.Add(notify);
                    }
                }

                var auditlog = _context.Set(typeof(AuditLog));
                auditlog.AddRange(logList);

                var notifications = _context.Set(typeof(Notification));
                notifications.AddRange(notifies);

                changes = _context.SaveChangesAsync();


                NotificationHub.Notify();

                scope.Complete();
                return changes;
            }
        }

    }
}
