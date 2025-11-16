// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.StatusAndInfo;

namespace Wpf.Ui.Gallery.Views.Pages.StatusAndInfo;

[GalleryPage("Animated dots to represent indeterminate work.", SymbolRegular.ArrowClockwise24)]
public partial class LoadingCirclePage : INavigableView<LoadingCircleViewModel>
{
    public LoadingCircleViewModel ViewModel { get; }

    public LoadingCirclePage(LoadingCircleViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}