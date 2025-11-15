using System;

namespace Wpf.Ui.Controls;

/// <summary>
/// Attribute for file selection properties. Used with PropertyPanelAttribute
/// when Editor is set to FileSelect or DirectorySelect.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class PropertyFileAttribute : Attribute
{
    /// <summary>
    /// File extension filter for file dialogs (e.g., ".txt|.docx|.jpg").
    /// Format: "extension1|extension2|..." or ".txt|.docx"
    /// Leave empty or null for no filter (all files).
    /// </summary>
    public string? Extension { get; set; }

    /// <summary>
    /// Initializes a new instance of the PropertyFileAttribute class.
    /// </summary>
    public PropertyFileAttribute()
    {
        Extension = null;
    }

    /// <summary>
    /// Initializes a new instance of the PropertyFileAttribute class with extension filter.
    /// </summary>
    /// <param name="extension">File extension filter (e.g., ".txt|.docx|.jpg")</param>
    public PropertyFileAttribute(string? extension)
    {
        Extension = extension;
    }
}
