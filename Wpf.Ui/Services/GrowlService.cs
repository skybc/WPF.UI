// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Services
{
    using System.Windows.Threading;
    using Wpf.Ui.Controls;

    /// <summary>
    /// Default implementation of <see cref="IGrowlService"/>.
    /// </summary>
    public sealed class GrowlService : IGrowlService
    {
        private GrowlControl host;

        /// <inheritdoc />
        public TimeSpan DefaultDuration { get; set; } = TimeSpan.FromMilliseconds(3000);

        /// <inheritdoc />
        public void Attach(GrowlControl host)
        {
            this.host = host;
        }

        /// <inheritdoc />
        public GrowlControl GetHost()
        {
            return this.host;
        }

        /// <inheritdoc />
        public void Show(string message, GrowlType type, int? durationMs = null, bool autoClose = true)
        {
            var duration = TimeSpan.FromMilliseconds(durationMs ?? (int)this.DefaultDuration.TotalMilliseconds);

            if (this.host == null)
            {
                // If no host, try to find an existing one on the active window.
                var active = System.Windows.Application.Current != null
                    ? (System.Windows.Application.Current.Windows.Cast<System.Windows.Window>().FirstOrDefault(w => w.IsActive) ?? System.Windows.Application.Current.MainWindow)
                    : null;

                var growl = FindDescendant<GrowlControl>(active);
                if (growl != null)
                {
                    this.host = growl;
                }
            }

            if (this.host == null)
            {
                return; // silently ignore without host
            }

            var dispatcher = this.host.Dispatcher ?? Dispatcher.CurrentDispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(() => this.host.Show(message, type, duration, autoClose));
            }
            else
            {
                this.host.Show(message, type, duration, autoClose);
            }
        }

        /// <inheritdoc />
        public void Show(string title, string message, GrowlType type, int? durationMs = null, bool autoClose = true)
        {
            var duration = TimeSpan.FromMilliseconds(durationMs ?? (int)this.DefaultDuration.TotalMilliseconds);

            if (this.host == null)
            {
                var active = System.Windows.Application.Current != null
                    ? (System.Windows.Application.Current.Windows.Cast<System.Windows.Window>().FirstOrDefault(w => w.IsActive) ?? System.Windows.Application.Current.MainWindow)
                    : null;

                var growl = FindDescendant<GrowlControl>(active);
                if (growl != null)
                {
                    this.host = growl;
                }
            }

            if (this.host == null)
            {
                return;
            }

            var dispatcher = this.host.Dispatcher ?? Dispatcher.CurrentDispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(() => this.host.Show(title, message, type, duration, autoClose));
            }
            else
            {
                this.host.Show(title, message, type, duration, autoClose);
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            var currentHost = this.host;
            if (currentHost == null)
            {
                return;
            }

            if (!currentHost.Dispatcher.CheckAccess())
            {
                currentHost.Dispatcher.Invoke(() =>
                {
                    var items = currentHost.Items.Cast<object>().OfType<GrowlItem>().ToList();
                    foreach (var i in items)
                    {
                        currentHost.RemoveItem(i);
                    }
                });
            }
            else
            {
                var items = currentHost.Items.Cast<object>().OfType<GrowlItem>().ToList();
                foreach (var i in items)
                {
                    currentHost.RemoveItem(i);
                }
            }
        }

        private static T FindDescendant<T>(System.Windows.DependencyObject root)
            where T : class
        {
            if (root == null)
            {
                return null;
            }

            var count = System.Windows.Media.VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(root, i);
                if (child is T match)
                {
                    return match;
                }

                var nested = FindDescendant<T>(child);
                if (nested != null)
                {
                    return nested;
                }
            }

            return null;
        }
    }
}
