namespace KoperasiTenteraApi.Interfaces
{
    public interface ISmsService
    {
        Task<string> SendSmsAsync(string toPhoneNumber, string message);
    }
}
