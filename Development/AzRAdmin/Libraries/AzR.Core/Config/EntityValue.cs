using System;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace AzR.Core.Config
{
    public class EntityValue
    {
        public object GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }
        public object NewObject(DbEntityEntry entry, Type entityType = null)
        {
            var newEntity = entry.CurrentValues.PropertyNames.ToDictionary(key => key,
                key => entry.CurrentValues.GetValue<object>(key));
            var type = entityType ?? entry.Entity.GetType();

            var newObj = Activator.CreateInstance(type);
            foreach (var kv in newEntity)
            {
                if (type.GetProperty(kv.Key) != null)
                {
                    type.GetProperty(kv.Key).SetValue(newObj, kv.Value);
                }

            }
            return newObj;
        }
        public object OldObject(DbEntityEntry entry, Type entityType = null)
        {
            var oldEntity = entry.OriginalValues.PropertyNames.ToDictionary(key => key,
                key => entry.OriginalValues.GetValue<object>(key));
            var type = entityType ?? entry.Entity.GetType();

            var oldObj = Activator.CreateInstance(type);
            foreach (var kv in oldEntity)
            {
                if (type.GetProperty(kv.Key) != null)
                {
                    type.GetProperty(kv.Key).SetValue(oldObj, kv.Value);
                }

            }
            return oldObj;
        }
    }

}
