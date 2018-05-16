using AzR.Core.Config;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AzR.Core.IdentityConfig
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationUserStore _store;
        private readonly ApplicationUserManager _manager;
        public UserRepository(DbContext context) : base(context)
        {
            _store = new ApplicationUserStore(context);
            _manager = new ApplicationUserManager(_store);
        }

        public IEnumerable<ApplicationUser> GetAllActive()
        {
            return FindAll(r => r.IsActive == true);
        }
        public IEnumerable<ApplicationUser> GetAllDeactive()
        {
            return FindAll(r => r.IsActive == false);
        }

        public ApplicationUser GetByName(string username)
        {
            return Find(u => u.UserName == username);
        }


        public ApplicationUser GetBy(int id)
        {
            return Find(u => u.Id == id);
        }

        public async Task<ApplicationUser> GetUserByNameAsync(string username)
        {
            return await _store.FindByNameAsync(username);
        }
        public async Task<bool> IsExistAsync(string authorName)
        {
            var count = await _store.Users.CountAsync(u => u.UserName == authorName);
            return count > 0;
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _store.Users.ToArrayAsync();

        }

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return _store.Users.ToList();
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            return await _store.IsInRoleAsync(user, roleName);
        }


        public IEnumerable<ApplicationUser> GetAllInstituteUsers(long instituteId)
        {
            return _store.Users.Where(u => u.BranchId == instituteId).Include(u => u.Roles);
        }
        public async Task CreateAsync(ApplicationUser user, string password)
        {
            await _manager.CreateAsync(user, password);
        }
        public IdentityResult Create(ApplicationUser user, string password)
        {
            var result = _manager.Create(user, password);
            return result;
        }
        public void AddUserToRole(ApplicationUser user, string role)
        {
            _manager.AddToRole(user.Id, role);
        }
        public async Task DeleteUserAsync(ApplicationUser user)
        {
            await _manager.DeleteAsync(user);
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            await _manager.UpdateAsync(user);
        }

        public bool VerifyUserPassword(string hashedPassword, string providedPassword)
        {
            return _manager.PasswordHasher.VerifyHashedPassword(hashedPassword, providedPassword) ==
                   PasswordVerificationResult.Success;
        }

        public string HashPassword(string password)
        {
            return _manager.PasswordHasher.HashPassword(password);
        }

        public async Task AddUserToRoleAsync(ApplicationUser user, string role)
        {
            await _manager.AddToRoleAsync(user.Id, role);
        }

        public async Task<IEnumerable<string>> GetRolesForUserAsync(ApplicationUser user)
        {
            return await _manager.GetRolesAsync(user.Id);
        }
        public IEnumerable<string> GetRolesForUser(ApplicationUser user)
        {
            return _manager.GetRoles(user.Id);
        }
        public async Task RemoveUserFromRoleAsync(ApplicationUser user, params string[] roleNames)
        {
            await _manager.RemoveFromRolesAsync(user.Id, roleNames);
        }

        public async Task<ApplicationUser> GetLoginUserAsync(string username, string password)
        {
            return await _manager.FindAsync(username, password);
        }

        public async Task<ClaimsIdentity> CreateIdentityAsync(ApplicationUser user)
        {
            return await _manager.CreateIdentityAsync(
                user, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public async Task<ApplicationUser> GetUserByIdAsync(int id)
        {
            return await _store.FindByIdAsync(id);
        }

        public async Task<ApplicationUser> GetUserByAuthorNameAsync(string authorname)
        {
            return await _store.Users.FirstAsync(author => author.UserName == authorname);
        }
        public async Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            return await _manager.ChangePasswordAsync(userId, currentPassword, newPassword);
        }
        public IdentityResult ChangePassword(int userId, string currentPassword, string newPassword)
        {
            return _manager.ChangePassword(userId, currentPassword, newPassword);
        }

        public ApplicationUser GetAuthor(int id)
        {
            return _store.Users.FirstOrDefault(author => author.Id == id);
        }


        public void DeleteUser(ApplicationUser user)
        {
            _manager.Delete(user);
        }

        public void UpdateUser(ApplicationUser user)
        {
            _manager.Update(user);
        }

        public void RemoveUserFromRole(ApplicationUser user, params string[] roleNames)
        {
            _manager.RemoveFromRoles(user.Id, roleNames);
        }

        public ApplicationUser GetUserById(int id)
        {
            return _store.Users.FirstOrDefault(u => u.Id == id);
        }
        public string GetNameById(int id)
        {
            var user = _store.Users.FirstOrDefault(u => u.Id == id);
            return user.FullName;
        }

        public bool IsExist(string authorName)
        {
            var count = _store.Users.Count(u => u.UserName == authorName);
            return count > 0;
        }
        public ApplicationUser GetUserByUserName(string username)
        {
            return _store.Users.FirstOrDefault(u => u.UserName == username);
        }
        public ApplicationUser GetUserByName(string uniqueName)
        {
            return _store.Users.FirstOrDefault(u => u.UserName == uniqueName);
        }

        public bool IsActive(string userName)
        {
            var result = _store.Users.FirstOrDefault(p => p.IsActive && (p.Email == userName || p.UserName == userName));
            return result != null;
        }

    }
}
