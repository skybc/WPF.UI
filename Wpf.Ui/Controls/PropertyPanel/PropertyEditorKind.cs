using System;

namespace Wpf.Ui.Controls;

/// <summary>
/// Editor kind for PropertyPanel. When <see cref="Auto"/> is used, the editor is chosen based on the property type.
/// </summary>
public enum PropertyEditorKind
{
    Auto = 0,
    TextBox,
    CheckBox,
    ToggleSwitch,
    ComboBox,
    Slider,
    DatePicker,
    Custom
}
