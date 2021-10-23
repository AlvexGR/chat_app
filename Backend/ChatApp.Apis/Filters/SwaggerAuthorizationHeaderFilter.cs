using System.Collections.Generic;
using System.Linq;
using ChatApp.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ChatApp.Apis.Filters
{
    public class SwaggerAuthorizationHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var allowAnonymous = context.ApiDescription.ActionDescriptor
                .EndpointMetadata.Any(item => item is AllowAnonymousAttribute);

            if (allowAnonymous) return;

            operation.Security ??= new List<OpenApiSecurityRequirement>();
            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = GlobalConstants.AuthSchema
                }
            };

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>(),
            });
        }
    }
}
