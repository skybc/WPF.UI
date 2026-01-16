// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

namespace Wpf.Ui.Gallery.Views.Pages.BasicInput;

/// <summary>
/// FileSelect 控件演示页面
/// </summary>
[GalleryPage("File selection control.", SymbolRegular.OpenFolder24)]
public partial class FileSelectPage : INavigableView<FileSelectViewModel>
{
    public FileSelectViewModel ViewModel { get; }

    public FileSelectPage(FileSelectViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}
