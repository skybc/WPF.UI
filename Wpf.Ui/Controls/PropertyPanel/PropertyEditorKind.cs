using System;

namespace Wpf.Ui.Controls;

/// <summary>
/// Editor kind for PropertyPanel. When <see cref="Auto"/> is used, the editor is chosen based on the property type.
/// </summary>
public enum PropertyEditorKind
{
    /// <summary>The editor is selected automatically.</summary>
    Auto = 0,
    
    /// <summary>Edits the value through a plain text box.</summary>
    TextBox,
    
    /// <summary>Edits the value through a password box.</summary>
    Password,
    
    /// <summary>Edits the value through a checkbox.</summary>
    CheckBox,
    
    /// <summary>Edits the value through a toggle switch.</summary>
    ToggleSwitch,
    
    /// <summary>Edits the value through a combo box.</summary>
    ComboBox,
    
    /// <summary>Edits the value through a slider.</summary>
    Slider,
    
    /// <summary>Edits the value through a date picker.</summary>
    DatePicker,
    
    /// <summary>Edits the value through a color picker.</summary>
    ColorPicker,
    
    /// <summary>Custom editor provided via template.</summary>
    Custom,
}
