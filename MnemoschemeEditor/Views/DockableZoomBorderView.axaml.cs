using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.PanAndZoom;
using Avalonia.ExtendedToolkit.Extensions;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using AvAp2.Interfaces;
using AvAp2.Models.BaseClasses;
using AvAp2.Models.Controls;
using MnemoschemeEditor.ViewModels;
using ReactiveUI;

namespace MnemoschemeEditor.Views;

public partial class DockableZoomBorderView : ReactiveUserControl<DockableZoomBorderViewModel>
{
    private Point ModifyStartPoint { get; set; }
    private bool ModifyPressed { get; set; }
    
    public Interaction<VideoSettingsViewModel, DockableZoomBorderViewModel> ShowVideoSettings { get; }
    public ICommand ShowVideoSettingsCommand { get; }
    public DockableZoomBorderView()
    {
        InitializeComponent();
        ShowVideoSettings = new Interaction<VideoSettingsViewModel, DockableZoomBorderViewModel>();
        ShowVideoSettingsCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var settings = new VideoSettingsViewModel();
            var result = await ShowVideoSettings.Handle(settings);
        });
        this.WhenActivated(d => d(ShowVideoSettings.RegisterHandler((DoShowVideoSettingsAsync))));
        var mainWindow =
            (((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime)
                .MainWindow.DataContext as MainWindowViewModel);
        var zoomBorder = this.Find<ZoomBorder>("ZoomBorder");
        zoomBorder.Bind(ZoomBorder.ChildProperty, mainWindow.WhenAnyValue(x => x.CurrentMnemo));
        mainWindow.WhenAnyValue(x => x.CurrentMnemo).Subscribe(OnNext);
    }

    private async Task DoShowVideoSettingsAsync(
        InteractionContext<VideoSettingsViewModel, DockableZoomBorderViewModel> interaction)
    {
        var settings = new VideoSettingsWindow();
        settings.DataContext = interaction.Input;
        
        var mainWindow =
            (((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime)
                .MainWindow as MainWindow);
        var result = await settings.ShowDialog<DockableZoomBorderViewModel>(mainWindow);
        interaction.SetOutput(result);
    }

    private void OnNext(Canvas obj)
    {
        var window = (((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime)
            .MainWindow.DataContext as MainWindowViewModel);
        if (window!=null) 
            window.SelectedMnemoElements.Clear();
        obj.PointerPressed += CanvasOnPointerPressed;
        obj.Children.ForEach(x =>
        {
            ((Panel)x).PointerPressed += PanelOnPointerPressed;
            ((Panel)x).PointerMoved += PanelOnPointerMoved;
        });
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        ModifyPressed = false;
        base.OnPointerReleased(e);
    }



    private void CanvasOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var mouseButton = e.GetCurrentPoint(sender as Canvas).Properties.PointerUpdateKind;
        if (mouseButton == PointerUpdateKind.LeftButtonPressed)
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
            panel.PointerPressed += PanelOnPointerPressed;
            panel.PointerMoved += PanelOnPointerMoved;
            if (Activator.CreateInstance(selectedItem) is BasicMnemoElement control)
            {
                if (control is BasicEquipment equipment)
                {
                    if (equipment is CLine cLine)
                    {
                        cLine.ControlISSelected = true;
                    }
                    equipment.VoltageEnum = voltage;
                }

                if (control is IVideo video)
                {
                    panel.ContextMenu = new ContextMenu()
                    {
                        Items = new []{new MenuItem()
                        {
                            Header = "Настройки видеонаблюдения",
                            Command = ShowVideoSettingsCommand,

                        }},
                    };
                }
                panel.Children.Add(control);
                canvas.Children.Add(panel);
                
            }
            Canvas.SetTop(panel, e.GetPosition(canvas).Y - e.GetPosition(canvas).Y % 30);
            Canvas.SetLeft(panel, e.GetPosition(canvas).X - e.GetPosition(canvas).X % 30);
        }
    }

    private void PanelOnPointerMoved(object? sender, PointerEventArgs e)
    {
        bool changed = false;
        Canvas canvas = null;
        if (ModifyPressed)
        {
            var window = this.FindAncestorOfType<Window>();
            foreach (var mnemoElement in ((MainWindowViewModel)window.DataContext).SelectedMnemoElements)
            {
                canvas = mnemoElement.FindAncestorOfType<Canvas>();
                var dx = e.GetPosition(mnemoElement.FindAncestorOfType<Canvas>()).X - ModifyStartPoint.X;
                var dy = e.GetPosition(mnemoElement.FindAncestorOfType<Canvas>()).Y - ModifyStartPoint.Y;
                var x = Canvas.GetLeft(mnemoElement.Parent as AvaloniaObject);
                var y = Canvas.GetTop(mnemoElement.Parent as AvaloniaObject);
                if (Math.Floor(x / 30) != Math.Floor((x + dx) / 30) ||
                    Math.Floor(y / 30) != Math.Floor((y + dy) / 30))
                {
                    Canvas.SetLeft(mnemoElement.Parent as AvaloniaObject, Math.Floor((x + dx) / 30) * 30);
                    Canvas.SetTop(mnemoElement.Parent as AvaloniaObject, Math.Floor((y + dy) / 30) * 30);
                    changed = true;
                }
            }
        }

        if (changed)
            ModifyStartPoint = new Point(Canvas.GetLeft(sender as Panel), Canvas.GetTop(sender as Panel));
    }

    private void PanelOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var mnemoElement = (sender as Panel).Children[0] as BasicMnemoElement;
        var window = this.FindAncestorOfType<Window>();
        ModifyStartPoint = e.GetPosition(mnemoElement.FindAncestorOfType<Canvas>());
        ModifyPressed = true;
        if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
        {
            if (!mnemoElement.ControlISSelected && e.KeyModifiers == KeyModifiers.None)
            {
                mnemoElement.ControlISSelected = true;
                ((MainWindowViewModel)window.DataContext).SelectedMnemoElements.Add(mnemoElement);
            }
            else if (mnemoElement.ControlISSelected && e.KeyModifiers == KeyModifiers.Control)
            {
                ((MainWindowViewModel)window.DataContext).SelectedMnemoElements.Remove(mnemoElement);
                mnemoElement.ControlISSelected = false;
            }
        }else if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed)
        {
            (sender as Panel).ContextMenu?.Open();
        }

        e.Handled = true;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}