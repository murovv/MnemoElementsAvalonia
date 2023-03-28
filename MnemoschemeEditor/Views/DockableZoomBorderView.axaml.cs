using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Controls.Shapes;
using Avalonia.ExtendedToolkit.Extensions;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using AvAp2.Interfaces;
using AvAp2.Models.BaseClasses;
using AvAp2.Models.Controls;
using DynamicData;
using MnemoschemeEditor.ViewModels;
using ReactiveUI;

namespace MnemoschemeEditor.Views;

public partial class DockableZoomBorderView : ReactiveUserControl<DockableZoomBorderViewModel>
{
    private Point ModifyStartPoint { get; set; }
    private bool ModifyPressed { get; set; }
    private bool SelectionPressed { get; set; }
    private Rectangle SelectionRect { get; set; }
    private bool IsCLineDrawing { get; set; }
    private CLine CurrentLine { get; set; }
    
    private Point ModifyCLineStartPoint { get; set; }
    
    private bool IsCRectangleDrawing { get; set; }
    
    private CRectangle CurrentRectangle { get; set; }
    
    private Point ModifyCRectangleStartPoint { get; set; }
    private List<IDisposable> bindings = new List<IDisposable>();
    
    public Interaction<VideoSettingsViewModel, VideoSettingsViewModel> ShowVideoSettings { get; }

    public DockableZoomBorderView()
    {
        InitializeComponent();
        ShowVideoSettings = new Interaction<VideoSettingsViewModel, VideoSettingsViewModel>();
        this.WhenActivated(d => d(ShowVideoSettings.RegisterHandler((DoShowVideoSettingsAsync))));
        var mainWindow =
            (((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime)
                .MainWindow.DataContext as MainWindowViewModel);
        var zoomBorder = this.Find<ZoomBorder>("ZoomBorder");
        bindings.Add(zoomBorder.Bind(ZoomBorder.ChildProperty, mainWindow.WhenAnyValue(x => x.CurrentMnemo)));
        
        mainWindow.WhenAnyValue(x => x.CurrentMnemo).Buffer(2)
            .Select(b => (Previous: b[0], Current: b[1])).Subscribe(OnNext);
        SubscribeMnemo(mainWindow.CurrentMnemo);
    }

    private void OnNext((Canvas Previous, Canvas Current) obj)
    {
        var window = (((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime)
            .MainWindow.DataContext as MainWindowViewModel);
        if (window != null)
            window.SelectedMnemoElements.Clear();
        UnSubscribeMnemo(obj.Previous);
        SubscribeMnemo(obj.Current);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        bindings.ForEach(x=>x.Dispose());
        UnSubscribeMnemo((GetWindow().DataContext as MainWindowViewModel).CurrentMnemo);
        base.OnDetachedFromVisualTree(e);
    }

    ~DockableZoomBorderView()
    {
          
    }

    private ICommand InitShowVideoSettingsCommand(BasicEquipment control)
    {
        return ReactiveCommand.CreateFromTask(async () =>
        {
            var settings = new VideoSettingsViewModel()
            {
                VideoLogin = control.VideoLogin,
                VideoPassword = control.VideoPassword,
                VideoChannelPTZ = control.VideoChannelPTZ
            };
            var result = await ShowVideoSettings.Handle(settings);
            control.VideoLogin = result.VideoLogin;
            control.VideoPassword = result.VideoPassword;
            control.VideoChannelPTZ = result.VideoChannelPTZ;
        });
    }

    private async Task DoShowVideoSettingsAsync(
        InteractionContext<VideoSettingsViewModel, VideoSettingsViewModel> interaction)
    {
        var settings = new VideoSettingsWindow();
        settings.DataContext = interaction.Input;

        var mainWindow =
            (((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime)
                .MainWindow as MainWindow);
        var result = await settings.ShowDialog<VideoSettingsViewModel>(mainWindow);
        interaction.SetOutput(result);
    }
    

    private void CanvasOnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (SelectionPressed)
        {
            SelectionPressed = false;
            var canvas = sender as Canvas;
            canvas.Children.Remove(SelectionRect);
            canvas.Children
                .Where(x => SelectionRect.Bounds
                    .Contains(new Point(Canvas.GetLeft((AvaloniaObject)x), Canvas.GetTop((AvaloniaObject)x))))
                .Select(x => (x as Panel).Children[0] as BasicMnemoElement)
                .ToList()
                .ForEach(
                    element =>
                    {
                        if (!element.ControlISSelected)
                        {
                            var window = GetWindow();
                            element.ControlISSelected = true;
                            (window.DataContext as MainWindowViewModel).SelectedMnemoElements.Add(element);
                        }
                    });
        }else if (IsCLineDrawing)
        {
            if (CurrentLine.CoordinateX2 == 0 && CurrentLine.CoordinateY2 == 0)
            {
                (sender as Canvas).Children.Remove(CurrentLine.Parent);
            }
            else
            {
                var window = GetWindow();
                (window.DataContext as MainWindowViewModel).SelectedMnemoElements.Add(CurrentLine);
                ModifyCLineStartPoint = new Point(0, 0);
            }

            IsCLineDrawing = false;
        }else if (IsCRectangleDrawing)
        {
            if (CurrentRectangle.CoordinateX2 == 0 && CurrentRectangle.CoordinateY2 == 0)
            {
                (sender as Canvas).Children.Remove(CurrentRectangle.Parent);
            }
            else
            {
                var window = GetWindow();
                (window.DataContext as MainWindowViewModel).SelectedMnemoElements.Add(CurrentRectangle);
                ModifyCRectangleStartPoint = new Point(0, 0);
            }

            IsCRectangleDrawing = false;
        }
    }

    private void CanvasOnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (SelectionPressed)
        {
            SelectionRect.Stroke = Brushes.Red;
            SelectionRect.StrokeDashOffset = 1;
            SelectionRect.StrokeThickness = 1;
            var newHeight = e.GetPosition(sender as Canvas).Y - Canvas.GetTop(SelectionRect);
            SelectionRect.Height = (newHeight > 0 ? newHeight : 0);
            var newWidth = e.GetPosition(sender as Canvas).X - Canvas.GetLeft(SelectionRect);
            SelectionRect.Width = (newWidth > 0 ? newWidth : 0);
        }else if (IsCLineDrawing)
        {
            var currentPoint = e.GetPosition(CurrentLine);
            int deltaStep = 30;

            int deltaX = (int)(currentPoint.X - ModifyCLineStartPoint.X) / deltaStep;
            int deltaY = (int)(currentPoint.Y - ModifyCLineStartPoint.Y) / deltaStep;

            if ((Math.Abs(deltaX) > 0) || ((Math.Abs(deltaY) > 0)))
            {
                CurrentLine.CoordinateX2 += (deltaX * deltaStep);
                CurrentLine.CoordinateY2 += (deltaY * deltaStep);
                ModifyCLineStartPoint = new Point(ModifyCLineStartPoint.X + (deltaX * deltaStep), 
                    ModifyCLineStartPoint.Y + (deltaY * deltaStep));
                            
            }
        }else if (IsCRectangleDrawing)
        {
            var currentPoint = e.GetPosition(CurrentRectangle);
            int deltaStep = 30;

            int deltaX = (int)(currentPoint.X - ModifyCRectangleStartPoint.X) / deltaStep;
            int deltaY = (int)(currentPoint.Y - ModifyCRectangleStartPoint.Y) / deltaStep;

            if ((Math.Abs(deltaX) > 0) || ((Math.Abs(deltaY) > 0)))
            {
                CurrentRectangle.CoordinateX2 += (deltaX * deltaStep);
                CurrentRectangle.CoordinateY2 += (deltaY * deltaStep);
                ModifyCRectangleStartPoint = new Point(ModifyCRectangleStartPoint.X + (deltaX * deltaStep), 
                    ModifyCRectangleStartPoint.Y + (deltaY * deltaStep));
                            
            }
        }

        e.Handled = true;
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
            var window = GetWindow();
            var selectedItem = ((MainWindowViewModel)window.DataContext).SelectedMnemoElement;
            if (sender is not Canvas)
            {
                return;
            }

            var canvas = sender as Canvas;
            if (selectedItem == null)
            {
                SelectionPressed = true;
                SelectionRect = new Rectangle()
                {
                    Width = 0,
                    Height = 0,
                };
                canvas.Children.Add(SelectionRect);
                Canvas.SetTop(SelectionRect, e.GetPosition(canvas).Y);
                Canvas.SetLeft(SelectionRect, e.GetPosition(canvas).X);
                return;
            }

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
                if (control is CRectangle)
                {
                    control.ControlISSelected = true;
                    IsCRectangleDrawing = true;
                    CurrentRectangle = control as CRectangle;
                }
                if (control is BasicEquipment equipment)
                {
                    if (equipment is CLine)
                    {
                        control.ControlISSelected = true;
                        IsCLineDrawing = true;
                        CurrentLine = control as CLine;
                    }
                    equipment.VoltageEnum = voltage;
                    panel.ContextMenu = new ContextMenu()
                    {
                        Items = new[]
                        {
                            new MenuItem()
                            {
                                Header = "Настройки видеонаблюдения",
                                Command = InitShowVideoSettingsCommand(equipment),
                            },
                        }
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
            var window = GetWindow();
            canvas = ((MainWindowViewModel)window.DataContext).CurrentMnemo;
            foreach (var mnemoElement in ((MainWindowViewModel)window.DataContext).SelectedMnemoElements)
            {
                var panel = canvas.Children.First(x => ((Panel)x).Children[0] == mnemoElement);
                var dx = e.GetPosition(canvas).X - ModifyStartPoint.X;
                var dy = e.GetPosition(canvas).Y - ModifyStartPoint.Y;
                var x = Canvas.GetLeft((AvaloniaObject)panel);
                var y = Canvas.GetTop((AvaloniaObject)panel);
                if (Math.Floor(x / 30) != Math.Floor((x + dx) / 30) ||
                    Math.Floor(y / 30) != Math.Floor((y + dy) / 30))
                {
                    Canvas.SetLeft((AvaloniaObject)panel, Math.Floor((x + dx) / 30) * 30);
                    Canvas.SetTop((AvaloniaObject)panel, Math.Floor((y + dy) / 30) * 30);
                    changed = true;
                }
            }
        }

        if (changed)
            ModifyStartPoint = new Point(Canvas.GetLeft(sender as Panel), Canvas.GetTop(sender as Panel));
        e.Handled = true;
    }

    private void PanelOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var mnemoElement = (sender as Panel).Children[0] as BasicMnemoElement;
        var window = ((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime).MainWindow;
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

            e.Handled = true;
        }
        else if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed)
        {
            (sender as Panel).ContextMenu?.Open();
            e.Handled = true;
        }

        
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private Window GetWindow()
    {
        return ((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime)
            .MainWindow;
    }

    private void SubscribeMnemo(Canvas canvas)
    {
        canvas.PointerPressed += CanvasOnPointerPressed;
        canvas.PointerMoved += CanvasOnPointerMoved;
        canvas.PointerReleased += CanvasOnPointerReleased;
        canvas.Children.ToList().ForEach(x =>
        {
            ((Panel)x).PointerPressed += PanelOnPointerPressed;
            ((Panel)x).PointerMoved += PanelOnPointerMoved;
        });
    }

    private void UnSubscribeMnemo(Canvas canvas)
    {
        canvas.PointerPressed -= CanvasOnPointerPressed;
        canvas.PointerMoved -= CanvasOnPointerMoved;
        canvas.PointerReleased -= CanvasOnPointerReleased;
        canvas.Children.ToList().ForEach(x =>
        {
            ((Panel)x).PointerPressed -= PanelOnPointerPressed;
            ((Panel)x).PointerMoved -= PanelOnPointerMoved;
        });
    }
}