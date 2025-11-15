// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Collections;

public partial class TransferViewModel : ViewModel
{
    [ObservableProperty]
    private ObservableCollection<string> _availableItems =
    [
        "Adobe Photoshop",
        "Autodesk Maya",
        "Blender",
        "Cinema 4D",
        "CorelDRAW",
        "GIMP",
        "Inkscape",
        "Krita",
        "LightWave 3D",
        "Lightroom",
        "Pixelmator",
    ];

    [ObservableProperty]
    private ObservableCollection<string> _selectedItems = new ObservableCollection<string>
    {
        "Visual Studio",
        "Visual Studio Code",
    };
}
