﻿using System.ComponentModel;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using AvAp2.Models.BaseClasses;

namespace AvAp2.Models.Controls
{
    [Description("Элемент отображения дискретных данных")]
    public class CCurrentDataDiscret : BasicWithState
    {
        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"), PropertyGridFilter, DisplayName("Сетка"), Browsable(false)]
        public override bool ControlIs30Step
        {
            get => false;
        }

        //[Category("Свойства элемента мнемосхемы"), Description("Инвертировать значение дискретного тега"), PropertyGridFilterAttribute, DisplayName("Инвертировать дискрет"), Browsable(true)]
        //public bool InvertDiscret
        //{
        //    get => (bool)GetValue(InvertDiscretProperty);
        //    set => SetValue(InvertDiscretProperty, value);
        //}
        //public static DependencyProperty InvertDiscretProperty = DependencyProperty.Register("InvertDiscret", typeof(bool), typeof(CCurrentDataDiscret), new PropertyMetadata(false));
                
        internal protected RadialGradientBrush RadialGradientBrushMask;
        public CCurrentDataDiscret() : base()
        {
            this.TextNameFontSize = 18;
            this.TextNameColor = Color.FromRgb(255, 190, 0);
            this.ContentColor = Colors.Green;
            this.ContentColorAlternate = Colors.Gray;
            this.MarginTextName = new Thickness(0);
            this.ControlISHitTestVisible = true;


            RadialGradientBrushMask = new RadialGradientBrush() { GradientOrigin = new RelativePoint( new Point(0.4, 0.4), RelativeUnit.Absolute)};
            RadialGradientBrushMask.GradientStops.Add(new GradientStop(Colors.WhiteSmoke, 0.0));
            RadialGradientBrushMask.GradientStops.Add(new GradientStop(Colors.Transparent, .4));

        }

        public override string ElementTypeFriendlyName
        {
            get => "Элемент отображения дискретных данных";
        }
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        public override void Render(DrawingContext drawingContext)
        {
#warning Не лучший вариант, но по 104 Сашина библиотека выдаёт строки "0" и "1"
            bool isActiveState = false;// При настройке, пока ничего не привязано, рисуем цветом 
            if (TagDataMainState == null)
                isActiveState = true;
            else
            {
                if (TagDataMainState.TagValueString != null)
                    if (TagDataMainState.TagValueString.Equals("1"))// В работе если что-то привязано и там "1"
                        isActiveState = true;
            }
            
            var rotate = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);                
            drawingContext.DrawEllipse(isActiveState ? BrushContentColor : BrushContentColorAlternate, PenBlack1, new Point(15, 15), 10, 10);
            drawingContext.DrawEllipse(RadialGradientBrushMask, PenBlack, new Point(15, 15), 9, 9);  
                
            FormattedText ftTextName = new FormattedText(TextName, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Normal),
                TextNameFontSize, BrushTextNameColor);

            ftTextName.TextAlignment = TextAlignment.Left;
            drawingContext.DrawText(ftTextName, new Point(35, 0));
            rotate.Dispose();
        }

        protected override void DrawIsSelected(DrawingContext ctx)
        {
            if ((this.Content as Canvas).Bounds.Width>0 && ControlISSelected)
            {
                var transform = ctx.PushPostTransform(new RotateTransform(Angle).Value);
                ctx.DrawRectangle(BrushIsSelected, PenIsSelected, (this.Content as Canvas).Bounds);
                transform.Dispose();
            }
        }
        
        protected override void DrawMouseOver(DrawingContext ctx)
        {
            if ((this.Content as Canvas).Bounds.Width > 0)
            {
                var transform = ctx.PushPostTransform(new RotateTransform(Angle).Value);
                ctx.DrawRectangle(BrushMouseOver, PenMouseOver, (this.Content as Canvas).Bounds);
                transform.Dispose();
            }
        }

        protected override void DrawText(DrawingContext ctx)
        {
            
        }

        protected override void DrawQuality(DrawingContext ctx)
        {
            if (TagDataMainState != null)
            {
                if (TagDataMainState.Quality == TagValueQuality.Handled)
                {
                    var translate = ctx.PushPostTransform(new TranslateTransform(-15, 0).Value);
                    ctx.DrawGeometry(BrushContentColor, PenHand, HandGeometry());
                    translate.Dispose();
                }
                else if (TagDataMainState.Quality == TagValueQuality.Invalid)
                {

                    FormattedText ft = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                        new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Normal),
                        12, BrushContentColor);
                    ctx.DrawText(ft, new Point(-5,0));
                }
            }
        }
    }
}