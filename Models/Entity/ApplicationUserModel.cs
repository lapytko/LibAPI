using System.Collections.Generic;
using System.Linq;
using LibAPI.Attributes;
using LibAPI.Entities;

namespace LibAPI.Models.Entity
{
    public class ApplicationUserModel : BaseModel<ApplicationUserData, ApplicationUserModel>
    {
        public override object ToObject()
        {
            return new
            {
                Value.Id,
                Value.Email,
                Value.UserName,
                fio = $"{Value.Surname} {Value.Name} {Value.Middle}".Trim(),
                Value.Surname,
                Value.Name,
                Value.Middle,
                Roles = Value.Roles?.Select(x => x.ToObjectReflection()),
                CanChange = Value.Roles?
                    .FirstOrDefault(x => x.Value.Name is "admin" or "superAdmin") != null,
                Value.Description
            };
        }
    }

    public class ApplicationUserData : ApplicationUser
    {
        [IgnoreIfNullData] public List<ApplicationRoleModel> Roles { get; set; }
    }
}