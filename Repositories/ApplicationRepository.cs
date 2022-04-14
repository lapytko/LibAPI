using AutoMapper;
using LibAPI.Context;
using LibAPI.Entities;
using LibAPI.Models;

namespace LibAPI.Repositories
{
    public class ApplicationRepository<TR,TD, TI> : BaseRepository<TR,TD, TI, ApplicationDbContext>
        where TI : BaseEntity where TR : BaseModel<TD, TR>, new()
    {
        public ApplicationRepository(IMapper mapper) : base(mapper)
        {
        }
    }
}