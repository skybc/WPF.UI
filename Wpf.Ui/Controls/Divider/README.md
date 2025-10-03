# Divider 控件使用指南

## 概述

`Divider` 是一个用于视觉分隔的控件,可以在横向或纵向方向上绘制分隔线,并可选地在分隔线中间显示文本。

## 功能特性

- ✅ 支持横向 (Horizontal) 和纵向 (Vertical) 方向
- ✅ 支持在分隔线中间显示文本内容
- ✅ 支持文本对齐方式:左对齐、居中、右对齐
- ✅ 可自定义线条粗细和颜色
- ✅ 支持主题自动切换 (Light/Dark)
- ✅ 可调整文本与分隔线之间的间距

## 基本用法

### 1. 简单的分隔线

```xaml
<ui:Divider />
```

### 2. 带文本的分隔线 (居中)

```xaml
<ui:Divider Content="选项" />
<!-- 或 -->
<ui:Divider Content="选项" ContentAlignment="Center" />
```

### 3. 左对齐文本

```xaml
<ui:Divider Content="选项" ContentAlignment="Left" />
```

### 4. 右对齐文本

```xaml
<ui:Divider Content="选项" ContentAlignment="Right" />
```

### 5. 纵向分隔线

```xaml
<ui:Divider Orientation="Vertical" Height="100" />
```

### 6. 纵向分隔线带文本

```xaml
<ui:Divider Orientation="Vertical" Height="200" Content="Section" />
```

### 7. 自定义样式

```xaml
<ui:Divider 
    Content="自定义分隔线"
    LineThickness="2"
    LineStroke="Red"
    Spacing="20"
    FontSize="16"
    Foreground="Blue" />
```

## 完整示例

```xaml
<Window xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:ui="clr-namespace:Wpf.Ui.Controls;assembly=Wpf.Ui"
        Width="600" Height="400">
    
    <StackPanel Margin="20">
        <!-- 基础分隔线 -->
        <TextBlock Text="Section 1" FontSize="16" FontWeight="Bold" />
        <ui:Divider />
        <TextBlock Text="Content of section 1..." />
        
        <!-- 带标题的分隔线 (居中) -->
        <ui:Divider Content="Section 2" Margin="0,20,0,10" />
        <TextBlock Text="Content of section 2..." />
        
        <!-- 左对齐标题 -->
        <ui:Divider Content="Section 3" ContentAlignment="Left" Margin="0,20,0,10" />
        <TextBlock Text="Content of section 3..." />
        
        <!-- 右对齐标题 -->
        <ui:Divider Content="Section 4" ContentAlignment="Right" Margin="0,20,0,10" />
        <TextBlock Text="Content of section 4..." />
        
        <!-- 纵向分隔线示例 -->
        <Grid Height="100" Margin="0,20,0,0">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="*" />
                <ColumnDefinition Width="Auto" />
                <ColumnDefinition Width="*" />
            </Grid.ColumnDefinitions>
            
            <TextBlock Grid.Column="0" Text="Left Content" VerticalAlignment="Center" HorizontalAlignment="Center" />
            <ui:Divider Grid.Column="1" Orientation="Vertical" Margin="10,0" />
            <TextBlock Grid.Column="2" Text="Right Content" VerticalAlignment="Center" HorizontalAlignment="Center" />
        </Grid>
    </StackPanel>
</Window>
```

## 属性说明

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `Orientation` | `Orientation` | `Horizontal` | 分隔线方向:横向或纵向 |
| `Content` | `object` | `null` | 分隔线中间显示的内容 |
| `ContentAlignment` | `HorizontalAlignment` | `Center` | 内容对齐方式:Left, Center, Right |
| `LineThickness` | `double` | `1` | 分隔线粗细 |
| `LineStroke` | `Brush` | (主题颜色) | 分隔线颜色 |
| `Spacing` | `double` | `12` | 内容与分隔线之间的间距 |
| `Foreground` | `Brush` | (主题颜色) | 文本颜色 |
| `FontSize` | `double` | `14` | 文本字体大小 |
| `Margin` | `Thickness` | `0,8,0,8` | 控件外边距 |

## 应用场景

1. **表单分组**: 在表单中分隔不同的输入区域
2. **内容分隔**: 在文章或页面中分隔不同的内容区块
3. **导航菜单**: 分隔菜单项或菜单组
4. **设置页面**: 分隔不同的设置分类
5. **对话框**: 在对话框中分隔不同的操作区域
6. **侧边栏**: 使用纵向分隔线分隔不同的面板

## 主题支持

Divider 控件完全支持主题切换,使用以下动态资源:

- `DividerStrokeColorDefaultBrush` - 分隔线默认颜色
- `TextFillColorSecondaryBrush` - 文本默认颜色

在亮色和暗色主题之间切换时,分隔线和文本颜色会自动适配。

## 注意事项

1. 当使用纵向分隔线时,建议设置 `Height` 属性以确保适当的显示高度
2. `ContentAlignment` 属性仅在横向 (Horizontal) 方向时有效
3. 当 `Content` 为 `null` 或空字符串时,将只显示分隔线,不显示内容区域
4. 自定义 `LineStroke` 会覆盖主题默认颜色

## 版本历史

- **v1.0.0** (2025-10-03): 初始版本
  - 支持横向和纵向分隔线
  - 支持文本内容和对齐方式
  - 支持主题系统
  - 可自定义样式属性
