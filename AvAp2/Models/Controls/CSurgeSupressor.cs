using System.ComponentModel;
using Avalonia;
using Avalonia.Media;

namespace AvAp2.Models
{
    [Description("Ограничитель перенапряжений")]
    public class CSurgeSuppressor : BasicEquipment
    {
        public CSurgeSuppressor() : base()
        {
        }
        public override string ElementTypeFriendlyName
        {
            get => "Ограничитель перенапряжений";
        }

        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        private static StreamGeometry StickGeometry()
        {
            StreamGeometry geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(3, 28), false);
                context.LineTo(new Point(23, 2));
                context.LineTo(new Point(31, 2));
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

            DrawingContext.PushedState rotation;
            using (rotation = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value))
            {
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(2, 15));
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(27, 15), new Point(35, 15));

                drawingContext.DrawRectangle(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, new Rect(3, 8, 23, 14));

                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(35, 2), new Point(35, 28));
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(40, 5), new Point(40, 25));
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(45, 8), new Point(45, 22));

                var geometry = StickGeometry();
                drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, geometry);
            }
        }
    }
}