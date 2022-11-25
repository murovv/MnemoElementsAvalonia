using System.ComponentModel;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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
        //TODO оно и не должно влиять на рендер? пока сделаю чтоб влиял.
        public static StyledProperty<string> TextUomProperty = AvaloniaProperty.Register<CCurrentDataAnalog,string>(nameof(TextUom), "");

       
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
        

        static CCurrentDataAnalog()
        {
            AffectsRender<CCurrentDataAnalog>(TextUomProperty);
        }
        public CCurrentDataAnalog() : base()
        {
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
        protected override void DrawIsSelected()
        {
            if (this.Bounds.Width > 0 && ControlISSelected)
            {
                DrawingIsSelected.Geometry = new RectangleGeometry(this.Bounds);
            }

            DrawingIsSelected.Brush = BrushIsSelected;
            DrawingIsSelected.Pen = PenIsSelected;
            DrawingIsSelectedWrapper.Source = new DrawingImage(DrawingIsSelected);
            DrawingIsSelectedWrapper.RenderTransform = new RotateTransform(Angle);
        }
        
        protected override void DrawMouseOver()
        {
            
            if (this.Bounds.Width > 0)
            {
                DrawingMouseOver.Geometry = new RectangleGeometry(this.Bounds);
            }

            DrawingMouseOver.Brush = BrushMouseOver;
            DrawingMouseOver.Pen = PenMouseOver;
            DrawingMouseOverWrapper.Source = new DrawingImage(DrawingMouseOver);
            DrawingMouseOverWrapper.RenderTransform = new RotateTransform(Angle);
        }
        protected override void DrawText()
        {
            DrawingVisualText.Opacity = 0;
        }
    }
}