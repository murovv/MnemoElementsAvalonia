using System.ComponentModel;
using System.Globalization;
using Avalonia;
using Avalonia.Media;

namespace AvAp2.Models
{
    [Description("Заземляющий нож")]
    public class CPESwitch : BasicCommutationDevice
    {
        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Номер переносного заземления"), PropertyGridFilterAttribute, DisplayName("Номер заземления"), Browsable(true)]
        public string ManualPENumber
        {
            get => (string)GetValue(ManualPENumberProperty);
            set
            {
                SetValue(ManualPENumberProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> ManualPENumberProperty = AvaloniaProperty.Register<CPESwitch,string>(nameof(ManualPENumber),"");


        [Category("Свойства элемента мнемосхемы"), Description("Видимость признака короткозамыкателя"), PropertyGridFilterAttribute, DisplayName("Короткозамыкатель"), Browsable(true)]
        public bool ShortCircuitIsExist
        {
            get => (bool)GetValue(ShortCircuitIsExistProperty);
            set => SetValue(ShortCircuitIsExistProperty, value);
        }
        public static StyledProperty<bool> ShortCircuitIsExistProperty = AvaloniaProperty.Register<CPESwitch, bool>(nameof(ShortCircuitIsExist), false);


        public CPESwitch() : base()
        {
            this.TextNameWidth = (double)60;
            this.MarginTextName = new Thickness(-30, -7, 0, 0);
            this.ControlISHitTestVisible = true;
        }
        public override string ElementTypeFriendlyName
        {
            get => "Заземляющий нож";
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
            DrawingContext.PushedState transform = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
            if (IsConnectorExistLeft)
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(2, 15));
                //if (IsConnectorExistRight)
                //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(28, 15), new Point(45, 15));

                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(28, 15), new Point(35, 15));
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(35, 2), new Point(35, 28));
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(40, 5), new Point(40, 25));
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(45, 8), new Point(45, 22));

                drawingContext.DrawLine(PenContentColor, new Point(1, 1), new Point(1, 28));
                drawingContext.DrawLine(PenContentColor, new Point(28, 1), new Point(28, 28));

                if (state == CommutationDeviceStates.On)
                {
                    drawingContext.DrawLine(PenContentColor, new Point(5, 15), new Point(24, 15));
                    transform.Dispose();
                }
                else if (state == CommutationDeviceStates.Off)
                {
                    drawingContext.DrawLine(PenContentColor, new Point(15, 5), new Point(15, 25));
                    transform.Dispose();
                }
                else if (state == CommutationDeviceStates.UnDefined)
                {
                    drawingContext.DrawRectangle(Brushes.WhiteSmoke, null, new Rect(3, 3, 23, 24));
                    transform.Dispose();
                    FormattedText ft = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                        new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Black, FontStretch.Normal),
                        16, BrushContentColor);
                    ft.TextAlignment = TextAlignment.Center;
                    drawingContext.DrawText(ft, new Point(15, 4));
                }
                else if (state == CommutationDeviceStates.Broken)
                {
                    transform.Dispose();
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