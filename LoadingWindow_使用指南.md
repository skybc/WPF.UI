# WPF.UI Loading Window 使用指南

## 概述

WPF.UI Loading Window 是一个功能强大的加载提示窗口组件，专为 WPF.UI 框架设计。它提供了半透明的黑色背景、圆角设计和流畅的动画效果，支持异步操作和 MVVM 模式。

## 主要特性

- ✨ **现代化设计**：半透明黑色背景、圆角边框、居中显示
- 🎨 **WPF.UI 集成**：使用 WPF.UI 的 ProgressRing 组件和样式系统
- ⚡ **流畅动画**：淡入淡出动画效果
- 🔄 **异步支持**：完全支持异步操作，不阻塞主线程
- 🎯 **MVVM 友好**：支持数据绑定和命令模式
- 📝 **自定义消息**：可动态更新显示文本
- 🪟 **父窗口支持**：可设置父窗口关系

## 快速开始

### 1. 最简单的使用方式

```csharp
// 显示加载窗口
LoadingHelper.Show("加载中...");

// 执行你的操作
await YourAsyncOperation();

// 隐藏加载窗口
LoadingHelper.Hide();
```

### 2. 使用 ExecuteWithLoadingAsync（推荐）

```csharp
// 自动管理显示和隐藏
await LoadingHelper.ExecuteWithLoadingAsync(async () =>
{
    // 你的异步操作
    await SomeAsyncWork();
}, "正在处理请求...");
```

### 3. 带返回值的异步操作

```csharp
var result = await LoadingHelper.ExecuteWithLoadingAsync(async () =>
{
    // 执行操作并返回结果
    return await FetchDataAsync();
}, "正在获取数据...");
```

## 详细使用方法

### 基本操作

```csharp
// 显示默认消息
LoadingHelper.Show();

// 显示自定义消息
LoadingHelper.Show("正在保存文件...");

// 异步显示
await LoadingHelper.ShowAsync("连接服务器中...");

// 隐藏窗口
LoadingHelper.Hide();

// 异步隐藏
await LoadingHelper.HideAsync();

// 更新消息
LoadingHelper.UpdateMessage("新的状态消息");

// 设置父窗口
LoadingHelper.SetOwner(this); // this 是当前窗口

// 检查是否正在显示
bool isShowing = LoadingHelper.IsShowing;
```

### 高级使用场景

#### 多步骤操作

```csharp
await LoadingHelper.ShowAsync("开始处理...");

try
{
    LoadingHelper.UpdateMessage("步骤 1：验证数据...");
    await ValidateDataAsync();
    
    LoadingHelper.UpdateMessage("步骤 2：处理数据...");
    await ProcessDataAsync();
    
    LoadingHelper.UpdateMessage("步骤 3：保存结果...");
    await SaveResultAsync();
    
    LoadingHelper.UpdateMessage("完成！");
    await Task.Delay(500); // 短暂显示完成消息
}
finally
{
    await LoadingHelper.HideAsync();
}
```

#### 进度显示

```csharp
await LoadingHelper.ShowAsync("上传中 0%");

for (int i = 0; i <= 100; i += 10)
{
    LoadingHelper.UpdateMessage($"上传中 {i}%");
    await Task.Delay(100); // 模拟上传进度
}

await LoadingHelper.HideAsync();
```

### MVVM 模式使用

#### ViewModel 示例

```csharp
public class MyViewModel : INotifyPropertyChanged
{
    private bool isLoading;
    
    public bool IsLoading
    {
        get => isLoading;
        private set
        {
            isLoading = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand LoadDataCommand { get; }
    
    public MyViewModel()
    {
        LoadDataCommand = new RelayCommand(async () =>
        {
            IsLoading = true;
            
            await LoadingHelper.ExecuteWithLoadingAsync(async () =>
            {
                // 加载数据的逻辑
                await LoadDataAsync();
            }, "正在加载数据...");
            
            IsLoading = false;
        });
    }
    
    private async Task LoadDataAsync()
    {
        // 实际的数据加载逻辑
        await Task.Delay(2000);
    }
}
```

#### XAML 绑定

```xml
<Grid>
    <Button Content="加载数据" 
            Command="{Binding LoadDataCommand}"
            IsEnabled="{Binding IsLoading, Converter={StaticResource InvertBooleanConverter}}" />
    
    <TextBlock Text="正在加载..." 
               Visibility="{Binding IsLoading, Converter={StaticResource BooleanToVisibilityConverter}}" />
</Grid>
```

### 依赖注入使用

```csharp
// 在启动时注册服务
services.AddSingleton<ILoadingService, LoadingService>();

// 在类中使用
public class DataService
{
    private readonly ILoadingService loadingService;
    
    public DataService(ILoadingService loadingService)
    {
        this.loadingService = loadingService;
    }
    
    public async Task LoadDataAsync()
    {
        await loadingService.ShowAsync("正在加载数据...");
        
        try
        {
            // 数据加载逻辑
            await FetchDataFromApiAsync();
        }
        finally
        {
            await loadingService.HideAsync();
        }
    }
}
```

### 直接使用 LoadingWindow

```csharp
// 创建窗口实例
var loadingWindow = new LoadingWindow("自定义加载消息");

// 设置父窗口
loadingWindow.SetOwner(this);

// 显示
loadingWindow.ShowWithAnimation();

// 更新消息
loadingWindow.LoadingMessage = "新消息";

// 隐藏
loadingWindow.HideWithAnimation();

// 关闭
loadingWindow.CloseWithAnimation();
```

## API 参考

### LoadingHelper 静态方法

| 方法 | 说明 |
|------|------|
| `Show(string message = "加载中...")` | 显示加载窗口 |
| `ShowAsync(string message = "加载中...")` | 异步显示加载窗口 |
| `Hide()` | 隐藏加载窗口 |
| `HideAsync()` | 异步隐藏加载窗口 |
| `UpdateMessage(string message)` | 更新显示消息 |
| `SetOwner(Window owner)` | 设置父窗口 |
| `ExecuteWithLoadingAsync(Func<Task> operation, string message)` | 执行异步操作并自动管理加载窗口 |
| `ExecuteWithLoadingAsync<T>(Func<Task<T>> operation, string message)` | 执行有返回值的异步操作 |

### LoadingHelper 属性

| 属性 | 类型 | 说明 |
|------|------|------|
| `IsShowing` | `bool` | 获取当前是否正在显示 |
| `Default` | `ILoadingService` | 获取默认的加载服务实例 |

### ILoadingService 接口

```csharp
public interface ILoadingService
{
    void Show(string message = "加载中...");
    Task ShowAsync(string message = "加载中...");
    void Hide();
    Task HideAsync();
    void UpdateMessage(string message);
    void SetOwner(Window owner);
    bool IsShowing { get; }
}
```

### LoadingWindow 类

```csharp
public partial class LoadingWindow : Window, INotifyPropertyChanged
{
    public string LoadingMessage { get; set; }
    
    public LoadingWindow();
    public LoadingWindow(string message);
    
    public void ShowWithAnimation();
    public void HideWithAnimation();
    public void CloseWithAnimation();
    public void SetOwner(Window owner);
}
```

## 样式自定义

Loading Window 使用了 WPF.UI 的样式系统，你可以通过以下资源键来自定义外观：

- `ControlFillColorDefaultBrush` - 主内容区域背景
- `ControlStrokeColorDefaultBrush` - 边框颜色
- `AccentTextFillColorPrimaryBrush` - ProgressRing 颜色
- `TextFillColorPrimaryBrush` - 文本颜色
- `ControlElevationBorderBrush` - 阴影效果

## 最佳实践

1. **使用 ExecuteWithLoadingAsync**：这是最简单和最安全的方式，自动处理显示和隐藏。

2. **设置父窗口**：为了获得更好的用户体验，建议设置父窗口关系。

3. **及时隐藏**：确保在操作完成或出现异常时隐藏加载窗口。

4. **有意义的消息**：提供清晰、有意义的加载消息，让用户了解当前进度。

5. **避免嵌套显示**：在显示新的加载窗口之前，确保之前的已经隐藏。

6. **异步优先**：优先使用异步方法，避免阻塞 UI 线程。

## 常见问题

**Q: 加载窗口不显示怎么办？**
A: 确保在 UI 线程上调用，或者使用提供的异步方法。

**Q: 如何在 WPF.UI 主题下获得最佳效果？**
A: 确保你的应用程序正确配置了 WPF.UI 主题系统。

**Q: 可以同时显示多个加载窗口吗？**
A: LoadingHelper 使用单例模式，同时只能显示一个。如需多个，请直接创建 LoadingWindow 实例。

**Q: 如何处理异常情况？**
A: 建议使用 try-finally 块或 ExecuteWithLoadingAsync 方法来确保加载窗口被正确隐藏。

## 示例项目

查看 `LoadingTestWindow.xaml` 和 `LoadingTestWindow.xaml.cs` 获取完整的使用示例。