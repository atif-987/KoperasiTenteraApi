using KoperasiTenteraApi.CommonFunctions;
using KoperasiTenteraApi.Interfaces;
using KoperasiTenteraApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KoperasiTenteraApi.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

       public  Customer CreateCustomer(Customer customer)
        {
            Customer newCustomer = new Customer()
            {
                CustomerName = customer.CustomerName,
                ICNumber = customer.ICNumber,
                MobileNumber = customer.MobileNumber,
                EmailAddress = customer.EmailAddress,
                MobileOtp = Common.GenerateOtp(4),
                MobileOtpExpiry=DateTime.UtcNow.AddMinutes(5),
                EmailOtp = Common.GenerateOtp(4),
                EmailOtpExpiry= DateTime.UtcNow.AddMinutes(5),
                CreatedOn =DateTime.UtcNow
            };
            _context.Customers.Add(newCustomer);
             _context.SaveChanges();
            return newCustomer;
        }

        public Customer UpdateCustomer(Customer customer)
        {
            Customer updatedCustomer= _context.Customers.Update(customer).Entity;
            _context.SaveChanges();
            return updatedCustomer;
        }

        public IQueryable<Customer> GetCustomers()
        {
            return _context.Customers;
        }


        public async Task<bool> VerifyMobileOtpAsync(string phoneNumber, string otp)
        {
            var otpEntry = await _context.Customers
                .Where(o => o.MobileNumber == phoneNumber && o.MobileOtp == otp)
                .FirstOrDefaultAsync();

            if (otpEntry == null || otpEntry.MobileOtpExpiry < DateTime.UtcNow)
                return false;

            otpEntry.MobileOtp = null;
            otpEntry.MobileOtpExpiry = null;
            otpEntry.IsMobileVerified = true;
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<bool> VerifyEmailOtpAsync(string emailAddress, string otp)
        {
            var otpEntry = await _context.Customers
                .Where(o => o.EmailAddress == emailAddress && o.EmailOtp == otp)
                .FirstOrDefaultAsync();

            if (otpEntry == null || otpEntry.EmailOtpExpiry < DateTime.UtcNow)
                return false;

            otpEntry.EmailOtp = null;
            otpEntry.EmailOtpExpiry = null;
            otpEntry.IsEmailVerified = true;
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<Customer?> GetCustomerByEmail(string emailAddress)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.EmailAddress == emailAddress);
        }


        public async Task<Customer?> GetCustomerByICNumber(string icNumber)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.ICNumber == icNumber);
        }

        public async Task<Customer?> GetCustomerByPinAndEmailAddress(CustomerPinRequest request)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Pin == request.pin && x.EmailAddress==request.emailAddress);
        }

        public async Task<Customer?> GetCustomerByICNumberOrEmailAddress(CustomerDetailRequest request)
        {
            return await _context.Customers.FirstOrDefaultAsync(x =>  (!string.IsNullOrEmpty(request.ICNumber) &&  x.ICNumber == request.ICNumber) || (!string.IsNullOrEmpty(request.emailAddress) && x.EmailAddress == request.emailAddress));
        }

    }
}
