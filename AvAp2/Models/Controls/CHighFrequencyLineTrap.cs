using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using AvAp2.Interfaces;

namespace AvAp2.Models
{
    [Description("Высокочастотный заградитель")]
    public class CHighFrequencyLineTrap : BasicEquipment, IRegulator
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

        public static StyledProperty<bool> IsRegulatorProperty =
            AvaloniaProperty.Register<CHighFrequencyLineTrap, bool>(nameof(IsRegulator), false);

        #endregion

        public CHighFrequencyLineTrap() : base()
        {
        }
        public override string ElementTypeFriendlyName
        {
            get => "Высокочастотный заградитель";
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
                context.LineTo(new Point(1, 15));
                context.ArcTo(new Point(10, 15), new Size(4, 4), 0, false, SweepDirection.Clockwise);
                context.ArcTo(new Point(19, 15), new Size(4, 4), 0, false, SweepDirection.Clockwise);
                context.ArcTo(new Point(28, 15), new Size(4, 4), 0, false, SweepDirection.Clockwise);
                context.LineTo(new Point(45, 15));
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
            var geometry = TheGeometry();
            geometry.Transform = new RotateTransform(Angle, 15, 15);
            drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, geometry);
                
            
        }
    }
}