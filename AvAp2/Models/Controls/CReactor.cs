using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using AvAp2.Interfaces;

namespace AvAp2.Models
{
    [Description("Реактор")]
    public class CReactor : BasicEquipment, IRegulator
    {
        #region IRegulator
        [Category("Свойства элемента мнемосхемы"), Description("Наличие регулировки"), PropertyGridFilterAttribute, DisplayName("Наличие регулировки"), Browsable(true)]
        public bool IsRegulator
        {
            get => (bool)GetValue(IsRegulatorProperty);
            set
            {
                SetValue(IsRegulatorProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> IsRegulatorProperty = AvaloniaProperty.Register<CReactor,bool>(nameof(IsRegulator), false);



        #endregion

        public CReactor() : base()
        {
            AffectsRender<CReactor>(IsRegulatorProperty);
        }
        public override string ElementTypeFriendlyName
        {
            get => "Реактор";
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
                context.LineTo(new Point(2, 15));
                context.ArcTo(new Point(15, 27), new Size(12.5, 12.5), 0, true, SweepDirection.Clockwise);
                context.LineTo(new Point(15, 15));
                context.LineTo(new Point(45, 15));
            }
            return geometry;
        }

        private static StreamGeometry ArrowGeometry()
        {
            //Data="M0,28 L35,-7 L33,-8 L38,-10 L37,-5 L35,-7 z"
            StreamGeometry geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(-3, 32), false);
                context.LineTo(new Point(35, -7));
                context.LineTo(new Point(33, -8));
                context.LineTo(new Point(38, -10));
                context.LineTo(new Point(37, -5));
                context.LineTo(new Point(35, -7));
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
            //drawingContext.PushTransform(new RotateTransform(Angle, 15, 15));//Если нужно трансформировать только часть элементов Push-Pop
                var geometry = TheGeometry();
                geometry.Transform = new RotateTransform(Angle, 15, 15);
                drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, geometry);
                if (IsRegulator)
                {
                    var geometryArrow = ArrowGeometry();
                    drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, geometryArrow);
                }
        }
    }
}