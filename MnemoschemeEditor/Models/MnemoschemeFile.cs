using System.IO;
using System.Runtime.Serialization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using MnemoschemeEditor.jsons;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MnemoschemeEditor.Models;

public class MnemoschemeFile:IMnemoscheme
{
    private string _path;
    private PanelPropertiesResolver _resolver;
    public string Name { get; }

    public MnemoschemeFile(string path)
    {
        _path = path;
        _resolver = new PanelPropertiesResolver(new[]
        {
            "Parent", "Owner", "FocusAdorner", "DataContext", "Classes", "Background", "Resources", "Template",
            "RenderTransform", "ManifestModule", "ContextMenu", "Drawing", "Color", "TabIndex", "Margin", "Bounds", "Content", "Clock", "Transitions", "Theme", "Alignment", "UseLayoutRouting", "IsEnabled"
        }, new PointsFile(_path));
        Name = Path.GetFileNameWithoutExtension(path);
    }

    public Canvas GetMnemoscheme()
    {
        var output = new JsonSerializer();
        output.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        output.Converters.Add(new ControlsConverter());
        output.ContractResolver = _resolver;
        output.TypeNameHandling = TypeNameHandling.All;
        JToken canvas;
        using (var sr = new StreamReader(_path))
        {
            JArray array = JArray.Parse(sr.ReadToEnd());
            canvas = array[0];
        }
        return canvas.ToObject<Canvas>(output);
    }

    public void SaveMnemoscheme(Canvas canvas)
    {
        var output = new JsonSerializer();
        output.ContractResolver = _resolver;
        output.TypeNameHandling = TypeNameHandling.All;
        StreamWriter sw = File.CreateText(_path);
        using (JsonTextWriter jw = new JsonTextWriter(sw))
        {
            output.Context = new StreamingContext(StreamingContextStates.Other, sw);
            output.Serialize(jw, canvas);
        }
    }
    
    
}