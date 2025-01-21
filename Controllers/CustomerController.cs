using KoperasiTenteraApi.CommonFunctions;
using KoperasiTenteraApi.Interfaces;
using KoperasiTenteraApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoperasiTenteraApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ISmsService _smsService;
        private readonly IEmailService _emailService;


        public CustomerController(ICustomerService customerService,ISmsService smsService,IEmailService emailService)
        {
            _customerService = customerService;
            _smsService = smsService;
            _emailService = emailService;
        }


        [HttpPost("CreateCustomer")]
        public  async Task<ApiResponse> CreateCustomer(Customer customer)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                IQueryable<Customer> customers = _customerService.GetCustomers();
                if (customers.Any(x => x.ICNumber.ToLower().Equals(customer.ICNumber.ToLower())))
                {
                    response.IsSuccess = false;
                    response.Message = "Account Already exist";
                    response.response = "There is account registered with the IC number. Please login to continue.";
                    return response;
                }

                if (customers.Any(x => x.EmailAddress.ToLower().Equals(customer.EmailAddress.ToLower())))
                {
                    response.IsSuccess = false;
                    response.Message = "There is account registered with the email address.";
                    return response;
                }

                if (customers.Any(x => x.MobileNumber.ToLower().Equals(customer.MobileNumber.ToLower())))
                {
                    response.IsSuccess = false;
                    response.Message = "There is account registered with the mobile number.";
                    return response;
                }

                if(!customer.MobileNumber.Contains("+"))
                {
                    response.IsSuccess = false;
                    response.Message = "Please add country code in your mobile number.";
                    return response;
                }
                Customer addedCustomer= _customerService.CreateCustomer(customer);
                string sms = await _smsService.SendSmsAsync(customer.MobileNumber, $"{addedCustomer.MobileOtp} is your otp code for verification.");
                await _emailService.SendEmailAsync(customer.EmailAddress, "OTP code verification", $"{addedCustomer.EmailOtp} is your otp code for verification");
                response.IsSuccess = true;
                response.Message = "Customer Created Successfully";

            }
            catch(Exception ex) {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.response=ex;
            }
            return response;
        }


        [HttpPost("VerifyMobileOTP")]
        public async Task<ApiResponse> VerifyMobileOTP(VerifyCustomerMobileDTO request)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                bool isVerified = await _customerService.VerifyMobileOtpAsync(request.MobileNumber, request.Otp);
                if(isVerified)
                {
                    response.IsSuccess = true;
                    response.Message = "Otp verified";

                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Incorrect OTP";
                    response.response = "Please enter your  OTP again";
                }
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.response = ex;
            }
            return response;
        }


        [HttpPost("VerifyEmailOTP")]
        public async Task<ApiResponse> VerifyEmailOTP(VerifyCustomerEmailDTO request)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                bool isVerified = await _customerService.VerifyEmailOtpAsync(request.EmailAddress, request.Otp);
                if (isVerified)
                {
                    response.IsSuccess = true;
                    response.Message = "Otp verified";

                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Incorrect OTP";
                    response.response = "Please enter your  OTP again";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.response = ex;
            }
            return response;
        }


        [HttpPost("SetCustomerPin")]
        public async Task<ApiResponse> SetPin(CustomerPinRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                Customer? customer = await _customerService.GetCustomerByEmail(request.emailAddress);
                if (customer == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Customer not found";
                    return response;
                }

                customer.Pin = request.pin;
                Customer updatedCustomer = _customerService.UpdateCustomer(customer);
                response.IsSuccess = true;
                response.Message = "Customer Updated Successfully";


            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.response = ex;
            }
            return response;
        }

        [HttpPost("LoginCustomer")]
        public async Task<ApiResponse> LoginCustomer(LoginCustomerRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                Customer? customer = await _customerService.GetCustomerByICNumber(request.ICNumber);
                if (customer == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Customer not found";
                    return response;
                }
                customer.MobileOtp = Common.GenerateOtp(4);
                customer.MobileOtpExpiry = DateTime.UtcNow.AddMinutes(5);
                customer.EmailOtp= Common.GenerateOtp(4);
                customer.EmailOtpExpiry=DateTime.UtcNow.AddMinutes(5);
                Customer dataCustomer = _customerService.UpdateCustomer(customer);
                string sms = await _smsService.SendSmsAsync(customer.MobileNumber, $"{customer.MobileOtp} is your otp code for verification.");
                await _emailService.SendEmailAsync(customer.EmailAddress, "OTP code verification", $"{customer.EmailOtp} is your otp code for verification");
                response.IsSuccess = true;
                response.Message = "Customer Login Successfully";


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.response = ex;
            }
            return response;

        }


        [HttpPost("ConfirmPin")]
        public async Task<ApiResponse> ConfirmPin(CustomerPinRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                Customer? customer = await _customerService.GetCustomerByPinAndEmailAddress(request);
                if (customer == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Unmatched PIN";
                    response.response = "Please enter your PIN again";
                    return response;
                }
                response.IsSuccess = true;
                response.Message = "PIN matched";
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.response = ex;
            }
            return response;
        }

        [HttpPost("GetCustomerDetails")]
        public async Task<ApiResponse> GetCustomerDetails(CustomerDetailRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                Customer? customer = await _customerService.GetCustomerByICNumberOrEmailAddress(request);
                if (customer == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Customer not found";
                    return response;
                }
                var customerDetails = new CustomerDetailsResponse
                {
                    EmailAddress=customer.EmailAddress,
                    ICNumber=customer.ICNumber,
                    MobileNumber=customer.MobileNumber,
                    CustomerName=customer.CustomerName,
                };
                response.IsSuccess = true;
                response.response=customerDetails;
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.response = ex;
            }
            return response;

        }





    }
}
