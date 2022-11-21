using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvAp2;
using AvAp2.Models;

namespace PanAndZoomTesting.Views;

public partial class ZoomPanel : UserControl
{
    public ZoomPanel()
    {
        InitializeComponent();
        
        
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}