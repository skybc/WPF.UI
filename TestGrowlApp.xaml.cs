using System.Windows;
using Wpf.Ui.Controls;

public partial class TestGrowlApp : Window
{
    private GrowlItem testItem;

    public TestGrowlApp()
    {
        InitializeComponent();
    }

    private void InfoButton_Click(object sender, RoutedEventArgs e)
    {
        var item = new GrowlItem
        {
            Type = GrowlType.Info,
            Message = "这是一个信息消息",
            Time = System.DateTime.Now,
            ShowDateTime = true
        };
        GrowlPanel.Show(item);
    }

    private void SuccessButton_Click(object sender, RoutedEventArgs e)
    {
        var item = new GrowlItem
        {
            Type = GrowlType.Success,
            Message = "这是一个成功消息",
            Time = System.DateTime.Now,
            ShowDateTime = true
        };
        GrowlPanel.Show(item);
    }

    private void WarningButton_Click(object sender, RoutedEventArgs e)
    {
        var item = new GrowlItem
        {
            Type = GrowlType.Warning,
            Message = "这是一个警告消息",
            Time = System.DateTime.Now,
            ShowDateTime = true
        };
        GrowlPanel.Show(item);
    }

    private void ErrorButton_Click(object sender, RoutedEventArgs e)
    {
        var item = new GrowlItem
        {
            Type = GrowlType.Error,
            Message = "这是一个错误消息",
            Time = System.DateTime.Now,
            ShowDateTime = true
        };
        GrowlPanel.Show(item);
    }

    private void ChangeTypeButton_Click(object sender, RoutedEventArgs e)
    {
        if (testItem == null)
        {
            testItem = new GrowlItem
            {
                Type = GrowlType.Info,
                Message = "这是一个会变化类型的消息",
                Time = System.DateTime.Now,
                ShowDateTime = true
            };
            GrowlPanel.Show(testItem);
        }
        else
        {
            // 循环切换类型
            switch (testItem.Type)
            {
                case GrowlType.Info:
                    testItem.Type = GrowlType.Success;
                    testItem.Message = "现在是成功类型";
                    break;
                case GrowlType.Success:
                    testItem.Type = GrowlType.Warning;
                    testItem.Message = "现在是警告类型";
                    break;
                case GrowlType.Warning:
                    testItem.Type = GrowlType.Error;
                    testItem.Message = "现在是错误类型";
                    break;
                case GrowlType.Error:
                    testItem.Type = GrowlType.Info;
                    testItem.Message = "现在又是信息类型";
                    break;
            }
        }
    }
}