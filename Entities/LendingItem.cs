using System;
using LibAPI.Entities.Enums;

namespace LibAPI.Entities
{
    public class LendingItem : BaseEntity
    {
        public Book Book { get; set; }

        public DateTime StartDate { get; set; }

        public byte DayCount { get; set; }

        public DateTime? EndDate { get; set; }

        public LendingStatus Status { get; set; }
        
        public Lending Lending { get; set; }
    }
}
