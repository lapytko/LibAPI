using System;
using System.Collections.Generic;
using LibAPI.Entities.Enums;

namespace LibAPI.Entities
{
    public class Lending : BaseEntity
    {
        public string Number { get; set; }

        public Client Client { get; set; }

        public List<LendingItem> Items { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime CreationDate { get; set; }

        public ApplicationUser Creator { get; set; }

        public LendingStatus Status { get; set; }

        public string Description { get; set; }
    }
}
