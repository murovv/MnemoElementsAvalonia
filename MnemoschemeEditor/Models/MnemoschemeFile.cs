using System.IO;
using Avalonia.Controls;
using Newtonsoft.Json;

namespace MnemoschemeEditor.Models;

public class MnemoschemeFile:IMnemoscheme
{
    private string _path;
    
    public string Name { get; }

    public MnemoschemeFile(string path)
    {
        _path = path;
        Name = Path.GetFileNameWithoutExtension(path);
    }

    public Canvas GetMnemoscheme()
    {
        PanelSerializer serializer = new PanelSerializer();
        using (StreamReader sr = new StreamReader(_path))
        using (JsonTextReader jr = new JsonTextReader(sr))
        {
            return serializer.ReadJson(jr, typeof(Canvas), null, false, new JsonSerializer()) as Canvas;
        }
        
    }

    public void SaveMnemoscheme(Canvas canvas)
    {
        PanelSerializer serializer = new PanelSerializer();
        using (StreamWriter sw = new StreamWriter(_path))
        using (JsonTextWriter jw = new JsonTextWriter(sw))
        {
            serializer.WriteJson(jw, canvas, new JsonSerializer());
        }
    }
    
    
}