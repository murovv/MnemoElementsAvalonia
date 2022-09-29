using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace AvAp2.Models
{
    public abstract class BasicWithTextName:BasicMnemoElement
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
                //RiseMnemoNeedSave();
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
                //RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<bool> TextNameISVisibleProperty =
            AvaloniaProperty.Register<BasicWithTextName, bool>(nameof(TextNameISVisible), defaultValue: true);

        private Color _textNameColor = Color.FromArgb(255, 0, 0, 0);

        [Category("Свойства элемента мнемосхемы"), Description("Цвет текста диспетчерского наименования в 16-ричном представлении 0x FF(прозрачность) FF(красный) FF(зелёный) FF(синий)"), PropertyGridFilterAttribute, DisplayName("Текст цвет (hex)"), Browsable(true)]
        public Color TextNameColor
        {
            get => _textNameColor;
            set
            {
                SetAndRaise(TextNameColorProperty,ref _textNameColor, value);
                //RiseMnemoNeedSave();
            }
        }
        public static DirectProperty<BasicWithTextName,Color> TextNameColorProperty = AvaloniaProperty.RegisterDirect<BasicWithTextName, Color>(nameof(TextNameColor),x=>x.TextNameColor);
        [Category("Свойства элемента мнемосхемы"), Description("Ширина текстового поля диспетчерского наименования. По ширине будет происходить перенос по словам. Если не влезет слово - оно будет обрезано."), PropertyGridFilterAttribute, DisplayName("Текст ширина "), Browsable(true)]
        public virtual double TextNameWidth
        {
            get => (double)GetValue(TextNameWidthProperty);
            set
            {
                if (value > 0)
                {
                    SetValue(TextNameWidthProperty, value);
                    //RiseMnemoNeedSave();
                }
            }
        }

        public static StyledProperty<double> TextNameWidthProperty =
            AvaloniaProperty.Register<BasicWithTextName, double>(nameof(TextNameWidth),defaultValue:90);
        [Category("Свойства элемента мнемосхемы"), Description("Отступ диспетчерского наименования в формате (0,0,0,0). Через запятую отступ слева, сверху, справа, снизу. Отступ может быть отрицательным (-10). Имеет значение только отступ слева и сверху"), PropertyGridFilterAttribute, DisplayName("Текст отступ (дисп. наименование)"), Browsable(true)]
        public virtual Thickness MarginTextName
        {
            get => (Thickness)GetValue(MarginTextNameProperty);
            set
            {
                SetValue(MarginTextNameProperty, value);
                //RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<Thickness> MarginTextNameProperty =
            AvaloniaProperty.Register<BasicWithTextName, Thickness>(nameof(MarginTextName), new Thickness(0, 0, 0, 0));
        public BasicWithTextName() : base()
        {
            AffectsRender<BasicWithTextName>(MarginTextNameProperty);
            AffectsRender<BasicWithTextName>(TextNameISVisibleProperty);
            AffectsRender<BasicWithTextName>(TextNameProperty);
            DrawingVisualText = new TextBlock();
            
            //this.Content = DrawingVisualText;
            TextNameColorProperty.Changed.AddClassHandler<BasicWithTextName>(x => x.OnColorChanged);
            MarginTextNameProperty.Changed.AddClassHandler<BasicWithTextName>(x => x.OnTextChanged);
            PenBlack = new Pen(Brushes.Black, .5);
            PenBlack.ToImmutable(); 
            PenBlack1 = new Pen(Brushes.Black, 1);
            PenBlack1.ToImmutable();
            PenWhite1 = new Pen(Brushes.WhiteSmoke, 1);
            PenWhite1.ToImmutable();
        }

        public TextBlock DrawingVisualText { get; set; }

        public override void Render(DrawingContext drawingContext)
        {
            if (TextNameISVisible)
            {
                /*FormattedText ft = new FormattedText(TextName, new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.SemiBold/*, FontStretch.Normal#1#),
                    14,TextAlignment.Center, TextWrapping.NoWrap, Size.Empty)/*, TextFormattingMode.Ideal#1#;
                    
                //ft.MaxTextWidth = TextNameWidth > 10 ? TextNameWidth: 10;
                    ft.TextAlignment = TextAlignment.Center;*/
                    DrawingVisualText.Text = TextName;
                    
                    DrawingVisualText.MaxWidth = TextNameWidth > 10 ? TextNameWidth : 10;
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
                    


            }
            else
                DrawingVisualText.Opacity = 0;
            /*DrawingVisualText.Render(drawingContext);*/
        }

        private void OnTextChanged(AvaloniaPropertyChangedEventArgs obj)
        {
        }
        private void OnColorChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            BasicWithTextName t = this as BasicWithTextName;
            /*t.BrushTextNameColor = new SolidColorBrush((Color)obj.NewValue);
            t.BrushTextNameColor.ToImmutable();*/
            //t.DrawText();
        }
        internal protected bool IsModifyPressed = false;
        internal protected bool IsTextPressed = false;
        internal protected Point ModifyStartPoint = new Point(0, 0);
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            ModifyStartPoint = e.GetPosition(this);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            if (IsModifyPressed)
            {
                IsTextPressed = IsModifyPressed = false;
                e.Handled = true;
            }
        }
        

    }
}