using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibAPI.Context;
using LibAPI.Entities;
using LibAPI.Models;
using LibAPI.Repositories;

namespace LibAPI.Service
{
    public class ApplicationService<TR, TD, TI, TRep> : IBaseService<TR, TI>
        where TI : BaseEntity
        where TRep : BaseRepository<TR, TD, TI, ApplicationDbContext>
        where TR : BaseModel<TD, TR>, new()
    {
        public ApplicationService(TRep repository)
        {
            Repository = repository;
        }

        public virtual Task<List<TR>> GetAll()
        {
            return Repository.GetAll();
        }

        public virtual Task<TR> GetById(Guid id)
        {
            return Repository.GetById(id.ToString());
        }

        public virtual Task<int> Save(TI model)
        {
            return Repository.Save(model);
        }

        public virtual Task<int> SaveRange(IEnumerable<TI> models)
        {
            return Repository.SaveRange(models);
        }

        public virtual Task<int> Remove(Guid id)
        {
            return Repository.Remove(id.ToString());
        }

        public virtual Task<int> RemoveRange(IEnumerable<Guid> ids)
        {
            return Repository.RemoveRange(ids.Select(x => x.ToString()));
        }

        protected TRep Repository { get; }
    }
}