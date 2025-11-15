![WPF UI Banner Dark](https://user-images.githubusercontent.com/13592821/174165081-9c62d188-ecb6-4200-abd8-419afbaf32c2.png#gh-dark-mode-only)
![WPF UI Banner Light](https://user-images.githubusercontent.com/13592821/174165388-921c4745-90ed-4396-9a4b-9c86478f7447.png#gh-light-mode-only)

# WPF UI

## 🔗 源代码声明

本库是基于开源项目 [WPF UI](https://github.com/lepoco/wpfui) 进行的修改和定制。感谢原作者 [lepo.co](https://lepo.co/) 和开源社区的贡献。

---

[由波兰 lepo.co 用 ❤ 创建](https://lepo.co/) 和 [优秀的开源社区](https://github.com/lepoco/wpfui/graphs/contributors)  
WPF UI 在您熟知和喜爱的 WPF 框架中提供 Fluent 设计体验。直观的设计、主题、导航和新的沉浸式控件。完全本机化和毫不费力。该库改变了基础元素如 `Page`、`ToggleButton` 或 `List`，还包括额外的控件如 `Navigation`、`NumberBox`、`Dialog` 或 `Snackbar`。

[![Discord](https://img.shields.io/discord/1071051348348514375?label=discord)](https://discord.gg/AR9ywDUwGq) [![GitHub license](https://img.shields.io/github/license/lepoco/wpfui)](https://github.com/lepoco/wpfui/blob/master/LICENSE) [![Nuget](https://img.shields.io/nuget/v/WPF-UI)](https://www.nuget.org/packages/WPF-UI/) [![Nuget](https://img.shields.io/nuget/dt/WPF-UI?label=nuget)](https://www.nuget.org/packages/WPF-UI/)

![ua](https://user-images.githubusercontent.com/13592821/184498735-d296feb8-0f9b-45df-bc0d-b7f0b6f580ed.png)
  
  
## 🚀 快速开始

获取入门指南，请查看我们的[文档](https://wpfui.lepo.co/documentation/)。

**WPF UI Gallery** 是一个免费应用程序，可在 _Microsoft Store_ 中获得，您可以使用它来测试所有功能。  
https://apps.microsoft.com/store/detail/wpf-ui/9N9LKV8R9VGM?cid=windows-lp-hero

```powershell
$ winget install 'WPF UI'
```

**WPF UI** 通过 **NuGet** 包管理器提供。您可以在此处找到该包：  
https://www.nuget.org/packages/wpf-ui/

**Visual Studio**  
**Visual Studio 2022** 的插件让您可以轻松使用 **WPF UI** 创建新项目。  
https://marketplace.visualstudio.com/items?itemName=lepo.wpf-ui

## 📷 截图

![Demo App Sample](https://user-images.githubusercontent.com/13592821/166259110-0fb98120-fe34-4e6d-ab92-9f72ad7113c3.png)

![Monaco Editor](https://user-images.githubusercontent.com/13592821/258610583-7d71f69d-45b3-4be6-bcb8-8cf6cd60a2ff.png)

![Store App Sample](https://user-images.githubusercontent.com/13592821/165918914-6948fb42-1ee1-4c36-870e-65bb8ffe3c8a.png)

## 🏗️ 与 Visual Studio 设计器兼容

![VS2022 Designer Preview](https://user-images.githubusercontent.com/13592821/165919228-0aa3a36c-fb37-4198-835e-53488845226c.png)

## ❤️ 纯 WPF 中的自定义托盘图标和菜单

![WPF UI Tray menu in WPF](https://user-images.githubusercontent.com/13592821/166259470-2d48a88e-47ce-4f8f-8f07-c9b110de64a5.png)

## ⚓ 适用于 TitleBar 的自定义 Windows 11 SnapLayout。

![WPF UI Snap Layout for WPF](https://user-images.githubusercontent.com/13592821/166259869-e60d37e4-ded4-46bf-80d9-f92c47266f34.png)

## 📖 文档

文档可以在 https://wpfui.lepo.co/ 找到。我们还为新手提供了[教程](#-快速开始)。

## 🚧 开发

如果您想提出新功能或提交错误修复，请为 [main](https://github.com/lepoco/wpfui/tree/main) 分支创建 [Pull Request](https://github.com/lepoco/wpfui/compare/main...main)。

## 📐 如何使用？

首先，您的应用程序需要加载自定义样式，在 **MyApp\App.xaml** 文件中添加：

```xml
<Application
  ...
  xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml">
  <Application.Resources>
    <ResourceDictionary>
      <ResourceDictionary.MergedDictionaries>
        <ui:ThemesDictionary Theme="Dark" />
        <ui:ControlsDictionary />
      </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
  </Application.Resources>
</Application>
```

如果您的应用程序没有 **MyApp\App.xaml** 文件，请在 `frameworkElement` 的构造函数中使用 `ApplicationThemeManager.Apply(frameworkElement)` 来应用/更新主题资源。

```C#
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        ApplicationThemeManager.Apply(this);
    }
}
```

现在您可以创建出色的应用程序，例如只需一个按钮：

```xml
<ui:FluentWindow
  ...
  xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml">
  <StackPanel>
      <ui:TitleBar Title="WPF UI"/>
      <ui:Card Margin="8">
          <ui:Button Content="Hello World" Icon="{ui:SymbolIcon Fluent24}" />
      </ui:Card>
  </StackPanel>
</ui:FluentWindow>
```

## 特别感谢

在没有 ReSharper 或 XAML Styler 这样的工具创建者的情况下，为 .NET 创建应用程序永远不会是如此奇妙的冒险。

- [🔗 JetBrains ReSharper](https://www.jetbrains.com/resharper/)
- [🔗 XAML Styler](https://github.com/Xavalon/XamlStyler)

JetBrains 非常友好地为 WPF UI 开发提供了开源 **dotUltimate** 许可证。在此了解更多信息：

- https://www.jetbrains.com/dotnet/
- https://www.jetbrains.com/opensource/

## 微软资产

界面的设计、颜色的选择和控件的外观受到 Microsoft 为 Windows 11 制作的项目的启发。  
Wpf.Ui.Gallery 应用程序包含来自 _Microsoft WinUI 3 Gallery_ 应用程序的图标。它们在这里用作为 Microsoft 系统创建工具的示例。

## Segoe Fluent 图标

**WPF UI** 使用 Fluent 系统图标。虽然这个字体也是由 Microsoft 创建的，但它不包含 Windows 11 的所有图标。如果您需要缺少的图标，请将 Segoe Fluent Icons 添加到您的应用程序中。  
根据 Segoe Fluent Icons 的最终用户许可协议，我们不能将其副本与此 dll 一起提供。Segoe Fluent Icons 在 Windows 11 上默认安装，但如果您想在 Windows 10 及以下版本的应用程序中使用这些图标，您必须手动将字体添加到应用程序的资源中。  
[https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font](https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font)  
[https://docs.microsoft.com/en-us/windows/apps/design/downloads/#fonts](https://docs.microsoft.com/en-us/windows/apps/design/downloads/#fonts)

在应用程序字典中，您可以添加字体的替代路径

```XML
<FontFamily x:Key="SegoeFluentIcons">pack://application:,,,/;component/Fonts/#Segoe Fluent Icons</FontFamily>
```

## 行为准则

本项目已采纳由贡献者公约定义的行为准则，以阐明我们社区中的预期行为。

## 许可证

**WPF UI** 是在 **MIT 许可证** 下发布的免费开源软件。您可以在私人和商业项目中使用它。  
请记住，您必须在项目中包含许可证的副本。
