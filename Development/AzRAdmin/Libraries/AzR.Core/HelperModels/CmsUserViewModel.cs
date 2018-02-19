using System;
using System.Collections.Generic;
using System.Linq;

namespace AzR.Core.HelperModels
{
    public class CmsUserViewModel
    {

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public long Expaired { get; set; }
        public string UserImage { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }

        private List<int> _roleIds = new List<int>();

        public List<int> RoleIdList
        {
            get { return _roleIds; }
            set { _roleIds = value; }
        }

        public string RoleIds
        {
            get { return string.Join(",", _roleIds); }
            set
            {
                if (string.IsNullOrEmpty(value)) _roleIds = new List<int> { 0 };
                else
                {
                    _roleIds = value.Contains(",")
                        ? value.Split(',').Select(s => Convert.ToInt32(s.Trim())).ToList()
                        : new List<int> { Convert.ToInt32(value) };
                }
            }
        }

        private List<string> _roleName = new List<string>();

        public List<string> RoleNameList
        {
            get { return _roleName; }
            set { _roleName = value; }
        }

        public string RoleNames
        {
            get { return string.Join(",", _roleName); }
            set
            {
                _roleName = value.Contains(",")
                    ? value.Split(',').Select(s => s.Trim()).ToList()
                    : new List<string> { value };
            }
        }

    }
}
