using System.Collections.Generic;
using System.IO;
using Avalonia;
using Newtonsoft.Json;

namespace MnemoschemeEditor.Models;

public class PointsFile:IPointsAccessor
{
    private string _path;
    public PointsFile(string path)
    {
        _path = path;
    }

    public List<Point> GetPoints()
    {
        using (StreamReader sr = new StreamReader(_path))
        {
            var list =  JsonConvert.DeserializeObject<List<Point>>(sr.ReadToEnd());
            return list ?? new List<Point>();
        }
        
        
    }

    public void SavePoints(List<Point> points)
    {
        using (StreamWriter sw = new StreamWriter(_path)) 
            sw.Write(JsonConvert.SerializeObject(points, typeof(List<Point>), Formatting.Indented, null));

    }
}