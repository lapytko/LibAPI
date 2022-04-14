using System;

namespace LibAPI.Entities
{
    public class Passport : BaseEntity
    {
        public string Serial { get; set; }

        public string Number { get; set; }

        public string? IdentNumber { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string GivenBy { get; set; }
        
        public Client Client { get; set; }
        
        
    }
}
