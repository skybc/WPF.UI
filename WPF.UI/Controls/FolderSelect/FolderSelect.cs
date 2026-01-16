// FolderSelect 控件 - 目录选择控件
// 提供文本框 + 浏览按钮的组合控件，用于目录路径选择

using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// 目录选择控件，包含文本框和浏览按钮
/// </summary>
/// <remarks>
/// FolderSelect 控件提供了一个用户友好的目录选择界面，包含：
/// - 文本框：显示选定的目录路径（只读）
/// - 浏览按钮：打开目录选择对话框
/// 
/// 支持双向绑定和灵活的配置选项
/// </remarks>
public partial class FolderSelect : System.Windows.Controls.Control
{
    /// <summary>
    /// 标识 FolderPath 依赖属性
    /// </summary>
    public static readonly DependencyProperty FolderPathProperty = DependencyProperty.Register(
        nameof(FolderPath),
        typeof(string),
        typeof(FolderSelect),
        new FrameworkPropertyMetadata(
            string.Empty,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
        )
    );

    /// <summary>
    /// 标识 IsRequired 依赖属性
    /// </summary>
    public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register(
        nameof(IsRequired),
        typeof(bool),
        typeof(FolderSelect),
        new PropertyMetadata(false)
    );

    /// <summary>
    /// 标识 DialogTitle 依赖属性
    /// </summary>
    public static readonly DependencyProperty DialogTitleProperty = DependencyProperty.Register(
        nameof(DialogTitle),
        typeof(string),
        typeof(FolderSelect),
        new PropertyMetadata("选择文件夹")
    );
 
    /// <summary>
    /// 标识 ShowNewFolderButton 依赖属性
    /// </summary>
    public static readonly DependencyProperty ShowNewFolderButtonProperty = DependencyProperty.Register(
        nameof(ShowNewFolderButton),
        typeof(bool),
        typeof(FolderSelect),
        new PropertyMetadata(true)
    );

    public FolderSelect()
    {
        // 设置样式
        var style = new ResourceDictionary();
        style.Source = new Uri("pack://application:,,,/Wpf.Ui;component/Controls/FolderSelect/FolderSelect.xaml");
        Resources.MergedDictionaries.Add(style);

        this.Style = (System.Windows.Style)Resources[typeof(FolderSelect)];
    }

    /// <summary>
    /// 获取或设置目录路径
    /// </summary>
    [Bindable(true)]
    [Category("Common")]
    public string FolderPath
    {
        get => (string)GetValue(FolderPathProperty);
        set => SetValue(FolderPathProperty, value);
    }

    /// <summary>
    /// 获取或设置是否必选
    /// </summary>
    [Bindable(true)]
    [Category("Common")]
    public bool IsRequired
    {
        get => (bool)GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }

    /// <summary>
    /// 获取或设置文件夹选择对话框标题
    /// </summary>
    [Bindable(true)]
    [Category("Common")]
    public string DialogTitle
    {
        get => (string)GetValue(DialogTitleProperty);
        set => SetValue(DialogTitleProperty, value);
    }
 
    /// <summary>
    /// 获取或设置是否显示"新建文件夹"按钮
    /// </summary>
    [Bindable(true)]
    [Category("Common")]
    public bool ShowNewFolderButton
    {
        get => (bool)GetValue(ShowNewFolderButtonProperty);
        set => SetValue(ShowNewFolderButtonProperty, value);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        // 获取浏览按钮并绑定点击事件
        if (GetTemplateChild("BrowseButton") is Button browseButton)
        {
            browseButton.Click += BrowseButton_Click;
        }
    }

    /// <summary>
    /// 浏览按钮点击事件处理
    /// </summary>
    private void BrowseButton_Click(object sender, RoutedEventArgs e)
    {


        var folderBrowserDialog = new OpenFolderDialog();

        folderBrowserDialog.Title = DialogTitle;

        // 如果已有选定的路径，则设置为初始路径
        if (!string.IsNullOrWhiteSpace(FolderPath) && System.IO.Directory.Exists(FolderPath))
        {
            folderBrowserDialog.FolderName = FolderPath;
        }

        if (folderBrowserDialog.ShowDialog() == true)
        {
            FolderPath = folderBrowserDialog.FolderName;
        }

    }
}
