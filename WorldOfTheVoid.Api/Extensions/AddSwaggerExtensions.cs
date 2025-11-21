using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WorldOfTheVoid.Domain;

namespace WorldOfTheVoid.Extensions;

public static class AddSwaggerExtensions
{
    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "World of the Void API",
                Version = "v1"
            });

            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Enter ONLY your JWT token. 'Bearer ' will be added automatically.",

                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            // This defines the scheme so the Authorize button appears
            c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

            // This applies the scheme to all operations
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
            
            c.SchemaFilter<EntityIdSchemaFilter>();
        });
    }
}

public sealed class EntityIdSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(EntityId))
        {
            schema.Type = "string";
            schema.Format = null;
            schema.Properties.Clear();
            schema.Reference = null;
        }
    }
}
