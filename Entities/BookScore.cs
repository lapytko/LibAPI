namespace LibAPI.Entities
{
    public class BookScore:BaseEntity
    {
     public Book Book { get; set; }
     public ushort TotalCount { get; set; }
     public ushort? GivenCount { get; set; }
    }
}
