﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Dock.Avalonia.Controls;
using Dock.Model.Mvvm.Controls;
using MnemoschemeEditor.Models;
using MnemoschemeEditor.ViewModels;

namespace MnemoschemeEditor.Views;

public partial class DockableMnemoSchemeSelectorView : UserControl
{
    public DockableMnemoSchemeSelectorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox listBox && listBox.SelectedItem is IMnemoscheme mnemoscheme)
        {
            var mainWindow =
                (((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime)
                    .MainWindow.DataContext as MainWindowViewModel);
            mainWindow.CurrentMnemo = mnemoscheme.GetMnemoscheme();
            mainWindow.CurrentMnemo.Background = new ImageBrush(
                new Bitmap(@"C:\Users\murov\RiderProjects\AvaloniaApplication1\MnemoschemeEditor\Assets\plate.png"))
            {
                Stretch = Stretch.Fill,
                DestinationRect = new RelativeRect(0, 0, 15, 15, RelativeUnit.Absolute),
                TileMode = TileMode.FlipXY
            };
        }
    }
}