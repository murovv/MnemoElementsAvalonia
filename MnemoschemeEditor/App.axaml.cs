using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Dock.Model.Core;
using MnemoschemeEditor.Models;
using MnemoschemeEditor.ViewModels;
using MnemoschemeEditor.Views;

namespace MnemoschemeEditor
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var factory = new MainDocFactory(null);
            var layout = factory.CreateLayout();
            factory.InitLayout(layout);

            var mainWindowViewModel = new MainWindowViewModel()
            {
                Factory = factory,
                Layout = layout
            };

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                
                var mainWindow = new MainWindow()
                {
                    DataContext = mainWindowViewModel
                };

                mainWindow.Closing += (_, _) =>
                {
                    if (layout is IDock dock)
                    {
                        if (dock.Close.CanExecute(null))
                        {
                            dock.Close.Execute(null);
                        }
                    }
                };

                desktopLifetime.MainWindow = mainWindow;

                desktopLifetime.Exit += (_, _) =>
                {
                    if (layout is IDock dock)
                    {
                        if (dock.Close.CanExecute(null))
                        {
                            dock.Close.Execute(null);
                        }
                    }
                };
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}