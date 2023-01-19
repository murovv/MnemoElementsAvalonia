using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Platform;
using Avalonia.Controls.Shapes;
using Avalonia.ExtendedToolkit.Extensions;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Remote.Protocol.Designer;
using Avalonia.Threading;
using Avalonia.Utilities;
using Avalonia.VisualTree;
using AvAp2.Interfaces;
using AvAp2.Models.BaseClasses;
using AvAp2.Models.Controls;
using DynamicData;
using FirLib.Core.Patterns;
using MnemoschemeEditor.jsons;
using MnemoschemeEditor.Models;
using MnemoschemeEditor.ViewModels;
using Newtonsoft.Json;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MnemoschemeEditor.Views;

public partial class DockableZoomBorderView : UserControl
{
    private Point ModifyStartPoint { get; set; }
    private bool ModifyPressed { get; set; }
    
    public ICommand OpenVideoSettingsWindow { get; }
    
    public DockableZoomBorderView()
    {
        InitializeComponent();
        var canvas = this.Find<Canvas>("Canvas1");
        canvas.PointerPressed+= CanvasOnPointerPressed;
        
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
        if (!mnemoElement.ControlISSelected && e.KeyModifiers == KeyModifiers.None)
        {
            mnemoElement.ControlISSelected = true;
            ((MainWindowViewModel)window.DataContext).SelectedMnemoElements.Add(mnemoElement);
        }else if (mnemoElement.ControlISSelected && e.KeyModifiers == KeyModifiers.Control)
        {
            ((MainWindowViewModel)window.DataContext).SelectedMnemoElements.Remove(mnemoElement);
            mnemoElement.ControlISSelected = false;
        }
        e.Handled = true;
        SaveState();
    }

    public async void SaveState()
    {
        var canvas = this.Find<Canvas>("Canvas1"); 
        ToJSON(canvas);
    }
    

    public void ToJSON(object obj)
    {
        using (StreamWriter fs = new StreamWriter(@"C:\Users\murov\RiderProjects\AvaloniaApplication1\MnemoschemeEditor\jsons\canvas.json"))
        using (JsonTextWriter tr = new JsonTextWriter(fs))
        {
            var output = new Newtonsoft.Json.JsonSerializer();
            output.ContractResolver = new IgnorePropertiesResolver(new[] { "Parent", "Owner", "FocusAdorner", "DataContext", "Classes", "Background", "Resources", "Template"});
            output.TypeNameHandling = TypeNameHandling.All;
            output.Serialize(tr, obj);
        }
        using (StreamReader fs = new StreamReader(@"C:\Users\murov\RiderProjects\AvaloniaApplication1\MnemoschemeEditor\jsons\canvas.json"))
        using (JsonTextReader tr = new JsonTextReader(fs))
        {
            var output = new Newtonsoft.Json.JsonSerializer();
            output.Converters.Add(new ControlsConverter());
            output.ContractResolver = new IgnorePropertiesResolver(new[] { "Parent", "Owner", "FocusAdorner", "DataContext", "Classes"});
            output.TypeNameHandling = TypeNameHandling.All;
            var canvas = output.Deserialize<Canvas>(tr);
        }
    }

   

    public async Task LoadState()
    {
        using (FileStream fs = new FileStream("canvas.json", FileMode.OpenOrCreate))
        {
            Canvas? person = await JsonSerializer.DeserializeAsync<Canvas>(fs);
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}