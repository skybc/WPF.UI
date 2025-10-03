// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

using System;
using System.Windows.Media;

/// <summary>
/// Helper class for color conversions between RGB, HSV, and Hex formats.
/// </summary>
internal static class ColorHelper
{
    /// <summary>
    /// Converts HSV (Hue, Saturation, Value) to RGB color.
    /// </summary>
    /// <param name="hue">Hue value (0-360)</param>
    /// <param name="saturation">Saturation value (0-1)</param>
    /// <param name="value">Value/Brightness (0-1)</param>
    /// <param name="alpha">Alpha value (0-1)</param>
    /// <returns>Color in RGB format</returns>
    public static Color HsvToRgb(double hue, double saturation, double value, double alpha = 1.0)
    {
        // Normalize hue to 0-360 range
        hue = hue % 360;
        if (hue < 0) hue += 360;

        double chroma = value * saturation;
        double hueSegment = hue / 60.0;
        double x = chroma * (1 - Math.Abs(hueSegment % 2 - 1));
        double m = value - chroma;

        double r = 0, g = 0, b = 0;

        if (hueSegment >= 0 && hueSegment < 1)
        {
            r = chroma;
            g = x;
            b = 0;
        }
        else if (hueSegment >= 1 && hueSegment < 2)
        {
            r = x;
            g = chroma;
            b = 0;
        }
        else if (hueSegment >= 2 && hueSegment < 3)
        {
            r = 0;
            g = chroma;
            b = x;
        }
        else if (hueSegment >= 3 && hueSegment < 4)
        {
            r = 0;
            g = x;
            b = chroma;
        }
        else if (hueSegment >= 4 && hueSegment < 5)
        {
            r = x;
            g = 0;
            b = chroma;
        }
        else
        {
            r = chroma;
            g = 0;
            b = x;
        }

        return Color.FromArgb(
            (byte)Math.Round(alpha * 255),
            (byte)Math.Round((r + m) * 255),
            (byte)Math.Round((g + m) * 255),
            (byte)Math.Round((b + m) * 255));
    }

    /// <summary>
    /// Converts RGB color to HSV (Hue, Saturation, Value).
    /// </summary>
    /// <param name="color">RGB color.</param>
    /// <returns>Tuple of (Hue, Saturation, Value, Alpha).</returns>
    public static (double Hue, double Saturation, double Value, double Alpha) RgbToHsv(Color color)
    {
        double r = color.R / 255.0;
        double g = color.G / 255.0;
        double b = color.B / 255.0;
        double alpha = color.A / 255.0;

        double max = Math.Max(r, Math.Max(g, b));
        double min = Math.Min(r, Math.Min(g, b));
        double delta = max - min;

        double hue = 0;
        if (delta > 0)
        {
            if (max == r)
            {
                hue = 60 * (((g - b) / delta) % 6);
            }
            else if (max == g)
            {
                hue = 60 * (((b - r) / delta) + 2);
            }
            else
            {
                hue = 60 * (((r - g) / delta) + 4);
            }
        }

        if (hue < 0)
        {
            hue += 360;
        }

        double saturation = max == 0 ? 0 : delta / max;
        double value = max;

        return (hue, saturation, value, alpha);
    }

    /// <summary>
    /// Converts RGB color to hex string.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <param name="includeAlpha">Whether to include alpha channel.</param>
    /// <returns>Hex string representation of the color.</returns>
    public static string ColorToHex(Color color, bool includeAlpha = true)
    {
        if (includeAlpha)
        {
            return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    /// <summary>
    /// Converts hex string to RGB color.
    /// </summary>
    /// <param name="hex">The hex string to convert.</param>
    /// <returns>Color from hex string.</returns>
    public static Color HexToColor(string hex)
    {
        hex = hex.Replace("#", string.Empty);

        if (hex.Length == 6)
        {
            return Color.FromRgb(
                Convert.ToByte(hex.Substring(0, 2), 16),
                Convert.ToByte(hex.Substring(2, 2), 16),
                Convert.ToByte(hex.Substring(4, 2), 16));
        }
        else if (hex.Length == 8)
        {
            return Color.FromArgb(
                Convert.ToByte(hex.Substring(0, 2), 16),
                Convert.ToByte(hex.Substring(2, 2), 16),
                Convert.ToByte(hex.Substring(4, 2), 16),
                Convert.ToByte(hex.Substring(6, 2), 16));
        }

        return Colors.Black;
    }

    /// <summary>
    /// Checks if a hex string is valid.
    /// </summary>
    /// <param name="hex">The hex string to validate.</param>
    /// <returns>True if valid, false otherwise.</returns>
    public static bool IsValidHex(string hex)
    {
        hex = hex.Replace("#", string.Empty);
        return (hex.Length == 6 || hex.Length == 8) &&
               System.Text.RegularExpressions.Regex.IsMatch(hex, "^[0-9A-Fa-f]+$");
    }
}
