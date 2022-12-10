using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using AvAp2.Models.BaseClasses;

namespace AvAp2.Models.Controls
{
    [Description("Выкатной элемент ячейки 1")]
    public class CCellCart : BasicCommutationDevice
    {
        public CCellCart():base()
        {
            ControlISHitTestVisible = true;
        }
        public override string ElementTypeFriendlyName
        {
            get => "Выкатной элемент ячейки";
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
            var rotate = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);

                if (IsConnectorExistLeft)
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(4, 15));
                //if (IsConnectorExistRight)
                //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(28, 15), new Point(45, 15));

               
                if (state == CommutationDeviceStates.On)
                {
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(4, 15), new Point(15, 5));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(4, 15), new Point(15, 25));

                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(9, 16), new Point(21, 5));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(10, 15), new Point(30, 15));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(9, 14), new Point(21, 25));
                }
                else if (state == CommutationDeviceStates.Off)
                {
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(4, 15), new Point(15, 5));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(4, 15), new Point(15, 25));

                }


                else if (state == CommutationDeviceStates.UnDefined)
                {
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-6, 5), new Point(15, 25));
                }
                else if (state == CommutationDeviceStates.Broken)
                {
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-6, 5), new Point(15, 25));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-6, 25), new Point(15, 5));
                }

                if (ShowNormalState)
                {
                    if (state != NormalState)
                        drawingContext.DrawRectangle(Brushes.Transparent, PenNormalState, new Rect(-1, -1, 32, 32));
                }
                rotate.Dispose();
        }

        
    }
}