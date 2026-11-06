// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;

namespace Wpf.Ui.Gallery.Views.Pages.Layout;

/// <summary>
/// Interaction logic for FormPage.xaml
/// </summary>
[GalleryPage("Form control - hosts multiple FormItem with shared title settings.", SymbolRegular.DocumentEdit24)]
public partial class FormPage : Page
{
    public FormPage()
    {
        InitializeComponent();
    }
}
