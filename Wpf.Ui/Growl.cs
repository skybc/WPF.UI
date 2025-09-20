// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui
{
    using Wpf.Ui.Controls;
    using Wpf.Ui.Services;

    /// <summary>
    /// Static facade for growl, provides HandyControl-like APIs: Info/Success/Warning/Error.
    /// </summary>
    public static class Growl
    {
        private static readonly IGrowlService ServiceInstance = new GrowlService();

        /// <summary>
        /// Show an info message.
        /// </summary>
        /// <param name="message">The message text.</param>
        /// <param name="duration">Optional duration in milliseconds.</param>
        /// <param name="autoClose">Whether to auto-close.</param>
        public static void Info(string message, int? duration = null, bool autoClose = true)
        {
            ServiceInstance.Show(message, Controls.GrowlType.Info, duration, autoClose);
        }

        /// <summary>
        /// Show a success message.
        /// </summary>
        /// <param name="message">The message text.</param>
        /// <param name="duration">Optional duration in milliseconds.</param>
        /// <param name="autoClose">Whether to auto-close.</param>
        public static void Success(string message, int? duration = null, bool autoClose = true)
        {
            ServiceInstance.Show(message, Controls.GrowlType.Success, duration, autoClose);
        }

        /// <summary>
        /// Show a warning message.
        /// </summary>
        /// <param name="message">The message text.</param>
        /// <param name="duration">Optional duration in milliseconds.</param>
        /// <param name="autoClose">Whether to auto-close.</param>
        public static void Warning(string message, int? duration = null, bool autoClose = true)
        {
            ServiceInstance.Show(message, Controls.GrowlType.Warning, duration, autoClose);
        }

        /// <summary>
        /// Show an error message.
        /// </summary>
        /// <param name="message">The message text.</param>
        /// <param name="duration">Optional duration in milliseconds.</param>
        /// <param name="autoClose">Whether to auto-close.</param>
        public static void Error(string message, int? duration = null, bool autoClose = true)
        {
            ServiceInstance.Show(message, Controls.GrowlType.Error, duration, autoClose);
        }

        /// <summary>
        /// Show an info message with title.
        /// </summary>
        /// <param name="title">The title text.</param>
        /// <param name="message">The message text.</param>
        /// <param name="duration">Optional duration in milliseconds.</param>
        /// <param name="autoClose">Whether to auto-close.</param>
        public static void Info(string title, string message, int? duration = null, bool autoClose = true)
        {
            ServiceInstance.Show(title, message, Controls.GrowlType.Info, duration, autoClose);
        }

        /// <summary>
        /// Show a success message with title.
        /// </summary>
        /// <param name="title">The title text.</param>
        /// <param name="message">The message text.</param>
        /// <param name="duration">Optional duration in milliseconds.</param>
        /// <param name="autoClose">Whether to auto-close.</param>
        public static void Success(string title, string message, int? duration = null, bool autoClose = true)
        {
            ServiceInstance.Show(title, message, Controls.GrowlType.Success, duration, autoClose);
        }

        /// <summary>
        /// Show a warning message with title.
        /// </summary>
        /// <param name="title">The title text.</param>
        /// <param name="message">The message text.</param>
        /// <param name="duration">Optional duration in milliseconds.</param>
        /// <param name="autoClose">Whether to auto-close.</param>
        public static void Warning(string title, string message, int? duration = null, bool autoClose = true)
        {
            ServiceInstance.Show(title, message, Controls.GrowlType.Warning, duration, autoClose);
        }

        /// <summary>
        /// Show an error message with title.
        /// </summary>
        /// <param name="title">The title text.</param>
        /// <param name="message">The message text.</param>
        /// <param name="duration">Optional duration in milliseconds.</param>
        /// <param name="autoClose">Whether to auto-close.</param>
        public static void Error(string title, string message, int? duration = null, bool autoClose = true)
        {
            ServiceInstance.Show(title, message, Controls.GrowlType.Error, duration, autoClose);
        }

        /// <summary>
        /// Clear all active messages.
        /// </summary>
        public static void Clear()
        {
            ServiceInstance.Clear();
        }

        /// <summary>
        /// Internal: attach a placed host to the global service.
        /// </summary>
        /// <param name="host">The visual host control.</param>
        internal static void Attach(GrowlControl host)
        {
            ServiceInstance.Attach(host);
        }
    }
}
