using System;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Controls;
using Wpf.Ui.Services;

namespace LoadingDemo
{
    /// <summary>
    /// 美化 Loading 窗口演示窗口的交互逻辑.
    /// </summary>
    public partial class EnhancedLoadingDemoWindow : Window
    {
        private LoadingWindow customLoadingWindow;

        public EnhancedLoadingDemoWindow()
        {
            InitializeComponent();
            UpdateStatus("演示程序已启动");
            UpdateThemeStatus();
            UpdateLoadingStatus();
        }

        #region 基础美化演示

        private async void ShowBasicLoading_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("显示基础加载效果 (3秒)");
            
            LoadingHelper.SetOwner(this);
            LoadingHelper.Show("正在处理您的请求...");
            
            await Task.Delay(3000);
            LoadingHelper.Hide();
            
            UpdateStatus("基础加载演示完成");
        }

        private void ShowWithSubtitle_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("显示带副标题的加载窗口");
            
            LoadingHelper.SetOwner(this);
            LoadingHelper.Show("上传文件中", "请稍候，正在处理您的文件...");
            
            UpdateStatus("已显示带副标题的加载窗口");
        }

        private void ShowLongText_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("显示长文本加载窗口");
            
            LoadingHelper.SetOwner(this);
            LoadingHelper.Show(
                "正在执行复杂的数据处理操作，这可能需要一些时间", 
                "系统正在分析大量数据并生成详细报告，请耐心等待处理完成"
            );
            
            UpdateStatus("已显示长文本加载窗口");
        }

        private void HideLoading_Click(object sender, RoutedEventArgs e)
        {
            LoadingHelper.Hide();
            UpdateStatus("已手动隐藏加载窗口");
        }

        #endregion

        #region 动画效果演示

        private async void ShowSmoothAnimation_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("演示流畅动画效果");
            
            LoadingHelper.SetOwner(this);
            await LoadingHelper.ShowAsync("观察流畅的动画效果", "淡入、缩放、脉动动画");
            
            await Task.Delay(2000);
            
            await LoadingHelper.HideAsync();
            UpdateStatus("流畅动画演示完成");
        }

        private async void ShowTextEmphasis_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("演示文本强调动画");
            
            LoadingHelper.SetOwner(this);
            LoadingHelper.Show("观察文本更新效果");
            
            await Task.Delay(1000);
            LoadingHelper.UpdateMessage("第一次更新", "注意文字的强调动画");
            
            await Task.Delay(1000);
            LoadingHelper.UpdateMessage("第二次更新", "每次更新都有轻微的缩放效果");
            
            await Task.Delay(1000);
            LoadingHelper.UpdateMessage("演示完成", "文本强调动画展示结束");
            
            await Task.Delay(1000);
            LoadingHelper.Hide();
            
            UpdateStatus("文本强调动画演示完成");
        }

        private async void ShowMultiStep_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("演示多步骤加载流程");
            
            string[] steps = {
                "初始化系统组件",
                "加载配置文件",
                "连接数据库",
                "验证用户权限",
                "加载用户数据",
                "准备用户界面",
                "启动后台服务",
                "完成初始化"
            };
            
            string[] subtitles = {
                "正在启动核心模块...",
                "读取系统配置信息...",
                "建立数据库连接...",
                "检查访问权限...",
                "获取个人设置...",
                "渲染界面组件...",
                "启动监控服务...",
                "系统准备就绪！"
            };
            
            LoadingHelper.SetOwner(this);
            await LoadingHelper.ShowAsync(steps[0], subtitles[0]);
            
            for (int i = 1; i < steps.Length; i++)
            {
                await Task.Delay(800);
                LoadingHelper.UpdateMessage(steps[i], subtitles[i]);
            }
            
            await Task.Delay(1000);
            await LoadingHelper.HideAsync();
            
            UpdateStatus("多步骤加载演示完成");
        }

        private async void ShowFastSwitch_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("演示快速切换效果");
            
            LoadingHelper.SetOwner(this);
            
            for (int i = 0; i < 5; i++)
            {
                await LoadingHelper.ShowAsync($"快速切换 {i + 1}", "测试动画性能");
                await Task.Delay(500);
                await LoadingHelper.HideAsync();
                await Task.Delay(200);
            }
            
            UpdateStatus("快速切换演示完成");
        }

        #endregion

        #region 实际场景演示

        private async void SimulateFileUpload_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("模拟文件上传场景");
            
            LoadingHelper.SetOwner(this);
            await LoadingHelper.ShowAsync("准备上传", "正在检查文件...");
            
            await Task.Delay(500);
            LoadingHelper.UpdateMessage("开始上传", "连接到服务器...");
            
            for (int progress = 0; progress <= 100; progress += 10)
            {
                await Task.Delay(200);
                LoadingHelper.UpdateMessage($"上传中 {progress}%", $"已上传 {progress}% 的内容");
            }
            
            LoadingHelper.UpdateMessage("上传完成", "文件已成功上传到服务器！");
            await Task.Delay(1000);
            
            await LoadingHelper.HideAsync();
            UpdateStatus("文件上传模拟完成");
        }

        private async void SimulateDataSync_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("模拟数据同步场景");
            
            LoadingHelper.SetOwner(this);
            await LoadingHelper.ShowAsync("同步数据", "正在连接到云端...");
            
            await Task.Delay(800);
            LoadingHelper.UpdateMessage("下载更新", "正在获取最新数据...");
            
            await Task.Delay(1000);
            LoadingHelper.UpdateMessage("处理数据", "正在合并本地和远程数据...");
            
            await Task.Delay(1200);
            LoadingHelper.UpdateMessage("应用更改", "正在保存到本地数据库...");
            
            await Task.Delay(800);
            LoadingHelper.UpdateMessage("同步完成", "所有数据已成功同步！");
            
            await Task.Delay(1000);
            await LoadingHelper.HideAsync();
            
            UpdateStatus("数据同步模拟完成");
        }

        private async void SimulateNetworkRequest_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("模拟网络请求场景");
            
            try
            {
                LoadingHelper.SetOwner(this);
                await LoadingHelper.ShowAsync("网络请求", "正在连接服务器...");
                
                // 模拟网络延迟
                await Task.Delay(1000);
                LoadingHelper.UpdateMessage("发送请求", "正在传输数据包...");
                
                await Task.Delay(800);
                LoadingHelper.UpdateMessage("等待响应", "服务器正在处理请求...");
                
                await Task.Delay(1200);
                LoadingHelper.UpdateMessage("接收数据", "正在下载响应内容...");
                
                await Task.Delay(600);
                LoadingHelper.UpdateMessage("请求完成", "数据传输成功！");
                
                await Task.Delay(800);
            }
            catch (Exception ex)
            {
                LoadingHelper.UpdateMessage("请求失败", $"错误：{ex.Message}");
                await Task.Delay(2000);
            }
            finally
            {
                await LoadingHelper.HideAsync();
                UpdateStatus("网络请求模拟完成");
            }
        }

        private async void SimulateSystemInit_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("模拟系统初始化场景");
            
            LoadingHelper.SetOwner(this);
            
            var initSteps = new[]
            {
                ("系统启动", "正在初始化核心模块..."),
                ("加载驱动", "正在加载设备驱动程序..."),
                ("检查硬件", "正在检测系统硬件配置..."),
                ("启动服务", "正在启动系统服务..."),
                ("加载界面", "正在加载用户界面..."),
                ("准备就绪", "系统初始化完成！")
            };
            
            await LoadingHelper.ShowAsync(initSteps[0].Item1, initSteps[0].Item2);
            
            for (int i = 1; i < initSteps.Length; i++)
            {
                await Task.Delay(1000);
                LoadingHelper.UpdateMessage(initSteps[i].Item1, initSteps[i].Item2);
            }
            
            await Task.Delay(1000);
            await LoadingHelper.HideAsync();
            
            UpdateStatus("系统初始化模拟完成");
        }

        #endregion

        #region 主题切换演示

        private async void SwitchToDark_Click(object sender, RoutedEventArgs e)
        {
            // 这里应该调用实际的主题切换逻辑
            // UiApplication.SetTheme(ThemeType.Dark);
            
            UpdateThemeStatus("Dark");
            UpdateStatus("已切换到深色主题");
            
            // 演示主题切换后的效果
            LoadingHelper.SetOwner(this);
            LoadingHelper.Show("深色主题", "Loading 窗口已适配深色主题");
            
            await Task.Delay(2000);
            LoadingHelper.Hide();
        }

        private async void SwitchToLight_Click(object sender, RoutedEventArgs e)
        {
            // 这里应该调用实际的主题切换逻辑
            // UiApplication.SetTheme(ThemeType.Light);
            
            UpdateThemeStatus("Light");
            UpdateStatus("已切换到浅色主题");
            
            // 演示主题切换后的效果
            LoadingHelper.SetOwner(this);
            LoadingHelper.Show("浅色主题", "Loading 窗口已适配浅色主题");
            
            await Task.Delay(2000);
            LoadingHelper.Hide();
        }

        private async void ShowThemeSwitch_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("演示主题切换加载");
            
            LoadingHelper.SetOwner(this);
            await LoadingHelper.ShowAsync("切换主题", "正在应用新的主题设置...");
            
            await Task.Delay(1000);
            LoadingHelper.UpdateMessage("重新加载样式", "正在更新界面颜色...");
            
            await Task.Delay(1000);
            LoadingHelper.UpdateMessage("主题切换完成", "新主题已成功应用！");
            
            await Task.Delay(1000);
            await LoadingHelper.HideAsync();
            
            UpdateStatus("主题切换演示完成");
        }

        #endregion

        #region 自定义配置演示

        private async void ShowNoTextPulse_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("演示禁用文本脉动动画");
            
            var customWindow = new LoadingWindow("无文本动画", "观察静态文本效果");
            customWindow.EnableTextPulse = false;
            customWindow.SetOwner(this);
            
            customWindow.ShowWithAnimation();
            
            await Task.Delay(3000);
            customWindow.HideWithAnimation();
            
            UpdateStatus("禁用文本动画演示完成");
        }

        private async void ShowNoRingPulse_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("演示禁用环形脉动动画");
            
            var customWindow = new LoadingWindow("无环形动画", "观察静态进度环效果");
            customWindow.EnableRingPulse = false;
            customWindow.SetOwner(this);
            
            customWindow.ShowWithAnimation();
            
            await Task.Delay(3000);
            customWindow.HideWithAnimation();
            
            UpdateStatus("禁用环形动画演示完成");
        }

        private async void ShowStatic_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("演示完全静态显示");
            
            var customWindow = new LoadingWindow("完全静态", "无任何脉动动画效果");
            customWindow.EnableTextPulse = false;
            customWindow.EnableRingPulse = false;
            customWindow.SetOwner(this);
            
            customWindow.ShowWithAnimation();
            
            await Task.Delay(3000);
            customWindow.HideWithAnimation();
            
            UpdateStatus("静态显示演示完成");
        }

        private async void ShowCustom_Click(object sender, RoutedEventArgs e)
        {
            string message = CustomMessageTextBox.Text;
            string subtitle = CustomSubtitleTextBox.Text;
            
            UpdateStatus($"显示自定义消息: {message}");
            
            LoadingHelper.SetOwner(this);
            LoadingHelper.Show(message, subtitle);
            
            await Task.Delay(3000);
            LoadingHelper.Hide();
            
            UpdateStatus("自定义消息演示完成");
        }

        #endregion

        #region 性能测试

        private async void PerformanceTest_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("开始性能测试：快速显示隐藏");
            
            LoadingHelper.SetOwner(this);
            
            var startTime = DateTime.Now;
            
            for (int i = 0; i < 10; i++)
            {
                await LoadingHelper.ShowAsync($"性能测试 {i + 1}/10");
                await Task.Delay(100);
                await LoadingHelper.HideAsync();
                await Task.Delay(50);
            }
            
            var endTime = DateTime.Now;
            var duration = endTime - startTime;
            
            UpdateStatus($"性能测试完成，耗时: {duration.TotalMilliseconds:F0}ms");
        }

        private async void MemoryTest_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("开始内存泄漏测试");
            
            for (int i = 0; i < 20; i++)
            {
                var testWindow = new LoadingWindow($"内存测试 {i + 1}", "创建和销毁窗口实例");
                testWindow.SetOwner(this);
                testWindow.ShowWithAnimation();
                
                await Task.Delay(100);
                testWindow.CloseWithAnimation();
                
                // 强制垃圾回收
                if (i % 5 == 0)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            
            UpdateStatus("内存泄漏测试完成，建议观察内存使用情况");
        }

        private async void AsyncStressTest_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("开始异步压力测试");
            
            LoadingHelper.SetOwner(this);
            
            // 并发显示和隐藏操作
            var tasks = new Task[5];
            
            for (int i = 0; i < tasks.Length; i++)
            {
                int index = i;
                tasks[i] = Task.Run(async () =>
                {
                    for (int j = 0; j < 3; j++)
                    {
                        await LoadingHelper.ShowAsync($"并发测试 {index + 1}-{j + 1}");
                        await Task.Delay(200 + index * 50);
                        await LoadingHelper.HideAsync();
                        await Task.Delay(100);
                    }
                });
            }
            
            await Task.WhenAll(tasks);
            UpdateStatus("异步压力测试完成");
        }

        #endregion

        #region 辅助方法

        private void UpdateStatus(string message)
        {
            Dispatcher.Invoke(() =>
            {
                StatusText.Text = $"{DateTime.Now:HH:mm:ss} - {message}";
            });
        }

        private void UpdateThemeStatus(string theme = "Auto")
        {
            Dispatcher.Invoke(() =>
            {
                ThemeText.Text = theme;
            });
        }

        private void UpdateLoadingStatus()
        {
            // 这里可以添加定时器来监控 Loading 状态
            Dispatcher.Invoke(() =>
            {
                LoadingStatusText.Text = LoadingHelper.IsShowing ? "显示中" : "隐藏";
            });
        }

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            // 确保所有 Loading 窗口都被关闭
            LoadingHelper.Hide();
            customLoadingWindow?.Close();
            base.OnClosed(e);
        }
    }
}