using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Any;

namespace TreeApi.Swagger
{
    public class CustomSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Applies custom schema configurations to Swagger documentation
        /// </summary>
        /// <param name="schema">The OpenAPI schema to modify</param>
        /// <param name="context">The schema filter context</param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Type == "string" && schema.Format == "date-time")
            {
                schema.Format = "datetime";
            }

            if (schema.Type == "string" && schema.Format == null)
            {
                schema.Format = "string";
            }

            if (schema.Type == "string" && schema.Format == "datetime")
            {
                if (context.MemberInfo?.Name == "CreatedAt")
                {
                    if (context.Type.Name == "MJournalInfo")
                    {
                        schema.Example = new OpenApiString("2025-05-23T12:18:16.922346Z");
                    }
                    else
                    {
                        schema.Example = new OpenApiString("2025-05-23T12:18:16.9222634Z");
                    }
                }
                else if (context.MemberInfo?.Name == "From")
                {
                    schema.Example = new OpenApiString("2025-05-23T12:18:16.9223615Z");
                }
                else if (context.MemberInfo?.Name == "To")
                {
                    schema.Example = new OpenApiString("2025-05-23T12:18:16.9223726Z");
                }
            }
        }
    }
}
