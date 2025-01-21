namespace KoperasiTenteraApi.Models.DTOs
{
    public class VerifyCustomerMobileDTO
    {
        public required string MobileNumber { get; set; }
        public required string Otp { get; set;}
    }

    public class VerifyCustomerEmailDTO
    {
        public required string EmailAddress { get; set; }
        public required string Otp { get; set; }
    }

    public class CustomerPinRequest
    {
        public required string pin { get; set; }
        public required string emailAddress { get; set; }
    }

    public class LoginCustomerRequest
    {
        public required string ICNumber { get; set; }
    }
    public class CustomerDetailRequest
    {
        public string? ICNumber { get; set; }

        public string? emailAddress { get; set; }


    }
    public class CustomerDetailsResponse
    {
        public string CustomerName { get; set; } = null!;

        public string ICNumber { get; set; } = null!;

        public string MobileNumber { get; set; } = null!;

        public string EmailAddress { get; set; } = null!;

    }


}
