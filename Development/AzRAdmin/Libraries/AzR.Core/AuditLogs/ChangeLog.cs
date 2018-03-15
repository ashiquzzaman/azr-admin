using System.Collections.Generic;
using AzR.Core.Enumerations;
using AzR.Utilities.Models;

namespace AzR.Core.AuditLogs
{
    public class ChangeLog
    {
        public ChangeLog()
        {
            Changes = new List<ObjectChangeLog>();
        }
        public string ActionTime { get; set; }
        public ActionType ActionType { get; set; }
        public string ActionTypeName { get; set; }
        public string ActionBy { get; set; }
        public string ActionUrl { get; set; }
        public string ActionAgent { get; set; }
        public List<ObjectChangeLog> Changes { get; set; }
        public string KeyFieldId { get; set; }
    }

}