using CyberPulse10.Shared.Entities.Gene;
using Microsoft.AspNetCore.Identity;

namespace CyberPulse10.Backend.Helpers;

public class CustomEmailValidator : IUserValidator<User>
{
    private readonly List<string> _allowedEmailDomains = new List<string> { "gmail.com", "sena.edu.co", "sedarauca.edu.co", "yopmail.com" };

    public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
    {
        var email = user.Email;

        if (string.IsNullOrEmpty(email))
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "EmailDomainInvalid",
                Description = "El correo electrónico es obligatorio y debe ser de un dominio válido."
            }));
        }

        var domain = email.Split('@').LastOrDefault();

        if (string.IsNullOrEmpty(domain) || !_allowedEmailDomains.Contains(domain.ToLowerInvariant()))
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "EmailDomainInvalid",
                Description = "El correo electrónico debe ser de un dominio permitido (sena.edu.co o sedarauca.edu.co)."
            }));
        }

        return Task.FromResult(IdentityResult.Success);

    }
}