using System.IO;
using System.Runtime.Serialization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using MnemoschemeEditor.jsons;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MnemoschemeEditor.Models;

public class MnemoschemeAccessor
{
    private string _path;
    private PanelPropertiesResolver _resolver;
    private JsonSerializer _serializer;

    public MnemoschemeAccessor(string path)
    {
        _path = path;
        _resolver = new PanelPropertiesResolver(new[]
        {
            "Parent", "Owner", "FocusAdorner", "DataContext", "Classes", "Background", "Resources", "Template",
            "RenderTransform", "ManifestModule", "ContextMenu", "Drawing", "Color", "TabIndex", "Margin", "Bounds", "Content", "Clock", "Transitions", "Theme", "Alignment", "UseLayoutRouting", "IsEnabled"
        }, new PointsFile(_path));
        _serializer = new JsonSerializer();
        _serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        _serializer.Converters.Add(new ControlsConverter());
        _serializer.ContractResolver = _resolver;
        _serializer.TypeNameHandling = TypeNameHandling.All;
    }

    public string Serialize(Canvas canvas)
    {
        StringWriter sw = new StringWriter();
        using (JsonTextWriter jw = new JsonTextWriter(sw))
        {
            _serializer.Context = new StreamingContext(StreamingContextStates.Other, sw);
            _serializer.Serialize(jw, canvas);
            return jw.ToString();
        }
    }
    public Canvas Deserialize(string json)
    {
        JArray array = JArray.Parse(json);
        var canvas = array[0];
        return canvas.ToObject<Canvas>(_serializer);
    }
    

    public string GetMnemoschemeMarkup()
    {
        using (var sr = new StreamReader(_path))
        {
            JArray array = JArray.Parse(sr.ReadToEnd());
            var canvas = array[0];
            return canvas.ToObject<string>(_serializer);
        }
    }

    public void SaveMnemoscheme(Canvas canvas)
    {
        
        StreamWriter sw = File.CreateText(_path);
        using (JsonTextWriter jw = new JsonTextWriter(sw))
        {
            _serializer.Context = new StreamingContext(StreamingContextStates.Other, sw);
            _serializer.Serialize(jw, canvas);
        }
    }
    
    
}