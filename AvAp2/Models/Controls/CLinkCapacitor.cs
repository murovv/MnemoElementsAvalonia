using System.ComponentModel;
using Avalonia;
using Avalonia.Media;

namespace AvAp2.Models
{
    [Description("Конденсатор связи")]
    public class CLinkCapacitor : BasicEquipment
    {
        public CLinkCapacitor() : base()
        {
        }

        public override string ElementTypeFriendlyName
        {
            get => "Конденсатор связи";
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

                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(12, 15));
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(18, 15), new Point(45, 15));
                drawingContext.DrawLine(isActiveState ? PenContentColorThin : PenContentColorThinAlternate, new Point(12, 5), new Point(12, 25));
                drawingContext.DrawLine(isActiveState ? PenContentColorThin : PenContentColorThinAlternate, new Point(18, 5), new Point(18, 25));
            }
        }
    }
}