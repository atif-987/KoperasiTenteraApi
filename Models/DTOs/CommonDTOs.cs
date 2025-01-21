namespace KoperasiTenteraApi.Models.DTOs
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        public dynamic? response { get; set; }
    }
}
