using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using AvAp2.Interfaces;
using AvAp2.Models.BaseClasses;

namespace AvAp2.Models.Controls
{
    public class CFuse : BasicEquipment, IConnector
    {
        static CFuse()
        {
            AffectsRender<CFuse>(IsConnectorExistLeftProperty);
            AffectsRender<CFuse>(IsConnectorExistRightProperty);
        }
        public override string ElementTypeFriendlyName { get => "Предохранитель"; }
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        #region IConnector
        [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителя левого "), DisplayName("Видимость соединителя левого "), Browsable(true)]
        public bool IsConnectorExistLeft
        {
            get => (bool)GetValue(IsConnectorExistLeftProperty);
            set
            {
                SetValue(IsConnectorExistLeftProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static readonly StyledProperty<bool> IsConnectorExistLeftProperty = AvaloniaProperty.Register<CFuse, bool>(nameof(IsConnectorExistLeft),true);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителя правого "), DisplayName("Видимость соединителя правого "), Browsable(true)]
        public bool IsConnectorExistRight
        {
            get => (bool)GetValue(IsConnectorExistRightProperty);
            set
            {
                SetValue(IsConnectorExistRightProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static readonly StyledProperty<bool> IsConnectorExistRightProperty = AvaloniaProperty.Register<CFuse, bool>(nameof(IsConnectorExistRight),true);

        #endregion IConnector

        public override void Render(DrawingContext drawingContext)
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

            //drawingContext.PushTransform(new RotateTransform(Angle, 15, 15));
            if (IsConnectorExistLeft)
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15),
                    new Point(2, 15));
            if (IsConnectorExistRight)
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(28, 15),
                    new Point(45, 15));

            drawingContext.DrawLine(isActiveState ? PenContentColorThin : PenContentColorThinAlternate,
                new Point(2, 15), new Point(27, 15));
            drawingContext.DrawRectangle(Brushes.Transparent,
                isActiveState ? PenContentColor : PenContentColorAlternate, new Rect(2, 7, 25, 16));
        
        }
    }
}