using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TreeApi.Swagger
{
    public class MinLengthRemoverSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Removes MinLength constraints from schema properties in Swagger documentation
        /// </summary>
        /// <param name="schema">The OpenAPI schema to modify</param>
        /// <param name="context">The schema filter context</param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties != null)
            {
                foreach (var property in schema.Properties.Values)
                {
                    property.MinLength = null;
                }
            }
        }
    }
}
