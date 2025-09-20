using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Controls;
using Wpf.Ui.Services;

namespace LoadingTestApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑.
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoadingWindow directLoadingWindow;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        #region 基本操作事件处理

        private async void ShowBasicLoading_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("显示基本加载窗口...");
            
            LoadingHelper.Show("加载中，请稍候...");
            
            // 模拟 3 秒操作
            await Task.Delay(3000);
            
            LoadingHelper.Hide();
            UpdateStatus("基本加载完成");
        }

        private void ShowCustomMessage_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("显示自定义消息加载窗口");
            LoadingHelper.Show("正在处理您的请求，请耐心等待...");
        }

        private void HideLoading_Click(object sender, RoutedEventArgs e)
        {
            LoadingHelper.Hide();
            UpdateStatus("手动隐藏加载窗口");
        }

        private void UpdateMessage_Click(object sender, RoutedEventArgs e)
        {
            LoadingHelper.UpdateMessage("消息已更新：" + DateTime.Now.ToString("HH:mm:ss"));
            UpdateStatus("更新加载消息");
        }

        private void SetOwner_Click(object sender, RoutedEventArgs e)
        {
            LoadingHelper.SetOwner(this);
            LoadingHelper.Show("已设置父窗口");
            UpdateStatus("设置当前窗口为父窗口");
        }

        #endregion

        #region 异步操作演示

        private async void SimulateDataLoading_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("开始模拟数据加载...");
            
            await LoadingHelper.ExecuteWithLoadingAsync(async () =>
            {
                // 模拟数据加载过程
                await Task.Delay(1000);
                LoadingHelper.UpdateMessage("正在连接数据库...");
                
                await Task.Delay(1000);
                LoadingHelper.UpdateMessage("正在查询数据...");
                
                await Task.Delay(1000);
                LoadingHelper.UpdateMessage("正在处理数据...");
                
                await Task.Delay(1000);
                
            }, "开始加载数据...");
            
            UpdateStatus("数据加载完成");
        }

        private async void SimulateFileUpload_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("开始模拟文件上传...");
            
            var result = await LoadingHelper.ExecuteWithLoadingAsync(async () =>
            {
                // 模拟文件上传过程
                for (int i = 0; i <= 100; i += 20)
                {
                    LoadingHelper.UpdateMessage($"上传进度: {i}%");
                    await Task.Delay(500);
                }
                
                return "upload_success.jpg";
                
            }, "正在上传文件...");
            
            UpdateStatus($"文件上传完成: {result}");
        }

        private async void SimulateNetworkRequest_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("开始模拟网络请求...");
            
            try
            {
                await LoadingHelper.ShowAsync("正在连接服务器...");
                
                await Task.Delay(1000);
                LoadingHelper.UpdateMessage("正在发送请求...");
                
                await Task.Delay(1000);
                LoadingHelper.UpdateMessage("正在等待响应...");
                
                await Task.Delay(1000);
                LoadingHelper.UpdateMessage("正在处理响应数据...");
                
                await Task.Delay(1000);
                
                await LoadingHelper.HideAsync();
                UpdateStatus("网络请求完成");
            }
            catch (Exception ex)
            {
                await LoadingHelper.HideAsync();
                UpdateStatus($"网络请求失败: {ex.Message}");
            }
        }

        private async void ExecuteWithLoadingAsync_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("ExecuteWithLoadingAsync 演示...");
            
            var result = await LoadingHelper.ExecuteWithLoadingAsync(async () =>
            {
                await Task.Delay(2000);
                return $"操作完成于: {DateTime.Now}";
                
            }, "使用 ExecuteWithLoadingAsync 执行操作...");
            
            UpdateStatus($"ExecuteWithLoadingAsync 结果: {result}");
        }

        private async void MultiStepLoading_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("开始多步骤加载演示...");
            
            string[] steps = {
                "初始化系统...",
                "加载配置文件...",
                "连接数据库...",
                "验证用户权限...",
                "加载用户数据...",
                "准备用户界面...",
                "完成加载"
            };
            
            await LoadingHelper.ShowAsync(steps[0]);
            
            for (int i = 1; i < steps.Length; i++)
            {
                await Task.Delay(800);
                LoadingHelper.UpdateMessage(steps[i]);
            }
            
            await Task.Delay(500);
            await LoadingHelper.HideAsync();
            
            UpdateStatus("多步骤加载完成");
        }

        #endregion

        #region 直接控件演示

        private void CreateNewWindow_Click(object sender, RoutedEventArgs e)
        {
            this.directLoadingWindow = new LoadingWindow("直接创建的加载窗口");
            this.directLoadingWindow.SetOwner(this);
            UpdateStatus("创建了新的 LoadingWindow 实例");
        }

        private void ShowWindowInstance_Click(object sender, RoutedEventArgs e)
        {
            if (this.directLoadingWindow == null)
            {
                CreateNewWindow_Click(sender, e);
            }
            
            this.directLoadingWindow?.ShowWithAnimation();
            UpdateStatus("显示 LoadingWindow 实例");
        }

        private void HideWindowInstance_Click(object sender, RoutedEventArgs e)
        {
            this.directLoadingWindow?.HideWithAnimation();
            UpdateStatus("隐藏 LoadingWindow 实例");
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

        #endregion
    }

    /// <summary>
    /// MainWindow 的 ViewModel.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private string customMessage = "MVVM 绑定消息";
        private bool isLoading;

        public string CustomMessage
        {
            get => this.customMessage;
            set
            {
                this.customMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => this.isLoading;
            private set
            {
                this.isLoading = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowLoadingCommand { get; }
        public ICommand HideLoadingCommand { get; }
        public ICommand UpdateMessageCommand { get; }

        public MainViewModel()
        {
            ShowLoadingCommand = new RelayCommand(async () =>
            {
                IsLoading = true;
                await LoadingHelper.ShowAsync("通过 MVVM 显示的加载窗口");
            });

            HideLoadingCommand = new RelayCommand(() =>
            {
                LoadingHelper.Hide();
                IsLoading = false;
            });

            UpdateMessageCommand = new RelayCommand(() =>
            {
                LoadingHelper.UpdateMessage(CustomMessage);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// 简单的 RelayCommand 实现.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            this.execute();
        }
    }
}