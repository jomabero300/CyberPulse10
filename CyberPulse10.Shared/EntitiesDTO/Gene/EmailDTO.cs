using CyberPulse10.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse10.Shared.EntitiesDTO.Gene;

public sealed class EmailDTO
{
    [Display(Name = "Email", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [EmailAddress(ErrorMessageResourceName = "ValidEmail", ErrorMessageResourceType = typeof(Literals))]
    public string Email { get; set; } = null!;

    public string Language { get; set; } = null!;
}
