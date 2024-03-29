﻿using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using AvAp2.Models.BaseClasses;

namespace AvAp2.Models.Controls
{
    [Description("Выкатной элемент ячейки 2")]
    public class CCellCart2 : BasicCommutationDevice 
    {

        public CCellCart2() : base()
        {
            this.TextNameWidth = (double)60;
            this.MarginTextName = new Thickness(-30, -40, 0, 0);
            this.TextNameISVisible = false;
            this.ControlISHitTestVisible = true;
            DataContext = this;
        }

        public override string ElementTypeFriendlyName
        {
            get => "Выкатной элемент ячейки 2";
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
            if (IsConnectorExistLeft)
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(4, 15));
                if (IsConnectorExistRight)
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(86, 15), new Point(105, 15));


                if (state == CommutationDeviceStates.On)
                {
                    // Слева
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(4, 15), new Point(15, 5));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(4, 15), new Point(15, 25));

                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(9, 16), new Point(21, 5));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(10, 15), new Point(30, 15));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(9, 14), new Point(21, 25));

                    // Справа
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(86, 15), new Point(75, 5));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(86, 15), new Point(75, 25));

                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(81, 16), new Point(69, 5));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(60, 15), new Point(79, 15));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(81, 14), new Point(69, 25));
                }
                else if (state == CommutationDeviceStates.Off)
                {
                    // Слева
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(4, 15), new Point(15, 5));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(4, 15), new Point(15, 25));

                    // Справа
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(86, 15), new Point(75, 5));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(86, 15), new Point(75, 25));

                }

                else if (state == CommutationDeviceStates.UnDefined)
                {
                    // Слева
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-6, 5), new Point(15, 25));
                    // Справа
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(75, 5), new Point(95, 25));
                }
                else if (state == CommutationDeviceStates.Broken)
                {
                    // Слева
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-6, 5), new Point(15, 25));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-6, 25), new Point(15, 5));
                    // Справа
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(75, 5), new Point(95, 25));
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(75, 25), new Point(95, 5));
                }
                if (ShowNormalState)
                {
                    if (state != NormalState)
                    {
                        drawingContext.DrawRectangle(Brushes.Transparent, PenNormalState, new Rect(-1, -1, 32, 32));
                        drawingContext.DrawRectangle(Brushes.Transparent, PenNormalState, new Rect(59, -1, 32, 32));
                    }
                }
        }
        protected override void DrawIsSelected(DrawingContext ctx)
        {
            if (ControlISSelected)
            {
                var transform = ctx.PushPostTransform(new RotateTransform(Angle).Value);
                ctx.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(0, 0, 29, 29));
                ctx.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(60, 0, 29, 29));
                transform.Dispose();
            }
        }

        protected override void DrawMouseOver(DrawingContext ctx)
        { 
            var transform = ctx.PushPostTransform(new RotateTransform(Angle).Value);
            ctx.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(0, 0, 29, 29));
            ctx.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(60, 0, 29, 29));
            transform.Dispose();
        }
    }
}
