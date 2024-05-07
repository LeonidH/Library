namespace Library.Data.Entities;

public class Book : BaseEntity<Guid>
{
    public string Title { get; set; }
    
    public string Author { get; set; }
}