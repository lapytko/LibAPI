using System.Collections.Generic;

namespace LibAPI.Entities
{
    public class Style:BaseEntity
    {
        public string Name { get; set; }
        
        public List<Book>Books { get; set; }
    }
}
