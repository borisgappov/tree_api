using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreeApi.Data.Entities
{
    [Table("nodes")]
    public class Node
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        
        [Required]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [Column("tree_id")]
        public long TreeId { get; set; }
        
        [Column("parent_id")]
        public long? ParentId { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        [ForeignKey("TreeId")]
        public virtual Tree Tree { get; set; } = null!;
        
        [ForeignKey("ParentId")]
        public virtual Node? Parent { get; set; }
        
        public virtual ICollection<Node> Children { get; set; } = new List<Node>();
    }
}
