using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibAPI.Service
{
    public interface IBaseService<TR, in TI>
    {
        Task<List<TR>> GetAll();
        Task<TR> GetById(Guid id);
        Task<int> Save(TI model);
        Task<int> Remove(Guid id);
    }
    
    public interface IBaseService<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(Guid id);
        Task<int> Save(T model);
        Task<int> Remove(Guid id);
    }
}