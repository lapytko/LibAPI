namespace LibAPI.Entities
{
    public class Address : BaseEntity
    {
        public Country Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        /// <summary>
        /// /Дом
        /// </summary>
        public string House { get; set; }

        /// <summary>
        /// Корпус
        /// </summary>
        public string? Building { get; set; }
        
        /// <summary>
        /// rdfhnbhf
        /// </summary>
        public  string? Flat { get; set; }
    }
}
