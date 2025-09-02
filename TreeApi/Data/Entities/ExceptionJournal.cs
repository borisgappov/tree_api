using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TreeApi.Utils;

namespace TreeApi.Data.Entities
{
    [Table("exception_journal")]
    public class ExceptionJournal
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        
        [Required]
        [Column("event_id")]
        public long EventId { get; set; } = SnowflakeIdGenerator.NextId();
        
        [Required]
        [Column("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        [Column("query_parameters")]
        public string? QueryParameters { get; set; }
        
        [Column("body_parameters")]
        public string? BodyParameters { get; set; }
        
        [Required]
        [Column("stack_trace")]
        public string StackTrace { get; set; } = string.Empty;
        
        [Column("exception_message")]
        public string? ExceptionMessage { get; set; }
        
        [Column("request_path")]
        public string? RequestPath { get; set; }
        
        [Column("http_method")]
        public string? HttpMethod { get; set; }
    }
}
