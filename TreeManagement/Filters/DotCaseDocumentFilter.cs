using System.Text.RegularExpressions;

namespace Api.Filters;

public class DotCaseDocumentFilter : IDocumentFilter
{

    public static string ToDotCase(string text)
    {
        return Regex.Replace(Regex.Replace(text, "(.)([A-Z][a-z]+)", "$1.$2"), "([a-z0-9])([A-Z])", "$1.$2").ToLower();

    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var tag in swaggerDoc.Tags)
        {
            tag.Name = ToDotCase(tag.Name);
        }

        foreach (var path in swaggerDoc.Paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                foreach (var t in operation.Value.Tags)
                {
                    t.Name = ToDotCase(t.Name);
                }
            }
        }
    }

}