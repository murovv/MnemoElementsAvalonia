using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MnemoschemeEditor.Views;

public partial class MnemoscemePanel : UserControl
{
    public MnemoscemePanel()
    {
        InitializeComponent();
        
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}