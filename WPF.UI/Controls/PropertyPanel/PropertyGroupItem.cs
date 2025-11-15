// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

/// <summary>
/// Represents a group of PropertyItem objects grouped by GroupName.
/// Used when PropertyPanelAttribute.GroupName is set.
/// </summary>
public sealed class PropertyGroupItem : INotifyPropertyChanged, IDisposable
{
    private string groupName;
    private bool isExpanded = true;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyGroupItem"/> class.
    /// </summary>
    /// <param name="groupName">The name of the group.</param>
    public PropertyGroupItem(string groupName)
    {
        this.groupName = groupName ?? "General";
        this.Items = new ObservableCollection<PropertyItem>();
    }

    /// <summary>
    /// Gets or sets the name of the group.
    /// </summary>
    public string GroupName
    {
        get => this.groupName;
        set
        {
            if (this.groupName != value)
            {
                this.groupName = value;
                this.OnPropertyChanged(nameof(GroupName));
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this group is expanded.
    /// </summary>
    public bool IsExpanded
    {
        get => this.isExpanded;
        set
        {
            if (this.isExpanded != value)
            {
                this.isExpanded = value;
                this.OnPropertyChanged(nameof(IsExpanded));
            }
        }
    }

    /// <summary>
    /// Gets the collection of PropertyItem objects in this group.
    /// </summary>
    public ObservableCollection<PropertyItem> Items { get; }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Cleans up resources and disposes items.
    /// </summary>
    public void Dispose()
    {
        if (this.isDisposed)
        {
            return;
        }

        this.isDisposed = true;

        foreach (var item in this.Items)
        {
            item.Dispose();
        }

        this.Items.Clear();
    }

    private void OnPropertyChanged(string name)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
