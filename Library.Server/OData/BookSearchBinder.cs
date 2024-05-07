using System.Linq.Expressions;
using Library.Data.Entities;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.OData.UriParser;

namespace Library.Server.OData;

public class BookSearchBinder : QueryBinder, ISearchBinder
{
    public Expression BindSearch(SearchClause searchClause, QueryBinderContext context)
    {
        SearchTermNode? node = searchClause.Expression as SearchTermNode;

        Expression<Func<Book, bool>> exp = p => 
            p.Title.Contains(node.Text) || p.Author.Contains(node.Text);

        return exp;
    }
}