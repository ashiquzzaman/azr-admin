namespace AzR.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "C##AZRADMIN.LoginHistories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LoginTime = c.Decimal(nullable: false, precision: 19, scale: 0),
                        LogoutTime = c.Decimal(precision: 19, scale: 0),
                        UserId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        AgentId = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "C##AZRADMIN.Organizations",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        OrgCode = c.String(nullable: false, maxLength: 50),
                        ParentId = c.Decimal(precision: 10, scale: 0),
                        Name = c.String(nullable: false, maxLength: 256),
                        Email = c.String(nullable: false, maxLength: 256),
                        Phone = c.String(nullable: false, maxLength: 128),
                        Address = c.String(maxLength: 500),
                        Created = c.Decimal(nullable: false, precision: 19, scale: 0),
                        Expired = c.Decimal(nullable: false, precision: 19, scale: 0),
                        IsBranch = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IsActive = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("C##AZRADMIN.Organizations", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "C##AZRADMIN.Users",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        EmployeeId = c.String(maxLength: 50),
                        FullName = c.String(maxLength: 256),
                        OrgId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ImageUrl = c.String(maxLength: 255),
                        Created = c.Decimal(nullable: false, precision: 19, scale: 0),
                        Expired = c.Decimal(nullable: false, precision: 19, scale: 0),
                        InVacation = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IsActive = c.Decimal(nullable: false, precision: 1, scale: 0),
                        AgentId = c.String(maxLength: 100),
                        Modified = c.Decimal(nullable: false, precision: 19, scale: 0),
                        Email = c.String(nullable: false, maxLength: 128),
                        EmailConfirmed = c.Decimal(nullable: false, precision: 1, scale: 0),
                        PasswordHash = c.String(maxLength: 256),
                        SecurityStamp = c.String(maxLength: 256),
                        PhoneNumber = c.String(maxLength: 256),
                        PhoneNumberConfirmed = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TwoFactorEnabled = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Decimal(nullable: false, precision: 1, scale: 0),
                        AccessFailedCount = c.Decimal(nullable: false, precision: 10, scale: 0),
                        UserName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("C##AZRADMIN.Organizations", t => t.OrgId, cascadeDelete: true)
                .Index(t => t.OrgId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "C##AZRADMIN.UserClaims",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        UserId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ClaimType = c.String(maxLength: 150),
                        ClaimValue = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("C##AZRADMIN.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "C##AZRADMIN.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("C##AZRADMIN.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "C##AZRADMIN.UserPrivileges",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        UserId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        MenuId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        FullPermission = c.Decimal(nullable: false, precision: 1, scale: 0),
                        AddPermission = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ViewPermission = c.Decimal(nullable: false, precision: 1, scale: 0),
                        EditPermission = c.Decimal(nullable: false, precision: 1, scale: 0),
                        DeletePermission = c.Decimal(nullable: false, precision: 1, scale: 0),
                        DetailViewPermission = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ReportViewPermission = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IsActive = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("C##AZRADMIN.Menus", t => t.MenuId, cascadeDelete: true)
                .ForeignKey("C##AZRADMIN.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.MenuId);
            
            CreateTable(
                "C##AZRADMIN.Menus",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        DisplayName = c.String(nullable: false, maxLength: 128),
                        Url = c.String(nullable: false, maxLength: 500),
                        MenuOrder = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Icon = c.String(maxLength: 120),
                        MenuType = c.Decimal(nullable: false, precision: 10, scale: 0),
                        RoleId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ParentId = c.Decimal(precision: 10, scale: 0),
                        IsActive = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("C##AZRADMIN.Menus", t => t.ParentId)
                .ForeignKey("C##AZRADMIN.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "C##AZRADMIN.Roles",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        RoleCode = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 250),
                        IsDisplay = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IsActive = c.Decimal(nullable: false, precision: 1, scale: 0),
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "C##AZRADMIN.UserRoles",
                c => new
                    {
                        UserId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        RoleId = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("C##AZRADMIN.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("C##AZRADMIN.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("C##AZRADMIN.UserRoles", "UserId", "C##AZRADMIN.Users");
            DropForeignKey("C##AZRADMIN.UserPrivileges", "UserId", "C##AZRADMIN.Users");
            DropForeignKey("C##AZRADMIN.Menus", "RoleId", "C##AZRADMIN.Roles");
            DropForeignKey("C##AZRADMIN.UserRoles", "RoleId", "C##AZRADMIN.Roles");
            DropForeignKey("C##AZRADMIN.UserPrivileges", "MenuId", "C##AZRADMIN.Menus");
            DropForeignKey("C##AZRADMIN.Menus", "ParentId", "C##AZRADMIN.Menus");
            DropForeignKey("C##AZRADMIN.Users", "OrgId", "C##AZRADMIN.Organizations");
            DropForeignKey("C##AZRADMIN.UserLogins", "UserId", "C##AZRADMIN.Users");
            DropForeignKey("C##AZRADMIN.UserClaims", "UserId", "C##AZRADMIN.Users");
            DropForeignKey("C##AZRADMIN.Organizations", "ParentId", "C##AZRADMIN.Organizations");
            DropIndex("C##AZRADMIN.UserRoles", new[] { "RoleId" });
            DropIndex("C##AZRADMIN.UserRoles", new[] { "UserId" });
            DropIndex("C##AZRADMIN.Roles", "RoleNameIndex");
            DropIndex("C##AZRADMIN.Menus", new[] { "ParentId" });
            DropIndex("C##AZRADMIN.Menus", new[] { "RoleId" });
            DropIndex("C##AZRADMIN.UserPrivileges", new[] { "MenuId" });
            DropIndex("C##AZRADMIN.UserPrivileges", new[] { "UserId" });
            DropIndex("C##AZRADMIN.UserLogins", new[] { "UserId" });
            DropIndex("C##AZRADMIN.UserClaims", new[] { "UserId" });
            DropIndex("C##AZRADMIN.Users", "UserNameIndex");
            DropIndex("C##AZRADMIN.Users", new[] { "OrgId" });
            DropIndex("C##AZRADMIN.Organizations", new[] { "ParentId" });
            DropTable("C##AZRADMIN.UserRoles");
            DropTable("C##AZRADMIN.Roles");
            DropTable("C##AZRADMIN.Menus");
            DropTable("C##AZRADMIN.UserPrivileges");
            DropTable("C##AZRADMIN.UserLogins");
            DropTable("C##AZRADMIN.UserClaims");
            DropTable("C##AZRADMIN.Users");
            DropTable("C##AZRADMIN.Organizations");
            DropTable("C##AZRADMIN.LoginHistories");
        }
    }
}
