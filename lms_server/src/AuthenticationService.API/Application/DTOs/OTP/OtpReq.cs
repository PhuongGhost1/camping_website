namespace AuthenticationService.API.Application.DTOs.OTP;

public record VerifyOtpRequest
{
    public string Otp { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
