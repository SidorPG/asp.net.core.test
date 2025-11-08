using System.Reflection;
using Api.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ExampleSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        foreach (var prop in context.Type.GetProperties())
        {
            var exampleAttr = prop.GetCustomAttribute<ExampleAttribute>();
            if (exampleAttr != null)
                if (schema.Properties.Keys.Any(x => x.ToLower() == prop.Name.ToLower()))
                    schema.Properties[prop.Name].Example = ConvertToOpenApiAny(exampleAttr.Text);
        }
    }

    private IOpenApiAny ConvertToOpenApiAny(object value)
    {
        return value switch
        {
            string s => new OpenApiString(s),
            int i => new OpenApiInteger(i),
            long l => new OpenApiLong(l),
            float f => new OpenApiFloat(f),
            double d => new OpenApiDouble(d),
            bool b => new OpenApiBoolean(b),
            DateTime dt => new OpenApiDateTime(dt),
            _ => new OpenApiString(value?.ToString() ?? "")
        };
    }
}