using CyberPulse10.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace CyberPulse10.Frontend.Pages;

public partial class NotFound
{
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
}