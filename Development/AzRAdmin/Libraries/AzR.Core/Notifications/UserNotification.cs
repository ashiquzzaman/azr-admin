using AzR.Core.Config;
using AzR.Core.Enumerations;
using AzR.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AzR.Core.Notifications
{
    [IgnoreLog]
    public class UserNotification : IBaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(128)]
        public string Id { get; set; }

        private List<long> _userIds = new List<long>();
        public List<long> UserIdList
        {
            get { return _userIds; }
            set { _userIds = value; }
        }

        public string UserIds
        {
            get { return string.Join(",", _userIds); }
            set
            {
                if (string.IsNullOrEmpty(value)) _userIds = new List<long> { 0 };
                else
                {
                    _userIds = value.Contains(",")
                        ? value.Split(',').Select(s => Convert.ToInt64(s.Trim())).ToList()
                        : new List<long> { Convert.ToInt64(value) };
                }
            }
        }
        [StringLength(128)]
        public string NotificationId { get; set; }
        [ForeignKey("NotificationId")]
        public virtual Notification Notification { get; set; }
        [StringLength(256)]
        public string Alert { get; set; }
        [Display(Name = "Alert Type")]
        public AlertType AlertType { get; set; }
        public bool IsRead { get; set; }
        public long? AlertTime { get; set; }
        public long? ReadTime { get; set; }
        [StringLength(50)]
        public string CultureName { get; set; }

    }
}
