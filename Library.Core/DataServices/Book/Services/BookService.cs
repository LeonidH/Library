using Library.Data;

namespace Library.Core.DataServices.Book.Services;

public partial class BookService(ApplicationDbContext applicationDbContext)
    : BaseDataService<Data.Entities.Book, Guid>(applicationDbContext)
{
}