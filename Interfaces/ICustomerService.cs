using KoperasiTenteraApi.Models.DTOs;

namespace KoperasiTenteraApi.Interfaces
{
    public interface ICustomerService
    {
        Customer CreateCustomer(Customer customer);
        Customer UpdateCustomer(Customer customer);

        IQueryable<Customer> GetCustomers();

        Task<bool> VerifyMobileOtpAsync(string phoneNumber, string otp);

        Task<bool> VerifyEmailOtpAsync(string emailAddress, string otp);

        Task<Customer?> GetCustomerByEmail(string emailAddress);

        Task<Customer?> GetCustomerByICNumber(string icNumber);

        Task<Customer?> GetCustomerByPinAndEmailAddress(CustomerPinRequest request);

        Task<Customer?> GetCustomerByICNumberOrEmailAddress(CustomerDetailRequest request);
    }
}
