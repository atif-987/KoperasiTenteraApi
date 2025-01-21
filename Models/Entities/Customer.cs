using System.ComponentModel.DataAnnotations;

public class Customer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string ICNumber { get; set; } = null!;

    public string MobileNumber { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    [MaxLength(4)]
    public string? MobileOtp { get; set; }

    [MaxLength(4)]
    public string? EmailOtp { get; set; }

    [MaxLength(6)]
    public string? Pin { get; set; }

    public bool? IsMobileVerified { get; set; }

    public bool? IsEmailVerified { get; set; }

    public DateTime? MobileOtpExpiry { get; set; } 

    public DateTime? EmailOtpExpiry { get; set; }  

    public DateTime CreatedOn { get; set; }
}
