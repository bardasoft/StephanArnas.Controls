using System.Text.Json;

namespace CraftUI.Library.Maui.Common.Models;

public class DisplayValueItem(string displayValue, string value, object? valueObject = null)
{
    public string DisplayValue { get; set; } = displayValue;
    public string Value { get; set; } = value;
    public object? ValueObject { get; set; } = valueObject;
    
    public T? GetValueObjectDeserialized<T>()
    {
        if (ValueObject is T value)
        {
            return value;
        }

        if (ValueObject is string valueString && (valueString.StartsWith('{') || valueString.StartsWith('[')))
        {
            return JsonSerializer.Deserialize<T>(ValueObject.ToString()!);
        }

        return default;
    }
}