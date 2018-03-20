using AzR.Core.Config;
using AzR.Core.Enumerations;
using AzR.Utilities.Attributes;
using AzR.Utilities.Exentions;
using AzR.Utilities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace AzR.Core.AuditLogs
{
    public static class WriteLog
    {
        public static AuditLog Create<T>(ActionType action, string keyFieldId, T oldObject, T newObject)
        {

            var ignorClass = typeof(T).IsDefined(typeof(IgnoreLogAttribute), false);
            if (ignorClass)
            {
                return null;
            }
            var deltaList = newObject.Compare(oldObject);
            var audit = new AuditLog
            {
                Id = AppIdentity.AuditId,
                LoginId = AppIdentity.AppUser.Id,
                ActionType = action,
                EntityName = typeof(T).Name,
                EntityFullName = typeof(T).FullName,
                ActionTime = DateTime.UtcNow.ToLong(),
                BranchId = AppIdentity.AppUser.ActiveBranchId,
                KeyFieldId = keyFieldId,
                UserId = AppIdentity.AppUser.UserId,
                ActionUrl = "",
                ActionAgent = AppIdentity.AgentInfo,
                ActionUser = AppIdentity.AppUser.Name + "(" + AppIdentity.AppUser.UserName + ")",
                ValueBefore = JsonConvert.SerializeObject(oldObject),
                ValueAfter = JsonConvert.SerializeObject(newObject),
                ValueChange = JsonConvert.SerializeObject(deltaList)
            };
            return audit;
        }

        public static AuditLog Create(DbEntityEntry entry, int status, Type entityType = null)
        {
            var entityValue = new EntityValue();
            var type = entityType ?? entry.Entity.GetType();
            //   var type = Type.GetType(entry.Entity.GetType().FullName + "," + entry.Entity.GetType().Assembly.GetName().Name)?? entry.Entity.GetType().BaseType;

            var ignorClass = type.IsDefined(typeof(IgnoreLogAttribute), false);
            if (ignorClass)
            {
                return null;
            }
            var actionType = (ActionType)status;
            object oldObject, newObject;
            string keyValue;
            IEnumerable<ObjectChangeLog> deltaList;

            switch (actionType)
            {
                case ActionType.Create:
                    {
                        oldObject = Activator.CreateInstance(type);
                        newObject = entityValue.NewObject(entry, type);
                        deltaList = newObject.ToChangeLog();
                        keyValue = entry.OriginalValues.PropertyNames.Any(s => s == "Id")
                            ? entry.OriginalValues.GetValue<object>("Id").ToString()
                            : "0";
                        break;
                    }
                case ActionType.Delete:
                    oldObject = entityValue.OldObject(entry, type);
                    newObject = oldObject;
                    deltaList = newObject.Compare(oldObject);
                    keyValue = entry.OriginalValues.PropertyNames.Any(s => s == "Id")
                        ? entry.OriginalValues.GetValue<object>("Id").ToString()
                        : "0";

                    break;
                default:
                    {
                        if (entry.OriginalValues.PropertyNames.Any(s => s == "Active") &&
                            !entry.CurrentValues.GetValue<bool>("Active"))
                        {
                            actionType = ActionType.Cancel;
                        }
                        newObject = entityValue.NewObject(entry, type);
                        oldObject = entityValue.OldObject(entry, type);
                        deltaList = newObject.Compare(oldObject);
                        keyValue = entry.OriginalValues.PropertyNames.Any(s => s == "Id")
                            ? entry.OriginalValues.GetValue<object>("Id").ToString()
                            : "0";
                        break;
                    }
            }

            var audit = new AuditLog
            {
                Id = AppIdentity.AuditId,
                LoginId = AppIdentity.AppUser.Id,
                ActionType = actionType,
                EntityName = type.Name,
                EntityFullName = type.FullName,
                ActionTime = DateTime.UtcNow.ToLong(),
                KeyFieldId = keyValue,
                UserId = AppIdentity.AppUser.UserId,
                BranchId = AppIdentity.AppUser.ActiveBranchId,
                ActionUrl = "",
                ActionUser = AppIdentity.AppUser.Name + "(" + AppIdentity.AppUser.UserName + ")",
                ActionAgent = AppIdentity.AgentInfo,
                ValueBefore = JsonConvert.SerializeObject(oldObject),
                ValueAfter = JsonConvert.SerializeObject(newObject),
                ValueChange = JsonConvert.SerializeObject(deltaList)
            };

            return audit;
        }

    }
}
