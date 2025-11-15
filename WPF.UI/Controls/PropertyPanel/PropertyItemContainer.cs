// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

/// <summary>
/// Container that wraps a PropertyItem and its associated editor control.
/// Handles layout and lifetime management to prevent memory leaks.
/// </summary>
internal sealed class PropertyItemContainer : Grid, IDisposable
{
    private FrameworkElement editor;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyItemContainer"/> class.
    /// </summary>
    public PropertyItemContainer()
    {
        this.VerticalAlignment = VerticalAlignment.Top; 
        this.Margin = new Thickness(0, 4, 0, 4);

        // Create columns
        this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });  // Default width
        this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(8) });
        this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        this.Loaded += this.OnLoaded;
        this.Unloaded += this.OnUnloaded;
        this.DataContextChanged += this.OnDataContextChanged;
    }

    /// <summary>
    /// Gets the PropertyItem associated with this container.
    /// </summary>
    public PropertyItem Item => this.DataContext as PropertyItem;

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        // Initialize when DataContext is set via ItemsControl/DataTemplate
        if (e.NewValue is PropertyItem item && this.Children.Count == 0)
        {
            this.InitializeContainer(item);
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // Fallback: initialize when loaded if not already initialized
        if (this.DataContext is PropertyItem item && this.Children.Count == 0)
        {
            this.InitializeContainer(item);
        }
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        this.Dispose();
    }

    private void InitializeContainer(PropertyItem item)
    {
        // Clear existing children
        this.Children.Clear();

        if (item == null)
        {
            return;
        }

        // Get parent PropertyPanel from visual tree
        PropertyPanel panel = this.FindParent<PropertyPanel>(this);
        if (panel == null)
        {
            return;
        }

        // Update Grid columns based on panel settings
        this.ColumnDefinitions.Clear();
        double labelWidth = item.DisplayNameWidth > 0 ? item.DisplayNameWidth : panel.DisplayNameWidth;
        this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(labelWidth) });  // Label column
        this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(8) });  // Spacer
        this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });  // Editor column

        // Create label
        var label = new TextBlock
        {
            Text = item.DisplayName,
            VerticalAlignment = panel.DisplayNameVerticalAlignment,
            HorizontalAlignment = panel.DisplayNameHorizontalAlignment,
            Margin = new Thickness(0, 0, 4, 0),
        };

        Grid.SetColumn(label, 0);
        this.Children.Add(label);

        // Create editor
        this.editor = PropertyEditorFactory.CreateEditor(item, panel);
        this.editor.VerticalAlignment = VerticalAlignment.Center;
        this.editor.Margin = new Thickness(0, 2, 0, 2);
        Grid.SetColumn(this.editor, 2);
        this.Children.Add(this.editor);
    }

    /// <summary>
    /// Cleans up resources and unbinds to prevent memory leaks.
    /// </summary>
    public void Dispose()
    {
        if (this.isDisposed)
        {
            return;
        }

        this.isDisposed = true;

        this.Loaded -= this.OnLoaded;
        this.Unloaded -= this.OnUnloaded;

        // Clear all bindings on the editor to prevent memory leaks
        if (this.editor != null)
        {
            BindingOperations.ClearAllBindings(this.editor);

            // If editor is a panel with children, clear their bindings too
            if (this.editor is Panel editorPanel)
            {
                foreach (UIElement child in editorPanel.Children)
                {
                    if (child is FrameworkElement fe)
                    {
                        BindingOperations.ClearAllBindings(fe);
                    }
                }
            }
        }

        this.Children.Clear();
        BindingOperations.ClearAllBindings(this);
    }

    /// <summary>
    /// Finds the parent element of the specified type in the visual tree.
    /// </summary>
    private T FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        DependencyObject parent = VisualTreeHelper.GetParent(child);

        while (parent != null)
        {
            if (parent is T typedParent)
            {
                return typedParent;
            }

            parent = VisualTreeHelper.GetParent(parent);
        }

        return null;
    }
}
