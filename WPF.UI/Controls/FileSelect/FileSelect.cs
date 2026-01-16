// FileSelect 控件 - 文件选择控件
// 提供文本框 + 浏览按钮的组合控件，用于文件路径选择

using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// 文件选择控件，包含文本框和浏览按钮
/// </summary>
/// <remarks>
/// FileSelect 控件提供了一个用户友好的文件选择界面，包含：
/// - 文本框：显示选定的文件路径（只读）
/// - 浏览按钮：打开文件选择对话框
/// 
/// 支持文件扩展名验证和双向绑定
/// </remarks>
public partial class FileSelect : Control
{
    /// <summary>
    /// 标识 FilePath 依赖属性
    /// </summary>
    public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register(
        nameof(FilePath),
        typeof(string),
        typeof(FileSelect),
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
        typeof(FileSelect),
        new PropertyMetadata(false)
    );

    /// <summary>
    /// 标识 FileExtensions 依赖属性
    /// </summary>
    public static readonly DependencyProperty FileExtensionsProperty = DependencyProperty.Register(
        nameof(FileExtensions),
        typeof(string),
        typeof(FileSelect),
        new PropertyMetadata("*.*")
    );

    /// <summary>
    /// 标识 DialogTitle 依赖属性
    /// </summary>
    public static readonly DependencyProperty DialogTitleProperty = DependencyProperty.Register(
        nameof(DialogTitle),
        typeof(string),
        typeof(FileSelect),
        new PropertyMetadata("选择文件")
    );

    /// <summary>
    /// 标识 InitialDirectory 依赖属性
    /// </summary>
    public static readonly DependencyProperty InitialDirectoryProperty = DependencyProperty.Register(
        nameof(InitialDirectory),
        typeof(string),
        typeof(FileSelect),
        new PropertyMetadata(Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
    );

    public FileSelect()
    {
        // 设置样式
        var style = new ResourceDictionary();
        style.Source = new Uri("pack://application:,,,/Wpf.Ui;component/Controls/FileSelect/FileSelect.xaml");
        Resources.MergedDictionaries.Add(style);

        this.Style = (Style)Resources[typeof(FileSelect)];
    }

    /// <summary>
    /// 获取或设置文件路径
    /// </summary>
    [Bindable(true)]
    [Category("Common")]
    public string FilePath
    {
        get => (string)GetValue(FilePathProperty);
        set => SetValue(FilePathProperty, value);
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
    /// 获取或设置允许的文件扩展名，多个用分号分隔
    /// 示例: "*.txt;*.pdf;*.doc"
    /// </summary>
    [Bindable(true)]
    [Category("Common")]
    public string FileExtensions
    {
        get => (string)GetValue(FileExtensionsProperty);
        set => SetValue(FileExtensionsProperty, value);
    }

    /// <summary>
    /// 获取或设置文件对话框标题
    /// </summary>
    [Bindable(true)]
    [Category("Common")]
    public string DialogTitle
    {
        get => (string)GetValue(DialogTitleProperty);
        set => SetValue(DialogTitleProperty, value);
    }

    /// <summary>
    /// 获取或设置文件对话框初始目录
    /// </summary>
    [Bindable(true)]
    [Category("Common")]
    public string InitialDirectory
    {
        get => (string)GetValue(InitialDirectoryProperty);
        set => SetValue(InitialDirectoryProperty, value);
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
        var openFileDialog = new OpenFileDialog
        {
            Title = DialogTitle,
            InitialDirectory = InitialDirectory,
            Filter = BuildFilterString(FileExtensions),
            CheckFileExists = true,
            CheckPathExists = true,
            Multiselect = false
        };

        if (openFileDialog.ShowDialog() == true)
        {
            FilePath = openFileDialog.FileName;
        }
    }

    /// <summary>
    /// 构建文件对话框的筛选字符串
    /// </summary>
    /// <param name="extensions">扩展名字符串，格式如 "*.txt;*.pdf"</param>
    /// <returns>筛选字符串，格式如 "Text files (*.txt)|*.txt|PDF files (*.pdf)|*.pdf"</returns>
    private string BuildFilterString(string extensions)
    {
        if (string.IsNullOrWhiteSpace(extensions) || extensions == "*.*")
        {
            return "All files (*.*)|*.*";
        }

        var filterParts = new List<string>();
        var extensionList = extensions.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        // 添加指定扩展名
        foreach (var ext in extensionList)
        {
            var cleanExt = ext.Trim();
            if (cleanExt.StartsWith("*."))
            {
                cleanExt = cleanExt.Substring(2);
            }

            var displayName = char.ToUpper(cleanExt[0]) + cleanExt.Substring(1) + " files";
            filterParts.Add($"{displayName} (*.{cleanExt})|*.{cleanExt}");
        }

        // 添加"所有文件"选项
        filterParts.Add("All files (*.*)|*.*");

        return string.Join("|", filterParts);
    }
}
