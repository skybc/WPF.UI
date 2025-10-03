# EnhancedCard 控件使用指南

## 概述

`EnhancedCard` 是一个增强版的卡片控件，提供了丰富的功能和高度的可定制性。

## 功能特性

- ✅ **三区域插槽**：支持 Header、Content、Footer
- ✅ **圆角可配置**：通过 `CornerRadius` 属性自定义
- ✅ **阴影效果**：通过 `Elevation` 属性控制阴影深度（0-5级）
- ✅ **主题适配**：自动适应亮色/暗色主题
- ✅ **点击支持**：通过 `Command` 属性实现点击功能
- ✅ **悬停效果**：可点击卡片支持悬停视觉反馈

## 基本用法

### 1. 简单卡片

```xaml
<ui:EnhancedCard>
    <TextBlock Text="这是一个简单的卡片" />
</ui:EnhancedCard>
```

### 2. 带 Header 和 Footer 的卡片

```xaml
<ui:EnhancedCard>
    <ui:EnhancedCard.Header>
        <TextBlock Text="卡片标题" FontSize="16" FontWeight="SemiBold" />
    </ui:EnhancedCard.Header>
    
    <StackPanel>
        <TextBlock Text="这是卡片的主要内容区域" />
        <TextBlock Text="可以放置任意控件" Margin="0,8,0,0" />
    </StackPanel>
    
    <ui:EnhancedCard.Footer>
        <TextBlock Text="最后更新：2025-10-03" Foreground="Gray" />
    </ui:EnhancedCard.Footer>
</ui:EnhancedCard>
```

### 3. 自定义圆角和阴影

```xaml
<!-- 大圆角，高阴影 -->
<ui:EnhancedCard 
    CornerRadius="12" 
    Elevation="4">
    <TextBlock Text="更加立体的卡片效果" />
</ui:EnhancedCard>

<!-- 无阴影，扁平风格 -->
<ui:EnhancedCard 
    Elevation="0" 
    BorderThickness="2"
    BorderBrush="{DynamicResource SystemAccentColorPrimary}">
    <TextBlock Text="扁平风格卡片" />
</ui:EnhancedCard>
```

### 4. 可点击卡片

```xaml
<ui:EnhancedCard 
    Command="{Binding CardClickCommand}"
    CommandParameter="Card1">
    <ui:EnhancedCard.Header>
        <TextBlock Text="点击我" />
    </ui:EnhancedCard.Header>
    <TextBlock Text="这是一个可点击的卡片" />
</ui:EnhancedCard>
```

### 5. 自定义背景和边框

```xaml
<ui:EnhancedCard 
    Background="#FFF3E0"
    BorderBrush="#FF9800"
    BorderThickness="2"
    CornerRadius="8"
    Elevation="3">
    <TextBlock Text="自定义颜色的卡片" />
</ui:EnhancedCard>
```

## 属性说明

| 属性 | 类型 | 默认值 | 说明 |
|-----|------|--------|------|
| `Header` | object | null | 卡片头部内容 |
| `Footer` | object | null | 卡片底部内容 |
| `CornerRadius` | CornerRadius | 4 | 圆角半径 |
| `Elevation` | int | 2 | 阴影深度（0-5） |
| `Command` | ICommand | null | 点击命令 |
| `CommandParameter` | object | null | 命令参数 |
| `IsClickable` | bool | false | 是否可点击（设置 Command 时自动为 true） |
| `Background` | Brush | CardBackground | 背景色 |
| `BorderBrush` | Brush | CardBorderBrush | 边框颜色 |
| `BorderThickness` | Thickness | 1 | 边框粗细 |

## 阴影级别说明

- **Elevation 0**：无阴影，扁平风格
- **Elevation 1**：轻微阴影，适合低层级内容
- **Elevation 2**：默认阴影，适合大多数场景
- **Elevation 3**：中等阴影，适合突出显示
- **Elevation 4**：较深阴影，适合悬浮效果
- **Elevation 5**：最深阴影，适合模态对话框等

## 完整示例

```xaml
<Window x:Class="YourApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml">
    
    <StackPanel Margin="20" Spacing="16">
        
        <!-- 信息卡片 -->
        <ui:EnhancedCard Elevation="2">
            <ui:EnhancedCard.Header>
                <StackPanel Orientation="Horizontal">
                    <ui:SymbolIcon Symbol="Info24" Margin="0,0,8,0" />
                    <TextBlock Text="系统通知" FontWeight="SemiBold" />
                </StackPanel>
            </ui:EnhancedCard.Header>
            
            <TextBlock TextWrapping="Wrap">
                您的系统已成功更新到最新版本。
            </TextBlock>
            
            <ui:EnhancedCard.Footer>
                <TextBlock Text="2 小时前" />
            </ui:EnhancedCard.Footer>
        </ui:EnhancedCard>
        
        <!-- 可点击的功能卡片 -->
        <ui:EnhancedCard 
            Command="{Binding OpenSettingsCommand}"
            Elevation="3"
            CornerRadius="8">
            <ui:EnhancedCard.Header>
                <TextBlock Text="设置" FontSize="18" FontWeight="Bold" />
            </ui:EnhancedCard.Header>
            
            <StackPanel>
                <TextBlock Text="管理您的应用程序设置" />
                <TextBlock Text="点击进入设置页面" 
                          Foreground="Gray" 
                          Margin="0,4,0,0" />
            </StackPanel>
        </ui:EnhancedCard>
        
        <!-- 统计卡片 -->
        <Grid>
            <Grid.ColumnDefinitions>
                <ColumnDefinition />
                <ColumnDefinition Width="16" />
                <ColumnDefinition />
            </Grid.ColumnDefinitions>
            
            <ui:EnhancedCard Grid.Column="0" Elevation="1">
                <StackPanel HorizontalAlignment="Center">
                    <TextBlock Text="1,234" 
                              FontSize="32" 
                              FontWeight="Bold"
                              HorizontalAlignment="Center" />
                    <TextBlock Text="总访问量" 
                              Foreground="Gray"
                              HorizontalAlignment="Center" />
                </StackPanel>
            </ui:EnhancedCard>
            
            <ui:EnhancedCard Grid.Column="2" Elevation="1">
                <StackPanel HorizontalAlignment="Center">
                    <TextBlock Text="567" 
                              FontSize="32" 
                              FontWeight="Bold"
                              HorizontalAlignment="Center" />
                    <TextBlock Text="今日访问" 
                              Foreground="Gray"
                              HorizontalAlignment="Center" />
                </StackPanel>
            </ui:EnhancedCard>
        </Grid>
        
    </StackPanel>
</Window>
```

## 主题适配

EnhancedCard 自动支持亮色和暗色主题，无需额外配置。控件会自动使用主题中定义的颜色资源。

## 最佳实践

1. **合理使用阴影**：过多的阴影会让界面显得杂乱，建议同一界面的卡片使用统一的 Elevation
2. **保持一致性**：在同一应用中保持卡片的圆角和间距一致
3. **避免嵌套**：不建议在 EnhancedCard 内嵌套另一个 EnhancedCard
4. **响应式设计**：确保卡片内容在不同窗口尺寸下都能正常显示
5. **可点击提示**：可点击的卡片应该有明确的视觉提示（如图标、文字等）

## 注意事项

- 设置 `Command` 属性时，`IsClickable` 会自动设为 `true`
- Header 和 Footer 使用不同的 Padding 以优化视觉效果
- Footer 区域会自动应用次要文本颜色
- 阴影效果通过 Border 层叠实现，性能优于 DropShadowEffect
