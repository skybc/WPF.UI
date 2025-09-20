// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Services;

using Wpf.Ui.Controls;

/// <summary>
/// Global service that routes growl messages to a placed <see cref="GrowlControl"/>.
/// </summary>
public interface IGrowlService
{
    /// <summary>
    /// Gets or sets default timeout for messages when not specified.
    /// </summary>
    TimeSpan DefaultDuration { get; set; }

    /// <summary>
    /// Attach a visual host. Typically called once from XAML loaded code-behind automatically.
    /// </summary>
    /// <param name="host">The growl host control placed in a Window or Page.</param>
    void Attach(GrowlControl host);

    /// <summary>
    /// Get the current host if any.
    /// </summary>
    /// <returns>The active <see cref="GrowlControl"/> host, or null if none.</returns>
    GrowlControl GetHost();

    /// <summary>
    /// Show a message with type and auto-close options.
    /// </summary>
    /// <param name="message">The message content.</param>
    /// <param name="type">The message type.</param>
    /// <param name="durationMs">Optional duration in milliseconds; uses default when null.</param>
    /// <param name="autoClose">Whether to auto-close after the timeout.</param>
    void Show(string message, GrowlType type, int? durationMs = null, bool autoClose = true);

    /// <summary>
    /// Show a message with title and content.
    /// </summary>
    /// <param name="title">The title text.</param>
    /// <param name="message">The message content.</param>
    /// <param name="type">The message type.</param>
    /// <param name="durationMs">Optional duration in milliseconds; uses default when null.</param>
    /// <param name="autoClose">Whether to auto-close after the timeout.</param>
    void Show(string title, string message, GrowlType type, int? durationMs = null, bool autoClose = true);

    /// <summary>
    /// Clear all messages if a host is attached.
    /// </summary>
    void Clear();
}
