using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using AvAp2.Interfaces;

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

        public void OnColorChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
        {
            (obj.Sender as BasicWithTextName).BrushTextNameColor = new SolidColorBrush(obj.NewValue.Value);
            DrawText();
            
        }

        public void OnTextChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            
            DrawText();
            DrawIsSelected();
            DrawMouseOver();
            
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

        //BUG нормально MouseOver отрисовывается только после второго наведения
        static BasicWithTextName()
        {
            AffectsRender<BasicWithTextName>(MarginTextNameProperty);
            AffectsRender<BasicWithTextName>(TextNameISVisibleProperty);
            AffectsRender<BasicWithTextName>(TextNameProperty);
        }
        public BasicWithTextName() : base()
        {
            DrawingMouseOverWrapper.RenderTransform =
                new MatrixTransform(
                    new RotateTransform(AngleTextName).Value.Append(
                        new TranslateTransform(MarginTextName.Left, MarginTextName.Top).Value));
            DrawingIsSelectedWrapper.RenderTransform = new MatrixTransform(
                new RotateTransform(AngleTextName).Value.Append(
                    new TranslateTransform(MarginTextName.Left, MarginTextName.Top).Value));
            BrushTextNameColor = new SolidColorBrush(TextNameColor);
            DrawingVisualText = new TextBlock();
            DrawingVisualText.ClipToBounds = false;
            ClipToBounds = false;
            DrawingVisualText.Loaded+= DrawingVisualTextOnLoaded;
            #region subscribtions
            TextNameColorProperty.Changed.Subscribe(OnColorChanged);
            TextNameProperty.Changed.Subscribe(OnTextChanged);
            TextNameISVisibleProperty.Changed.Subscribe(OnTextChanged);
            TextNameWidthProperty.Changed.Subscribe(OnTextChanged);
            TextNameFontSizeProperty.Changed.Subscribe(OnTextChanged);
            MarginTextNameProperty.Changed.Subscribe(OnTextChanged);
            AngleTextNameProperty.Changed.Subscribe(OnTextChanged);
            #endregion
            //this.Content = DrawingVisualText;
            /*TextNameColorProperty.Changed.AddClassHandler<BasicWithTextName>(x => x.OnColorChanged);
            MarginTextNameProperty.Changed.AddClassHandler<BasicWithTextName>(x => x.OnTextChanged);*/
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
            Loaded+= OnLoaded;
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            DrawText();
            
        }

        internal protected Brush BrushTextNameColor;

        public TextBlock DrawingVisualText { get; set; } = new TextBlock();
        

        protected virtual void DrawText()
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
                // DrawingVisualText.RenderTransform = ;
                DrawingVisualText.Margin = MarginTextName;
                /*Matrix transform = new TranslateTransform(MarginTextName.Left, MarginTextName.Top).Value;
                transform.Prepend();*/
                DrawingVisualText.Opacity = 1;
                DrawingVisualText.RenderTransform = new RotateTransform(AngleTextName);
            }
            else
                DrawingVisualText.Opacity = 0;
            
        }

        private void DrawingVisualTextOnLoaded(object? sender, RoutedEventArgs e)
        {
            DrawText();
            DrawIsSelected();
            DrawMouseOver();
        }
        
        internal protected bool IsModifyPressed = false;
        internal protected bool IsTextPressed = false;
        internal protected Point ModifyStartPoint = new Point(0, 0);
        internal protected bool IsResizerPressed = false;

        
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            ModifyStartPoint = e.GetPosition(this);
#warning в авалонии 11 хит тест начал работать по  другому, так что пока так
            if (DrawingVisualText.Bounds.Contains(ModifyStartPoint))
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
                DrawIsSelected();
                DrawMouseOver();
            }
        }


        protected override void DrawMouseOver()
        {
            var geometryGroup = new GeometryGroup
            {
                FillRule = FillRule.NonZero
            };
            /*if (DrawingVisualText.Bounds.Height > 0)
            {
                geometryGroup.Children.Add( new RectangleGeometry(DrawingVisualText.Bounds));
                // DrawingMouseOver.Geometry.Transform = new RotateTransform(AngleTextName);
            }*/
            geometryGroup.Children.Add(new RectangleGeometry(new Rect(0,0,29,29)));
            DrawingMouseOver.Geometry = geometryGroup;
            DrawingMouseOver.Brush = BrushMouseOver;
            DrawingMouseOver.Pen = PenMouseOver;
            DrawingMouseOverWrapper.Source = new DrawingImage(DrawingMouseOver);
            DrawingMouseOverWrapper.RenderTransform = new RotateTransform(Angle);
        }
        }
}