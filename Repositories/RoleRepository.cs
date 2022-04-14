using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibAPI.Context;
using LibAPI.Entities;
using LibAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace LibAPI.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IMapper _mapper;

        public RoleRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<List<ApplicationRoleModel>> GetAll()
        {
            var context = new ApplicationDbContext();
            return (from m in context.Roles
                where m.Visible && !m.IsDeleted
                select new ApplicationRoleModel {Value = _mapper.Map<ApplicationRoleData>(m)}).ToListAsync();
        }

        public Task<ApplicationRoleModel> GetById(string id)
        {
            var context = new ApplicationDbContext();
            return (from r in context.Roles
                where r.Id == id
                select new ApplicationRoleModel {Value = _mapper.Map<ApplicationRoleData>(r)}
                    .SetProperty(
                        new Dictionary<string, object>
                        {
                            // {
                            //     nameof(ApplicationRoleModel.Value.Menus),
                            //     (from m in context.ApplicationMenus
                            //         where !m.IsDeleted
                            //         select new ApplicationMenuModel {Value = _mapper.Map<ApplicationMenuData>(m)})
                            //     .ToList()
                            // },
                            // {
                            //     nameof(ApplicationRoleModel.Value.Functions),
                            //     (from m in context.ApplicationFunctions
                            //         where  !m.IsDeleted
                            //         select new ApplicationFunctionModel
                            //             {Value = _mapper.Map<ApplicationFunctionData>(m)})
                            //     .ToList()
                            // }
                        })).FirstOrDefaultAsync();
        }

        public Task<int> Save(ApplicationRole model)
        {
            throw new NotImplementedException();
        }

        public Task<int> Remove(string id)
        {
            var context = new ApplicationDbContext();
            var role = context.Roles.First(x => x.Id == id);
            role.IsDeleted = true;
            context.Roles.Update(role);
            return context.SaveChangesAsync();
        }
    }

    public interface IRoleRepository : IBaseRepository<ApplicationRoleModel, ApplicationRole>
    {
    }
}