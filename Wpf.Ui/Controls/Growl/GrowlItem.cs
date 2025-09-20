// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

/// <summary>
/// Growl item model used by <see cref="GrowlControl"/>.
/// </summary>
public sealed class GrowlItem
{
    /// <summary>Gets or sets the unique id.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the title text.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Gets or sets the text content.</summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>Gets or sets the message type.</summary>
    public GrowlType Type { get; set; } = GrowlType.Info;

    /// <summary>Gets or sets a value indicating whether auto close is enabled.</summary>
    public bool AutoClose { get; set; } = true;

    /// <summary>Gets or sets the duration for auto close.</summary>
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(3000);

    /// <summary>Gets or sets the cancellation token source used for auto-close delay.</summary>
    public System.Threading.CancellationTokenSource Cancellation { get; set; }
}
