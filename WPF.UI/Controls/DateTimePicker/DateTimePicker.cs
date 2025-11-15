// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

/// <summary>
/// Represents a control that allows a user to pick a date and time value.
/// </summary>
public class DateTimePicker : System.Windows.Controls.Control
{
    /// <summary>Identifies the <see cref="Header"/> dependency property.</summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(object),
        typeof(DateTimePicker),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="DateTime"/> dependency property.</summary>
    public static readonly DependencyProperty DateTimeProperty = DependencyProperty.Register(
        nameof(DateTime),
        typeof(System.DateTime),
        typeof(DateTimePicker),
        new PropertyMetadata(System.DateTime.Now, OnDateTimeChanged)
    );

    /// <summary>Identifies the <see cref="SelectedDateTime"/> dependency property.</summary>
    public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register(
        nameof(SelectedDateTime),
        typeof(System.DateTime?),
        typeof(DateTimePicker),
        new PropertyMetadata(null)
    );

    
    /// <summary>Identifies the <see cref="DateTimeFormat"/> dependency property.</summary>
    public static readonly DependencyProperty DateTimeFormatProperty = DependencyProperty.Register(
        nameof(DateTimeFormat),
        typeof(string),
        typeof(DateTimePicker),
        new PropertyMetadata("yyyy-MM-dd HH:mm:ss", OnDateTimeFormatChanged)
    );

    private Calendar? _calendar;
    private Popup? _popup;
    private System.Windows.Controls.TextBox? _dateTimeTextBox;
    private NumberBox? _hourNumberBox;
    private NumberBox? _minuteNumberBox;
    private NumberBox? _secondNumberBox;
    private bool _isUpdatingFromDateTime;
    private bool _isUpdatingFromTextBox;
    private bool _numberBoxButtonClicked;

    /// <summary>
    /// Gets or sets the content for the control's header.
    /// </summary>
    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the date and time currently set in the date time picker.
    /// </summary>
    public System.DateTime DateTime
    {
        get => (System.DateTime)GetValue(DateTimeProperty);
        set => SetValue(DateTimeProperty, value);
    }

    /// <summary>
    /// Gets or sets the date and time currently selected in the date time picker.
    /// </summary>
    public System.DateTime? SelectedDateTime
    {
        get => (System.DateTime?)GetValue(SelectedDateTimeProperty);
        set => SetValue(SelectedDateTimeProperty, value);
    }

 

    /// <summary>
    /// Gets or sets the date time format string.
    /// </summary>
    public string DateTimeFormat
    {
        get => (string)GetValue(DateTimeFormatProperty);
        set => SetValue(DateTimeFormatProperty, value);
    }

    public DateTimePicker()
    {
    }

    /// <summary>
    /// Called when the template is applied to initialize popup controls.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        // Get references to the popup controls
        _popup = GetTemplateChild("PART_Popup") as Popup;
        _calendar = GetTemplateChild("PART_Calendar") as Calendar;
        _dateTimeTextBox = GetTemplateChild("PART_DateTimeTextBox") as System.Windows.Controls.TextBox;
        _hourNumberBox = GetTemplateChild("PART_HourNumberBox") as NumberBox;
        _minuteNumberBox = GetTemplateChild("PART_MinuteNumberBox") as NumberBox;
        _secondNumberBox = GetTemplateChild("PART_SecondNumberBox") as NumberBox;

        // Get the button and subscribe to its click event
        var button = GetTemplateChild("PART_Button") as System.Windows.Controls.Button;
        if (button != null)
        {
            button.Click -= DateTimePicker_Click;
            button.Click += DateTimePicker_Click;
        }

        // Initialize with current datetime
        UpdateDisplay();

        // Subscribe to calendar selection changed
        if (_calendar != null)
        {
            _calendar.SelectedDatesChanged -= Calendar_SelectedDatesChanged;
            _calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
        }

        // Subscribe to datetime textbox text changed
        if (_dateTimeTextBox != null)
        {
            _dateTimeTextBox.TextChanged -= DateTimeTextBox_TextChanged;
            _dateTimeTextBox.TextChanged += DateTimeTextBox_TextChanged;
        }

        // Subscribe to hour, minute, and second numberbox changes
        if (_hourNumberBox != null)
        {
            _hourNumberBox.ValueChanged -= TimeNumberBox_ValueChanged;
            _hourNumberBox.ValueChanged += TimeNumberBox_ValueChanged;
        }

        if (_minuteNumberBox != null)
        {
            _minuteNumberBox.ValueChanged -= TimeNumberBox_ValueChanged;
            _minuteNumberBox.ValueChanged += TimeNumberBox_ValueChanged;
        }

        if (_secondNumberBox != null)
        {
            _secondNumberBox.ValueChanged -= TimeNumberBox_ValueChanged;
            _secondNumberBox.ValueChanged += TimeNumberBox_ValueChanged;
        }
 
    }

    private void Popup_Closed(object sender, EventArgs e)
    {
        // If a NumberBox button was just clicked, reopen the popup
        if (_numberBoxButtonClicked && _popup != null)
        {
            _numberBoxButtonClicked = false;
            _popup.IsOpen = true;
        }
    }

    private void DateTimePicker_Click(object sender, RoutedEventArgs e)
    {

        if (_popup != null)
        {
            _popup.IsOpen = true;
        }

        UpdateControls();
        // Set focus to calendar to prevent losing focus
        if (_calendar != null)
        {
            _calendar.Focus();
        }

    }

   
    private void UpdateControls()
    {
        _isUpdatingFromDateTime = true;

        if (_calendar != null)
        {
            _calendar.SelectedDate = DateTime.Date;
        }

        if (_hourNumberBox != null)
        {
            _hourNumberBox.Value = DateTime.Hour;
        }

        if (_minuteNumberBox != null)
        {
            _minuteNumberBox.Value = DateTime.Minute;
        }

        if (_secondNumberBox != null)
        {
            _secondNumberBox.Value = DateTime.Second;
        }

        _isUpdatingFromDateTime = false;
    }

    private void UpdateDisplay()
    {
        if (_dateTimeTextBox != null)
        {
            _isUpdatingFromTextBox = true;
            _dateTimeTextBox.Text = DateTime.ToString(DateTimeFormat);
            _isUpdatingFromTextBox = false;
        }
    }

    private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_isUpdatingFromDateTime && _calendar != null && _hourNumberBox != null && _minuteNumberBox != null && _secondNumberBox != null)
        {
            System.DateTime selectedDate = _calendar.SelectedDate ?? System.DateTime.Now;
            int hour = (int)(_hourNumberBox.Value ?? 0);
            int minute = (int)(_minuteNumberBox.Value ?? 0);
            int second = (int)(_secondNumberBox.Value ?? 0);

            _isUpdatingFromDateTime = true;
            System.DateTime newDateTime = selectedDate.Date.AddHours(hour).AddMinutes(minute).AddSeconds(second);
            DateTime = newDateTime;
            SelectedDateTime = newDateTime;
            UpdateDisplay();
            _isUpdatingFromDateTime = false;

            // Keep popup open
            if (_popup != null && !_popup.IsOpen)
            {
                _popup.IsOpen = true;
            }
        }
    }

    private void DateTimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_isUpdatingFromTextBox && _dateTimeTextBox != null)
        {
            if (System.DateTime.TryParseExact(_dateTimeTextBox.Text, DateTimeFormat, null, System.Globalization.DateTimeStyles.None, out var parsedDateTime))
            {
                _isUpdatingFromDateTime = true;
                DateTime = parsedDateTime;
                SelectedDateTime = parsedDateTime;
                UpdateControls();
                _isUpdatingFromDateTime = false;
            }
        }
    }

    private void TimeNumberBox_ValueChanged(object sender, NumberBoxValueChangedEventArgs e)
    {
        if (!_isUpdatingFromDateTime && _calendar != null && _hourNumberBox != null && _minuteNumberBox != null && _secondNumberBox != null)
        {
            System.DateTime selectedDate = _calendar.SelectedDate ?? System.DateTime.Now;
            int hour = (int)(_hourNumberBox.Value ?? 0);
            int minute = (int)(_minuteNumberBox.Value ?? 0);
            int second = (int)(_secondNumberBox.Value ?? 0);

            // Validate ranges
            if (hour >= 0 && hour < 24 && minute >= 0 && minute < 60 && second >= 0 && second < 60)
            {
                _isUpdatingFromDateTime = true;
                System.DateTime newDateTime = selectedDate.Date.AddHours(hour).AddMinutes(minute).AddSeconds(second);
                DateTime = newDateTime;
                SelectedDateTime = newDateTime;
                UpdateDisplay();
                _isUpdatingFromDateTime = false;

                // Keep popup open
                if (_popup != null && !_popup.IsOpen)
                {
                    _popup.IsOpen = true;
                }
            }
        }
    }

    private static void OnDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DateTimePicker dateTimePicker && !dateTimePicker._isUpdatingFromDateTime)
        {
            dateTimePicker.UpdateDisplay();
            dateTimePicker.UpdateControls();
        }
    }

    private static void OnDateTimeFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DateTimePicker dateTimePicker && !dateTimePicker._isUpdatingFromDateTime)
        {
            dateTimePicker.UpdateDisplay();
        }
    }
}
