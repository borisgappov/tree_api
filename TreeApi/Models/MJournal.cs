using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace TreeApi.Models
{
    [SwaggerSchema(Description = "FxNet.Web.Def.Api.Diagnostic.Model.MJournal")]
    public class MJournal
    {
        [Required]
        [SwaggerSchema(Description = "text")]
        public string Text { get; set; } = string.Empty;
        
        [Required]
        [SwaggerSchema(Description = "id")]
        public long Id { get; set; }
        
        [Required]
        [SwaggerSchema(Description = "eventId")]
        public long EventId { get; set; }
        
        [Required]
        [SwaggerSchema(Description = "createdAt", Format = "datetime")]
        public DateTime CreatedAt { get; set; }
    }
}
