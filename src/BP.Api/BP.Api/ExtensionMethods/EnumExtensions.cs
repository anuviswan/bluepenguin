using System.ComponentModel; 
using System.Reflection;

namespace BP.Api.ExtensionMethods;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        if (value == null)
            return string.Empty;

        FieldInfo? field = value.GetType().GetField(value.ToString());

        if (field == null)
            return value.ToString();

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? value.ToString();
    }
}
