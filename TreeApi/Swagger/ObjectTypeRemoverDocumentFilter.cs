using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TreeApi.Swagger
{
    public class ObjectTypeRemoverDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// Removes Type property from all schemas in Swagger documentation
        /// </summary>
        /// <param name="doc">The OpenAPI document to modify</param>
        /// <param name="context">The document filter context</param>
        public void Apply(OpenApiDocument doc, DocumentFilterContext context)
        {
            foreach (var schema in doc.Components.Schemas.Values)
            {
                schema.Type = null;
            }
        }
    }
}
