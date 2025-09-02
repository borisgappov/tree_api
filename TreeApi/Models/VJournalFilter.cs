using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace TreeApi.Models
{
    [SwaggerSchema(Description = "FxNet.Web.Def.Api.Diagnostic.View.VJournalFilter")]
    public class VJournalFilter
    {
        [SwaggerSchema(Description = "from", Format = "datetime")]
        public DateTime? From { get; set; }
        
        [SwaggerSchema(Description = "to", Format = "datetime")]
        public DateTime? To { get; set; }
        
        [Required]
        [SwaggerSchema(Description = "search")]
        public string Search { get; set; } = string.Empty;
    }
}
