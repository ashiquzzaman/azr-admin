using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AzR.Core.AppContexts;
using AzR.Core.Entities;
using AzR.Core.Repositoies.Interface;
using AzR.Core.ViewModels.Admin;
using AzR.Utilities;

namespace AzR.Core.Repositoies.Implementation
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(DbContext context) : base(context) { }
        public IQueryable<Organization> GetAllActive()
        {
            return FindAll(r => r.IsActive == true && r.ParentId != null && r.IsBranch);
        }
        public IQueryable<Organization> GetAllDeactive()
        {
            return FindAll(r => r.IsActive == false);
        }
        public Organization GetBy(int id)
        {
            return Find(r => r.Id == id && r.IsActive == true);
        }


        public IQueryable<Organization> TakeBy(int number)
        {
            return FindAll(a => a.IsActive == true).Take(number);
        }

        public OrganizationViewModel EntityFactory(Organization model)
        {
            return new OrganizationViewModel
            {
                Id = model.Id,
                OrgCode = model.OrgCode,
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                Created = model.Created,
                Expired = model.Expired,
                ParentId = model.ParentId,
                Active = model.IsActive

            };
        }
        public Organization ModelFactory(OrganizationViewModel model)
        {
            return new Organization
            {
                Id = model.Id,
                OrgCode = model.OrgCode,
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                Created = model.Created ?? DateTime.UtcNow.ToLong(),
                Expired = model.Expired ?? DateTime.UtcNow.AddDays(400).ToLong(),
                ParentId = model.ParentId,
                IsActive = model.Active,
                IsBranch = model.Id != 1

            };
        }
        public Expression<Func<Organization, OrganizationViewModel>> ModelExpression
        {
            get
            {
                return model => new OrganizationViewModel
                {
                    Id = model.Id,
                    OrgCode = model.OrgCode,
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    Created = model.Created,
                    Expired = model.Expired,
                    ParentId = model.ParentId,
                    Active = model.IsActive
                };
            }
        }
    }
}
