// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Controls;

/// <summary>
/// Registry for property editors. Manages the creation and selection of appropriate editors.
/// </summary>
internal static class PropertyEditorRegistry
{
    private static readonly List<IPropertyEditor> _registeredEditors = new();
    private static bool _initialized = false;

    /// <summary>
    /// Initializes the editor registry with default editors.
    /// </summary>
    public static void Initialize()
    {
        if (_initialized)
        {
            return;
        }

        _initialized = true;

        // Register default editors in order of precedence
        RegisterEditor(new FileSelectEditor());
        RegisterEditor(new DirectorySelectEditor());
        RegisterEditor(new ColorPickerEditor());
        RegisterEditor(new DatePickerEditor());
        RegisterEditor(new SliderEditor());
        RegisterEditor(new ComboBoxEditor());
        RegisterEditor(new CheckBoxEditor());
        RegisterEditor(new PasswordEditor());
        RegisterEditor(new TextBoxEditor());
    }

    /// <summary>
    /// Registers a custom editor.
    /// </summary>
    public static void RegisterEditor(IPropertyEditor editor)
    {
        if (editor == null)
        {
            throw new ArgumentNullException(nameof(editor));
        }

        // Insert at the beginning so custom editors have priority
        _registeredEditors.Insert(0, editor);
    }

    /// <summary>
    /// Gets the appropriate editor for the given property item.
    /// </summary>
    public static IPropertyEditor GetEditor(PropertyItem item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        // First, try explicit editor kind
        var editor = _registeredEditors.FirstOrDefault(e => e.CanHandle(item));
        if (editor != null)
        {
            return editor;
        }

        // Fallback to TextBoxEditor for unknown types
        return _registeredEditors.OfType<TextBoxEditor>().FirstOrDefault() 
            ?? throw new InvalidOperationException("TextBoxEditor not registered");
    }

    /// <summary>
    /// Clears all registered editors.
    /// </summary>
    public static void Clear()
    {
        _registeredEditors.Clear();
        _initialized = false;
    }
}
