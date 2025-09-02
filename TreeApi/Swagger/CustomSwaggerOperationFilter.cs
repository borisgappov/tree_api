using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TreeApi.Swagger
{
    public class CustomSwaggerOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies custom operation configurations to Swagger documentation
        /// </summary>
        /// <param name="operation">The OpenAPI operation to modify</param>
        /// <param name="context">The operation filter context</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var methodName = context.MethodInfo.Name;
            var controllerName = context.MethodInfo.DeclaringType?.Name;

            switch (methodName)
            {
                case "GetRange" when controllerName == "JournalController":
                    operation.Summary = "";
                    operation.Description = "Provides the pagination API. Skip means the number of items should be skipped by server. Take means the maximum number items should be returned by server. All fields of the filter are optional. ";
                    break;
                case "GetSingle" when controllerName == "JournalController":
                    operation.Summary = "";
                    operation.Description = "Returns the information about an particular event by ID.";
                    break;
                case "RememberMe" when controllerName == "PartnerController":
                    operation.Summary = "";
                    operation.Description = "";
                    break;
                case "Get" when controllerName == "TreeController":
                    operation.Summary = "";
                    operation.Description = "Returns your entire tree. If your tree doesn't exist it will be created automatically.";
                    break;
                case "Create" when controllerName == "TreeNodeController":
                    operation.Summary = "";
                    operation.Description = "Create a new node in your tree. You must to specify a parent node ID that belongs to your tree. A new node name must be unique across all siblings.";
                    break;
                case "Delete" when controllerName == "TreeNodeController":
                    operation.Summary = "";
                    operation.Description = "Delete an existing node in your tree. You must specify a node ID that belongs your tree.";
                    break;
                case "Rename" when controllerName == "TreeNodeController":
                    operation.Summary = "";
                    operation.Description = "Rename an existing node in your tree. You must specify a node ID that belongs your tree. A new name of the node must be unique across all siblings.";
                    break;
            }

            foreach (var parameter in operation.Parameters)
            {
                if (parameter.In.ToString() == "Query")
                {
                    switch (parameter.Name)
                    {
                        case "skip":
                        case "take":
                        case "id":
                        case "code":
                        case "treeName":
                        case "parentNodeId":
                        case "nodeName":
                        case "nodeId":
                        case "newNodeName":
                            parameter.Required = true;
                            break;
                    }
                }
            }

            foreach (var response in operation.Responses)
            {
                if (response.Key == "200")
                {
                    response.Value.Description = "Successful response";
                }
            }
        }
    }
}
