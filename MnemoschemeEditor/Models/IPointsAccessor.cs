using System.Collections.Generic;
using Avalonia;

namespace MnemoschemeEditor.Models;

public interface IPointsAccessor
{
    public List<Point> GetPoints();
    public void SavePoints(List<Point> points);

    public Point GetPoint();
}