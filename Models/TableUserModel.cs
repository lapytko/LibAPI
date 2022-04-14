using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace LibAPI.Models
{
    [Serializable]
    public class TableUserModel
    {
        public string Id { get; set; }
        [JsonIgnore] public string Email { get; set; }
        public string UserName { get; set; }
        public string Fio => $"{Surname} {Name} {Middle}".Trim();
        public string Middle { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public bool CanChange { get; set; }
    }
}