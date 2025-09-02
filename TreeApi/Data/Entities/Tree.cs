using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreeApi.Data.Entities
{
    [Table("trees")]
    public class Tree
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        
        [Required]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public virtual ICollection<Node> Nodes { get; set; } = new List<Node>();
    }
}
