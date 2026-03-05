namespace CyberPulse10.Frontend.Services;

public interface ILoginService
{
    Task LoginAsync(string token);
    Task LogoutAsync();
}