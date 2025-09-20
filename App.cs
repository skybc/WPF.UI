using System.Windows;

namespace SimpleSymbolIconTest
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var window = new SimpleSymbolIconTest();
            window.Show();
        }
    }
}