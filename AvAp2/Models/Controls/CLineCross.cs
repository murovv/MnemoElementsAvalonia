using System.ComponentModel;
using Avalonia;
using Avalonia.Media;

namespace AvAp2.Models
{
    [Description("Пересечение линий")]
    public class CLineCross : BasicEquipment
    {
        [Category("Свойства элемента мнемосхемы"), Description("Толщина линии"), PropertyGridFilterAttribute, DisplayName("Толщина"), Browsable(true)]
        public double LineThickness
        {
            get => (double)GetValue(LineThicknessProperty);
            set
            {
                SetValue(LineThicknessProperty, value);
                SetBrushes();
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<double> LineThicknessProperty = AvaloniaProperty.Register<CLineCross, double>(nameof(LineThickness), 3.0);
        
        public CLineCross() : base()
        {
            AffectsRender<CLineCross>(LineThicknessProperty);
        }
        public override string ElementTypeFriendlyName
        {
            get => "Пересечение линий";
        }
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        private static StreamGeometry TheGeometry()
        {
            StreamGeometry geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(-15, 15), false);
                context.LineTo(new Point(8, 15));

                context.ArcTo(new Point(22, 15), new Size(5, 5), 0, false, SweepDirection.Clockwise);

                context.LineTo(new Point(45, 15));
            }
            return geometry;
        }

        private void SetBrushes()
        {
            PenContentColorAlternate = new Pen(BrushContentColorAlternate, LineThickness);
            PenContentColorAlternate.ToImmutable();
            PenContentColor = new Pen(BrushContentColor, LineThickness);
            PenContentColor.ToImmutable();
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
            var geometry = TheGeometry();
            DrawingContext.PushedState rotation;
            using (rotation = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value))
            {
                drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, geometry);
            }
        }
    }
}