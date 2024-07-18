using System.ComponentModel.DataAnnotations;

namespace Application.Common.Extensions;
public static class UtilityExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var member = enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault();

        var displayAttribute = member?
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .OfType<DisplayAttribute>()
            .FirstOrDefault();

        return displayAttribute?.GetName() ?? nameof(enumValue);
    }
}
