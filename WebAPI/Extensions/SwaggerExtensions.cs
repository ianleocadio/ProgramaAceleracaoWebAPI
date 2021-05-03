using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace WebAPI.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Programa de Aceleração - API",
                        Version = "v1",
                        Description = "API do Programa de Aceleração"
                    });

                var securityScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                };

                options.AddSecurityDefinition("Bearer", securityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });


                // use it if you want to hide Paths and Definitions from OpenApi documentation correctly
                options.UseAllOfToExtendReferenceSchemas();

                // if you want to add xml comments from inheritdocs (from summary and remarks) into the swagger documentation, add:
                // you can exclude remarks for concrete types
                options.IncludeXmlCommentsFromInheritDocs(includeRemarks: true, excludedTypes: typeof(string));

                // or add without remarks
                //options.IncludeXmlComments(xmlFilePath);

                // Add filters to fix enums
                // use by default:
                //options.AddEnumsWithValuesFixFilters();

                // or configured:
                options.AddEnumsWithValuesFixFilters(services, o =>
                {
                    // add schema filter to fix enums (add 'x-enumNames' for NSwag) in schema
                    o.ApplySchemaFilter = true;

                    // add parameter filter to fix enums (add 'x-enumNames' for NSwag) in schema parameters
                    o.ApplyParameterFilter = true;

                    // add document filter to fix enums displaying in swagger document
                    o.ApplyDocumentFilter = true;

                    // add descriptions from DescriptionAttribute or xml-comments to fix enums (add 'x-enumDescriptions' for schema extensions) for applied filters
                    o.IncludeDescriptions = true;

                    // add remarks for descriptions from xml-comments
                    o.IncludeXEnumRemarks = true;

                    // get descriptions from DescriptionAttribute then from xml-comments
                    o.DescriptionSource = DescriptionSources.DescriptionAttributesThenXmlComments;
                });

            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Programa de Aceleração API");
                c.RoutePrefix = string.Empty;  // Set Swagger UI at apps root
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
            });

            return app;
        }
    }
}
