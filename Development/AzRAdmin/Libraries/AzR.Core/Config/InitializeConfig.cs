using AzR.Core.Entities;
using AzR.Core.IdentityConfig;
using AzR.Utilities.Exentions;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace AzR.Core.Config
{
    public class InitializeConfig
    {
        public ApplicationDbContext Context { get; set; }
        public void InitializeBranch()
        {
            #region Branch

            Context.Set<Branch>().AddOrUpdate(i => i.Name,
                new Branch
                {
                    BranchCode = "001",
                    ParentId = null,
                    Name = "AzR Admin",
                    Email = "ashiquzzaman@outlook.com",
                    Phone = "+8801841252600",
                    Address = "AzR Admin",
                    Created = DateTime.UtcNow.ToLong(),
                    Expired = DateTime.UtcNow.AddDays(400).ToLong(),
                    IsActive = true,
                    IsBranch = false
                });

            #endregion
            Context.SaveChanges();

        }

        public void InitializeRole()
        {
            #region Role
            Context.Roles.AddOrUpdate(r => r.Name,
                new ApplicationRole { RoleCode = "1", Name = "ADMIN", Description = "Administrator", IsActive = true, IsDisplay = true },
                new ApplicationRole { RoleCode = "2", Name = "SUBSCRIBER", Description = "Subscriber", IsActive = true, IsDisplay = true }
            );
            #endregion
            Context.SaveChanges();

        }
        public void InitializeAdmin()
        {

            #region Admin User
            var role = Context.Roles.First(s => s.Name == "ADMIN");
            Context.Roles.AddOrUpdate(s => s.Id, role);
            var user = new ApplicationUser
            {
                EmployeeId = "003",
                FullName = "Ashiquzzaman",
                BranchId = 1,
                ImageUrl = "/Images/user.png",
                Created = DateTime.UtcNow.ToLong(),
                Expired = DateTime.UtcNow.AddMonths(3).ToLong(),
                InVacation = false,
                IsActive = false,
                LoginId = "127.0.0.1",
                Modified = DateTime.UtcNow.ToLong(),
                UserName = "admin",
                Email = "ashiquzzaman@outlook.com",
                PhoneNumber = "+8801841252600",
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                PasswordHash = new PasswordHasher().HashPassword("Azr@00575"),
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEndDateUtc = null
            };
            user.Roles.Add(new ApplicationUserRole { RoleId = role.Id, UserId = user.Id });
            Context.Users.AddOrUpdate(r => r.UserName, user);
            #endregion
            Context.SaveChanges();

        }

    }
}
