using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace TreeApi.Models
{
    [SwaggerSchema(Description = "FxNet.Web.Def.Api.Diagnostic.Model.MJournalInfo")]
    public class MJournalInfo
    {
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
