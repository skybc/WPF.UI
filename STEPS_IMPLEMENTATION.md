# Steps 控件实现文档

## 概述
实现了类似 Element Plus `<el-steps>` 的步骤控件，包含水平和垂直两种布局方式。

## 创建的文件

### 1. 控件逻辑文件
- **WPF.UI/Controls/Steps/StepStatus.cs** - 步骤状态枚举（Wait, Process, Finish, Error）
- **WPF.UI/Controls/Steps/Step.cs** - 单个步骤项控件类
- **WPF.UI/Controls/Steps/Steps.cs** - 步骤容器控制类

### 2. 模板文件
- **WPF.UI/Controls/Steps/Step.xaml** - Step 控件的样式和模板
- **WPF.UI/Controls/Steps/Steps.xaml** - Steps 容器控制的样式和模板

### 3. 转换器
- **WPF.UI/Converters/AddOneConverter.cs** - 用于将步骤索引转换为显示编号（0→1, 1→2 等）

### 4. 示例页面
- **Wpf.Ui.Gallery/Views/Pages/Layout/StepsPage.xaml** - 示例页面 XAML
- **Wpf.Ui.Gallery/Views/Pages/Layout/StepsPage.xaml.cs** - 示例页面代码隐藏

### 5. 资源注册
- **WPF.UI/Resources/Converter.xaml** - 已添加 AddOneConverter
- **WPF.UI/Resources/Wpf.Ui.xaml** - 已添加 Step.xaml 和 Steps.xaml 资源字典引用

## 主要特性

### Step 控件
- **Title** (DependencyProperty) - 步骤标题
- **Description** (DependencyProperty) - 步骤描述
- **Status** (DependencyProperty) - 步骤状态 (Wait/Process/Finish/Error)
- **Index** (DependencyProperty) - 步骤索引（0-based）
- **IsClickable** (DependencyProperty) - 是否可点击
- **Clicked** (RoutedEvent) - 步骤被点击事件

### Steps 容器
- **Orientation** (DependencyProperty) - 布局方向 (Horizontal/Vertical)
- **CurrentStep** (DependencyProperty) - 当前步骤索引
- **StepChanged** (RoutedEvent) - 步骤变更事件
- 自动管理 Step 的状态：
  - 当前步骤前的所有步骤 → Finish（完成）
  - 当前步骤 → Process（进行中）
  - 当前步骤后的所有步骤 → Wait（等待）
  - 可点击性由 Steps 容器自动控制

## 样式设计

### 颜色方案
- **Wait（等待）**: 使用 ControlAltFillColorSecondaryBrush 灰色
- **Process（进行中）**: 使用 AccentFillColorDefaultBrush 强调色（通常为蓝色）
- **Finish（完成）**: 使用 SuccessFillColorDefaultBrush 成功色（通常为绿色）
- **Error（错误）**: 使用 ErrorFillColorDefaultBrush 错误色（通常为红色）

### 显示元素
- **Wait** 状态：显示步骤编号 (1, 2, 3...)
- **Process** 状态：显示 ● (填充的圆点)
- **Finish** 状态：显示 ✓ (勾号)
- **Error** 状态：显示 ✕ (叉号)

## 使用示例

### 基本用法
```xaml
<ui:Steps CurrentStep="0" Orientation="Horizontal">
    <ui:Step Title="步骤 1" Description="初始步骤" />
    <ui:Step Title="步骤 2" Description="处理中" />
    <ui:Step Title="步骤 3" Description="完成" />
</ui:Steps>
```

### 垂直布局
```xaml
<ui:Steps CurrentStep="1" Orientation="Vertical">
    <ui:Step Title="提交表单" Description="填写表单信息" />
    <ui:Step Title="审核" Description="管理员审核中" />
    <ui:Step Title="完成" Description="审核完成" />
</ui:Steps>
```

### 代码绑定
```csharp
// 切换到第 2 步（0-based 索引）
steps.CurrentStep = 1;

// 监听步骤变更事件
steps.StepChanged += (s, e) => { /* 处理步骤变更 */ };
```

## 示例页面功能

StepsPage.xaml 展示了三个示例：

1. **水平步骤条** - 3 个步骤，可点击切换
2. **垂直步骤条** - 4 个步骤，包含错误状态演示
3. **动态演示** - 自动播放步骤进度

## 注意事项

1. 当 Status 改变为 Process 时，IsClickable 会自动设为 false
2. Wait 状态的步骤默认可点击，点击后会触发 Clicked 事件
3. Finish 状态的步骤也可点击，方便回溯之前的步骤
4. Steps 容器会自动在 CurrentStep 变更时更新所有 Step 的状态

## 主题集成

所有颜色都使用主题资源，支持亮色/暗色主题切换：
- `AccentFillColorDefaultBrush` - 强调色
- `SuccessFillColorDefaultBrush` - 成功色
- `ErrorFillColorDefaultBrush` - 错误色
- `TextFillColorPrimaryBrush` - 主文本色
- `TextFillColorSecondaryBrush` - 次文本色
- `ControlAltFillColorSecondaryBrush` - 控件背景色
- `ControlElevationBorderBrush` - 边框色
