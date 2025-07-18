namespace CraftUI.Library.Maui.Common.Extensions;

public static class ObjectExtension
{
    public static T GetPropertyValue<T>(this object? item, string? propertyName)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (string.IsNullOrEmpty(propertyName))
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        var type = item.GetType();
        var propertyInfo = type.GetProperty(propertyName);

        if (propertyInfo == null)
        {
            throw new ArgumentException($"Property {propertyName} was not found in the object {type.Name}");
        }

        return (T)propertyInfo.GetValue(item)!;
    }
    
    public static string? GetDisplayString(this object? item, string? propertyName)
    {
        if (item == null || string.IsNullOrEmpty(propertyName))
        {
            return item?.ToString();
        }

        var prop = item.GetType().GetProperty(propertyName);
        return prop?.GetValue(item)?.ToString();
    }
}