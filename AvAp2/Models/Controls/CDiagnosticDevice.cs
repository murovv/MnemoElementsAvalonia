using System;
using System.ComponentModel;
using System.Globalization;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using AvAp2.Interfaces;

namespace AvAp2.Models
{
    /// <summary>
    /// Базовый класс всех контролов схемы диагностики.
    /// </summary>
    [Description("Устройство - элемент диагностической схемы")]
    public class CDiagnosticDevice : CRectangle, IBasicWithState, IConnector
    {
        // Скрываем чтобы не давать править, всегда пишем внизу по центру прямоугольника
        [Browsable(false)]
        public override Thickness MarginTextName
        {
            get => (Thickness)GetValue(MarginTextNameProperty);
        }

        // Скрываем чтобы не давать править, всегда пишем внизу по ширине прямоугольника
        [Browsable(false)]
        public override double TextNameWidth
        {
            get => (double)GetValue(TextNameWidthProperty);
        }


        #region IConnector
        [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителя левого "), PropertyGridFilterAttribute, DisplayName("Видимость соединителя левого "), Browsable(false)]
        public bool IsConnectorExistLeft
        {
            get => (bool)GetValue(IsConnectorExistLeftProperty);
            set
            {
                SetValue(IsConnectorExistLeftProperty, value);
                //RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> IsConnectorExistLeftProperty = AvaloniaProperty.Register<CDiagnosticDevice, bool>(nameof(IsConnectorExistLeft), false);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителя правого "), PropertyGridFilterAttribute, DisplayName("Видимость соединителя правого "), Browsable(true)]
        public bool IsConnectorExistRight
        {
            get => (bool)GetValue(IsConnectorExistRightProperty);
            set
            {
                SetValue(IsConnectorExistRightProperty, value);
                //RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> IsConnectorExistRightProperty = AvaloniaProperty.Register<CDiagnosticDevice, bool>(nameof(IsConnectorExistRight), false);
        
        #endregion IConnector


        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"), PropertyGridFilterAttribute, DisplayName("Сетка"), Browsable(false)]
        public override bool ControlIs30Step
        {
            get => false;
        }

        [Category("Свойства элемента мнемосхемы"), Description("Цвет элемента в 16-ричном представлении 0x FF(прозрачность) FF(красный) FF(зелёный) FF(синий)"), PropertyGridFilterAttribute, DisplayName("Цвет заливки"), Browsable(true)]
        public Color BackColor
        {
            get => (Color)GetValue(BackColorProperty);
            set => SetValue(BackColorProperty, value);
        }
        public static StyledProperty<Color> BackColorProperty = AvaloniaProperty.Register<CDiagnosticDevice, Color>(nameof(BackColor), Color.FromArgb(255, 11, 100, 141));

        [Category("Свойства элемента мнемосхемы"), Description("Имя файла изображения, файл должен лежать в папке Assets"), PropertyGridFilterAttribute, DisplayName("Изображение"), Browsable(true)]
        public string ImageFileName
        {
            get => (string)GetValue(ImageFileNameProperty);
            set
            {
                SetValue(ImageFileNameProperty, value);
                //RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> ImageFileNameProperty = AvaloniaProperty.Register<CDiagnosticDevice, string>(nameof(ImageFileName), "HyperLink.png");
        private void OnASUImageFileNamePropertyChanged(AvaloniaPropertyChangedEventArgs<string> obj)
        {
            var assests = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            if (obj.NewValue.Value.Length > 0)
            {
                try
                {
                    
                    var img = new Bitmap(assests.Open(new Uri($@"avares://{name}/Assets/{obj.NewValue.Value}")));
                   
                    ImageSource = img;
                    return;
                }
                catch (Exception) { }// Если по какой-либо причине не удалось, установим по умолчанию
            }
            try
            {
                Bitmap imgDef = new Bitmap(assests.Open(new Uri($@"avares://{name}/Assets/Device.png")));
                ImageSource = imgDef;
                return;
            }
            catch (Exception) { }// Если по какой-либо причине не удалось, установим по умолчанию
            ImageSource = null;
        }

        [Category("Свойства элемента мнемосхемы"), Description("Источник изображения"), PropertyGridFilterAttribute, DisplayName("Изображение"), Browsable(false)]
        private Bitmap ImageSource
        {
            get => (Bitmap)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }
        public static StyledProperty<Bitmap> ImageSourceProperty = AvaloniaProperty.Register<CDiagnosticDevice, Bitmap>("ImageSource", new Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>().Open(new Uri($@"avares://{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}/Assets/HyperLink.png"))));

        public CDiagnosticDevice() : base()
        {
            this.CoordinateX2 = 90;
            this.CoordinateY2 = 30;
            this.ControlISHitTestVisible = true;
            this.LineThickness = 3;
        }

        public override string ElementTypeFriendlyName
        {
            get => "Терминал";
        }
        
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        #region Рисование

        /// <summary>
        /// Метод отрисовки качества сигнала состояния самого элемента (плохое/ручной ввод)
        /// </summary>
        protected override void DrawQuality()
        {
            if (TagDataMainState != null)
            {
                if (TagDataMainState.Quality == TagValueQuality.Handled)
                {
                    StreamGeometry geometry = HandGeometry();
                    geometry.Transform = new TranslateTransform(-10, -10);
                    DrawingQuality.Geometry = geometry;
                    DrawingQuality.Brush = BrushContentColor;
                    DrawingQuality.Pen = PenHand;
                }
                else if (TagDataMainState.Quality == TagValueQuality.Invalid)
                {

                        FormattedText ft = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                            new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Normal),
                            12, BrushContentColor);
                        DrawingQuality.Geometry = ft.BuildGeometry(new Point(-10, -10));
                        DrawingQuality.Brush = BrushContentColor;
                }
            }
        }

        public override void Render(DrawingContext drawingContext)
        {
#warning Не лучший вариант, но по 104 Сашина библиотека выдаёт строки "0" и "1"
            bool isActiveState = false;// При настройке, пока ничего не привязано, рисуем цветом класса напряжения
            if (TagDataMainState == null)
                isActiveState = true;
            else
            {
                if (TagDataMainState.TagValueString != null)
                    if (TagDataMainState.TagValueString.Equals("1"))// В работе если что-то привязано и там "1"
                        isActiveState = true;
            }
            var rotate = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
                
            drawingContext.DrawRectangle(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, new Rect(0, 0, CoordinateX2>0? CoordinateX2 : 1, CoordinateY2 > 0 ? CoordinateY2 : 1));
            if (IsConnectorExistRight)
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(CoordinateX2 > 0 ? CoordinateX2 : 1, 15), new Point(CoordinateX2 > 0 ? CoordinateX2 + 15 : 16, 15));
            if (ImageSource != null)
            {
                var maxY = CoordinateY2 > TextNameFontSize * 3 ? CoordinateY2 - TextNameFontSize * 3 : 30;
                var ratioX = CoordinateX2 / ImageSource.PixelSize.Width;
                var ratioY = maxY / ImageSource.PixelSize.Height;
                var ratio = Math.Min(ratioX, ratioY);

                var imgWidth = ImageSource.PixelSize.Width * ratio;
                var imgHeight = ImageSource.PixelSize.Height * ratio;

                var offsetX = (CoordinateX2 - imgWidth) / 2;
                var offsetY = (maxY - imgHeight) / 2;

                drawingContext.DrawImage(ImageSource, new Rect(offsetX, offsetY, imgWidth, imgHeight));
            }
            //drawingContext.DrawImage(ImageSource, new Rect(1, 1, CoordinateX2 > 30 ? CoordinateX2 : 30, CoordinateY2  > TextNameFontSize ? CoordinateY2 - TextNameFontSize : 30));
            rotate.Dispose();
        }

        protected override void DrawIsSelected()
        {
            if (ControlISSelected)
            {
                DrawingIsSelected.Geometry = new RectangleGeometry(new Rect(-1, -1, CoordinateX2 > 0 ? CoordinateX2+2 : 1, CoordinateY2 > 0 ? CoordinateY2+2 : 1));
                var ellipse = new EllipseGeometry();
                
                ellipse.Center = new Point(CoordinateX2, CoordinateY2);
                ellipse.RadiusX = ellipse.RadiusY = 3;
                DrawingResizer.Geometry = ellipse;

            }
            else
            {
                DrawingIsSelected.Geometry = new GeometryGroup();
                DrawingResizer.Geometry = new GeometryGroup();
            }

            DrawingIsSelected.Brush = BrushIsSelected;
            DrawingIsSelected.Pen = PenIsSelected;
            DrawingResizer.Brush = Brushes.WhiteSmoke;
            DrawingResizer.Pen = new Pen(Brushes.WhiteSmoke);
        }

        protected override void DrawMouseOver()
        {
            DrawingMouseOver.Geometry = new RectangleGeometry(new Rect(-1, -1, CoordinateX2 > 0 ? CoordinateX2+2 : 1, CoordinateY2 > 0 ? CoordinateY2+2 : 1));
            DrawingMouseOver.Brush = BrushMouseOver;
            DrawingMouseOver.Pen = PenMouseOver;
        }
        
        protected override void DrawText()
        {
            if (TextNameISVisible)
            {
                DrawingVisualText.Text = TextName;
                DrawingVisualText.MaxWidth = TextNameWidth > 10 ? TextNameWidth : 10;
                DrawingVisualText.TextWrapping = TextWrapping.Wrap;
                DrawingVisualText.FontFamily = new FontFamily("Segoe UI");
                DrawingVisualText.FontStyle = FontStyle.Normal;
                DrawingVisualText.FontWeight = FontWeight.SemiBold;
                DrawingVisualText.FontSize = 14;
                DrawingVisualText.TextAlignment = TextAlignment.Center;
                // DrawingVisualText.RenderTransform = new TranslateTransform(MarginTextName.Left, MarginTextName.Top);
                DrawingVisualText.Margin = MarginTextName;
                /*drawingContext.
                drawingContext.PushTransform(new TranslateTransform(MarginTextName.Left, MarginTextName.Top));
                drawingContext.PushTransform(new RotateTransform(AngleTextName));*/
                DrawingVisualText.Opacity = 1;
                DrawingVisualText.Padding = new Thickness(0, CoordinateY2 > TextNameFontSize*3 ? CoordinateY2 - TextNameFontSize*3 : 30);
            }
            else
                DrawingVisualText.Opacity = 0;
        }

        #region Изменение размеров
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (IsModifyPressed && ControlISSelected)
            {
                Point currentPoint = e.GetPosition(this);
                double dX = currentPoint.X - ModifyStartPoint.X;
                double dY = currentPoint.Y - ModifyStartPoint.Y;
                
                if (IsTextPressed)
                {
                    ModifyStartPoint = currentPoint;
                    MarginTextName = new Thickness(MarginTextName.Left + dX, MarginTextName.Top + dY, 0, 0);
                    RiseMnemoMarginChanged(nameof(MarginTextName));
                }
                if (IsResizerPressed)
                {
                    #region перетаскивание
                    int deltaStep = 30;

                    int deltaX = (int)(currentPoint.X - ModifyStartPoint.X) / deltaStep;
                    int deltaY = (int)(currentPoint.Y - ModifyStartPoint.Y) / deltaStep;

                    if ((Math.Abs(deltaX) > 0) || ((Math.Abs(deltaY) > 0)))
                    {
                        double resizerDX = 0;
                        if (CoordinateX2 + (deltaX * deltaStep) > 0)
                        {
                            resizerDX = (deltaX * deltaStep);
                            SetValue(CoordinateX2Property, CoordinateX2 + resizerDX); 
                        }
                        double resizerDY = 0;
                        if (CoordinateY2 + (deltaY * deltaStep) > 0)
                        {
                            resizerDY = (deltaY * deltaStep);
                            SetValue(CoordinateY2Property, CoordinateY2 + resizerDY);
                        }

                        ModifyStartPoint = new Point(ModifyStartPoint.X + resizerDX, ModifyStartPoint.Y + resizerDY);
                    }
                    #endregion перетаскивание
                }
                InvalidateStyles();
            }
        }
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            ModifyStartPoint = e.GetPosition(this);
            if (IsPointInResizer(e.GetPosition(this)))
            {
                IsResizerPressed = IsModifyPressed = true;
                IsTextPressed = false;
            }
        }
        

        #endregion Изменение размеров

        #endregion Рисование
    }
}