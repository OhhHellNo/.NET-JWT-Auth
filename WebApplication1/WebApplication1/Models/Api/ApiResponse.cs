namespace WebApplication1.Models.Api
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(int statusCode, T? data, string message = "Success")
        {
            StatusCode = statusCode;
            Data = data;
            Message = message;
        }
    }

}
