using System.ComponentModel;
using System.Reflection;

namespace Web.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum e)
    {
        var name = e.ToString();
        var memberInfo = e.GetType().GetMember(name)[0];
        var descriptionAttributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (descriptionAttributes.Any()) return ((DescriptionAttribute)descriptionAttributes.First()).Description;
        return name;
    }
}
