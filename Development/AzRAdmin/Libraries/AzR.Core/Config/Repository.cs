﻿using AzR.Core.AuditLogs;
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
        #region CONFIG

        private DbContext _context;//IAppDbContext
        private bool _shareContext;
        private bool _disposed;
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


        #endregion

        #region Disposed

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

        #region LINQ

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

        public int CounFunc(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().Count(predicate);
        }

        public string MaxFunc(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where)
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
            var entry = _context.Entry(item);
            DbSet.Attach(item);
            entry.State = EntityState.Modified;
            return !ShareContext ? SaveChanges() : 0;
        }

        public TEntity CreateOrUpdate(Expression<Func<TEntity, bool>> predicate, TEntity newItem)
        {
            var record = DbSet.FirstOrDefault(predicate);
            if (record == null)
            {
                DbSet.Add(newItem);
            }
            else
            {
                var entry = _context.Entry(record);
                //DbSet.Attach(record);
                entry.CurrentValues.SetValues(newItem);
            }


            var result = !ShareContext ? SaveChanges() : 0;
            return result > 0 ? newItem : null;
        }
        public TEntity CreateOrUpdate(TEntity item)
        {
            var pi = item.GetType().GetProperty("Id");
            var keyFieldId = pi != null ? pi.GetValue(item, null) : 0;

            var record = DbSet.Find(keyFieldId);
            if (record == null)
            {
                DbSet.Add(item);
            }
            else
            {
                _context.Entry(record).CurrentValues.SetValues(item);
            }

            var result = !ShareContext ? SaveChanges() : 0;
            return result > 0 ? item : null;
        }

        public int Update(Expression<Func<TEntity, bool>> predicate)
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
            return !ShareContext ? SaveChanges() : 0;
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

        #region SQL
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

        public async Task<TEntity> CreateOrUpdateAsync(TEntity item)
        {
            var pi = item.GetType().GetProperty("Id");
            var keyFieldId = pi != null ? pi.GetValue(item, null) : 0;

            var record = await DbSet.FindAsync(keyFieldId);
            if (record == null)
            {
                DbSet.Add(item);
            }
            else
            {
                _context.Entry(record).CurrentValues.SetValues(item);
            }

            var result = !ShareContext ? await SaveChangesAsync() : 0;
            return result > 0 ? item : null;
        }
        public async Task<TEntity> CreateOrUpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity newItem)
        {
            var record = await DbSet.FirstOrDefaultAsync(predicate);
            if (record == null)
            {
                DbSet.Add(newItem);
            }
            else
            {
                var entry = _context.Entry(record);
                // DbSet.Attach(record);
                entry.CurrentValues.SetValues(newItem);
            }

            var result = !ShareContext ? await SaveChangesAsync() : 0;
            return result > 0 ? newItem : null;

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

            var result = _context.Database.SqlQuery<string>(dictonaryQuery, dictonaryParams.ToArray()).FirstOrDefault();
            return result;
        }

        public int Update(Dictionary<string, object> model, string tableName, string id)
        {
            var dictonaryParams = model.Select(o => new SqlParameter("@" + o.Key, o.Value)).ToList();
            var dictonary = model.Aggregate(string.Empty, (current, o) => current + o.Key + "=" + "@" + o.Key + ",");
            var dictonaryQuery = string.Format("UPDATE {2} SET {0} WHERE Id={1}",
                dictonary.Remove(dictonary.LastIndexOf(",", StringComparison.Ordinal)), id, tableName);
            var result = _context.Database.ExecuteSqlCommand(dictonaryQuery, dictonaryParams.ToArray());
            return result;
        }

        #endregion

        #region SaveChange

        public int SaveChanges()
        {
            try
            {
                var ignorClass = typeof(TEntity).IsDefined(typeof(IgnoreLogAttribute), false);
                int result;

                using (var scope = new TransactionScope())
                {
                    if (!ignorClass) CreateLog();
                    result = _context.SaveChanges();
                    scope.Complete();
                }

                NotificationHub.Notify();
                return result;
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
        public async Task<int> SaveChangesAsync()
        {

            try
            {
                var ignorClass = typeof(TEntity).IsDefined(typeof(IgnoreLogAttribute), false);
                int result;

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (!ignorClass) CreateLog();
                    result = await _context.SaveChangesAsync();
                    scope.Complete();
                }

                NotificationHub.Notify();
                return result;
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


        #endregion

        #region LogNotify

        public int CreateLog()
        {
            var logList = new List<AuditLog>();
            var notifies = new List<Notification>();
            var addedEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();

            if (addedEntries.Count > 0)
            {
                _context.SaveChanges();
                foreach (var entry in addedEntries)
                {
                    var audit = WriteLog.Create(entry, 1, typeof(TEntity));
                    if (audit == null) continue;
                    logList.Add(audit);
                    var notify = Notification.ActionNotifyForGroup(entry, 1, audit.Id, typeof(TEntity));
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

                    var audit = WriteLog.Create(entry, 2, typeof(TEntity));
                    if (audit == null) continue;
                    logList.Add(audit);

                    var notify = Notification.ActionNotifyForGroup(entry, 1, audit.Id, typeof(TEntity));
                    if (notify == null) continue;
                    notifies.Add(notify);
                }
            }

            var deleteEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();
            if (deleteEntries.Count > 0)
            {
                foreach (var entry in deleteEntries)
                {
                    var audit = WriteLog.Create(entry, 3, typeof(TEntity));
                    if (audit == null) continue;
                    logList.Add(audit);

                    var notify = Notification.ActionNotifyForGroup(entry, 1, audit.Id, typeof(TEntity));
                    if (notify == null) continue;
                    notifies.Add(notify);
                }
            }

            if (addedEntries.Count <= 0 && modifiedEntries.Count <= 0 && deleteEntries.Count <= 0) return 0;

            LogNotifyList(logList, notifies);

            return 1;
        }
        public void LogNotifyList(IEnumerable<AuditLog> logList, IEnumerable<Notification> notifies)
        {
            //if (_context.GetType() == typeof(ApplicationDbContext))
            //{
            var auditlog = _context.Set(typeof(AuditLog));
            auditlog.AddRange(logList);

            var notifications = _context.Set(typeof(Notification));
            notifications.AddRange(notifies);
            //}
            //else
            //{
            //    using (var db = ApplicationDbContext.Create())
            //    {
            //        db.AuditLogs.AddRange(logList);
            //        db.Notifications.AddRange(notifies);
            //        db.SaveChangesAsync();
            //    }
            //}

        }
        public void LogList(IEnumerable<AuditLog> logList)
        {
            //if (_context.GetType() == typeof(ApplicationDbContext))
            //{
            var auditlog = _context.Set(typeof(AuditLog));
            auditlog.AddRange(logList);
            //}
            //else
            //{
            //    using (var db = ApplicationDbContext.Create())
            //    {
            //        db.AuditLogs.AddRange(logList);
            //        db.SaveChangesAsync();
            //    }
            //}

        }
        public void NotifyList(IEnumerable<Notification> notifies)
        {
            //if (_context.GetType() == typeof(ApplicationDbContext))
            //{
            var notifications = _context.Set(typeof(Notification));
            notifications.AddRange(notifies);
            //}
            //else
            //{
            //    using (var db = ApplicationDbContext.Create())
            //    {
            //        db.Notifications.AddRange(notifies);
            //        db.SaveChangesAsync();
            //    }
            //}

        }
        public void LogNotify(AuditLog log, Notification notify)
        {
            //if (_context.GetType() == typeof(ApplicationDbContext))
            //{
            var auditlog = _context.Set(typeof(AuditLog));
            auditlog.Add(log);

            var notifications = _context.Set(typeof(Notification));
            notifications.Add(notify);
            //}
            //else
            //{
            //    using (var db = ApplicationDbContext.Create())
            //    {
            //        db.AuditLogs.Add(log);
            //        db.Notifications.Add(notify);
            //        db.SaveChangesAsync();
            //    }
            //}

        }
        public void Log(AuditLog log)
        {
            //if (_context.GetType() == typeof(ApplicationDbContext))
            //{
            var auditlog = _context.Set(typeof(AuditLog));
            auditlog.Add(log);
            //}
            //else
            //{
            //    using (var db = ApplicationDbContext.Create())
            //    {
            //        db.AuditLogs.Add(log);
            //        db.SaveChangesAsync();
            //    }
            //}

        }
        public void Notify(Notification notify)
        {
            //if (_context.GetType() == typeof(ApplicationDbContext))
            //{
            var notifications = _context.Set(typeof(Notification));
            notifications.Add(notify);
            //}
            //else
            //{
            //    using (var db = ApplicationDbContext.Create())
            //    {
            //        db.Notifications.Add(notify);
            //        db.SaveChangesAsync();
            //    }
            //}

        }
        #endregion

    }
}
