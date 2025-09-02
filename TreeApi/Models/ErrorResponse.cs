namespace TreeApi.Models
{
    public class ErrorResponse
    {
        public string Type { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public ErrorData Data { get; set; } = new ErrorData();
    }
    
    public class ErrorData
    {
        public string Message { get; set; } = string.Empty;
    }
}
