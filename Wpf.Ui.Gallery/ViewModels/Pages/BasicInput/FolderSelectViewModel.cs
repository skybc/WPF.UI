// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using CommunityToolkit.Mvvm.ComponentModel;

namespace Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

/// <summary>
/// FolderSelect 控件的 ViewModel
/// </summary>
public partial class FolderSelectViewModel : ObservableObject
{
    /// <summary>
    /// 选定的文件夹路径
    /// </summary>
    [ObservableProperty]
    private string _selectedFolderPath = string.Empty;
}
