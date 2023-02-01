using System.IO;
using Avalonia.Controls;
using DynamicData;
using MnemoschemeEditor._PropertyGrid;
using MnemoschemeEditor.jsons;
using Newtonsoft.Json;

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
            "RenderTransform", "ManifestModule"
        }, new PointsFile(@"C:\Users\murov\RiderProjects\AvaloniaApplication1\MnemoschemeEditor\points\canvas_points.json"));
        Name = Path.GetFileNameWithoutExtension(path);
    }

    public Canvas GetMnemoscheme()
    {
        var output = new JsonSerializer();
        output.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        output.Converters.Add(new ControlsConverter());
        output.ContractResolver = _resolver;
        output.TypeNameHandling = TypeNameHandling.All;
        using (StreamReader sr = new StreamReader(_path))
        using (JsonTextReader jr = new JsonTextReader(sr))
        {
            return output.Deserialize<Canvas>(jr);
        }
        
    }

    public void SaveMnemoscheme(Canvas canvas)
    {
        var output = new Newtonsoft.Json.JsonSerializer();
        output.ContractResolver = _resolver;
        output.TypeNameHandling = TypeNameHandling.All;
        using (StreamWriter sw = new StreamWriter(_path))
        using (JsonTextWriter jw = new JsonTextWriter(sw))
        {
            output.Serialize(jw, canvas);
        }
    }
    
    
}