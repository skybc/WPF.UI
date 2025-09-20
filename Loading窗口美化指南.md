# WPF.UI Loading Window - 美化版本使用指南

## 🎨 界面美化特性

### 1. 视觉强调效果
- **全屏半透明遮罩**：覆盖整个主窗口内容，确保用户专注于加载状态
- **模糊背景效果**：轻微模糊背景，突出前景加载动画
- **渐变阴影**：为主内容区域添加深度感
- **圆角设计**：现代化的圆角边框，符合 WPF.UI 设计语言

### 2. 动画效果增强
- **淡入淡出**：流畅的窗口出现和消失动画
- **缩放弹出**：内容区域带有弹性效果的缩放动画
- **文本脉动**：加载文字的呼吸效果，增强视觉吸引力
- **进度环脉动**：进度环的轻微缩放动画
- **消息更新强调**：更新消息时的文字强调动画

### 3. 主题自动适配
- **深色主题**：自动使用深色调色板
- **浅色主题**：自动使用浅色调色板
- **高对比度主题**：支持无障碍访问
- **动态资源绑定**：所有颜色都绑定到 WPF.UI 资源字典

## 🚀 快速使用

### 基本用法（与之前兼容）
```csharp
// 显示基本加载窗口
LoadingHelper.Show("处理中...");

// 隐藏加载窗口
LoadingHelper.Hide();
```

### 新增美化功能
```csharp
// 显示带副标题的加载窗口
LoadingHelper.Show("上传文件", "请稍候，正在处理您的文件...");

// 更新消息和副标题
LoadingHelper.UpdateMessage("处理完成", "文件已成功上传");

// 异步显示带副标题
await LoadingHelper.ShowAsync("连接服务器", "正在建立安全连接...");
```

### 直接使用窗口实例
```csharp
// 创建自定义配置的加载窗口
var loadingWindow = new LoadingWindow("自定义消息", "副标题");

// 禁用某些动画效果（可选）
loadingWindow.EnableTextPulse = false;
loadingWindow.EnableRingPulse = false;

// 显示
loadingWindow.ShowWithAnimation();

// 更新消息
loadingWindow.UpdateMessage("新消息", "新副标题");

// 异步隐藏
await loadingWindow.HideWithAnimationAsync();
```

## 🎯 主题适配说明

### 颜色资源映射

| 美化元素 | 浅色主题资源 | 深色主题资源 | 用途 |
|----------|--------------|--------------|------|
| 遮罩背景 | `LoadingOverlayBackgroundBrush` | 半透明黑色 | 全屏遮罩层 |
| 内容背景 | `ControlSolidFillColorDefault` | 自动适配 | 主内容区域背景 |
| 边框颜色 | `ControlStrokeColorDefault` | 自动适配 | 内容区域边框 |
| 文本颜色 | `TextFillColorPrimary` | 自动适配 | 主要文本 |
| 强调色 | `AccentTextFillColorPrimary` | 自动适配 | 进度环和装饰 |

### 主题切换适配
当应用程序主题发生变化时，Loading 窗口会自动适配新的颜色方案：

```csharp
// 主题切换示例
public void SwitchTheme()
{
    // 切换到深色主题
    UiApplication.SetTheme(ThemeType.Dark);
    
    // Loading 窗口会自动使用深色主题的颜色
    LoadingHelper.Show("主题已切换", "正在应用深色主题...");
}
```

## 🎬 动画配置

### 动画时长和效果
- **窗口淡入**：400ms，三次贝塞尔缓动
- **窗口淡出**：300ms，三次贝塞尔缓动
- **内容缩放**：400ms，回弹效果
- **文本脉动**：1200ms，正弦波循环
- **进度环脉动**：1500ms，正弦波循环

### 自定义动画
如需自定义动画效果，可以修改 `LoadingWindowTheme.xaml` 中的动画资源：

```xml
<!-- 自定义淡入动画 -->
<Storyboard x:Key="LoadingFadeInStoryboard">
    <DoubleAnimation
        Storyboard.TargetProperty="Opacity"
        From="0" To="1" Duration="0:0:0.6"
        AccelerationRatio="0.2" DecelerationRatio="0.8">
        <DoubleAnimation.EasingFunction>
            <ElasticEase EasingMode="EaseOut" Oscillations="2"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
</Storyboard>
```

## 📱 响应式设计

### 全屏适配
Loading 窗口自动适配不同屏幕尺寸：
- **主显示器**：自动检测并覆盖整个主屏幕
- **多显示器**：支持在特定显示器上显示
- **父窗口适配**：当设置父窗口时，只覆盖父窗口区域

### 内容自适应
- **文本换行**：长文本自动换行，最大宽度 180px
- **动态尺寸**：内容区域根据文本长度自动调整
- **最小尺寸**：确保在任何情况下都有足够的显示空间

## 🛠️ 高级用法示例

### 多步骤加载演示
```csharp
public async Task PerformMultiStepOperation()
{
    await LoadingHelper.ShowAsync("初始化", "正在准备工作环境...");
    
    await Task.Delay(1000);
    LoadingHelper.UpdateMessage("连接服务器", "正在建立连接...");
    
    await Task.Delay(1000);
    LoadingHelper.UpdateMessage("上传数据", "正在传输文件...");
    
    await Task.Delay(1000);
    LoadingHelper.UpdateMessage("处理完成", "操作成功完成！");
    
    await Task.Delay(500);
    await LoadingHelper.HideAsync();
}
```

### MVVM 模式集成
```csharp
public class MainViewModel : INotifyPropertyChanged
{
    private bool isLoading;
    private string loadingMessage = "加载中...";
    private string loadingSubtitle = "";

    public bool IsLoading
    {
        get => isLoading;
        set
        {
            isLoading = value;
            OnPropertyChanged();
            
            if (value)
                LoadingHelper.Show(LoadingMessage, LoadingSubtitle);
            else
                LoadingHelper.Hide();
        }
    }

    public string LoadingMessage
    {
        get => loadingMessage;
        set
        {
            loadingMessage = value;
            OnPropertyChanged();
            
            if (IsLoading)
                LoadingHelper.UpdateMessage(value, LoadingSubtitle);
        }
    }
    
    // ... 其他属性和方法
}
```

### 与异步操作结合
```csharp
public static class LoadingExtensions
{
    public static async Task<T> WithLoadingAsync<T>(
        this Task<T> task, 
        string message, 
        string subtitle = "请稍候...")
    {
        await LoadingHelper.ShowAsync(message, subtitle);
        try
        {
            return await task;
        }
        finally
        {
            await LoadingHelper.HideAsync();
        }
    }
}

// 使用示例
var result = await SomeAsyncOperation()
    .WithLoadingAsync("获取数据", "正在从服务器获取最新数据...");
```

## 🎨 自定义样式

### 修改颜色主题
在您的应用程序资源中覆盖默认颜色：

```xml
<Application.Resources>
    <!-- 自定义 Loading 窗口颜色 -->
    <SolidColorBrush x:Key="LoadingOverlayBackgroundBrush" Color="#BB000000" />
    <SolidColorBrush x:Key="LoadingProgressBrush" Color="#FF00B4D8" />
    
    <!-- 合并 WPF.UI 资源 -->
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="pack://application:,,,/Wpf.Ui;component/Resources/Wpf.Ui.xaml" />
    </ResourceDictionary.MergedDictionaries>
</Application.Resources>
```

### 添加自定义效果
```xml
<!-- 在 LoadingWindowTheme.xaml 中添加 -->
<BlurEffect x:Key="CustomLoadingBlurEffect" Radius="5" />
<DropShadowEffect x:Key="CustomLoadingShadow" 
                  Color="#FF0078D4" 
                  BlurRadius="25" 
                  ShadowDepth="0" />
```

## 📋 最佳实践

1. **及时隐藏**：确保在所有情况下都能正确隐藏加载窗口
2. **有意义的消息**：提供清晰的加载状态描述
3. **合理的动画**：避免过于频繁的动画更新影响性能
4. **主题一致性**：确保加载窗口与应用程序整体主题保持一致
5. **异步优先**：优先使用异步方法，避免阻塞 UI 线程

## 🐛 故障排除

### 常见问题

**Q: 加载窗口颜色不正确？**
A: 确保您的应用程序正确引用了 WPF.UI 主题资源。

**Q: 动画效果不流畅？**
A: 检查是否在 UI 线程上执行了大量计算，建议将耗时操作移到后台线程。

**Q: 加载窗口显示位置不对？**
A: 确保在显示窗口之前正确设置了父窗口关系。

**Q: 自定义颜色不生效？**
A: 确保自定义资源在 LoadingWindowTheme.xaml 加载之后定义。

## 🔧 性能优化

- 加载窗口使用单例模式，避免重复创建
- 动画使用硬件加速，确保流畅性能
- 自动管理窗口生命周期，防止内存泄漏
- 支持异步操作，不阻塞主 UI 线程