namespace WebApplication1.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public List<string> Errors { get; }

        public ApiException(int statusCode, string message, List<string>? errors = null)
            : base(message)
        {
            StatusCode = statusCode;
            Errors = errors ?? new();
        }
    }
}
