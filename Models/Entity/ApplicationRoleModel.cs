using System.Collections.Generic;
using LibAPI.Attributes;
using LibAPI.Entities;

namespace LibAPI.Models.Entity
{
    public class ApplicationRoleModel : BaseModel<ApplicationRoleData, ApplicationRoleModel>
    {
    }

    public class ApplicationRoleData : ApplicationRole
    {
        [IgnoreProperty]
        public override string ConcurrencyStamp { get; set; }
    }
}