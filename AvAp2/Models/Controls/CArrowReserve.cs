using System.ComponentModel;
using Avalonia;
using Avalonia.Media;

namespace AvAp2.Models
{
    [Description("Линия электропередачи резервная")]
    public class CArrowReserve : BasicEquipment
    {
        public CArrowReserve() : base()
        {
            TextName = "Резерв";
            MarginTextName = new Thickness(-30, -20, 0, 0);
        }
        public override string ElementTypeFriendlyName
        {
            get => "Линия электропередачи резервная";
        }
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        //private static PathGeometry StickGeometry()
        //{
        //    PathGeometry pathGeometry = new PathGeometry();
        //    pathGeometry.FillRule = FillRule.Nonzero;

        //    PathFigure pathFigure = new PathFigure();
        //    pathFigure.StartPoint = new Point(15, 45);
        //    pathFigure.IsClosed = false;
        //    pathGeometry.Figures.Add(pathFigure);

        //    LineSegment lineSegment1 = new LineSegment();
        //    lineSegment1.Point = new Point(15, 15);
        //    pathFigure.Segments.Add(lineSegment1);

        //    ArcSegment arcSegment = new ArcSegment();
        //    arcSegment.Point = new Point(30, 15);
        //    arcSegment.Size = new Size(10, 10);
        //    arcSegment.IsLargeArc = false;
        //    arcSegment.IsStroked = true;

        //    pathFigure.Segments.Add(arcSegment);

        //    //BezierSegment bezierSegment2 = new BezierSegment();
        //    //bezierSegment2.Point1 = new Point(6, 15);
        //    //bezierSegment2.Point2 = new Point(6, 13);
        //    //bezierSegment2.Point3 = new Point(5, 12);
        //    //pathFigure.Segments.Add(bezierSegment2);

        //    return pathGeometry;
        //}

        private static StreamGeometry StickGeometry()
        {
            StreamGeometry geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(15, 45), false);
                context.LineTo(new Point(15, 15));
                context.ArcTo(new Point(28, 15), new Size(4, 4), 0, false, SweepDirection.Clockwise);
                // context.BezierTo(new Point(100, 0), new Point(200, 200), new Point(300, 100), true, true);
            }
            return geometry;
        }

        public override void Render(DrawingContext drawingContext){
#warning Не лучший вариант, но по 104 Сашина библиотека выдаёт строки "0" и "1"
            bool isActiveState = false;// При настройке, пока ничего не привязано, рисуем цветом класса напряжения
            if (TagDataMainState == null)
                isActiveState = true;
            else
            {
                if (TagDataMainState.TagValueString != null)
                    if (TagDataMainState.TagValueString.Equals("1"))// В работе если что-то привязано и там "1"
                        isActiveState = true;
            }
            var geometry = StickGeometry();
            // drawingContext.PushTransform(new RotateTransform(Angle));//Если нужно трансформировать только часть элементов Push-Pop
                geometry.Transform = new RotateTransform(Angle, 15, 15);
                drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, geometry);
        }

    }
}