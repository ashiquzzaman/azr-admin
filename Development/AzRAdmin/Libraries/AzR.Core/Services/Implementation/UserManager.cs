using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AzR.Core.HelperModels;
using AzR.Core.IdentityConfig;
using AzR.Core.Repositoies.Interface;
using AzR.Core.Services.Interface;
using AzR.Core.ViewModels.Admin;
using AzR.Utilities;

namespace AzR.Core.Services.Implementation
{
    public class UserManager : IUserManager
    {
        private readonly IRoleRepository _roles;
        private readonly IUserRepository _user;
        private readonly IOrganizationRepository _organization;

        public UserManager(IRoleRepository roles, IUserRepository user, IOrganizationRepository organization)
        {
            _roles = roles;
            _user = user;
            _organization = organization;
        }

        public ApplicationUser GetById(int id)
        {
            return _user.GetUserById(id);
        }

        public IEnumerable<UserViewModel> GetAllInstituteUsers(int instituteId)
        {
            var userList = _user.GetAllInstituteUsers(instituteId).Where(u => u.UserName != "ashiquzzaman@outlook.com");
            var users = userList.Select(user => new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                OrgId = user.OrgId,
                ImageUrl = user.ImageUrl,
                InVacation = user.InVacation,
                Created = user.Created,
                Expired = user.Expired,
                Active = user.IsActive,
            }).ToList();
            return users;
        }
        public bool Create(UserViewModel model, int uid = 0, int sid = 0)
        {
            var existingUser = _user.GetUserByUserName(model.Email);
            var existingUserName = _user.IsExist(model.UserName);
            if (existingUser != null || string.IsNullOrWhiteSpace(model.NewPassword) || existingUserName)
            {
                return false;
            }

            var newUser = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Created = DateTime.UtcNow.ToLong(),
                Expired = model.Expired ?? DateTime.UtcNow.ToLong(),
                ImageUrl = model.ImageUrl,
                InVacation = model.InVacation,
                OrgId = model.OrgId ?? 1,
                IsActive = model.Active,
                AgentId = PcUniqueNumber.GetUserAgentInfo,
                Modified = DateTime.UtcNow.ToLong(),
            };
            _user.Create(newUser, model.NewPassword);
            foreach (var item in model.RoleNameList)
            {
                _user.AddUserToRole(newUser, item);
            }

            return true;
        }
        public ApplicationUser Create(UserViewModel model)
        {

            var existingUser = _user.GetUserByUserName(model.Email);
            var existingUserName = _user.IsExist(model.UserName);
            if (existingUser != null || string.IsNullOrWhiteSpace(model.NewPassword) || existingUserName)
            {
                return null;
            }

            var newUser = new ApplicationUser
            {
                EmployeeId = model.EmployeeId,
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                OrgId = model.OrgId ?? 1,
                Created = DateTime.UtcNow.ToLong(),
                Expired = model.Expired ?? DateTime.UtcNow.ToLong(),
                ImageUrl = model.ImageUrl ?? "/Images/user.png",
                IsActive = model.Active,
                InVacation = model.InVacation,
                AgentId = PcUniqueNumber.GetUserAgentInfo,
                Modified = DateTime.UtcNow.ToLong(),
            };
            _user.Create(newUser, model.NewPassword);
            foreach (var item in model.RoleNameList)
            {
                _user.AddUserToRole(newUser, item);
            }
            return newUser;
        }

        public IEnumerable<UserViewModel> GetAll()
        {
            var userList = _user.FindAll(u => u.UserName != "ashiquzzaman@outlook.com" && u.IsActive).Include(s => s.Roles).AsEnumerable();
            var users = userList.Select(user => new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                OrgId = user.OrgId,
                Created = user.Created,
                Expired = user.Expired,
                Active = user.IsActive,
                ImageUrl = user.ImageUrl,
                InVacation = user.InVacation,
                RoleNames = string.Join(", ", user.Roles.Select(r => _roles.FindById(r.RoleId).Name))
            }).ToList();
            return users;
        }


        public ApplicationUser Update(UserViewModel model, int uid = 0, int sid = 0)
        {
            var user = _user.GetUserById(model.Id);
            if (user == null)
            {
                return null;
            }
            if (model.Email != user.Email)
            {
                var existingUser = _user.GetUserByUserName(model.Email);
                if (existingUser != null)
                {
                    return null;
                }
            }
            if (model.PhoneNumber != user.PhoneNumber)
            {
                var existingUser = _user.GetUserByUserName(model.PhoneNumber);
                if (existingUser != null)
                {
                    return null;
                }
            }
            if (model.UserName != user.UserName)
            {
                var existingUser = _user.GetUserByUserName(model.UserName);
                if (existingUser != null)
                {
                    return null;
                }
            }

            if (!string.IsNullOrWhiteSpace(model.NewPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                var newHashedPassword = _user.HashPassword(model.NewPassword);
                user.PasswordHash = newHashedPassword;

            }
            user.Expired = DateTime.UtcNow.AddMonths(3).ToLong();
            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.OrgId = model.OrgId ?? 1;
            user.IsActive = model.Active;
            user.ImageUrl = model.ImageUrl;
            user.Modified = DateTime.UtcNow.ToLong();
            user.AgentId = PcUniqueNumber.GetUserAgentInfo;
            user.IsActive = model.Active;
            _user.Update(user);
            if (!model.RoleNameList.Any()) return user;
            var roles = _user.GetRolesForUser(user);
            _user.RemoveUserFromRole(user, roles.ToArray());
            foreach (var item in model.RoleNameList)
            {
                _user.AddUserToRole(user, item);
            }
            return user;

        }
        public UserViewModel GetUserByName(string name)
        {
            var user = _user.GetUserByName(name);

            if (user == null)
            {
                return null;
            }
            var item = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                OrgId = user.OrgId,
                Active = user.IsActive,
                Created = user.Created
            };

            return item;
        }
        public string GetNameById(int id)
        {
            return _user.GetNameById(id);
        }
        public UserViewModel GetUserById(int id)
        {
            var user = _user.GetUserById(id);

            if (user == null)
            {
                return null;
            }

            var item = new UserViewModel
            {
                Id = user.Id,
                EmployeeId = user.EmployeeId,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                OrgId = user.OrgId,
                Created = user.Created,
                Expired = user.Expired,
                Active = user.IsActive,
                ImageUrl = user.ImageUrl,
                InVacation = user.InVacation,
            };
            var role = _user.GetRolesForUser(user).ToList();
            item.RoleNameList = role;
            return item;
        }
        public IEnumerable<ApplicationUser> GetUsersInRole(string roleName)
        {
            var us = _roles.FindByName(roleName).Users;
            if (us == null || !us.Any()) return null;
            var role = us.First();
            var usersInRole =
                _user.GetAllUsers().Where(u => u.Roles.Select(r => r.RoleId).Contains(role.RoleId)).ToList();
            return usersInRole;
        }

        public bool IsInRole(int userId, string name)
        {
            var roles = _user.FindAll(u => u.Id == userId).Include(r => r.Roles).SelectMany(y => y.Roles).ToList();
            if (!roles.Any()) { return false; }

            var role = from ur in roles
                       join r in _roles.FindAll(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase)).ToList()
                       on ur.RoleId equals r.Id
                       select r;
            return role.Any();
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllRoleByUsersAsync(int userId)
        {

            var roles = from ur in _user.FindAll(u => u.Id == userId).Include(r => r.Roles).SelectMany(y => y.Roles)
                        join r in _roles.All() on ur.RoleId equals r.Id
                        select r;
            return await roles.ToListAsync();

        }

        public IEnumerable<UserViewModel> GetAllUserByRole(string roleName)
        {
            var us = GetUsersInRole(roleName);
            if (us == null)
            {
                return new List<UserViewModel> { new UserViewModel() };
            }
            var users = us.Select(user => new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                OrgId = user.OrgId,
                Active = user.IsActive,
                Created = user.Created
            }).ToList();
            return users;
        }
        public IEnumerable<UserViewModel> GetAllUserByRole(string roleName, int orgId)
        {
            var roleUsers = GetUsersInRole(roleName);
            if (roleUsers == null)
            {
                return new List<UserViewModel> { new UserViewModel() };
            }
            var users = roleUsers.Where(u => u.OrgId == orgId && u.IsActive).Select(user => new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                OrgId = user.OrgId,
                Active = user.IsActive,
                Created = user.Created,
            }).ToList().OrderBy(u => u.Id);
            return users;
        }
        public void DeActive(int id)
        {
            var user = _user.GetBy(id);
            user.IsActive = false;
            user.AgentId = PcUniqueNumber.GetUserAgentInfo;
            _user.SaveChanges();
        }
        public async Task<int> DeActiveAsync(int id)
        {
            var user = _user.GetBy(id);
            user.IsActive = false;
            user.AgentId = PcUniqueNumber.GetUserAgentInfo;
            return await _user.SaveChangesAsync();
        }



        public IEnumerable<DropDownItem> LoadUser(int orgId, string role)
        {

            var list = GetUsersInRole(role).Where(b => b.OrgId == orgId && b.IsActive).Select(
                 p => new DropDownItem
                 {
                     Value = p.Id.ToString(),
                     Text = p.FullName
                 });
            var returnlist = list.ToList();
            return returnlist;
        }

        public List<DropDownItem> LoadUsers()
        {
            var result = _user.GetAll().Where(b => b.IsActive).Select(r => new DropDownItem
            {
                Value = r.Id.ToString(),
                Text = r.FullName
            }).ToList();
            return result;
        }


    }
}
