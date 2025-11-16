// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// 步骤状态枚举 - 表示每个 Step 的状态
/// Step Status Enumeration - Represents the status of each step
/// </summary>
public enum StepStatus
{
    /// <summary>
    /// 等待状态 - 尚未开始的步骤
    /// Wait Status - Steps that have not started yet
    /// </summary>
    Wait = 0,

    /// <summary>
    /// 进行中 - 当前正在执行的步骤
    /// Process - Currently executing step
    /// </summary>
    Process = 1,

    /// <summary>
    /// 完成状态 - 已完成的步骤
    /// Finish - Completed step
    /// </summary>
    Finish = 2,

    /// <summary>
    /// 错误状态 - 执行过程中出错的步骤
    /// Error - Step with error during execution
    /// </summary>
    Error = 3
}
