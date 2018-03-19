using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace AzR.Utilities.Securities
{
    public class AppUserPrincipal : IAppUserPrincipal
    {
        public AppUserPrincipal(string userName)
        {
            Identity = new GenericIdentity(userName);
        }
        public AppUserPrincipal(string userName, string type)
        {
            Identity = new GenericIdentity(userName, type);
        }
        public bool IsInRole(string role)
        {
            return RoleNameList.Any(m => m.ToLower().Equals(role.ToLower()));
        }

        public IIdentity Identity { get; private set; }

        public string Id { get; set; }

        public int UserId { get; set; }
        public int ProfileId { get; set; }

        public string UserName { get; set; }
        public string UniqueName { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public long Expired { get; set; }
        public int ActiveRoleId { get; set; }

        public string ActiveRoleName { get; set; }


        public int ActiveBranchId { get; set; }
        public int BranchId { get; set; }
        public int ParentBranchId { get; set; }
        public bool HasAllPermission { get { return ParentBranchId == 0; } }

        private List<int> _permittedBranch = new List<int>();

        public List<int> PermittedBranchList
        {
            get { return _permittedBranch; }
            set { _permittedBranch = value; }
        }

        public string PermittedBranchs
        {
            get { return string.Join(",", _permittedBranch); }
            set
            {
                if (string.IsNullOrEmpty(value)) _permittedBranch = new List<int> { 0 };
                else
                {
                    _permittedBranch = (value.Contains(","))
                        ? value.Split(',').Select(s => Convert.ToInt32(s.Trim())).ToList()
                        : _permittedBranch = new List<int> { Convert.ToInt32(value) };
                }
            }
        }


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

        public Dictionary<string, object> GetDictionary(AppUserPrincipal user)
        {
            var result = new Dictionary<string, object>
            {
                {"1",user.Id },
                {"2",user.UserId },
                {"3",user.UserName },
                {"4",user.Name },
                {"5",user.Phone },
                {"6",user.Email },
                {"7",user.Expired },
                {"8",user.ActiveBranchId },
                {"9",user.ParentBranchId },
                {"10",user.ActiveRoleName },
                {"11",user.RoleNames },
                {"12",user.ActiveRoleId },
                {"13",user.RoleIds },
                {"14",user.PermittedBranchs },
                {"15",user.UniqueName },
                {"16",user.BranchId },


            };
            return result;
        }



    }
}
