using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AzR.Core.Config;
using Microsoft.AspNet.Identity;

namespace AzR.Core.IdentityConfig
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetAllActive();
        IEnumerable<ApplicationUser> GetAllDeactive();
        ApplicationUser GetByName(string username);
        ApplicationUser GetBy(int id);

        IdentityResult Create(ApplicationUser user, string password);
        void AddUserToRole(ApplicationUser user, string role);
        IEnumerable<ApplicationUser> GetAllUsers();
        Task<ApplicationUser> GetUserByNameAsync(string username);
        Task<ApplicationUser> GetUserByIdAsync(int id);
        Task<ApplicationUser> GetUserByAuthorNameAsync(string authorname);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task CreateAsync(ApplicationUser user, string password);
        Task DeleteUserAsync(ApplicationUser user);
        Task UpdateUserAsync(ApplicationUser user);
        void DeleteUser(ApplicationUser user);
        void UpdateUser(ApplicationUser user);
        IEnumerable<ApplicationUser> GetAllInstituteUsers(long instituteId);

        bool VerifyUserPassword(string hashedPassword, string providedPassword);
        string HashPassword(string password);

        Task AddUserToRoleAsync(ApplicationUser newUser, string p);

        Task<IEnumerable<string>> GetRolesForUserAsync(ApplicationUser user);

        Task RemoveUserFromRoleAsync(ApplicationUser user, params string[] roleNames);
        IEnumerable<string> GetRolesForUser(ApplicationUser user);
        Task<ApplicationUser> GetLoginUserAsync(string username, string password);
        ApplicationUser GetAuthor(int id);
        Task<ClaimsIdentity> CreateIdentityAsync(ApplicationUser user);
        IdentityResult ChangePassword(int userId, string currentPassword, string newPassword);
        Task<bool> IsExistAsync(string authorName);
        Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword);

        void RemoveUserFromRole(ApplicationUser user, params string[] roleNames);

        ApplicationUser GetUserById(int id);
        string GetNameById(int id);

        bool IsExist(string authorName);
        ApplicationUser GetUserByName(string uniqueName);
        ApplicationUser GetUserByUserName(string username);
        bool IsActive(string userName);

        Task<bool> IsInRoleAsync(ApplicationUser user, string roleName);
    }
}
