using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.ExtendedToolkit.Extensions;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using AvAp2.Models.BaseClasses;
using DynamicData;
using MnemoschemeEditor.ViewModels;

namespace MnemoschemeEditor.Views;

public partial class DockableZoomBorderView : UserControl
{
    
    public DockableZoomBorderView()
    {
        InitializeComponent();
        var canvas = this.Find<Canvas>("Canvas1");
        for (int i = 0; i < canvas.Width; i+=30)
        {
            for (int j = 0; j < canvas.Height; j+=30)
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Height = 3;
                ellipse.Width = 3;
                ellipse.Fill = Brushes.White;
                canvas.Children.Add(ellipse);
                Canvas.SetLeft(ellipse, j);
                Canvas.SetTop(ellipse, i);
            }
        }
        canvas.PointerPressed+= CanvasOnPointerPressed;
        
    }

    private void CanvasOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var window = this.FindAncestorOfType<Window>();
        var selectedItem = ((MainWindowViewModel)window.DataContext).SelectedMnemoElement;
        if (sender is not Canvas || selectedItem == null)
        {
            return;
        }

        var canvas = sender as Canvas;
        var voltage = ((MainWindowViewModel)window.DataContext).SelectedVoltage;
        BasicEquipment control = Activator.CreateInstance(selectedItem) as BasicEquipment;
        control.VoltageEnum = voltage;
        canvas.Children.Add(control);
        Canvas.SetTop(control, e.GetPosition(canvas).Y);
        Canvas.SetLeft(control, e.GetPosition(canvas).X);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}