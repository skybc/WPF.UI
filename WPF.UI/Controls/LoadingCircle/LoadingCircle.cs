// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Wpf.Ui.Controls;

/// <summary>
/// Animated loading circle inspired by HandyControl.
/// </summary>
public class LoadingCircle : LoadingBase
{
	/// <summary>Identifies the <see cref="DotOffSet"/> dependency property.</summary>
	public static readonly DependencyProperty DotOffSetProperty = DependencyProperty.Register(
		nameof(DotOffSet),
		typeof(double),
		typeof(LoadingCircle),
		new FrameworkPropertyMetadata(20d, FrameworkPropertyMetadataOptions.AffectsRender));

	/// <summary>Identifies the <see cref="NeedHidden"/> dependency property.</summary>
	public static readonly DependencyProperty NeedHiddenProperty = DependencyProperty.Register(
		nameof(NeedHidden),
		typeof(bool),
		typeof(LoadingCircle),
		new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

	static LoadingCircle()
	{
		DotSpeedProperty.OverrideMetadata(
			typeof(LoadingCircle),
			new FrameworkPropertyMetadata(6.0, FrameworkPropertyMetadataOptions.AffectsRender));
		DotDelayTimeProperty.OverrideMetadata(
			typeof(LoadingCircle),
			new FrameworkPropertyMetadata(220.0, FrameworkPropertyMetadataOptions.AffectsRender));
	}

	/// <summary>
	/// Gets or sets the offset applied to the dot animation.
	/// </summary>
	public double DotOffSet
	{
		get => (double)GetValue(DotOffSetProperty);
		set => SetValue(DotOffSetProperty, value);
	}

	/// <summary>
	/// Gets or sets a value indicating whether dots briefly hide between cycles.
	/// </summary>
	public bool NeedHidden
	{
		get => (bool)GetValue(NeedHiddenProperty);
		set => SetValue(NeedHiddenProperty, value);
	}

	/// <inheritdoc />
	protected sealed override void UpdateDots()
	{
		var dotCount = DotCount;
		double dotInterval = DotInterval;
		double dotSpeed = DotSpeed;
		double dotDelayTime = DotDelayTime;
		bool needHidden = NeedHidden;

		if (dotCount < 1)
		{
			return;
		}

		PrivateCanvas.Children.Clear();

		Storyboard = new Storyboard
		{
			RepeatBehavior = RepeatBehavior.Forever
		};

		for (var i = 0; i < dotCount; i++)
		{
			var border = CreateBorder(i, dotInterval, needHidden);

			var framesMove = new DoubleAnimationUsingKeyFrames
			{
				BeginTime = TimeSpan.FromMilliseconds(dotDelayTime * i)
			};

			var subAngle = -dotInterval * i;

			var frame0 = new LinearDoubleKeyFrame
			{
				Value = 0 + subAngle,
				KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero)
			};

			var frame1 = new EasingDoubleKeyFrame
			{
				EasingFunction = new PowerEase
				{
					EasingMode = EasingMode.EaseOut
				},
				Value = 180 + subAngle,
				KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (0.75 / 7)))
			};

			var frame2 = new LinearDoubleKeyFrame
			{
				Value = 180 + DotOffSet + subAngle,
				KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (2.75 / 7)))
			};

			var frame3 = new EasingDoubleKeyFrame
			{
				EasingFunction = new PowerEase
				{
					EasingMode = EasingMode.EaseIn
				},
				Value = 360 + subAngle,
				KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (3.5 / 7)))
			};

			var frame4 = new EasingDoubleKeyFrame
			{
				EasingFunction = new PowerEase
				{
					EasingMode = EasingMode.EaseOut
				},
				Value = 540 + subAngle,
				KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (4.25 / 7)))
			};

			var frame5 = new LinearDoubleKeyFrame
			{
				Value = 540 + DotOffSet + subAngle,
				KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (6.25 / 7)))
			};

			var frame6 = new EasingDoubleKeyFrame
			{
				EasingFunction = new PowerEase
				{
					EasingMode = EasingMode.EaseIn
				},
				Value = 720 + subAngle,
				KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed))
			};

			framesMove.KeyFrames.Add(frame0);
			framesMove.KeyFrames.Add(frame1);
			framesMove.KeyFrames.Add(frame2);
			framesMove.KeyFrames.Add(frame3);
			framesMove.KeyFrames.Add(frame4);
			framesMove.KeyFrames.Add(frame5);
			framesMove.KeyFrames.Add(frame6);

			Storyboard.SetTarget(framesMove, border);
			Storyboard.SetTargetProperty(framesMove, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
			Storyboard.Children.Add(framesMove);

			if (needHidden)
			{
				var frame7 = new DiscreteObjectKeyFrame
				{
					Value = Visibility.Collapsed,
					KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed))
				};

				var frame8 = new DiscreteObjectKeyFrame
				{
					Value = Visibility.Collapsed,
					KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed + 0.4))
				};

				var frame9 = new DiscreteObjectKeyFrame
				{
					Value = Visibility.Visible,
					KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero)
				};

				var framesVisibility = new ObjectAnimationUsingKeyFrames
				{
					BeginTime = TimeSpan.FromMilliseconds(dotDelayTime * i)
				};

				framesVisibility.KeyFrames.Add(frame9);
				framesVisibility.KeyFrames.Add(frame7);
				framesVisibility.KeyFrames.Add(frame8);

				Storyboard.SetTarget(framesVisibility, border);
				Storyboard.SetTargetProperty(framesVisibility, new PropertyPath("(UIElement.Visibility)"));
				Storyboard.Children.Add(framesVisibility);
			}

			PrivateCanvas.Children.Add(border);
		}

		Storyboard.Begin();

		if (!IsRunning)
		{
			Storyboard.Pause();
		}
	}

	private Border CreateBorder(int index, double dotInterval, bool needHidden)
	{
		var ellipse = CreateEllipse(index);
		ellipse.HorizontalAlignment = HorizontalAlignment.Center;
		ellipse.VerticalAlignment = VerticalAlignment.Bottom;
		var rotateTransform = new RotateTransform
		{
			Angle = -dotInterval * index
		};
		var transformGroup = new TransformGroup();
		transformGroup.Children.Add(rotateTransform);
		var border = new Border
		{
			RenderTransformOrigin = new Point(0.5, 0.5),
			RenderTransform = transformGroup,
			Child = ellipse,
			Visibility = needHidden ? Visibility.Collapsed : Visibility.Visible
		};
		border.SetBinding(WidthProperty, new Binding(nameof(Width)) { Source = this });
		border.SetBinding(HeightProperty, new Binding(nameof(Height)) { Source = this });

		return border;
	}
}

