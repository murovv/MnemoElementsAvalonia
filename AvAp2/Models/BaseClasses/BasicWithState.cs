using System.ComponentModel;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using AvAp2.Interfaces;

namespace AvAp2.Models
{
    public abstract class BasicWithState:BasicWithColor, IBasicWithState
    {
        static BasicWithState()
        {
            AffectsRender<BasicWithState>(TagDataMainStateProperty);
            AffectsRender<BasicWithState>(TdiStateStringProperty);
            AffectsRender<BasicWithState>(TagIDMainStateProperty);
        }
        public BasicWithState() : base()
        { 
            DrawingQuality = new GeometryDrawing();
            DrawingQualityWrapper = new Image();
            if (Content is null)
            {
                Content = new Canvas();
            }
            (Content as Canvas).Children.Add(DrawingQualityWrapper);
            Loaded+= OnLoaded;
            DrawingVisualText.Loaded+= DrawingVisualTextOnLoaded;
            
        }

        private void DrawingVisualTextOnLoaded(object? sender, RoutedEventArgs e)
        {
            DrawingMouseOver.InvalidateVisual();
            DrawingIsSelected.InvalidateVisual();
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            DrawText();
            DrawQuality();
        }

        public string TdiStateString
        {
            get => GetValue(TdiStateStringProperty);
            set => SetValue(TdiStateStringProperty, value);
        }
        public static readonly StyledProperty<string> TdiStateStringProperty = AvaloniaProperty.Register<BasicWithState, string>(nameof(TdiStateString));


        [Category("Привязки данных"), Description("Идентификатор тега состояния элемента. Для текущих данных - ток или дискрет, для оборудования - наличие напряжения для цвета"), DisplayName("ID тега состояния"), Browsable(true)]
        public string TagIDMainState
        {
            get => (string)GetValue(TagIDMainStateProperty);
            set => SetValue(TagIDMainStateProperty, value);
        }
        public static readonly StyledProperty<string> TagIDMainStateProperty = AvaloniaProperty.Register<BasicWithState, string>(nameof(TagIDMainState));

        public TagDataItem TagDataMainState
        {
            get => (TagDataItem)GetValue(TagDataMainStateProperty);
            set     
            {
                TagDataItem oldValue = GetValue(TagDataMainStateProperty);
                if (oldValue != value)
                {
                    if (oldValue != null)
                        oldValue.PropertyChanged -= Value_PropertyChanged;
                    if (value != null)
                        value.PropertyChanged += Value_PropertyChanged;
                }
                SetValue(TagDataMainStateProperty, value);
            }
        }

        public static StyledProperty<TagDataItem> TagDataMainStateProperty = AvaloniaProperty.Register<BasicWithState, TagDataItem>(nameof(TagDataMainState));


        private void Value_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null)
            {
                if (e.PropertyName.Equals(nameof(TagDataItem.Quality)))
                {
                    DrawQuality();
                }else if (e.PropertyName.Equals(nameof(TagDataItem.TagValueString)))
                {
                    InvalidateVisual();
                }
            }
        }
        
        public GeometryDrawing DrawingQuality { get; protected set; }

        public Image DrawingQualityWrapper { get; set; }

        protected virtual void DrawQuality()
        {
            if (TagDataMainState != null)
            {
                if (TagDataMainState.Quality == TagValueQuality.Handled)
                {
                    StreamGeometry geometry = HandGeometry();
                    DrawingQuality.Geometry = geometry;
                    DrawingQuality.Brush = BrushContentColor;
                    DrawingQuality.Pen = PenHand;
                }
                else if (TagDataMainState.Quality == TagValueQuality.Invalid)
                {

                    FormattedText ft = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                        new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Normal),
                        12, BrushContentColor);

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