using System.ComponentModel;
using System.Globalization;
using Avalonia;
using Avalonia.Media;

namespace AvAp2.Models
{
    [Description("Разъединитель")]
    public class CIsolatingSwitch : BasicCommutationDevice
    {
        [Category("Свойства элемента мнемосхемы"), Description("Видимость признака отделителя"), PropertyGridFilterAttribute, DisplayName("Отделитель"), Browsable(true)]
        public bool ShortCircuitIsExist
        {
            get => (bool)GetValue(ShortCircuitIsExistProperty);
            set
            {
                SetValue(ShortCircuitIsExistProperty, value);
                //RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> ShortCircuitIsExistProperty = AvaloniaProperty.Register<CIsolatingSwitch,bool>(nameof(ShortCircuitIsExist), false);



        public CIsolatingSwitch() : base()
        {
            this.TextNameWidth = (double)60;
            this.MarginTextName = new Thickness(-30, -7, 0, 0);
            this.ControlISHitTestVisible = true;
        }
        public override string ElementTypeFriendlyName
        {
            get => "Разъединитель";
        }
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        public override void Render(DrawingContext drawingContext)
        {
#warning Не лучший вариант, но по 104 Сашина библиотека выдаёт строки "0" и "1"
            bool isActiveState = true;// При настройке, пока ничего не привязано, рисуем цветом класса напряжения
            CommutationDeviceStates state = CommutationDeviceStates.UnDefined;
            //if (TagDataMainState == null)
            //     isActiveState = true;
            // else
            //  {
            if (TagDataMainState?.TagValueString != null)// В работе если что-то привязано и там "1"
            {
                switch (TagDataMainState.TagValueString)
                {
                    case "1":
                        state = CommutationDeviceStates.Off;
                        isActiveState = false;
                        break;
                    case "2":
                        state = CommutationDeviceStates.On;
                        break;
                    case "3":
                        state = CommutationDeviceStates.Broken;
                        break;
                }
            }
            //}
            DrawingContext.PushedState rotation = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value) ;
            if (IsConnectorExistLeft)
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(2, 15));
                if (IsConnectorExistRight)
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(28, 15), new Point(45, 15));

                drawingContext.DrawLine( PenContentColor , new Point(1, 1), new Point(1, 28));
                drawingContext.DrawLine( PenContentColor , new Point(28, 1), new Point(28, 28));

                if (state == CommutationDeviceStates.On)
                {
                    drawingContext.DrawLine(PenContentColor, new Point(5, 15), new Point(24, 15));
                    rotation.Dispose();
                }
                else if (state == CommutationDeviceStates.Off)
                {
                    drawingContext.DrawLine(PenContentColor, new Point(15, 5), new Point(15, 25));
                    rotation.Dispose();
                }
                else if (state == CommutationDeviceStates.UnDefined)
                {
                    drawingContext.DrawRectangle(Brushes.WhiteSmoke, null, new Rect(3, 3, 23, 24));
                    rotation.Dispose();// Убираем поворот
                    /*FormattedText ft = new FormattedText("?", new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Black),
                        16,TextAlignment.Center, TextWrapping.NoWrap, Size.Empty);
                    drawingContext.DrawText(isActiveState ? BrushContentColor : BrushContentColorAlternate, new Point(15, 4),ft);
                */
                }
                else if (state == CommutationDeviceStates.Broken)
                {
                    rotation.Dispose();// Убираем поворот
                    drawingContext.DrawLine(PenRed, new Point(-5, 35), new Point(35, -5));
                }
                if (ShowNormalState)
                {
                    if (state != NormalState)
                        drawingContext.DrawRectangle(Brushes.Transparent, PenNormalState, new Rect(-1, -1, 32, 32));
                }
                
        }
    }
}