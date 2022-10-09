using System;
using System.ComponentModel;
using System.Globalization;
using Avalonia;
using Avalonia.Media;

namespace AvAp2.Models
{
    [Description("Фильтр присоединения")]
    public class CFilterOfConnection:BasicEquipment
    {
        public CFilterOfConnection()
        {
            
        }
        

        public override string ElementTypeFriendlyName
        {
            get => "Фильтр присоединения";
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
            //drawingContext.PushTransform(new RotateTransform(Angle, 15, 15));
            drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(2, 15));
            drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(28, 15), new Point(35, 15));

            drawingContext.DrawRectangle(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, new Rect(2, 2, 25, 25));

            drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(35, 2), new Point(35, 28));
            drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(40, 5), new Point(40, 25));
            drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(45, 8), new Point(45, 22));

            /*FormattedText ft = new FormattedText("ФП", new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.SemiBold/*, FontStretch.Normal#1#),
                   14,TextAlignment.Center, TextWrapping.NoWrap, Size.Empty)/*, TextFormattingMode.Ideal#1#;

            //drawingContext.Pop();
            drawingContext.DrawText( isActiveState ? BrushContentColor : BrushContentColorAlternate,new Point(4, 0),ft );*/
        }
    }
}