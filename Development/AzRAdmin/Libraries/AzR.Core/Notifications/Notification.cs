using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AzR.Core.Enumerations;
using AzR.Utilities.Attributes;
using AzR.Utilities.Exentions;

namespace AzR.Core.Notifications
{
    [IgnoreLog]
    public class Notification
    {
        public Notification()
        {
            UserNotifications = new List<UserNotification>();

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(128)]
        public string Id { get; set; }
        [StringLength(128)]
        public string AuditLogId { get; set; }

        [StringLength(128)]
        public string KeyFieldId { get; set; }
        [StringLength(256)]
        public string EntityName { get; set; }

        public NotificationType NotificationType { get; set; }
        public ActionType? ActionType { get; set; }
        public NotifiedBody NotifiedBody { get; set; }

        private List<long> _notifiers = new List<long>();
        public List<long> NotifierList
        {
            get { return _notifiers; }
            set { _notifiers = value; }
        }

        public string NotifiedIds
        {
            get { return string.Join(",", _notifiers); }
            set
            {
                if (string.IsNullOrEmpty(value)) _notifiers = new List<long> { 0 };
                else
                {
                    _notifiers = value.Contains(",")
                        ? value.Split(',').Select(s => Convert.ToInt64(s.Trim())).ToList()
                        : new List<long> { Convert.ToInt64(value) };
                }
            }
        }
        public Module Module { get; set; }
        [StringLength(500)]
        public string EntityFullName { get; set; }
        [StringLength(500)]
        public string ActionUrl { get; set; }

        public long ModifiedBy { get; set; }
        public bool Active { get; set; }
        public long CreatedTime { get; set; }
        public List<UserNotification> UserNotifications { get; set; }
        public static Notification ActionNotifyForGroup(DbEntityEntry entry, int status, string auditLogId)
        {
            var eType = entry.Entity.GetType();
            var type = Type.GetType(eType.FullName) ?? eType.BaseType;
            var ignorClass = type.IsDefined(typeof(IgnoreLogAttribute), false);
            long userId = 0;
            if (ignorClass)
            {
                return null;
            }
            var actionType = (ActionType)status;
            string keyValue;

            switch (actionType)
            {
                case Enumerations.ActionType.Create:
                    {

                        keyValue = entry.OriginalValues.PropertyNames.Any(s => s == "Id")
                            ? entry.OriginalValues.GetValue<object>("Id").ToString()
                            : "0";
                        userId = entry.OriginalValues.PropertyNames.Any(s => s == "ModifiedBy")
                            ? entry.OriginalValues.GetValue<long>("ModifiedBy")
                            : 0;
                        break;
                    }
                case Enumerations.ActionType.Delete:
                    keyValue = entry.OriginalValues.PropertyNames.Any(s => s == "Id")
                        ? entry.OriginalValues.GetValue<object>("Id").ToString()
                        : "0";
                    userId = entry.OriginalValues.PropertyNames.Any(s => s == "ModifiedBy")
                        ? entry.CurrentValues.GetValue<long>("ModifiedBy")
                        : 0;

                    break;
                default:
                    {
                        if (entry.OriginalValues.PropertyNames.Any(s => s == "Active") &&
                            !entry.CurrentValues.GetValue<bool>("Active"))
                        {
                            actionType = Enumerations.ActionType.Cancel;
                        }
                        keyValue = entry.OriginalValues.PropertyNames.Any(s => s == "Id")
                            ? entry.OriginalValues.GetValue<object>("Id").ToString()
                            : "0";
                        userId = entry.OriginalValues.PropertyNames.Any(s => s == "ModifiedBy")
                            ? entry.CurrentValues.GetValue<long>("ModifiedBy")
                            : 0;
                        break;
                    }
            }

            var notify = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                AuditLogId = auditLogId,
                ActionType = actionType,
                EntityName = type.Name,
                EntityFullName = type.FullName,
                KeyFieldId = keyValue,
                ModifiedBy = userId,
                ActionUrl = "",
                NotifiedBody = NotifiedBody.ROLEGROUP,
                NotificationType = NotificationType.Action,
                NotifierList = new List<long> { 1, 2 },
                Module = Module.General,
                CreatedTime = DateTime.UtcNow.ToLong(),
            };
            return notify;
        }

    }
}
