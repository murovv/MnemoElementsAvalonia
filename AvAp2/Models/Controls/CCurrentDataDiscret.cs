using System.ComponentModel;
using System.Globalization;
using Avalonia;
using Avalonia.Media;

namespace AvAp2.Models
{
    [Description("Элемент отображения дискретных данных")]
    public class CCurrentDataDiscret : BasicWithState
    {
        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"), PropertyGridFilterAttribute, DisplayName("Сетка"), Browsable(false)]
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
            if (Bounds.Width>0 && ControlISSelected)
            {
                var transform = ctx.PushPostTransform(new RotateTransform(Angle).Value);
                ctx.DrawRectangle(BrushIsSelected, PenIsSelected, Bounds);
                transform.Dispose();
            }
        }
        
        protected override void DrawMouseOver(DrawingContext ctx)
        {
            if (Bounds.Width > 0)
            {
                var transform = ctx.PushPostTransform(new RotateTransform(Angle).Value);
                ctx.DrawRectangle(BrushMouseOver, PenMouseOver, Bounds);
                transform.Dispose();
            }
        }

        protected override void DrawText(DrawingContext ctx)
        {
            DrawingVisualText.Opacity = 0;
        }

        protected override void DrawQuality()
        {
            if (TagDataMainState != null)
            {
                if (TagDataMainState.Quality == TagValueQuality.Handled)
                {
                    StreamGeometry geometry = HandGeometry();
                    geometry.Transform = new TranslateTransform(-15, 0);
                    DrawingQuality.Geometry = geometry;
                    DrawingQuality.Brush = BrushContentColor;
                    DrawingQuality.Pen = PenHand;
                }

                else if (TagDataMainState.Quality == TagValueQuality.Invalid)
                {
                    FormattedText ft = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                        new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Normal),
                        12, BrushContentColor);
                    DrawingQuality.Geometry = ft.BuildGeometry(new Point(-5, 0));
                    DrawingQuality.Brush = BrushContentColor;
                }
                else
                {
                    DrawingQuality.Geometry = new StreamGeometry();
                }
            }
            DrawingQualityWrapper.Source = new DrawingImage(DrawingQuality);
            DrawingQualityWrapper.RenderTransform =
                new MatrixTransform(
                    new RotateTransform(Angle, 15, 15).Value.Prepend(new TranslateTransform(-10, -10).Value));
        }
    }
}