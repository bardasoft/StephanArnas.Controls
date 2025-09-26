using System.Text.Json;

namespace CraftUI.Library.Maui.Common.Models;

public class DisplayValueItem(string displayValue, string value, object? valueObject = null)
{
    public string DisplayValue { get; set; } = displayValue;
    public string Value { get; set; } = value;
    public object? ValueObject { get; set; } = valueObject;
    
    public T? GetValueObjectDeserialized<T>()
    {
        if (string.IsNullOrEmpty(ValueObject?.ToString()))
        {
            return default;
        }
        
        return JsonSerializer.Deserialize<T>(ValueObject.ToString()!);
    }
}