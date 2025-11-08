using System.Reflection;

namespace Api.Extentions;

public static class UseSwaggerExtensions
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            // pick comments from classes, including controller summary comments
            c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            // _OR_ enable the annotations on Controller classes [SwaggerTag], if no class comments present
            c.EnableAnnotations();
            c.SwaggerDoc("v1"
                , new OpenApiInfo()
                {
                    Title = "Swagger",
                    Version = "0.0.1",

                });
            c.DocumentFilter<DotCaseDocumentFilter>();
            c.SchemaFilter<ExampleSchemaFilter>();

        });
        return services;
    }

    public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app)
    {
        app.UseSwagger(c =>
        {
            c.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0;
        });

        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Swagger"));
        return app;
    }
}