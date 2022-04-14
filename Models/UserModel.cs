using System.Collections.Generic;
using LibAPI.Entities;

namespace LibAPI.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Middle { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public bool CanChange { get; set; }

    }
}