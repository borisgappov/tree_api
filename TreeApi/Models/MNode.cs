using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace TreeApi.Models
{
    [SwaggerSchema(Description = "ReactTest.Tree.Site.Model.MNode")]
    public class MNode
    {
        [Required]
        [SwaggerSchema(Description = "id")]
        public long Id { get; set; }
        
        [Required]
        [SwaggerSchema(Description = "name")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [SwaggerSchema(Description = "children")]
        public List<MNode> Children { get; set; } = new List<MNode>();
    }
}
