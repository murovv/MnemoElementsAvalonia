using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MnemoschemeEditor.Views;

public partial class DockablePropertyGridView : UserControl
{
    public DockablePropertyGridView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}