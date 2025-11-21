# Wpf.Ui 开发指南

这是 WPF UI 库（https://github.com/lepoco/wpfui）的定制版本，为 WPF 应用提供 Fluent Design System 组件，增强了主题支持和自定义控件。

## 架构概览

### 项目结构
- **WPF.UI/** - 核心库，包含所有控件、服务和主题管理
- **Wpf.Ui.Abstractions/** - 服务接口契约（INavigationService、ISnackbarService、IThemeService）
- **Wpf.Ui.Gallery/** - 演示应用，展示所有控件功能
- **Wpf.Ui.DependencyInjection/** - 依赖注入容器集成
- **Wpf.Ui.ToastNotifications/** - 吐司通知系统

### 基于服务的架构模式
核心功能通过接口化服务实现：

1. **NavigationService** (INavigationService) - 页面导航，通过 INavigationViewPageProvider 管理
   - 使用 `Navigate(Type pageType)` 或 `Navigate(string pageTag)` 进行页面转换
   - 支持传递 DataContext：`Navigate(pageType, dataContext)`
   - 页面类型必须在 INavigationViewPageProvider 中注册

2. **ThemeService** (IThemeService) - 主题和强调色管理
   - `GetTheme()` / `SetTheme(ApplicationTheme)` - 亮色/暗色/高对比度
   - `SetSystemAccent()` - 同步 Windows 系统强调色
   - `SetAccent(Color)` - 应用自定义强调色

3. **SnackbarService** (ISnackbarService) - 全局通知提示
   - 需在 XAML 中设置 SnackbarPresenter：`SetSnackbarPresenter(presenter)`
   - `Show(title, message, appearance, icon, timeout)` 使用 ControlAppearance 枚举

## 关键模式与约定

### 控件实现模式
控件遵循 WPF 模板部分（Template Part）模式：

```csharp
[TemplatePart(Name = "PART_SomeName", Type = typeof(Control))]
public class CustomControl : Control
{
    // 依赖属性，带 PropertyMetadata 处理
    public static readonly DependencyProperty MyPropertyProperty = 
        DependencyProperty.Register(nameof(MyProperty), typeof(string), 
        typeof(CustomControl), new PropertyMetadata(default, OnMyPropertyChanged));
    
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        // 根据名称获取模板部分
    }
}
```

### 资源管理
- 所有控件使用 `WPF.UI/Resources/` 中的资源字典
- **ThemesDictionary** - 颜色和画刷主题资源（亮色/暗色）
- **ControlsDictionary** - 控件模板和样式
- 主题颜色通过 `ThemeResource` 枚举标记访问：`{ui:ThemeResource TextFillColorPrimaryBrush}`

### 外观系统
- 使用 `ControlAppearance` 枚举：Primary、Secondary、Info、Success、Danger、Caution、Dark、Light、Transparent
- 颜色通过 `ApplicationThemeManager` 自动适应主题
- 代码库中的两个主题系统：
  - **Wpf.Ui 主题** - 现代 Fluent 颜色（AccentFillColorDefaultBrush、SuccessFillColorDefaultBrush 等）
  - **OhmStudio.UI 主题** - VisualStudio 2019/2022 主题

### 转换器与标记扩展模式
- 自定义转换器在 `WPF.UI/Converters/` 中（例如 AddOneConverter 用于索引 → 显示编号转换）
- 在 `WPF.UI/Resources/Converter.xaml` 中注册
- 标记扩展在 `WPF.UI/Markup/` 中（例如 ThemeResource、SymbolIcon）

## 本库的自定义控件

### Steps 控件（新增 - 生产就绪）
水平/垂直进度指示器（类似 Element Plus 的 `<el-steps>`）：
- **StepStatus 枚举**：Wait、Process、Finish、Error
- **Step 控件**：Title、Description、Status、IsClickable、Clicked 事件
- **Steps 容器**：Orientation、CurrentStep、自动管理子项状态
- 位置：`WPF.UI/Controls/Steps/`
- 示例：`Wpf.Ui.Gallery/Views/Pages/Layout/StepsPage.xaml`

### 增强型控件
- **LoadingCircle** - 圆形加载动画
- **LoadingScreen** - 全屏加载覆盖层
- **Growl** - 吐司通知系统（SnackBar 的另一选择）
- **FormItem** - 表单字段包装器（含标签和验证）

## 开发工作流

### 构建与运行
```powershell
# 构建主库
dotnet build WPF.UI/Wpf.Ui.csproj

# 运行 Gallery 演示应用
dotnet run --project Wpf.Ui.Gallery/Wpf.Ui.Gallery.csproj

# 运行特定测试应用
dotnet run --project TestGrowlApp/TestGrowlApp.csproj
```

### 添加新控件
1. 在 `WPF.UI/Controls/[ControlName]/[ControlName].cs` 中创建控件类
2. 在 `WPF.UI/Controls/[ControlName]/[ControlName].xaml` 中创建 XAML 模板
3. 在 `WPF.UI/Resources/Wpf.Ui.xaml` 合并字典中注册模板
4. 创建 Gallery 页面：`Wpf.Ui.Gallery/Views/Pages/[Category]/[ControlName]Page.xaml`
5. 用 `[GalleryPage]` 属性标记以自动注册

### 主题集成
始终为颜色使用主题资源画刷：
- 文本：`TextFillColorPrimaryBrush`、`TextFillColorSecondaryBrush`
- 填充：`ControlFillColorDefault`、`ControlAltFillColorSecondaryBrush`
- 强调色：`AccentFillColorDefaultBrush`、`SuccessFillColorDefaultBrush`
- 边框：`ControlStrokeColorDefaultBrush`

访问方式：在 XAML 中使用 `{DynamicResource ResourceKey}`，或在代码中调用 `SetCurrentValue()`。

## 必知的关键文件

| 目的 | 路径 |
|------|------|
| 主题颜色/画刷 | `WPF.UI/Appearance/ApplicationThemeManager.cs` |
| 控件模板根目录 | `WPF.UI/Resources/Wpf.Ui.xaml` |
| 转换器注册表 | `WPF.UI/Resources/Converter.xaml` |
| 导航服务设置 | `WPF.UI/NavigationService.cs` |
| Snackbar 服务设置 | `WPF.UI/SnackbarService.cs` |
| 全局类型导入 | `WPF.UI/GlobalUsings.cs` |
| Gallery 启动器 | `Wpf.Ui.Gallery/App.xaml.cs` |

## 常见陷阱

1. **SnackbarPresenter 未设置** - 必须先调用 `SetSnackbarPresenter()` 再调用 `Show()`
2. **导航失败** - 页面类型必须在 INavigationViewPageProvider 中注册
3. **主题未更新** - 使用 `SetCurrentValue()` 而非直接属性赋值来更新依赖属性
4. **模板部分为空** - 检查名称是否与 `[TemplatePart(Name = "PART_*")]` 完全匹配
5. **资源未找到** - 验证资源已在 `ControlsDictionary` 源 XAML 中注册

## 测试与验证

根目录下可用的测试应用：
- `TestGrowlApp` - 测试通知/吐司系统
- `TestLoadingCircleApp` - 加载动画测试
- `SimpleSymbolIconTest` - 图标渲染测试
- Gallery 应用包含所有控件的交互示例
