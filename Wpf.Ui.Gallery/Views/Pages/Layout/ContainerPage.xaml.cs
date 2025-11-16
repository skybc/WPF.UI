// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.Layout;

namespace Wpf.Ui.Gallery.Views.Pages.Layout;

/// <summary>
/// Interaction logic for ContainerPage.xaml
/// </summary>
[GalleryPage("Container layout control similar to Element Plus el-container.", SymbolRegular.AppGeneric24)]
public partial class ContainerPage : INavigableView<ContainerViewModel>
{
    public ContainerPage(ContainerViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
    }

    public ContainerViewModel ViewModel { get; }
}
