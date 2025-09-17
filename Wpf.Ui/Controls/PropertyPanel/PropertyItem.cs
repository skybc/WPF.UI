using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Wpf.Ui.Controls;

/// <summary>
/// Runtime item representing a reflected property to be edited.
/// </summary>
public sealed class PropertyItem : INotifyPropertyChanged
{
    private readonly object _owner;
    private readonly PropertyInfo _property;
    private readonly PropertyPanelAttribute _attribute;
    private readonly Type _propertyType;
    private readonly Type _underlyingType;
    private readonly INotifyPropertyChanged? _ownerInpc;

    public PropertyItem(object owner, PropertyInfo property, PropertyPanelAttribute attribute)
    {
        _owner = owner;
        _property = property;
        _attribute = attribute;

        _propertyType = property.PropertyType;
        _underlyingType = Nullable.GetUnderlyingType(_propertyType) ?? _propertyType;

        if (IsEnum)
        {
            EnumValues = Enum.GetValues(_underlyingType);
        }

        _ownerInpc = owner as INotifyPropertyChanged;
        if (_ownerInpc != null)
        {
            _ownerInpc.PropertyChanged += OwnerOnPropertyChanged;
        }
    }

    public string Name => _property.Name;

    public string DisplayName => string.IsNullOrWhiteSpace(_attribute.DisplayName) ? Name : _attribute.DisplayName!;

    public string? Description => _attribute.Description;

    public string? GroupName => _attribute.GroupName;

    public int Order => _attribute.Order;

    public PropertyPanelAttribute Attribute => _attribute;

    public Type PropertyType => _propertyType;

    public Type UnderlyingType => _underlyingType;

    public bool IsReadOnly => !_property.CanWrite;

    public bool IsEnum => _underlyingType.IsEnum;

    public Array? EnumValues { get; }

    public double Min => _attribute.Min;

    public double Max => _attribute.Max;

    public double Step => _attribute.Step;

    public object? CurrentValue
    {
        get => _property.GetValue(_owner);
        set
        {
            if (IsReadOnly)
                return;

            var converted = ConvertToPropertyType(value);
            _property.SetValue(_owner, converted);
            OnPropertyChanged(nameof(CurrentValue));
        }
    }

    private object? ConvertToPropertyType(object? value)
    {
        if (value == null)
        {
            return _propertyType.IsValueType && Nullable.GetUnderlyingType(_propertyType) == null
                ? Activator.CreateInstance(_propertyType)
                : null;
        }

        var targetType = _underlyingType;

        if (targetType.IsEnum)
        {
            if (value is string s)
            {
                return Enum.Parse(targetType, s);
            }
            if (value.GetType().IsEnum)
            {
                return value;
            }
            return Enum.ToObject(targetType, System.Convert.ChangeType(value, Enum.GetUnderlyingType(targetType), CultureInfo.InvariantCulture)!);
        }

        if (targetType == typeof(string))
            return value?.ToString();

        try
        {
            return System.Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
        }
        catch
        {
            return value;
        }
    }

    private void OwnerOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == Name || string.IsNullOrEmpty(e.PropertyName))
        {
            OnPropertyChanged(nameof(CurrentValue));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
