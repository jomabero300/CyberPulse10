using CyberPulse10.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse10.Shared.EntitiesDTO.Gene;

public class TaxeFormDTO:TaxeDTO
{
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public StatuDTO? Statu { get; set; }
}
