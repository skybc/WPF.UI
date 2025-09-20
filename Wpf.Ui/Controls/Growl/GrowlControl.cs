// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
#pragma warning disable SA1633 // File header should follow standard text

namespace Wpf.Ui.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using Wpf.Ui.Input;

    /// <summary>
    /// Lightweight toast-like growl container that stacks messages in the top-right corner.
    /// </summary>
    public class GrowlControl : ItemsControl
    {
        /// <summary>
        /// Identifies the <see cref="DefaultDuration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultDurationProperty = DependencyProperty.Register(
            nameof(DefaultDuration), typeof(int), typeof(GrowlControl), new PropertyMetadata(3000));

        /// <summary>
        /// Identifies the <see cref="MaxItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxItemsProperty = DependencyProperty.Register(
            nameof(MaxItems), typeof(int), typeof(GrowlControl), new PropertyMetadata(6, OnMaxItemsChanged));

        /// <summary>
        /// Identifies the <see cref="IsCompact"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCompactProperty = DependencyProperty.Register(
            nameof(IsCompact), typeof(bool), typeof(GrowlControl), new PropertyMetadata(false));

        private readonly ObservableCollection<GrowlItem> items = new ObservableCollection<GrowlItem>();
        private Popup popup;
        private ItemsControl popupItemsPresenter;

        static GrowlControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GrowlControl), new FrameworkPropertyMetadata(typeof(GrowlControl)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrowlControl"/> class.
        /// </summary>
        public GrowlControl()
        {
            this.SetCurrentValue(ItemsSourceProperty, this.items);

            this.Loaded += (_, __) =>
            {
                Wpf.Ui.Growl.Attach(this);
                this.EnsurePopup();
                this.OpenPopup();
            };

            this.Unloaded += (_, __) =>
            {
                if (this.popup != null)
                {
                    this.popup.SetCurrentValue(Popup.IsOpenProperty, false);
                }
            };
        }

        /// <summary>Gets or sets the default duration in milliseconds for auto-closing items.</summary>
        public int DefaultDuration
        {
            get { return (int)this.GetValue(DefaultDurationProperty); }
            set { this.SetValue(DefaultDurationProperty, value); }
        }

        /// <summary>Gets or sets the maximum number of items to keep visible. 0 means unlimited.</summary>
        public int MaxItems
        {
            get { return (int)this.GetValue(MaxItemsProperty); }
            set { this.SetValue(MaxItemsProperty, value); }
        }

        /// <summary>Gets or sets a value indicating whether compact mode is enabled (reduced padding and shadow).</summary>
        public bool IsCompact
        {
            get { return (bool)this.GetValue(IsCompactProperty); }
            set { this.SetValue(IsCompactProperty, value); }
        }

        /// <summary>Gets a command to close a specific item (used by the template).</summary>
        public IRelayCommand<GrowlItem> CloseItemCommand => new RelayCommand<GrowlItem>(i => this.RemoveItem(i));

        /// <summary>
        /// Show a growl message.
        /// </summary>
        /// <param name="message">The text content.</param>
        /// <param name="type">Message type/appearance.</param>
        /// <param name="duration">Duration before auto-close. If default, uses <see cref="DefaultDuration"/>.</param>
        /// <param name="autoClose">Whether the item should auto-close after <paramref name="duration"/>.</param>
        public void Show(string message, GrowlType type = GrowlType.Info, TimeSpan duration = default, bool autoClose = true)
        {
            var item = new GrowlItem
            {
                Id = Guid.NewGuid(),
                Title = string.Empty,
                Message = message,
                Type = type,
                AutoClose = autoClose,
                Duration = duration == default ? TimeSpan.FromMilliseconds(this.DefaultDuration) : duration,
            };

            this.AddItem(item);
        }

        /// <summary>
        /// Show a growl message with a title.
        /// </summary>
        /// <param name="title">The title text.</param>
        /// <param name="message">The content text.</param>
        /// <param name="type">Message type/appearance.</param>
        /// <param name="duration">Duration before auto-close. If default, uses <see cref="DefaultDuration"/>.</param>
        /// <param name="autoClose">Whether the item should auto-close after <paramref name="duration"/>.</param>
        public void Show(string title, string message, GrowlType type = GrowlType.Info, TimeSpan duration = default, bool autoClose = true)
        {
            var item = new GrowlItem
            {
                Id = Guid.NewGuid(),
                Title = title,
                Message = message,
                Type = type,
                AutoClose = autoClose,
                Duration = duration == default ? TimeSpan.FromMilliseconds(this.DefaultDuration) : duration,
            };

            this.AddItem(item);
        }

        /// <summary>Add an item to the stack.</summary>
        /// <param name="item">The item to add.</param>
        public void AddItem(GrowlItem item)
        {
            this.items.Insert(0, item);

            if (item.AutoClose && item.Duration.TotalMilliseconds > 0)
            {
                var cts = new CancellationTokenSource();
                item.Cancellation = cts;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await Task.Delay(item.Duration, cts.Token);
                    }
                    catch
                    {
                        return;
                    }

                    await this.Dispatcher.InvokeAsync(() => this.RemoveItem(item));
                });
            }

            this.InvalidatePopupPosition();
        }

        /// <summary>Remove the item from the stack.</summary>
        /// <param name="item">The item to remove.</param>
        public async void RemoveItem(GrowlItem item)
        {
            if (item.Cancellation != null)
            {
                item.Cancellation.Cancel();
            }

            if (this.TryGetContainer(item, out FrameworkElement container))
            {
                await this.PlayExitAnimationAsync(container);
            }

            this.items.Remove(item);
            this.InvalidatePopupPosition();
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size constraint)
        {
            return new Size(0, 0);
        }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            return new Size(0, 0);
        }

        // Static callback used by dependency property metadata.
        private static void OnMaxItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (GrowlControl)d;
            self.TrimItems();
        }

        private void TrimItems()
        {
            if (this.MaxItems <= 0)
            {
                return;
            }

            while (this.items.Count > this.MaxItems)
            {
                this.items.RemoveAt(this.items.Count - 1);
            }
        }

        private void EnsurePopup()
        {
            if (this.popup != null)
            {
                return;
            }

            var target = this.GetPlacementTarget();
            if (target == null)
            {
                return;
            }

            this.popupItemsPresenter = new ItemsControl
            {
                ItemsSource = this.items,
                IsTabStop = false,
                Focusable = false,
            };

            if (this.ItemTemplate != null)
            {
                this.popupItemsPresenter.SetCurrentValue(ItemsControl.ItemTemplateProperty, this.ItemTemplate);
            }

            if (this.ItemsPanel != null)
            {
                this.popupItemsPresenter.SetCurrentValue(ItemsControl.ItemsPanelProperty, this.ItemsPanel);
            }

            this.popup = new Popup
            {
                AllowsTransparency = true,
                StaysOpen = true,
                Focusable = false,
                Placement = PlacementMode.Custom,
                PlacementTarget = target,
                Child = this.popupItemsPresenter,
                CustomPopupPlacementCallback = (popupSize, targetSize, offset) =>
                {
                    var margin = this.Margin;
                    double x = targetSize.Width - popupSize.Width - margin.Right;
                    if (x < margin.Left)
                    {
                        x = margin.Left;
                    }

                    double y = margin.Top;
                    return new[] { new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Horizontal) };
                },
            };

            if (target is FrameworkElement fe)
            {
                fe.SizeChanged += (_, __) => this.InvalidatePopupPosition();
            }
        }

        private UIElement GetPlacementTarget()
        {
            var win = Window.GetWindow(this);
            if (win != null && win.Content is UIElement u)
            {
                return u;
            }

            return win as UIElement ?? this;
        }

        private void OpenPopup()
        {
            if (this.popup == null)
            {
                return;
            }

            this.popup.SetCurrentValue(Popup.IsOpenProperty, true);
            this.InvalidatePopupPosition();
        }

        private void InvalidatePopupPosition()
        {
            if (this.popup == null)
            {
                return;
            }

            var ho = this.popup.HorizontalOffset;
            this.popup.SetCurrentValue(Popup.HorizontalOffsetProperty, ho + 0.1);
            this.popup.SetCurrentValue(Popup.HorizontalOffsetProperty, ho);
        }

        private Task PlayExitAnimationAsync(FrameworkElement element)
        {
            var tcs = new TaskCompletionSource();

            var sb = new Storyboard();

            var fade = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.16),
            };
            Storyboard.SetTarget(fade, element);
            Storyboard.SetTargetProperty(fade, new PropertyPath("Opacity"));
            sb.Children.Add(fade);

            var slide = new DoubleAnimation
            {
                To = -6,
                Duration = TimeSpan.FromSeconds(0.16),
            };
            if (element.RenderTransform == null || element.RenderTransform.Value.IsIdentity)
            {
                element.SetCurrentValue(UIElement.RenderTransformProperty, new TranslateTransform());
            }

            Storyboard.SetTarget(slide, element);
            Storyboard.SetTargetProperty(slide, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
            sb.Children.Add(slide);

            sb.Completed += (_, __) => tcs.TrySetResult();
            sb.Begin();

            return tcs.Task;
        }

        private bool TryGetContainer(GrowlItem item, out FrameworkElement container)
        {
            container = null;
            if (this.popupItemsPresenter == null)
            {
                return false;
            }

            var ic = this.popupItemsPresenter;
            var obj = ic.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
            if (obj != null)
            {
                container = obj;
                return true;
            }

            return false;
        }
    }
}
