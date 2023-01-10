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
    private Point ModifyStartPoint { get; set; }
    private bool ModifyPressed { get; set; }
    
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

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        ModifyPressed = false;
        base.OnPointerReleased(e);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        Canvas canvas = null;
        if (ModifyPressed)
        {
            var window = this.FindAncestorOfType<Window>();
            foreach (var mnemoElement in ((MainWindowViewModel)window.DataContext).SelectedMnemoElements)
            {
                canvas = mnemoElement.FindAncestorOfType<Canvas>();
                var dx = e.GetPosition(mnemoElement.FindAncestorOfType<Canvas>()).X - ModifyStartPoint.X;
                var dy = e.GetPosition(mnemoElement.FindAncestorOfType<Canvas>()).Y - ModifyStartPoint.Y;
                var x = Canvas.GetLeft(mnemoElement.Parent as AvaloniaObject) + dx;
                var y = Canvas.GetTop(mnemoElement.Parent as AvaloniaObject) + dy;
                Canvas.SetTop(mnemoElement.Parent as AvaloniaObject, y);
                Canvas.SetLeft(mnemoElement.Parent as AvaloniaObject, x);
            }
        }

        ModifyStartPoint = e.GetPosition(canvas);
        base.OnPointerMoved( e);
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
        Panel panel = new Panel()
        {
            Width = 30,
            Height = 30,
        };
        panel.PointerPressed+= PanelOnPointerPressed;
        BasicEquipment control = Activator.CreateInstance(selectedItem) as BasicEquipment;
        panel.Children.Add(control);
        control.VoltageEnum = voltage;
        canvas.Children.Add(panel);
        Canvas.SetTop(panel, e.GetPosition(canvas).Y - e.GetPosition(canvas).Y%30);
        Canvas.SetLeft(panel, e.GetPosition(canvas).X - e.GetPosition(canvas).X%30);
    }

    private void PanelOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var mnemoElement = (sender as Panel).Children[0] as BasicMnemoElement;
        var window = this.FindAncestorOfType<Window>();
        ModifyStartPoint = e.GetPosition(mnemoElement.FindAncestorOfType<Canvas>());
        ModifyPressed = true;
        if (!mnemoElement.ControlISSelected && e.KeyModifiers == KeyModifiers.None)
        {
            mnemoElement.ControlISSelected = true;
            ((MainWindowViewModel)window.DataContext).SelectedMnemoElements.Add(mnemoElement);
        }else if (mnemoElement.ControlISSelected && e.KeyModifiers == KeyModifiers.Control)
        {
            mnemoElement.ControlISSelected = false;
            ((MainWindowViewModel)window.DataContext).SelectedMnemoElements.Remove(mnemoElement);
        }
        e.Handled = true;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}