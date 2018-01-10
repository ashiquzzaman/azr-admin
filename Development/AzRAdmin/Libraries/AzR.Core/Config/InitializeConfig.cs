using System;
using System.Data.Entity.Migrations;
using System.Linq;
using AzR.Core.Entity;
using AzR.Core.IdentityConfig;
using AzR.Core.Utilities;
using Microsoft.AspNet.Identity;

namespace AzR.Core.Config
{
    public class InitializeConfig
    {
        public ApplicationDbContext Context { get; set; }
        public void InitializeRole()
        {
            #region Role
            Context.Roles.AddOrUpdate(r => r.Name,
                new ApplicationRole { Name = "ADMIN", DisplayName = "Administrator", Description = "Administrator", Active = true },
                new ApplicationRole { Name = "SUBSCRIBER", DisplayName = "Subscriber", Description = "Subscriber", Active = true }
            );
            #endregion
            Context.SaveChanges();

        }
        public void InitializeOrganization()
        {
            #region Organization

            Context.Organizations.AddOrUpdate(i => i.Id,
                new Organization
                {
                    ParentId = null,
                    Name = "BASIC Bank",
                    Email = "info@basicbank.com",
                    Phone = "+8801717252600",
                    Address = "Basic Bank",
                    Created = DateTime.UtcNow.ToLong(),
                    Expired = DateTime.UtcNow.AddDays(400).ToLong(),
                    Active = true
                });

            #endregion
            Context.SaveChanges();

        }
        public void InitializeAdmin()
        {

            #region Admin User
            var role = Context.Roles.First(s => s.Name == "ADMIN");
            Context.Roles.AddOrUpdate(s => s.Name, role);
            var user = new ApplicationUser
            {
                Name = "Sayem Ahamed",
                OrgId = 1,
                Types = "ADMIN",
                ImageUrl = "/Images/user.png",
                WebSite = null,
                Biography = null,
                Latitude = null,
                Longitude = null,
                Created = DateTime.UtcNow.ToLong(),
                Expired = DateTime.UtcNow.AddYears(2).ToLong(),
                Active = true,
                AgentId = "127.0.0.1",
                Modified = DateTime.UtcNow.ToLong(),

                UserName = "sayem@datasoft-bd.com",
                Email = "sayem@datasoft-bd.com",
                PhoneNumber = "+8801611252600",
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                PasswordHash = new PasswordHasher().HashPassword("Azr@123456"),
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
