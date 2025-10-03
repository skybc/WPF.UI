# EnhancedCard 快速开始

## 🚀 快速开始（3分钟上手）

### 步骤 1：引入命名空间

在您的 XAML 文件中添加命名空间：

```xaml
<Window xmlns:controls="clr-namespace:Wpf.Ui.Controls;assembly=Wpf.Ui"
        ...>
```

### 步骤 2：使用控件

```xaml
<!-- 最简单的用法 -->
<controls:EnhancedCard>
    <TextBlock Text="Hello, EnhancedCard!" />
</controls:EnhancedCard>
```

### 步骤 3：添加标题和页脚（可选）

```xaml
<controls:EnhancedCard>
    <controls:EnhancedCard.Header>
        <TextBlock Text="卡片标题" FontWeight="SemiBold" />
    </controls:EnhancedCard.Header>
    
    <TextBlock Text="卡片内容" />
    
    <controls:EnhancedCard.Footer>
        <TextBlock Text="更新时间：2025-10-03" FontSize="12" />
    </controls:EnhancedCard.Footer>
</controls:EnhancedCard>
```

## 💡 常用配置

### 调整阴影深度
```xaml
<controls:EnhancedCard Elevation="3">
    <!-- 更明显的阴影效果 -->
</controls:EnhancedCard>
```

### 调整圆角
```xaml
<controls:EnhancedCard CornerRadius="12">
    <!-- 更圆润的卡片 -->
</controls:EnhancedCard>
```

### 添加点击功能
```xaml
<controls:EnhancedCard Command="{Binding MyCommand}">
    <!-- 可点击的卡片，会显示悬停效果 -->
</controls:EnhancedCard>
```

### 自定义颜色
```xaml
<controls:EnhancedCard 
    Background="#E3F2FD"
    BorderBrush="#2196F3"
    BorderThickness="2">
    <!-- 蓝色主题卡片 -->
</controls:EnhancedCard>
```

## 📖 完整文档

查看详细文档：`README_EnhancedCard.md`

查看示例代码：`EnhancedCardDemo.xaml`

## ⚡ 属性速查

| 属性 | 类型 | 默认值 | 说明 |
|-----|------|--------|------|
| `Header` | object | null | 标题 |
| `Footer` | object | null | 页脚 |
| `Elevation` | int | 2 | 阴影深度（0-5） |
| `CornerRadius` | CornerRadius | 4 | 圆角 |
| `Command` | ICommand | null | 点击命令 |
| `Background` | Brush | 主题色 | 背景 |
| `BorderBrush` | Brush | 主题色 | 边框 |

## 🎨 预设效果

```xaml
<!-- 扁平卡片 -->
<controls:EnhancedCard Elevation="0" />

<!-- 标准卡片（默认） -->
<controls:EnhancedCard Elevation="2" />

<!-- 浮起卡片 -->
<controls:EnhancedCard Elevation="4" />

<!-- 最大阴影 -->
<controls:EnhancedCard Elevation="5" />
```

## ❓ 常见问题

**Q: 如何让卡片填满整个区域？**
```xaml
<controls:EnhancedCard HorizontalAlignment="Stretch" VerticalAlignment="Stretch">
```

**Q: 如何去除边框？**
```xaml
<controls:EnhancedCard BorderThickness="0">
```

**Q: 如何让卡片可点击但不使用 Command？**
```xaml
<controls:EnhancedCard IsClickable="True" MouseLeftButtonDown="OnCardClick">
```

**Q: 如何禁用阴影？**
```xaml
<controls:EnhancedCard Elevation="0">
```

---

**开始使用吧！** 🎉

有问题？查看完整文档：`README_EnhancedCard.md`
