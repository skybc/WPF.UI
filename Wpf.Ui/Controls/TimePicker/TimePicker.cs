// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

/// <summary>
/// Represents a control that allows a user to pick a time value.
/// </summary>
public class TimePicker : System.Windows.Controls.Primitives.ButtonBase
{
    /// <summary>Identifies the <see cref="Header"/> dependency property.</summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(object),
        typeof(TimePicker),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="Time"/> dependency property.</summary>
    public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
        nameof(Time),
        typeof(TimeSpan),
        typeof(TimePicker),
        new PropertyMetadata(TimeSpan.Zero, OnTimeChanged)
    );

    /// <summary>Identifies the <see cref="SelectedTime"/> dependency property.</summary>
    public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(
        nameof(SelectedTime),
        typeof(TimeSpan?),
        typeof(TimePicker),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="MinuteIncrement"/> dependency property.</summary>
    public static readonly DependencyProperty MinuteIncrementProperty = DependencyProperty.Register(
        nameof(MinuteIncrement),
        typeof(int),
        typeof(TimePicker),
        new PropertyMetadata(1)
    );

    /// <summary>Identifies the <see cref="ClockIdentifier"/> dependency property.</summary>
    public static readonly DependencyProperty ClockIdentifierProperty = DependencyProperty.Register(
        nameof(ClockIdentifier),
        typeof(ClockIdentifier),
        typeof(TimePicker),
        new PropertyMetadata(ClockIdentifier.Clock24Hour)
    );

    /// <summary>Identifies the <see cref="IsPopupOpen"/> dependency property.</summary>
    public static readonly DependencyProperty IsPopupOpenProperty = DependencyProperty.Register(
        nameof(IsPopupOpen),
        typeof(bool),
        typeof(TimePicker),
        new PropertyMetadata(false)
    );

    private Slider? _hourSlider;
    private Slider? _minuteSlider;
    private Popup? _popup;
    private bool _isUpdatingFromTime;

    /// <summary>
    /// Gets or sets the content for the control's header.
    /// </summary>
    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the time currently set in the time picker.
    /// </summary>
    public TimeSpan Time
    {
        get => (TimeSpan)GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    /// <summary>
    /// Gets or sets the time currently selected in the time picker
    /// </summary>
    public TimeSpan? SelectedTime
    {
        get => (TimeSpan?)GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value that indicates the time increments shown in the minute picker.
    /// For example, 15 specifies that the TimePicker minute control displays only the choices 00, 15, 30, 45.
    /// </summary>
    public int MinuteIncrement
    {
        get => (int)GetValue(MinuteIncrementProperty);
        set => SetValue(MinuteIncrementProperty, value);
    }

    /// <summary>
    /// Gets or sets the clock system to use.
    /// </summary>
    public ClockIdentifier ClockIdentifier
    {
        get => (ClockIdentifier)GetValue(ClockIdentifierProperty);
        set => SetValue(ClockIdentifierProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the time picker popup is open.
    /// </summary>
    public bool IsPopupOpen
    {
        get => (bool)GetValue(IsPopupOpenProperty);
        set => SetValue(IsPopupOpenProperty, value);
    }

    public TimePicker()
    {
        Click += TimePicker_Click;
    }

    /// <summary>
    /// Called when the template is applied to initialize popup controls.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        // Get references to the popup controls
        _popup = GetTemplateChild("TimePickerPopup") as Popup;
        _hourSlider = GetTemplateChild("HourSlider") as Slider;
        _minuteSlider = GetTemplateChild("MinuteSlider") as Slider;

        // Initialize sliders with current time
        UpdateSliders();
    }

    private void TimePicker_Click(object sender, RoutedEventArgs e)
    {
        IsPopupOpen = !IsPopupOpen;
        if (_popup != null)
        {
            _popup.IsOpen = IsPopupOpen;
        }
        if (IsPopupOpen)
        {
            UpdateSliders();
        }
    }

    private void UpdateSliders()
    {
        _isUpdatingFromTime = true;

        if (_hourSlider != null)
        {
            _hourSlider.Value = Time.Hours;
            _hourSlider.ValueChanged -= HourSlider_ValueChanged;
            _hourSlider.ValueChanged += HourSlider_ValueChanged;
        }

        if (_minuteSlider != null)
        {
            _minuteSlider.Maximum = 59;
            _minuteSlider.TickFrequency = MinuteIncrement;
            _minuteSlider.Value = Time.Minutes;
            _minuteSlider.ValueChanged -= MinuteSlider_ValueChanged;
            _minuteSlider.ValueChanged += MinuteSlider_ValueChanged;
        }

        _isUpdatingFromTime = false;
    }

    private void HourSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (!_isUpdatingFromTime && _hourSlider != null && _minuteSlider != null)
        {
            int hours = (int)_hourSlider.Value;
            int minutes = (int)_minuteSlider.Value;
            Time = new TimeSpan(hours, minutes, 0);
            SelectedTime = Time;
        }
    }

    private void MinuteSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (!_isUpdatingFromTime && _hourSlider != null && _minuteSlider != null)
        {
            int hours = (int)_hourSlider.Value;
            int minutes = (int)_minuteSlider.Value;
            Time = new TimeSpan(hours, minutes, 0);
            SelectedTime = Time;
        }
    }

    private static void OnTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TimePicker timePicker && !timePicker._isUpdatingFromTime)
        {
            timePicker.UpdateSliders();
        }
    }
}
