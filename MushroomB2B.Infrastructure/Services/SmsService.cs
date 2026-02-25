using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MushroomB2B.Application.Interfaces;

namespace MushroomB2B.Infrastructure.Services;

public sealed class SmsService(
    HttpClient httpClient,
    IConfiguration configuration,
    ILogger<SmsService> logger) : ISmsService
{
    private readonly string _apiKey = configuration["Sms:KavenegarApiKey"]
        ?? throw new InvalidOperationException("Sms:KavenegarApiKey is not configured.");

    public async Task SendOtpAsync(string phoneNumber, string code)
    {
        var url = $"https://api.kavenegar.com/v1/{_apiKey}/verify/lookup.json";

        var payload = new Dictionary<string, string>
        {
            ["receptor"] = phoneNumber,
            ["token"] = code,
            ["template"] = "mushroomOtp"
        };

        try
        {
            var response = await httpClient.PostAsync(url, new FormUrlEncodedContent(payload));
            response.EnsureSuccessStatusCode();

            Console.WriteLine($"🔥 OTP Code: {code} sent to111 {phoneNumber}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send OTP to {PhoneNumber}", phoneNumber);
            throw;
        }
    }
}
