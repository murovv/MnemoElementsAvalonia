using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using AvAp2.Interfaces;
using AvAp2.Models.SubControls;
using ReactiveUI;

namespace AvAp2.Models
{
    public abstract class BasicWithTextName:BasicMnemoElement, IBasicWithTextName
    {
        internal protected Pen PenBlack;
        internal protected Pen PenBlack1;
        internal protected Pen PenWhite1;
        [Category("Свойства элемента мнемосхемы"), Description("Диспетчерское наименование элемента"), PropertyGridFilterAttribute, DisplayName("Диспетчерское наименование"), Browsable(true)]
        public string TextName
        {
            get => (string)GetValue(TextNameProperty);
            set
            {
                SetValue(TextNameProperty, value);
                RiseMnemoNeedSave();
            }
        }
        

        public static StyledProperty<string> TextNameProperty =
            AvaloniaProperty.Register<BasicWithTextName, string>(nameof(TextName), defaultValue: "");

        //[Category("Свойства элемента мнемосхемы"), Description("Расширенное диспетчерское наименование элемента, отображаемое в подсказке"), PropertyGridFilterAttribute, DisplayName("Подсказка диспетчерского наименования"), Browsable(false)]
        //public string TextNameToolTip
        //{
        //    get => (string)GetValue(TextNameToolTipProperty);
        //    set
        //    {
        //        SetValue(TextNameToolTipProperty, value);
        //        RiseMnemoNeedSave();
        //    }
        //}
        //public static  DependencyProperty TextNameToolTipProperty = DependencyProperty.Register("TextNameToolTip", typeof(string), typeof(BasicWithTextName), new PropertyMetadata(""));

        [Category("Свойства элемента мнемосхемы"), Description("Отображать на схеме диспетчерское наименование"), PropertyGridFilterAttribute, DisplayName("Диспетчерское наименование видимость"), Browsable(true)]
        public bool TextNameISVisible
        {
            get => (bool)GetValue(TextNameISVisibleProperty);
            set
            {
                SetValue(TextNameISVisibleProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<bool> TextNameISVisibleProperty =
            AvaloniaProperty.Register<BasicWithTextName, bool>(nameof(TextNameISVisible), defaultValue: true);



        [Category("Свойства элемента мнемосхемы"), Description("Цвет текста диспетчерского наименования в 16-ричном представлении 0x FF(прозрачность) FF(красный) FF(зелёный) FF(синий)"), PropertyGridFilterAttribute, DisplayName("Текст цвет (hex)"), Browsable(true)]
        public Color TextNameColor
        {
            get => GetValue(TextNameColorProperty);
            set
            {
                SetValue(TextNameColorProperty,value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<Color> TextNameColorProperty = AvaloniaProperty.Register<BasicWithTextName, Color>(nameof(TextNameColor),Color.FromArgb(255, 0, 0, 0));

        public static void OnColorChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
        {
            (obj.Sender as BasicWithTextName).BrushTextNameColor = new SolidColorBrush(obj.NewValue.Value);
            (obj.Sender as BasicWithTextName).DrawingVisualText.InvalidateVisual();
            (obj.Sender as BasicWithTextName).SetTextBounds();
            
        }

        public static void OnTextChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            (obj.Sender as BasicWithTextName).DrawingVisualText.InvalidateVisual();
            (obj.Sender as BasicWithTextName).SetTextBounds();
            (obj.Sender as BasicWithTextName).DrawingIsSelected.InvalidateVisual();
            (obj.Sender as BasicWithTextName).DrawingIsSelected.InvalidateVisual();
            
        }
        [Category("Свойства элемента мнемосхемы"), Description("Ширина текстового поля диспетчерского наименования. По ширине будет происходить перенос по словам. Если не влезет слово - оно будет обрезано."), PropertyGridFilterAttribute, DisplayName("Текст ширина "), Browsable(true)]
        public virtual double TextNameWidth
        {
            get => (double)GetValue(TextNameWidthProperty);
            set
            {
                if (value > 0)
                {
                    SetValue(TextNameWidthProperty, value);
                    RiseMnemoNeedSave();
                }
            }
        }
        public double TextNameFontSize
        {
            get => (double)GetValue(TextNameFontSizeProperty);
            set
            {
                SetValue(TextNameFontSizeProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<double> TextNameFontSizeProperty = AvaloniaProperty.Register<BasicWithTextName,double>(nameof(TextNameFontSize), 18);


        public static StyledProperty<double> TextNameWidthProperty =
            AvaloniaProperty.Register<BasicWithTextName, double>(nameof(TextNameWidth),defaultValue:90);
        [Category("Свойства элемента мнемосхемы"), Description("Отступ диспетчерского наименования в формате (0,0,0,0). Через запятую отступ слева, сверху, справа, снизу. Отступ может быть отрицательным (-10). Имеет значение только отступ слева и сверху"), PropertyGridFilterAttribute, DisplayName("Текст отступ (дисп. наименование)"), Browsable(true)]
        public virtual Thickness MarginTextName
        {
            get => (Thickness)GetValue(MarginTextNameProperty);
            set
            {
                SetValue(MarginTextNameProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<Thickness> MarginTextNameProperty =
            AvaloniaProperty.Register<BasicWithTextName, Thickness>(nameof(MarginTextName), new Thickness(0, 0, 0, 0));
        public double AngleTextName
        {
            get => (double)GetValue(AngleTextNameProperty);
            set
            {
                SetValue(AngleTextNameProperty, value);
                RiseMnemoNeedSave();
            }
        }

        
        public static StyledProperty<double> AngleTextNameProperty = AvaloniaProperty.Register<BasicWithTextName, double>(nameof(AngleTextName),0.0);

        public static StyledProperty<Rect> TextBoundsProperty =
            AvaloniaProperty.Register<BasicWithTextName, Rect>(nameof(TextBounds), new Rect(0,0,0,0));
        public Rect TextBounds
        {
            get => GetValue(TextBoundsProperty);
            set=>SetValue(TextBoundsProperty, value);
            
        }
        static BasicWithTextName()
        {
            AffectsRender<BasicWithTextName>(MarginTextNameProperty);
            AffectsRender<BasicWithTextName>(TextNameISVisibleProperty);
            AffectsRender<BasicWithTextName>(TextNameProperty);
            TextNameColorProperty.Changed.Subscribe(OnColorChanged);
            TextNameProperty.Changed.Subscribe(OnTextChanged);
            TextNameISVisibleProperty.Changed.Subscribe(OnTextChanged);
            TextNameWidthProperty.Changed.Subscribe(OnTextChanged);
            TextNameFontSizeProperty.Changed.Subscribe(OnTextChanged);
            MarginTextNameProperty.Changed.Subscribe(OnTextChanged);
            AngleTextNameProperty.Changed.Subscribe(OnTextChanged);
        }
        
        

        public BasicWithTextName() : base()
        {
            BrushTextNameColor = new SolidColorBrush(TextNameColor);
            DrawingVisualText = new RenderCaller(DrawText)
            {
                Width = 0,
                Height = 0,
                ClipToBounds = false
            };
            ClipToBounds = false;
            PenBlack = new Pen(Brushes.Black, .5);
            PenBlack.ToImmutable(); 
            PenBlack1 = new Pen(Brushes.Black, 1);
            PenBlack1.ToImmutable();
            PenWhite1 = new Pen(Brushes.WhiteSmoke, 1);
            PenWhite1.ToImmutable();
            if (this.Content is null)
            {
                Content = new Canvas();
            }
            (this.Content as Canvas).Children.Add(DrawingVisualText);
            DrawingVisualText.Loaded+= DrawingVisualTextOnLoaded;
        }

        private void DrawingVisualTextOnLoaded(object? sender, RoutedEventArgs e)
        {
            DrawingVisualText.InvalidateVisual();
            SetTextBounds();
            DrawingMouseOver.InvalidateVisual();
            DrawingIsSelected.InvalidateVisual();
        }

        protected void SetTextBounds()
        {
            var drawingText = this.DrawingVisualText;
            var control = this;
            if(Math.Abs(drawingText.Width - control.TextBounds.Width) > 10e-6)
                drawingText.Width = control.TextBounds.Width;
            if(Math.Abs(drawingText.Height - control.TextBounds.Height) > 10e-6)
                drawingText.Height = control.TextBounds.Height;
            if (Math.Abs(drawingText.Bounds.X - control.MarginTextName.Left) > 10e-6 ||
                Math.Abs(drawingText.Bounds.Y - control.MarginTextName.Top) > 10e-6)
            {
                drawingText.Margin = new Thickness(control.MarginTextName.Left, control.MarginTextName.Top);
            }
                
        }
        
        


        internal protected Brush BrushTextNameColor;

        public Control DrawingVisualText { get; set; }
        

        protected virtual void DrawText(DrawingContext ctx)
        {
            if (TextNameISVisible && !String.IsNullOrEmpty(TextName))
            {
                
                FormattedText ft = new FormattedText(TextName, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                        new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Expanded),
                        TextNameFontSize, BrushTextNameColor);
                
                ft.MaxTextWidth = TextNameWidth > 10 ? TextNameWidth: 10;
                ft.TextAlignment = TextAlignment.Center;
                ft.Trimming = TextTrimming.None;
                //var translate = ctx.PushPostTransform(new TranslateTransform(MarginTextName.Left, MarginTextName.Top).Value);
                var rotate = ctx.PushPostTransform(new RotateTransform(AngleTextName, ft.Width/2, ft.Height/2).Value);
                ctx.DrawText(ft, new Point(0, 0));
                rotate.Dispose();
                //translate.Dispose();
                if (Math.Abs(TextBounds.Width - ft.Width) > 10e-6 || Math.Abs(TextBounds.Height - ft.Height) > 10e-6)
                {
                    TextBounds = new Rect(MarginTextName.Left, MarginTextName.Top, ft.Width, ft.Height);
                }
            }
        }
        
        
        internal protected bool IsModifyPressed = false;
        internal protected bool IsTextPressed = false;
        internal protected Point ModifyStartPoint = new Point(0, 0);
        internal protected bool IsResizerPressed = false;

        
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            ModifyStartPoint = e.GetPosition(this);
#warning в авалонии 11 хит тест начал работать по  другому, так что пока так
            if (DrawingVisualText.IsPointerOver)
            {
                IsTextPressed = IsModifyPressed = true;
                IsResizerPressed = false;
                e.Handled = true;
            }
        }



        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            if (IsModifyPressed)
            {
                IsTextPressed = IsModifyPressed = false;
            }
        }

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
                DrawingVisualText.InvalidateVisual();
                SetTextBounds();
                DrawingIsSelected.InvalidateVisual();
                DrawingMouseOver.InvalidateVisual();
                
            }
        }
        
        protected override void DrawIsSelected(DrawingContext ctx)
        {
            if (ControlISSelected)
            {
                var transform = ctx.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
                if (DrawingVisualText.Bounds.Width > 0)
                {
                    ctx.DrawRectangle(BrushIsSelected, PenIsSelected, DrawingVisualText.Bounds);
                }
                ctx.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(0,0,30,30));
                transform.Dispose();
            }
        }
        
        protected override void DrawMouseOver(DrawingContext ctx)
        {
            var transform = ctx.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
            if (DrawingVisualText.Bounds.Width > 0)
            {
                ctx.DrawRectangle(BrushMouseOver, PenMouseOver, DrawingVisualText.Bounds);
            }
            ctx.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(0,0,30,30));
            transform.Dispose();
        }
    }
}