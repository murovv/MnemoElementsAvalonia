using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvAp2.Models;
using AvAp2.ViewModels;
using AvAp2.Views;
using AvAp2.Views.TestViews;
using MainWindow = AvAp2.Views.MainWindow;

namespace AvAp2
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            BlinkAnimationController.GetInstance();
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow()
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
