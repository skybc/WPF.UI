using System.Windows;
using Wpf.Ui.Controls;

namespace TestApp
{
    public partial class GrowlTestWindow : Window
    {
        public GrowlTestWindow()
        {
            InitializeComponent();
        }

        private void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            GrowlPanel.Show("这是一条信息消息", "信息", GrowlType.Info);
        }

        private void ShowSuccess_Click(object sender, RoutedEventArgs e)
        {
            GrowlPanel.Show("操作成功完成！", "成功", GrowlType.Success);
        }

        private void ShowWarning_Click(object sender, RoutedEventArgs e)
        {
            GrowlPanel.Show("请注意这个警告", "警告", GrowlType.Warning);
        }

        private void ShowError_Click(object sender, RoutedEventArgs e)
        {
            GrowlPanel.Show("发生了一个错误", "错误", GrowlType.Error);
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            // 清除所有消息的方法需要在GrowlControl中实现
            // 暂时通过创建新的实例来清除
        }
    }
}