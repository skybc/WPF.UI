// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

public partial class FontPickerViewModel : ViewModel
{
    [ObservableProperty]
    private FontSettings _basicFontSettings = new(
        new FontFamily("Segoe UI"),
        14.0,
        FontWeights.Normal,
        FontStyles.Normal,
        new SolidColorBrush(Color.FromRgb(0, 0, 0))
    );

    [ObservableProperty]
    private FontSettings _titleFontSettings = new(
        new FontFamily("Segoe UI"),
        24.0,
        FontWeights.SemiBold,
        FontStyles.Normal,
        new SolidColorBrush(Color.FromRgb(0, 0, 0))
    );

    [ObservableProperty]
    private FontSettings _bodyFontSettings = new(
        new FontFamily("Segoe UI"),
        14.0,
        FontWeights.Normal,
        FontStyles.Normal,
        new SolidColorBrush(Color.FromRgb(0, 0, 0))
    );

    [ObservableProperty]
    private FontSettings _codeFontSettings = new(
        new FontFamily("Consolas"),
        12.0,
        FontWeights.Normal,
        FontStyles.Normal,
        new SolidColorBrush(Color.FromRgb(0, 0, 0))
    );
}
