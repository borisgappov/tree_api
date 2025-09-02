using Microsoft.OpenApi.Models;
using TreeApi.Swagger;

namespace TreeApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures Swagger with custom settings for API documentation
        /// </summary>
        /// <param name="services">The service collection to configure</param>
        /// <returns>The configured service collection for method chaining</returns>
        public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Swagger",
                    Version = "0.0.1"
                });

                c.UseAllOfToExtendReferenceSchemas();
                c.UseOneOfForPolymorphism();
                c.SelectDiscriminatorNameUsing(type => type.Name);
                
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
                
                c.TagActionsBy(api => 
                {
                    var controllerName = api.ActionDescriptor.RouteValues["controller"];
                    return controllerName switch
                    {
                        "Journal" => SwaggerTags.Journal,
                        "Partner" => SwaggerTags.Partner,
                        "Tree" => SwaggerTags.Tree,
                        "TreeNode" => SwaggerTags.TreeNode,
                        _ => new[] { controllerName }
                    };
                });
                
                c.DocInclusionPredicate((name, api) => true);
                
                c.CustomSchemaIds(type => 
                {
                    if (type.Name == "MJournal")
                        return "FxNet.Web.Def.Api.Diagnostic.Model.MJournal";
                    if (type.Name == "MJournalInfo")
                        return "FxNet.Web.Def.Api.Diagnostic.Model.MJournalInfo";
                    if (type.Name == "VJournalFilter")
                        return "FxNet.Web.Def.Api.Diagnostic.View.VJournalFilter";
                    if (type.Name == "MNode")
                        return "ReactTest.Tree.Site.Model.MNode";
                    if (type.Name == "MRangeMJournalInfo")
                        return "FxNet.Web.Model.MRange_MJournalInfo";
                    return type.Name;
                });
                
                c.OperationFilter<CustomSwaggerOperationFilter>();
                c.OperationFilter<RenameRequestBodyOperationFilter>();
                c.DocumentFilter<ObjectTypeRemoverDocumentFilter>();
                c.SchemaFilter<CustomSchemaFilter>();
                c.DocumentFilter<CustomDocumentFilter>();
                c.SchemaFilter<MinLengthRemoverSchemaFilter>();
            });

            return services;
        }
    }
}
