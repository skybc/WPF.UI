using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace Wpf.Ui.Controls;

/// <summary>
/// Runtime item representing a reflected property to be edited.
/// </summary>
public sealed class PropertyItem : INotifyPropertyChanged, IDisposable
{
    private readonly object _owner;
    private readonly PropertyInfo _property;
    private readonly PropertyPanelAttribute _attribute;
    private readonly PropertyFileAttribute? _fileAttribute;
    private readonly PropertyComboBoxAttribute? _comboBoxAttribute;
    private readonly Type _propertyType;
    private readonly Type _underlyingType;
    private readonly INotifyPropertyChanged? _ownerInpc;
    private IValueConverter? _converter;
    private bool _converterInitialized;
    private bool _isDisposed;

    public PropertyItem(object owner, PropertyInfo property, PropertyPanelAttribute attribute)
    {
        this._owner = owner;
        this._property = property;
        this._attribute = attribute;

        _propertyType = property.PropertyType;
        _underlyingType = Nullable.GetUnderlyingType(_propertyType) ?? _propertyType;

        // Try to get PropertyFileAttribute if it exists
        _fileAttribute = property.GetCustomAttribute<PropertyFileAttribute>();

        // Try to get PropertyComboBoxAttribute if it exists
        _comboBoxAttribute = property.GetCustomAttribute<PropertyComboBoxAttribute>();

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
    public object Owner => _owner;
    public string Name => _property.Name;

    public string DisplayName => string.IsNullOrWhiteSpace(_attribute.DisplayName) ? Name : _attribute.DisplayName!;

    public double DisplayNameWidth => _attribute.DisplayNameWidth;

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

    public PropertyFileAttribute? FileAttribute => _fileAttribute;

    public PropertyComboBoxAttribute? ComboBoxAttribute => _comboBoxAttribute;

    /// <summary>
    /// Gets the IValueConverter instance if one is specified in the attribute.
    /// Converter is lazily instantiated and cached.
    /// </summary>
    public IValueConverter? Converter => GetConverter();

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

    /// <summary>
    /// Gets or creates the IValueConverter instance from the Converter type specified in the attribute.
    /// </summary>
    /// <returns>An instance of the converter, or null if no converter is specified or instantiation fails.</returns>
    private IValueConverter? GetConverter()
    {
        if (_converterInitialized)
        {
            return _converter;
        }

        _converterInitialized = true;

        var converterType = _attribute.Converter;
        if (converterType == null)
        {
            return null;
        }

        // Validate that the type implements IValueConverter
        if (!typeof(IValueConverter).IsAssignableFrom(converterType))
        {
            System.Diagnostics.Debug.WriteLine(
                $"Warning: Converter type '{converterType.FullName}' does not implement IValueConverter");
            return null;
        }

        try
        {
            // Try to instantiate the converter with a parameterless constructor
            var instance = Activator.CreateInstance(converterType) as IValueConverter;
            _converter = instance;
            return instance;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Error instantiating converter '{converterType.FullName}': {ex.Message}");
            return null;
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

    /// <summary>
    /// Cleans up resources and event handlers to prevent memory leaks.
    /// </summary>
    public void Dispose()
    {
        if (this._isDisposed)
        {
            return;
        }

        this._isDisposed = true;

        // Unsubscribe from owner property changes
        if (this._ownerInpc != null)
        {
            this._ownerInpc.PropertyChanged -= OwnerOnPropertyChanged;
        }
    }
}
