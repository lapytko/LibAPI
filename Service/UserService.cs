using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibAPI.Entities;
using LibAPI.Models;
using LibAPI.Models.Entity;
using LibAPI.Repositories;
using LibAPI.Utils.DataFilterUtils;
using LibAPI.Utils.DataFilterUtils.Models;

namespace LibAPI.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Task<List<ApplicationUserModel>> GetAll()
        {
            return _userRepository.GetAll();
        }

        public Task<ApplicationUserModel> GetById(Guid id)
        {
            return _userRepository.GetById(id.ToString());
        }

        public Task<int> Save(ApplicationUser model)
        {
            return _userRepository.Save(model);
        }

        public Task<int> Remove(Guid id)
        {
            return _userRepository.Remove(id.ToString());
        }

        public async Task<(List<TableUserModel>, int)> GetByFilter(FilterModel filterModel)
        {
            return (
                (await _userRepository.GetAll()).Select(x => _mapper.Map<TableUserModel>(x.Value)).AsQueryable()
                .ToFilterView(filterModel, out var count).ToList(),
                count);
        }
    }

    public interface IUserService : IBaseService<ApplicationUserModel, ApplicationUser>
    {
        Task<(List<TableUserModel>, int)> GetByFilter(FilterModel filterModel);
    }
}