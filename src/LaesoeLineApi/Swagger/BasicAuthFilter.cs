using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

namespace LaesoeLineApi.Swagger
{
    // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/226#issuecomment-271524895
    public class BasicAuthFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var securityRequirements = new Dictionary<string, IEnumerable<string>>()
            {
                { "basic", Array.Empty<string>() }  // in swagger you specify empty list unless using OAuth2 scopes
            };

            swaggerDoc.Security = new[] { securityRequirements };
        }
    }
}
