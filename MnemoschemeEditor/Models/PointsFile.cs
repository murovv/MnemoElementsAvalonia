using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MnemoschemeEditor.Models;

public class PointsFile:IPointsAccessor
{
    private string _path;
    private List<Point> _points = new List<Point>();
    private int _pointer = 0;
    public PointsFile(string path)
    {
        _path = path;
        string canvas = File.ReadAllTextAsync(_path).Result;
        JArray jArray = JArray.Parse(canvas);
        _points = jArray[1].ToObject<List<Point>>();
    }

    public List<Point> GetPoints()
    {
        /*using (StreamReader sr = new StreamReader(_path))
            return _points = JsonConvert.DeserializeObject<List<Point>>(sr.ReadToEnd()) ?? new List<Point>();
    */
        throw new NotImplementedException();
    }
    public void SavePoints(List<Point> points)
    {
        string canvas = File.ReadAllText(_path);
        JObject canvasObject = JObject.Parse(canvas);
        JArray pointsObject =
            JArray.Parse(JsonConvert.SerializeObject(points, typeof(List<Point>), Formatting.Indented, null));
        JArray jArray = new JArray();
        jArray.Add(canvasObject);
        jArray.Add(pointsObject);
        using (StreamWriter sw = File.CreateText(_path))
        {
            _points = points;
            sw.Write(jArray.ToString());
        }

        _points = points;
    }

    public Point GetPoint()
    {
        var point = _points[_pointer];
        _pointer = (_pointer+1) % _points.Count;
        return point;
    }
}