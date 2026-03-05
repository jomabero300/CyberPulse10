using CyberPulse10.Shared.Responses;

namespace CyberPulse10.Backend.Helpers;

public interface IMailHelper
{
    Task<ActionResponse<string>> SendMailAsync(string toName, string toEmail, string subject, string body, string language);
}