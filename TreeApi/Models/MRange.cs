using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace TreeApi.Models
{
    [SwaggerSchema(Description = "FxNet.Web.Model.MRange_MJournalInfo")]
    public class MRange<T>
    {
        [Required]
        [SwaggerSchema(Description = "skip")]
        public int Skip { get; set; }
        
        [Required]
        [SwaggerSchema(Description = "count")]
        public int Count { get; set; }
        
        [Required]
        [SwaggerSchema(Description = "items")]
        public List<T> Items { get; set; } = new List<T>();
    }

    [SwaggerSchema(Description = "FxNet.Web.Model.MRange_MJournalInfo")]
    public class MRangeMJournalInfo
    {
        [Required]
        [SwaggerSchema(Description = "skip")]
        public int Skip { get; set; }
        
        [Required]
        [SwaggerSchema(Description = "count")]
        public int Count { get; set; }
        
        [Required]
        [SwaggerSchema(Description = "items")]
        public List<MJournalInfo> Items { get; set; } = new List<MJournalInfo>();
    }
}
