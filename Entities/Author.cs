using System;
using System.Collections.Generic;

namespace LibAPI.Entities
{
    public class Author : BaseEntity
    {
        public string? FirstName { get; set; }

        public string Name { get; set; }

        public string? LastName { get; set; }

        public DateTime? Birthday { get; set; }

        public DateTime? Endday { get; set; }

        public Country? Country { get; set; }

        public List<Book> Books { get; set; }
    }
}
