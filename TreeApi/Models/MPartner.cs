using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace TreeApi.Models
{
    [SwaggerSchema(Description = "Partner Model")]
    public class MPartner
    {
        [Required]
        [SwaggerSchema(Description = "id")]
        public long Id { get; set; }
        
        [Required]
        [SwaggerSchema(Description = "code")]
        public string Code { get; set; } = string.Empty;
    }
}
