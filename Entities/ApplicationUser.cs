using System.Collections.Generic;
using LibAPI.Attributes;
using Microsoft.AspNetCore.Identity;

namespace LibAPI.Entities
{
    public partial class ApplicationUser : IdentityUser
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Middle { get; set; }
        public string Description { get; set; }
        
        public List<Lending> Lendings { get; set; }

        [IgnoreProperty] public bool Visible { get; set; } = true;
        [IgnoreProperty] public bool IsDeleted { get; set; }
    }
}
