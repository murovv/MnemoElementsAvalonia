using System.ComponentModel;
using Avalonia;
using Avalonia.Media;

namespace AvAp2.Models
{
    public class CPEConnector : BasicEquipment
    {
        static CPEConnector()
        {
            AffectsRender<CPEConnector>(IsLineThinProperty);
        }
        public override bool ControlIs30Step
        {
            get => false;
        }
        [Category("Свойства элемента мнемосхемы"), Description("Элемент нарисован тонкой линией"), PropertyGridFilterAttribute, DisplayName("Тонкая линия"), Browsable(true)]
        public bool IsLineThin
        {
            get => (bool)GetValue(IsLineThinProperty);
            set
            {
                SetValue(IsLineThinProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> IsLineThinProperty = AvaloniaProperty.Register<CPEConnector,bool>(nameof(IsLineThin), false);
        public override string ElementTypeFriendlyName
        {
            get => "Заземление";
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
                if (IsLineThin)
                {
                    drawingContext.DrawLine(isActiveState ? PenContentColorThin : PenContentColorThinAlternate, new Point(-15, 15), new Point(10, 15));
                    drawingContext.DrawLine(isActiveState ? PenContentColorThin : PenContentColorThinAlternate, new Point(10, 2), new Point(10, 28));
                    drawingContext.DrawLine(isActiveState ? PenContentColorThin : PenContentColorThinAlternate, new Point(15, 5), new Point(15, 25));
                    drawingContext.DrawLine(isActiveState ? PenContentColorThin : PenContentColorThinAlternate, new Point(20, 8), new Point(20, 22));
                }
                else
                {
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(10, 15));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(10, 2), new Point(10, 28));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 5), new Point(15, 25));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(20, 8), new Point(20, 22));
                }
        }
    }
}