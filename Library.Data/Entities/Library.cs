namespace Library.Data.Entities;

public class Library : BaseEntity<Guid>
{
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    public string City { get; set; }
    
    public string Country { get; set; }
    
    public string PostalCode { get; set; }

    public ICollection<BookInstance> BookInstances { get; set; }

    public ICollection<ApplicationUser> Members { get; set; }
}