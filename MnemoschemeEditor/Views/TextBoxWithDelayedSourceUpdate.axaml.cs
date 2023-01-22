using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace MnemoschemeEditor.Views;

public partial class TextBoxWithDelayedSourceUpdate : TextBox, IStyleable
{
    Type IStyleable.StyleKey => typeof(TextBox);
    public TextBoxWithDelayedSourceUpdate()
    {
        InitializeComponent();
        
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}