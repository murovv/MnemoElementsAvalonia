using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace AvAp2.Models
{
    public abstract class BasicAlarmIndicator : BasicWithTextName
    {
        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке."),
         PropertyGridFilterAttribute, DisplayName("Сетка"), Browsable(false)]
        public override bool ControlIs30Step
        {
            get => false;
        }

        [Category("Свойства элемента мнемосхемы"), Description("Квитирование тревоги."), PropertyGridFilterAttribute,
         DisplayName("Квитирование тревоги"), Browsable(false)]
        public bool? IsReceipt
        {
            get => GetValue(IsReceiptProperty);
            set => SetValue(IsReceiptProperty, value);
        }

        public static StyledProperty<bool?> IsReceiptProperty =
            AvaloniaProperty.Register<BasicAlarmIndicator, bool?>(nameof(IsReceipt), null);

        public IDisposable Binding { get; set; }

        private static void OnReceiptChanged(AvaloniaPropertyChangedEventArgs<bool?> e)
        {
            #region Мигание индикатора

            //BasicAlarmIndicator ai = e.Sender as CAlarmIndicator;
            if (e.OldValue.Value != e.NewValue.Value)
            {
                if (e.NewValue.Value != null) 
                {
                    if (e.OldValue.Value == true)
                    {
                        (e.Sender as CAlarmIndicator).Binding.Dispose();
                    }
                    else
                    {
                        (e.Sender as CAlarmIndicator).Binding = (e.Sender as CAlarmIndicator).Bind(OpacityProperty, new Binding
                        {
                            Source = BlinkAnimationController.GetInstance(),
                            Path = "BlinkOpacity",
                            Mode = BindingMode.OneWay
                        });
                    }
                }
                else
                {
                    (e.Sender as CAlarmIndicator).Binding.Dispose();
                }
                    

                (e.Sender as CAlarmIndicator).RiseStateChangedEvent();
            }

            #endregion Мигание индикатора
        }

        [Category("Свойства элемента мнемосхемы"), Description("Группа тревоги."), PropertyGridFilterAttribute,
         DisplayName("Группа"), Browsable(false)]
        public int EventGroupID
        {
            get => GetValue(EventGroupIDProperty);
            set => SetValue(EventGroupIDProperty, value);
        }

        public static StyledProperty<int> EventGroupIDProperty =
            AvaloniaProperty.Register<BasicAlarmIndicator, int>(nameof(EventGroupID), -1);

        private static void OnAlarmChanged(AvaloniaPropertyChangedEventArgs e)
        {
            BasicAlarmIndicator? ai = e.Sender as BasicAlarmIndicator;
            if (ai != null)
            {
                ai.RiseStateChangedEvent();
            }
        }


        [Category("Свойства элемента мнемосхемы"), Description("Состояние тревоги активная-пролетевшая"),
         PropertyGridFilterAttribute, DisplayName("Состояние тревоги"), Browsable(false)]
        public bool? IsActive
        {
            get => (bool?)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public static StyledProperty<bool?> IsActiveProperty =
            AvaloniaProperty.Register<BasicAlarmIndicator, bool?>(nameof(IsActive), null);


        //public delegate void StateChangedHandler();
        // public event StateChangedHandler StateChangedEvent;
        public event EventHandler StateChangedEvent;

        internal protected Bitmap ImageSourceTransparent;
        internal protected Bitmap ImageSourceAlarm;
        internal protected Bitmap ImageSourceAlert1;
        internal protected Bitmap ImageSourceAlert2;
        internal protected Bitmap ImageSourceInfo;
        internal protected Bitmap ImageSourceDisabled;



        internal protected void RiseStateChangedEvent()
        {
            StateChangedEvent?.Invoke(this, new EventArgs());
        }

        static BasicAlarmIndicator()
        {
            AffectsRender<BasicAlarmIndicator>(IsActiveProperty, EventGroupIDProperty);
            IsActiveProperty.Changed.Subscribe(OnAlarmChanged);
            EventGroupIDProperty.Changed.Subscribe(OnAlarmChanged);
            IsReceiptProperty.Changed.Subscribe(OnReceiptChanged);
        }

        public BasicAlarmIndicator() : base()
        {
            var assests = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            ImageSourceTransparent =
                new Bitmap(assests.Open(new Uri($@"avares://{name}/Assets/Images/AlarmIndicator.png")));
            ImageSourceAlarm =
                new Bitmap(assests.Open(new Uri($@"avares://{name}/Assets/Images/AlarmIndicatorRed.png")));
            ImageSourceAlert1 =
                new Bitmap(assests.Open(new Uri($@"avares://{name}/Assets/Images/AlarmIndicatorOrange.png")));
            ImageSourceAlert2 =
                new Bitmap(assests.Open(new Uri($@"avares://{name}/Assets/Images/AlarmIndicatorBlue.png")));
            ImageSourceInfo =
                new Bitmap(assests.Open(new Uri($@"avares://{name}/Assets/Images/AlarmIndicatorGreen.png")));
            ImageSourceDisabled =
                new Bitmap(assests.Open(new Uri($@"avares://{name}/Assets/Images/AlarmIndicatorDisabled.png")));
        }

        public override void Render(DrawingContext drawingContext)
        {
            var assests = AvaloniaLocator.Current.GetService<IAssetLoader>();

            DrawingContext.PushedState rotation;
            using (rotation = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value))
            {
                if (IsActive == true)
                {
                    switch (EventGroupID) // Для активной в зависимости от уровня
                    {
                        case 1:
                            drawingContext.DrawImage(ImageSourceAlarm, new Rect(1, 1, 27, 27));
                            break;
                        case 2:
                            drawingContext.DrawImage(ImageSourceAlert1, new Rect(1, 1, 27, 27));
                            break;
                        case 3:
                            drawingContext.DrawImage(ImageSourceAlert2, new Rect(1, 1, 27, 27));
                            break;
                        case 4:
                            drawingContext.DrawImage(ImageSourceInfo, new Rect(1, 1, 27, 27));
                            break;
                        default:
                            drawingContext.DrawImage(ImageSourceDisabled, new Rect(1, 1, 27, 27));
                            break;
                    }
                }
                else
                    drawingContext.DrawImage(ImageSourceDisabled, new Rect(1, 1, 27, 27));

                /*FormattedText ftTextName = new FormattedText(TextName,
                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal,
                        FontWeight.Normal /*, FontStretch.Normal#1#),
                    14, TextAlignment.Left, TextWrapping.NoWrap, Size.Empty);

                ftTextName.TextAlignment = TextAlignment.Left;
                drawingContext.DrawText(BrushTextNameColor, new Point(35, 0), ftTextName);*/
            }
        }

        protected override void DrawText(DrawingContext ctx)
        {
            DrawingVisualText = new Control();
        }
        protected override void DrawIsSelected(DrawingContext ctx)
        {
            if (ControlISSelected)
            {
                ctx.DrawRectangle(BrushIsSelected, PenIsSelected, (this.Content as Canvas).Bounds);
            }
        }

        protected override void DrawMouseOver(DrawingContext ctx)
        {
            ctx.DrawRectangle(BrushMouseOver, PenMouseOver, (this.Content as Canvas).Bounds);
        }
    }
}