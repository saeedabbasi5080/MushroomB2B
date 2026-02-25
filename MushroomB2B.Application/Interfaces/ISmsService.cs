namespace MushroomB2B.Application.Interfaces;

public interface ISmsService
{
    Task SendOtpAsync(string phoneNumber, string code);
}
