using Library.Data.Entities;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Library.Server.OData;

public static class EdmModelBuilder
{
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();

        builder.EntitySet<Book>("Books");

        builder.EnableLowerCamelCaseForPropertiesAndEnums();
        return builder.GetEdmModel();
    }
}