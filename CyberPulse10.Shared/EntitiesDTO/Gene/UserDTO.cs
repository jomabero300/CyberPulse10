using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse10.Shared.EntitiesDTO.Gene;

public sealed class UserDTO:User
{
    [DataType(DataType.Password)]
    [Display(Name = "Password", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = "LengthField", ErrorMessageResourceType = typeof(Literals))]

    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
        ErrorMessageResourceName = "PasswordParameters",ErrorMessageResourceType = typeof(Literals))]

    public string Password { get; set; } = null!;

    [Compare("Password", ErrorMessageResourceName = "PasswordAndConfirmationDifferent", ErrorMessageResourceType = typeof(Literals))]
    [DataType(DataType.Password)]
    [Display(Name = "PasswordConfirm", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = "LengthField", ErrorMessageResourceType = typeof(Literals))]
    public string PasswordConfirm { get; set; } = null!;

    public string Language { get; set; } = null!;
}
