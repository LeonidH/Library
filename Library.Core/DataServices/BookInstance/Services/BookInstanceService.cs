using Library.Data;

namespace Library.Core.DataServices.BookInstance.Services;

public partial class BookInstanceService(ApplicationDbContext applicationDbContext)
    : BaseDataService<Data.Entities.BookInstance, Guid>(applicationDbContext)
{
}