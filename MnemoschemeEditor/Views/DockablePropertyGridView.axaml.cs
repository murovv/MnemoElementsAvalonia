using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using DynamicData.Binding;
using MnemoschemeEditor._PropertyGrid;
using MnemoschemeEditor.ViewModels;

namespace MnemoschemeEditor.Views;

public partial class DockablePropertyGridView : UserControl
{
    public DockablePropertyGridView()
    {
        InitializeComponent();
        var source = (((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime)
                                .MainWindow.DataContext as MainWindowViewModel).SelectedMnemoElements;
        var propertyGrid = this.FindControl<PropertyGrid>("propertyGrid");
        propertyGrid.SelectedObjects = source;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}