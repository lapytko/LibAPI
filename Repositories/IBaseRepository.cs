using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibAPI.Entities;
using LibAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibAPI.Repositories
{
    public class BaseRepository<TR, TD, TI, TContext> : IBaseRepository<TR, TI>
        where TContext : DbContext, new() where TI : BaseEntity where TR : BaseModel<TD, TR>, new()
    {
        protected IMapper Mapper { get; }

        public BaseRepository(IMapper mapper)
        {
            Mapper = mapper;
        }

        public virtual Task<List<TR>> GetAll()
        {
            return Execute(context =>
                context.Set<TI>().Where(x => x.Visible && !x.IsDeleted)
                    .Select(x => new TR().SetValue(Mapper.Map<TD>(x))).ToListAsync());
        }

        public virtual Task<TR> GetById(string id)
        {
            return Execute(context =>
                context.Set<TI>().Where(x => x.Id == id).Select(x => new TR().SetValue(Mapper.Map<TD>(x)))
                    .FirstOrDefaultAsync());
        }

        public virtual Task<int> Save(TI model)
        {
            return Execute(context =>
            {
                if (model.IsNew)
                {
                    model.Id = Guid.NewGuid().ToString();
                    context.Set<TI>().Add(model);
                }
                else
                    context.Set<TI>().Update(model);

                return context.SaveChangesAsync();
            });
        }

        public virtual Task<int> SaveRange(IEnumerable<TI> models)
        {
            return Execute(context =>
            {
                var baseEntities = models as TI[] ?? models.ToArray();
                var elementsForAdd = new List<TI>(baseEntities.Count(x => x.IsNew));
                var elementsForUpdate = new List<TI>(baseEntities.Count(x => !x.IsNew));
                foreach (var model in baseEntities)
                {
                    if (model.IsNew)
                    {
                        model.InitId<TI>();
                        elementsForAdd.Add(model);
                    }
                    else
                        elementsForUpdate.Add(model);
                }

                var dbSet = context.Set<TI>();
                dbSet.AddRange(elementsForAdd);
                dbSet.UpdateRange(elementsForUpdate);

                return context.SaveChangesAsync();
            });
        }

        public virtual Task<int> Remove(string id)
        {
            return Execute(context =>
            {
                var dbSet = context.Set<TI>();
                var entity = dbSet.Find(id);
                entity.IsDeleted = true;
                dbSet.Update(entity);
                return context.SaveChangesAsync();
            });
        }

        public virtual Task<int> RemoveRange(IEnumerable<string> ids)
        {
            return Execute(context =>
            {
                var dbSet = context.Set<TI>();

                foreach (var id in ids)
                {
                    var entity = dbSet.Find(id);
                    entity.IsDeleted = true;
                    dbSet.Update(entity);
                }
                return context.SaveChangesAsync();
            });
        }

        protected virtual async Task<TRes> Execute<TRes>(Func<TContext, Task<TRes>> action)
        {
            await using var context = new TContext();
            return await action(context);
        }
    }

    public interface IBaseRepository<TR, in TI>
    {
        Task<List<TR>> GetAll();
        Task<TR> GetById(string id);
        Task<int> Save(TI model);
        Task<int> Remove(string id);
    }

    public interface IBaseRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(string id);
        Task<int> Save(T model);
        Task<int> Remove(string id);
    }
}