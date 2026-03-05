using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CyberPulse10.Frontend.Helpers;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var member = value.GetType()
                          .GetMember(value.ToString())
                          .FirstOrDefault();

        if (member == null)
            return value.ToString();

        var attribute = member.GetCustomAttribute<DisplayAttribute>();

        if (attribute == null)
            return value.ToString();

        // 🔹 Esto usa ResourceType automáticamente
        //return attribute.GetName() ?? value.ToString();
        if (attribute.ResourceType != null)
        {
            // Lógica para obtener el valor localizado del recurso
            return attribute.GetName() ?? value.ToString();
        }

        // Si no hay ResourceType, usa el valor de Name
        return attribute.Name ?? value.ToString();
    }
}