using System.ComponentModel;
using Avalonia;
using Avalonia.Media;

namespace AvAp2.Models
{
    [Description("Трансформатор тока")]
    public class CCurrentTransformer : BasicEquipment
    {
        public CCurrentTransformer() : base()
        {
        }
        public override string ElementTypeFriendlyName
        {
            get => "Трансформатор тока";
        }
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
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

                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(45, 15));
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(10, 0), new Point(10, 6));
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(20, 0), new Point(20, 6));
                drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 15), 11, 11);
            }
        }

    }
}