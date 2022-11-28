using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.ComponentModel;
using System.Globalization;

namespace AvAp2.Models
{
    [Description("Автоматический выключатель")]
    public class CAutomaticSwitch :  BasicCommutationDevice
    {
        public CAutomaticSwitch()
        {
            /*this.TextNameWidth = (double)60;
            this.MarginTextName = new Thickness(-30, -7, 0, 0);
            this.TextNameISVisible = true;*/
            this.ControlISHitTestVisible = true;  
        }
        public override void Render(DrawingContext drawingContext)
        {
            DrawingContext.PushedState rotate =
                   drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
            var lineLength = Math.Sqrt((100 * 100) + (100 * 100));
            var diffX = LineBoundsHelper.CalculateAdjSide(Angle, lineLength);
            var diffY = LineBoundsHelper.CalculateOppSide(Angle, lineLength);
            var p1 = new Point(200, 200);
            var p2 = new Point(p1.X + diffX, p1.Y + diffY);
            //drawingContext.DrawLine(PenContentColor, p1, p2);
            //drawingContext.DrawRectangle(PenBlack, LineBoundsHelper.CalculateBounds(p1, p2, PenContentColor));
            //----------------------------------------------------------------------------------------------------------------

            bool isActiveState = true; // При настройке, пока ничего не привязано, рисуем цветом класса напряжения
            CommutationDeviceStates state = CommutationDeviceStates.UnDefined;

            if (TagDataMainState?.TagValueString != null) // В работе если что-то привязано и там "1"
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



            if (IsConnectorExistLeft)
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate,
                    new Point(-15, 15), new Point(2, 15));
            if (IsConnectorExistRight)
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate,
                    new Point(28, 15), new Point(45, 15));

            /*drawingContext.DrawRectangle(Brushes.Transparent, PenContentColor, new Rect(1, 1, 28, 28));*/

            if (state == CommutationDeviceStates.On)
            {
                drawingContext.DrawRectangle(Brushes.Red, null, new Rect(2.5, 2.5, 25, 25));
                drawingContext.DrawLine(PenBlack, new Point(5, 15), new Point(25, 15));
            }
            else if (state == CommutationDeviceStates.Off)
            {
                drawingContext.DrawRectangle(Brushes.Green, null, new Rect(2.5, 2.5, 25, 25));
                drawingContext.DrawLine(PenBlack, new Point(15, 5), new Point(15, 25));
            }
            else if (state == CommutationDeviceStates.UnDefined)
            {
                drawingContext.DrawRectangle(Brushes.WhiteSmoke, null, new Rect(2.5, 2.5, 25, 25));
                rotate.Dispose(); //Разворот вопросика в нормальное положение даже если КА повёрнут
                var ft = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Black,
                        FontStretch.Normal),
                    16, BrushContentColor);
                drawingContext.DrawText(ft, new Point(10, 7));

            }
            else if (state == CommutationDeviceStates.Broken)
            {
                drawingContext.DrawRectangle(Brushes.WhiteSmoke, null, new Rect(2.5, 2.5, 25, 25));
                drawingContext.DrawLine(PenRed, new Point(-5, 35), new Point(35, -5));
            }

            if (ShowNormalState)
            {
                if (state != NormalState)
                    drawingContext.DrawRectangle(Brushes.Transparent, PenNormalState, new Rect(-1, -1, 32, 32));
            }

            if (state != CommutationDeviceStates.UnDefined)
            {
                rotate.Dispose();
            }
            


        }

        public override string ElementTypeFriendlyName
        {
            get => "Автоматический выключатель";
        }

        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }
    }
}
