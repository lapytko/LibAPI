using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibAPI.Entities;
using LibAPI.Models.Entity;
using LibAPI.Repositories;

namespace LibAPI.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<List<ApplicationRoleModel>> GetAll()
        {
            return _roleRepository.GetAll();
        }

        public Task<ApplicationRoleModel> GetById(Guid id)
        {
            return _roleRepository.GetById(id.ToString());
        }

        public Task<int> Save(ApplicationRole model)
        {
            throw new NotImplementedException();
        }

        public Task<int> Remove(Guid id)
        {
            throw new NotImplementedException();
        }
    }

    public interface IRoleService : IBaseService<ApplicationRoleModel, ApplicationRole>
    {
    }
}