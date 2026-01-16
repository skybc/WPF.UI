// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

namespace Wpf.Ui.Gallery.Views.Pages.BasicInput;

/// <summary>
/// FolderSelect 控件演示页面
/// </summary>
[GalleryPage("Folder selection control.", SymbolRegular.Folder24)]
public partial class FolderSelectPage : INavigableView<FolderSelectViewModel>
{
    public FolderSelectViewModel ViewModel { get; }

    public FolderSelectPage(FolderSelectViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}
