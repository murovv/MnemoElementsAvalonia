using Avalonia;
using Avalonia.Media;
using AvAp2.Models.BaseClasses;

namespace AvAp2.Models.Controls
{
    public class CDischarger : BasicEquipment
    {
        public override string ElementTypeFriendlyName {  get => "Разрядник"; }
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }
        private static StreamGeometry ArrowGeometryL()
        {
            StreamGeometry geometry = new StreamGeometry();
            var context = geometry.Open();
            context.BeginFigure(new Point(-15, 15), true);
            context.LineTo(new Point(10, 15));
            context.LineTo(new Point(10, 13));
            context.LineTo(new Point(12, 15));
            context.LineTo(new Point(10, 17));
            context.LineTo(new Point(10, 15));
            //  context.LineTo(new Point(-15, 15), true, false);
            return geometry;
        }
        private static StreamGeometry ArrowGeometryR()
        {
            StreamGeometry geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(35, 15), true);
                context.LineTo(new Point(20, 15));
                context.LineTo(new Point(20, 13));
                context.LineTo(new Point(18, 15));
                context.LineTo(new Point(20, 17));
                context.LineTo(new Point(20, 15));
                // context.LineTo(new Point(35, 15), true, false);
            }

            return geometry;
        }

        public override void Render(DrawingContext drawingContext)
        {
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

            StreamGeometry geometryL = ArrowGeometryL();
            StreamGeometry geometryR = ArrowGeometryR();
            //drawingContext.PushTransform(new RotateTransform(Angle, 15, 15));
            drawingContext.DrawGeometry(BrushContentColor, isActiveState ? PenContentColor : PenContentColorAlternate, geometryL);
            drawingContext.DrawGeometry(BrushContentColor, isActiveState ? PenContentColor : PenContentColorAlternate, geometryR);

            drawingContext.DrawRectangle(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, new Rect(2, 7, 25, 16));

            drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(35, 2), new Point(35, 28));
            drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(40, 5), new Point(40, 25));
            drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(45, 8), new Point(45, 22));
        }
    }
}