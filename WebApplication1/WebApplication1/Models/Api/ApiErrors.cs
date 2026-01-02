namespace WebApplication1.Models.Api
{
    public class ApiError
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; } = false;
        public   required string Message { get; set; }
        public List<string> Errors { get; set; } = new();
    }

}
