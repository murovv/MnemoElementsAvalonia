using System.ComponentModel;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Styling;


namespace AvAp2.Models
{
    [Description("Элемент отображения аналоговых данных")]
    public class CCurrentDataAnalog : BasicWithState
    {
        
        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"), PropertyGridFilterAttribute, DisplayName("Сетка"), Browsable(false)]
        public override bool ControlIs30Step
        {
            get => false;
        }
        [Category("Свойства элемента мнемосхемы"), Description("Диспетчерское наименование элемента"), PropertyGridFilterAttribute, DisplayName("Диспетчерское наименование"), Browsable(true)]
        public string TextUom
        {
            get => (string)GetValue(TextUomProperty);
            set => SetValue(TextUomProperty, value);
        }
        public static StyledProperty<string> TextUomProperty = AvaloniaProperty.Register<CCurrentDataAnalog,string>(nameof(TextUom), "");
        //TODO разобраться как сделать без этого тупняка
        public TagValueQuality Quality
        {
            get => (TagValueQuality)GetValue(QualityProperty);
            set => SetValue(QualityProperty, value);
        }
        public static AttachedProperty<TagValueQuality> QualityProperty =
            AvaloniaProperty.RegisterAttached<TagDataItem, CCurrentDataAnalog, TagValueQuality>("Quality");
        
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
                InvalidateStyles();
            }
        }
        

        static CCurrentDataAnalog()
        {
            AffectsRender<CCurrentDataAnalog>(QualityProperty);
        }
        public CCurrentDataAnalog() : base()
        {
            DataContext = this;
            this.TextNameFontSize = 18;
            this.TextNameColor = Color.FromRgb(255, 190, 0);
            this.ContentColor = Colors.LimeGreen;
            this.ContentColorAlternate = Colors.Gray;
            this.MarginTextName = new Thickness(0);
            this.ControlISHitTestVisible = true;
        }

        public override string ElementTypeFriendlyName
        {
            get => "Элемент отображения аналоговых данных";
        }
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        public override void Render(DrawingContext drawingContext)
        {
#warning Не лучший вариант, но по 104 Сашина библиотека выдаёт строки "0" и "1"
            string dataValue = "?"; 
            if (TagDataMainState != null)           
                if (TagDataMainState.TagValueString != null)
                    dataValue = TagDataMainState.TagValueString;

            DrawingContext.PushedState rotate;
            using (rotate = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value))
            {
                FormattedText ftTextName = new FormattedText(TextName, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Normal),
                    TextNameFontSize, BrushTextNameColor);

                ftTextName.TextAlignment = TextAlignment.Left;
                drawingContext.DrawText(ftTextName, new Point(4, 0));
                FormattedText ftValue = new FormattedText(dataValue, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Normal),
                    TextNameFontSize, BrushContentColor);

                ftValue.TextAlignment = TextAlignment.Left;                
                drawingContext.DrawText(ftValue, new Point(ftTextName.Width + 9, 0));
                if (false == string.IsNullOrEmpty(TextUom))
                {
                    FormattedText ftUoM = new FormattedText(TextUom, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                        new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Normal),
                        TextNameFontSize, BrushTextNameColor);

                    ftUoM.TextAlignment = TextAlignment.Left;
                    drawingContext.DrawText(ftUoM, new Point(ftTextName.Width + ftValue.Width + 14, 0));
                }
            }
            
        }
        //TODO 
        /*internal protected override void DrawIsSelected()
        {
            using (var drawingContext = DrawingVisualIsSelected.RenderOpen())
            {
                drawingContext.PushTransform(new RotateTransform(Angle, 15, 15));                
                if (DrawingVisualBase.ContentBounds.Width > 0)
                {
                    Rect selectedRect = DrawingVisualBase.ContentBounds;
                    drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, selectedRect);
                }
                drawingContext.Close();
            }
            DrawingVisualIsSelected.Opacity = ControlISSelected ? .3 : 0;
        }

        internal protected override void DrawMouseOver()
        {
            using (var drawingContext = DrawingVisualIsMouseOver.RenderOpen())
            {
                drawingContext.PushTransform(new RotateTransform(Angle, 15, 15));
                
                if (DrawingVisualBase.ContentBounds.Width > 0)
                {
                    Rect selectedRect = DrawingVisualBase.ContentBounds;
                    drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, selectedRect);
                }
                drawingContext.Close();
            }
            DrawingVisualIsMouseOver.Opacity = 0;
        }

        internal protected override void DrawText()
        {
            DrawingVisualText.Opacity = 0;
        }

        internal protected override void DrawBaseQuality()
        {
            if (TagDataMainState != null)
            {
                if (TagDataMainState.Quality == (TagValueQuality)IProjectModel.TagValueQuality.Handled)
                {
                    StreamGeometry geometry = HandGeometry();
                    geometry.Transform = new TranslateTransform(-15, 0);
                    geometry.Freeze();
                    using (var drawingContext = DrawingVisualQuality.RenderOpen())
                    {
                        drawingContext.DrawGeometry(BrushHand, PenHand, geometry);
                        drawingContext.Close();
                    }
                }
                else if (TagDataMainState.Quality == IProjectModel.TagValueQuality.Invalid)
                {
                    using (var drawingContext = DrawingVisualQuality.RenderOpen())
                    {
                        FormattedText ft = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                            new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                            12, Brushes.Yellow, null, TextFormattingMode.Ideal);

                        drawingContext.DrawText(ft, new Point(-5, 0));
                        drawingContext.Close();
                    }
                }
                else
                {
                    using (var drawingContext = DrawingVisualQuality.RenderOpen())
                    {
                        drawingContext.Close();
                    }
                }
            }
        }*/
    }
}