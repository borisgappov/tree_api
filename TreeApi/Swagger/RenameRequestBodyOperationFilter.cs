using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Any;

namespace TreeApi.Swagger
{
    public class RenameRequestBodyOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Renames request body parameters in Swagger documentation
        /// </summary>
        /// <param name="operation">The OpenAPI operation to modify</param>
        /// <param name="context">The operation filter context</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.RequestBody != null)
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Required = false,
                    Content =
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object"
                            }
                        }
                    },
                    Extensions =
                    {
                        ["x-bodyName"] = new OpenApiString("filter"),
                        ["required"] = new OpenApiBoolean(false)
                    }
                };
            }
        }
    }
}
