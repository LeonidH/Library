namespace Library.Data.Entities;

public enum BookInstanceStatus
{
    Available,
    Borrowed,
    Reserved,
    Lost
}

public class BookInstance : BaseEntity<Guid>
{
    public string Isbn { get; set; }
    
    public Guid LibraryId { get; set; }
    
    public Guid BookId { get; set; }
    
    public BookInstanceStatus Status { get; set; }
    
    public Library Library { get; set; }
    
    public Book Book { get; set; }
}