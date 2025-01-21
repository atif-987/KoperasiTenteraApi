using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using KoperasiTenteraApi.Interfaces;

namespace KoperasiTenteraApi.Services
{
    public class SmsService : ISmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;

        public SmsService(IConfiguration configuration)
        {
            _accountSid = configuration["TwilioSmsSettings:AccountSid"] ??"";
            _authToken = configuration["TwilioSmsSettings:AuthToken"] ??"";
            _fromPhoneNumber = configuration["TwilioSmsSettings:FromPhoneNumber"] ??"";

            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task<string> SendSmsAsync(string toPhoneNumber, string message)
        {
            try
            {
                var messageResponse = await MessageResource.CreateAsync(
                    to: new PhoneNumber(toPhoneNumber),
                    from: new PhoneNumber(_fromPhoneNumber),
                    body: message
                );

                return messageResponse.Sid;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send SMS: {ex.Message}", ex);
            }
        }
    }
}
