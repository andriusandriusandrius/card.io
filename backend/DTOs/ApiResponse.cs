namespace backend.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { set; get; }
        public string? Message { set; get; }
        public T? Data { set; get; }
    }
}