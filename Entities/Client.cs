using System;
using System.Collections.Generic;
using BookLibWebApi.Models.Enums;
using LibAPI.Entities.Enums;

namespace LibAPI.Entities
{
    public class Client : BaseEntity
    {
        public string? FirstName { get; set; }

        public string Name { get; set; }

        public string? LastName { get; set; }

        public DateTime? Birthday { get; set; }
        
        public string Phone { get; set; }
        
        public Address Address { get; set; }

        public ClientStatus Status { get; set; }
        
        public Gender Gender { get; set; }
        public Passport Passport { get; set; }
        
        public List<Lending> Lendings { get; set; }
    }
}
