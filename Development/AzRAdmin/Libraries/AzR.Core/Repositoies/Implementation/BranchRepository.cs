using AzR.Core.Config;
using AzR.Core.Entities;
using AzR.Core.Repositoies.Interface;
using AzR.Core.ViewModels.Admin;
using AzR.Utilities.Exentions;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace AzR.Core.Repositoies.Implementation
{
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        public BranchRepository(DbContext context) : base(context) { }
        public IQueryable<Branch> GetAllActive()
        {
            return FindAll(r => r.IsActive == true && r.ParentId != null && r.IsBranch);
        }
        public IQueryable<Branch> GetAllDeactive()
        {
            return FindAll(r => r.IsActive == false);
        }
        public Branch GetBy(int id)
        {
            return Find(r => r.Id == id && r.IsActive == true);
        }


        public IQueryable<Branch> TakeBy(int number)
        {
            return FindAll(a => a.IsActive == true).Take(number);
        }

        public BranchViewModel EntityFactory(Branch model)
        {
            return new BranchViewModel
            {
                Id = model.Id,
                BranchCode = model.BranchCode,
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
        public Branch ModelFactory(BranchViewModel model)
        {
            return new Branch
            {
                Id = model.Id,
                BranchCode = model.BranchCode,
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
        public Expression<Func<Branch, BranchViewModel>> ModelExpression
        {
            get
            {
                return model => new BranchViewModel
                {
                    Id = model.Id,
                    BranchCode = model.BranchCode,
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
