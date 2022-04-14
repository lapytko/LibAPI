using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibAPI.Entities;
using LibAPI.Service;
using Microsoft.AspNetCore.Identity;

namespace LibAPI.Facade
{
    public class RoleServiceFacade
    {
        private readonly IRoleService _roleService;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleServiceFacade(IRoleService roleService, RoleManager<ApplicationRole> roleManager)
        {
            _roleService = roleService;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<object>> GetAll() => (await _roleService.GetAll()).Select(x => x.ToObject());

        public async Task<object> GetById(Guid id) => (await _roleService.GetById(id)).ToObject();

        public async Task<object> RemoveRole(Guid id)
        {
            var role = _roleManager.Roles.FirstOrDefault(x => x.Id == id.ToString());
            if (role == null) return false;
            
            role.IsDeleted = true;
            await _roleManager.UpdateAsync(role);

            return true;
        }
    }
}