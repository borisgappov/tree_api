using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TreeApi.Swagger
{
    public class CustomDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// Applies custom document configurations to Swagger documentation
        /// </summary>
        /// <param name="swaggerDoc">The OpenAPI document to modify</param>
        /// <param name="context">The document filter context</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = [
                new OpenApiTag { Name = "user.journal", Description = "Represents journal API" },
                new OpenApiTag { Name = "user.partner", Description = "" },
                new OpenApiTag { Name = "user.tree", Description = "Represents entire tree API" },
                new OpenApiTag { Name = "user.tree.node", Description = "Represents tree node API" }
            ];

            var newPaths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                var newPath = path.Key.Replace("/", ".");
                if (newPath.StartsWith('.'))
                {
                    newPath = string.Concat("/", newPath.AsSpan(1));
                }
                newPaths[newPath] = path.Value;
            }
            swaggerDoc.Paths = newPaths;
        }
    }
}
