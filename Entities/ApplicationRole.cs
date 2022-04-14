using LibAPI.Attributes;
using Microsoft.AspNetCore.Identity;

namespace LibAPI.Entities
{
    public partial class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
        [IgnoreProperty]
        public bool Visible { get; set; } = true;
        [IgnoreProperty]
        public bool IsDeleted { get; set; }
    }
}
