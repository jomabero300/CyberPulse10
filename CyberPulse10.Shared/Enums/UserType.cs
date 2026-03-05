using CyberPulse10.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse10.Shared.Enums;

public enum UserType
{
    [Display(Name = "Admi", ResourceType = typeof(Literals))]
    Admi,
    [Display(Name = "User", ResourceType = typeof(Literals))]
    User
}