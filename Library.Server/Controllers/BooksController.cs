using Library.Core.DataServices;
using Library.Core.DataServices.UserGroup;
using Library.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Library.Server.Controllers;

[Authorize(Roles = UserGroupConstant.Librarian)]
public class BooksController(BaseDataService<Book, Guid> bookService) : ODataController
{
    [HttpGet("odata/Books({id})")]
    [EnableQuery]
    public async Task<ActionResult<Book>> GetBook([FromRoute] Guid id)
    {
        var book = await bookService.GetAllAsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        return Ok(book) ?? throw new InvalidOperationException();
    }
    
    [EnableQuery(PageSize = 10)]
    public ActionResult<IQueryable<Book>> Get()
    {
        return Ok(bookService.GetAllAsQueryable());
    }
    
    [HttpPost]
    public async Task<ActionResult<Book>> Post([FromBody] Book book)
    {
        await bookService.CreateAsync(book);
        return Created(book);
    }
    
    [HttpPut("odata/Books")]
    public async Task<ActionResult<Book>> Put([FromBody] Book updatedBookData)
    {
        var updatedBook = await bookService.UpdateAsync(updatedBookData);
        return Ok(updatedBook);
    }
    
    [HttpDelete("odata/Books({id})")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        await bookService.DeleteAsync(id);
        return NoContent();
    }
}
