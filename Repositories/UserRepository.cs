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
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;

        public UserRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<List<ApplicationUserModel>> GetAll()
        {
            var context = new ApplicationDbContext();
            return (from u in context.Users
                    where u.Visible && !u.IsDeleted
                    select
                        new ApplicationUserModel()
                            .SetValue(_mapper.Map<ApplicationUserData>(u))
                            .SetProperty(new Dictionary<string, object>
                            {
                                {
                                    nameof(ApplicationUserModel.Value.Roles),
                                    (from ur in context.UserRoles
                                        join r in context.Roles on ur.RoleId equals r.Id
                                        where ur.UserId == u.Id & r.Visible & !r.IsDeleted 
                                        select new ApplicationRoleModel().SetValue(
                                            _mapper.Map<ApplicationRoleData>(r)))
                                    .ToList()
                                }
                            }))
                .ToListAsync();
        }

        public Task<ApplicationUserModel> GetById(string id)
        {
            var context = new ApplicationDbContext();
            return (from u in context.Users
                    where u.Id == id && !u.IsDeleted
                    select
                        new ApplicationUserModel {Value = _mapper.Map<ApplicationUserData>(u)}
                            .SetValue(_mapper.Map<ApplicationUserData>(u))
                            .SetProperty(new Dictionary<string, object>()
                            {
                                {
                                    nameof(ApplicationUserModel.Value.Roles),
                                    (from ur in context.UserRoles
                                        join r in context.Roles on ur.RoleId equals r.Id
                                        where ur.UserId == u.Id && !r.IsDeleted
                                        select new ApplicationRoleModel {Value = _mapper.Map<ApplicationRoleData>(r)})
                                    .ToList()
                                }
                            }))
                .FirstOrDefaultAsync();
        }

        public Task<int> Save(ApplicationUser model)
        {
            throw new NotImplementedException();
        }

        public Task<int> Remove(string id)
        {
            throw new NotImplementedException();
        }
    }

    public interface IUserRepository : IBaseRepository<ApplicationUserModel, ApplicationUser>
    {
    }
}