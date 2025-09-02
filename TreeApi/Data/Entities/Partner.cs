using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreeApi.Data.Entities
{
    [Table("partners")]
    public class Partner
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        
        [Required]
        [Column("code")]
        [StringLength(255)]
        public string Code { get; set; } = string.Empty;
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
