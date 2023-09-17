using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BoardGameModels
{
	public static class EnumExtensions
	{
		public static string? GetDisplayName(this Enum enumValue)
		{
			FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
			DisplayAttribute[]? attributes = fieldInfo?.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

			if (attributes is { Length: > 0 })
			{
				return attributes[0].Name;
			}
			else
			{
				return enumValue.ToString(); // Fallback to the enum name
			}
		}
	}
}
