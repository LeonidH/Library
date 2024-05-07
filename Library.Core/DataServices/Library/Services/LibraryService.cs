using Library.Data;

namespace Library.Core.DataServices.Library.Services;

public partial class LibraryService(ApplicationDbContext applicationDbContext)
    : BaseDataService<global::Library.Data.Entities.Library, Guid>(applicationDbContext)
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
}