namespace Library.Data.Entities;

public enum RequestType
{
    Borrow,
    Return
}

public enum RequestStatus
{
    Pending,
    Approved,
    Rejected,
    Borrowed,
    Returned
}


public class BookRequest
{
    public Guid Id { get; set; }

    public string Isbn { get; set; }

    public string UserId { get; set; }
    
    public RequestType RequestType { get; set; }
    
    public RequestStatus RequestStatus { get; set; }
    
    public DateTime RequestedOnUtc { get; set; }
    
    public DateTime? ApprovedOnUtc { get; set; }
    
    public DateTime? RejectedOnUtc { get; set; }
    
    public DateTime? BorrowedOnUtc { get; set; }
    
    public DateTime? ReturnedOnUtc { get; set; }
    
    public ApplicationUser User { get; set; }
    
    public BookInstance BookInstance { get; set; }
}