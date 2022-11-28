using System.ComponentModel;
using Avalonia;
using Avalonia.Media;

namespace AvAp2.Models
{
    [Description("Линия электропередачи")]
    public class CArrow : BasicEquipment
    {
        public override string ElementTypeFriendlyName
        {
            get => "Линия электропередачи";
        }

        public CArrow() : base()
        {
            DataContext = this;
            /*TextName = "Линия";
            MarginTextName = new Thickness(-30, -20, 0, 0);*/
        }

        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        private static StreamGeometry ArrowGeometry()
        {
            StreamGeometry geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(15, 45), true);
                context.LineTo(new Point(15, 25));
                context.LineTo(new Point(8, 25));
                context.LineTo(new Point(15, 7));
                context.LineTo(new Point(22, 25));
                context.LineTo(new Point(15, 25));
            }

            return geometry;
        }

        public override void Render(DrawingContext drawingContext)
        {
            {
#warning Не лучший вариант, но по 104 Сашина библиотека выдаёт строки "0" и "1"
                bool isActiveState = false; // При настройке, пока ничего не привязано, рисуем цветом класса напряжения
                if (TagDataMainState == null)
                    isActiveState = true;
                else
                {
                    if (TagDataMainState.TagValueString != null)
                        if (TagDataMainState.TagValueString.Equals("1")) // В работе если что-то привязано и там "1"
                            isActiveState = true;
                }

                StreamGeometry geometry = ArrowGeometry();
                // drawingContext.PushTransform(new RotateTransform(Angle));//Если нужно трансформировать только часть элементов Push-Pop
                geometry.Transform = new RotateTransform(Angle, 15, 15);
                drawingContext.DrawGeometry(BrushContentColor,
                    isActiveState ? PenContentColor : PenContentColorAlternate, geometry);
            }

            //internal protected override void DrawIsSelected()
            //{
            //    using (var drawingContext = DrawingVisualIsSelected.RenderOpen())
            //    {
            //        drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-5, -5, 39, 39));
            //        drawingContext.Close();
            //    }
            //    DrawingVisualIsSelected.Opacity = ControlISSelected ? .3 : 0;
            //}

        }
    }
}