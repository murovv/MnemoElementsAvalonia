using System.ComponentModel;
using Avalonia;
using Avalonia.Media;

namespace AvAp2.Models
{
    [Description("Индикатор порта Ethernet")]
    public class CPointLinkPort : CPointLink
    {
        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"), PropertyGridFilterAttribute, DisplayName("Сетка"), Browsable(false)]
        public override bool ControlIs30Step
        {
            get => true;
        }

        public CPointLinkPort() : base()
        {
            this.ControlISHitTestVisible = true;
            TextNameWidth = 30;
            TextNameFontSize = 12;
            MarginTextName = new Thickness(0, -20, 0, 0);
        }

        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        public override void Render(DrawingContext drawingContext)
        {
#warning Не лучший вариант, но по 104 Сашина библиотека выдаёт строки "0" и "1"
            IBrush brush = Brushes.LightGray;
            if (TagDataMainState == null)//Пока ничего не привязано рисуем зелёным
                brush = BrushContentColor;
            else// В рантайме появляется объект - источник привязки - начинаем анализировать
            {
                if (TagDataMainState.TagValueString != null)
                {
                    if (TagDataMainState.TagValueString.Equals(StringStateIsConnected))// В работе если что-то привязано и там связь есть
                        brush = BrushContentColor;
                    else
                        brush = BrushContentColorAlternate;
                }
            }

            DrawingContext.PushedState rotation;
            using (rotation = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value))
            {

                Pen penBorder = new Pen(Brushes.AntiqueWhite, 1);
                drawingContext.DrawRectangle(brush, penBorder, new Rect(2, 7, 25, 15));
                drawingContext.DrawRectangle(Brushes.Yellow, null, new Rect(2, 7, 7, 5));
                drawingContext.DrawRectangle(Brushes.Yellow, null, new Rect(20, 7, 7, 5));
            }
        }
    }
}