﻿using System;
using System.ComponentModel;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using AvAp2.Interfaces;

namespace AvAp2.Models
{
    public abstract class BasicCommutationDevice : BasicEquipment, IConnector
    {
        #region IConnector

        [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителя левого "),
         DisplayName("Видимость соединителя левого "), Browsable(true)]
        public bool IsConnectorExistLeft
        {
            get => (bool)GetValue(IsConnectorExistLeftProperty);
            set => SetValue(IsConnectorExistLeftProperty, value);
        }

        public static readonly StyledProperty<bool> IsConnectorExistLeftProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, bool>(nameof(IsConnectorExistLeft));

        [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителя правого "),
         DisplayName("Видимость соединителя правого "), Browsable(true)]
        public bool IsConnectorExistRight
        {
            get => (bool)GetValue(IsConnectorExistRightProperty);
            set => SetValue(IsConnectorExistRightProperty, value);
        }

        public static readonly StyledProperty<bool> IsConnectorExistRightProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, bool>(nameof(IsConnectorExistRight));

        #endregion IConnector

        #region нормальный режим

        [Category("Свойства элемента мнемосхемы"),
         Description("Нормальное состояние КА в соответствии со схемой нормального режима"),
         DisplayName("Нормальное состояние КА"), Browsable(true)]
        public CommutationDeviceStates NormalState
        {
            get => (CommutationDeviceStates)GetValue(NormalStateProperty);
            set => SetValue(NormalStateProperty, value);
        }

        public static readonly StyledProperty<CommutationDeviceStates> NormalStateProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, CommutationDeviceStates>(nameof(NormalState),
                CommutationDeviceStates.On);

        [Category("Свойства элемента мнемосхемы"), Description("Отображать отклонения от нормального режима"),
         DisplayName("Нормальное состояние отображать "), Browsable(true)]
        public bool ShowNormalState
        {
            get => (bool)GetValue(ShowNormalStateProperty);
            set => SetValue(ShowNormalStateProperty, value);
        }

        public static readonly StyledProperty<bool> ShowNormalStateProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, bool>(nameof(ShowNormalState));

        #endregion нормальный режим

        #region режим управления элемента

        [Category("Привязки данных"),
         Description("ID тега режима управления элемента. Тег расчётный, строится как цепочка блокировки"),
         PropertyGridFilterAttribute, DisplayName("ID тега режима"), Browsable(true)]
        public string TagIDControlMode
        {
            get => (string)GetValue(TagIDControlModeProperty);
            set
            {
                SetValue(TagIDControlModeProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<string> TagIDControlModeProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, string>(nameof(TagIDControlMode), "-1");

        [Browsable(false)]
        public TagDataItem TagDataControlMode
        {
            get => (TagDataItem)GetValue(TagDataControlModeProperty);
            set
            {
                TagDataItem oldValue = (TagDataItem)GetValue(TagDataControlModeProperty);
                if (oldValue != value)
                {
                    if (oldValue != null)
                        oldValue.PropertyChanged -= TdiControlMode_PropertyChanged;
                    if (value != null)
                    {
                        value.PropertyChanged += TdiControlMode_PropertyChanged;
                    }

                    SetValue(TagDataControlModeProperty, value);
                }
            }
        }

        public static StyledProperty<TagDataItem> TagDataControlModeProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, TagDataItem>(nameof(TagDataControlMode), null);

        private void TdiControlMode_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(TagDataItem.TagValueString)) ||
                e.PropertyName.Equals(nameof(TagDataItem.Quality)))
            {
                DrawControlMode();
            }

        }

        [Category("Свойства элемента мнемосхемы"), Description("Подсказка ключа режима"), PropertyGridFilterAttribute,
         DisplayName("Режим управления подсказка "), Browsable(true)]
        public string ControlModeToolTip
        {
            get => (string)GetValue(ControlModeToolTipProperty);
            set
            {
                SetValue(ControlModeToolTipProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<string> ControlModeToolTipProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, string>(nameof(ControlModeToolTip), "ДУ в ячейке");

        [Category("Свойства элемента мнемосхемы"),
         Description("Текст режима дистанция, например 'ДУ', 'ТУ', 'телеуправление'"), PropertyGridFilterAttribute,
         DisplayName("Режим управления дистанция"), Browsable(false)]
        public string ControlModeTextDistance
        {
            get => (string)GetValue(ControlModeTextDistanceProperty);
            set
            {
                SetValue(ControlModeTextDistanceProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<string> ControlModeTextDistanceProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, string>(nameof(ControlModeTextDistance), "ДУ");

        [Category("Свойства элемента мнемосхемы"), Description("Текст режима местное, например 'МУ', 'местн.'"),
         PropertyGridFilterAttribute, DisplayName("Режим управления местное"), Browsable(false)]
        public string ControlModeTextLocal
        {
            get => (string)GetValue(ControlModeTextLocalProperty);
            set
            {
                SetValue(ControlModeTextLocalProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<string> ControlModeTextLocalProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, string>(nameof(ControlModeTextLocal), "МУ");

        [Category("Свойства элемента мнемосхемы"),
         Description(
             "Отступ режима управления в формате (0,0,0,0). Через запятую отступ слева, сверху, справа, снизу. Отступ может быть отрицательным (-10). Имеет значение только отступ слева и сверху."),
         PropertyGridFilterAttribute, DisplayName("Режим управления отступ"), Browsable(true)]
        public Thickness MarginControlMode
        {
            get => (Thickness)GetValue(MarginControlModeProperty);
            set
            {
                SetValue(MarginControlModeProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<Thickness> MarginControlModeProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, Thickness>(nameof(MarginControlMode),
                new Thickness(-20, 0, 0, -10));

        private void OnControlModeChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            DrawControlMode();
            DrawIsSelected();
            DrawMouseOver();
        }

        [Category("Свойства элемента мнемосхемы"),
         Description(
             "Цвет текста режима управления в 16-ричном представлении 0x FF(прозрачность) FF(красный) FF(зелёный) FF(синий)"),
         PropertyGridFilterAttribute, DisplayName("Режим управления цвет (hex)"), Browsable(true)]
        public Color ControlModeTextColor
        {
            get => (Color)GetValue(ControlModeTextColorProperty);
            set
            {
                SetValue(ControlModeTextColorProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<Color> ControlModeTextColorProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, Color>(nameof(ControlModeTextColor),
                Color.FromArgb(255, 255, 190, 0));


        private void OnControlModeColorChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
        {

            BrushControlModeTextColor = new SolidColorBrush((Color)obj.NewValue.Value);
            BrushControlModeTextColor.ToImmutable();
            DrawControlMode();
        }


        #endregion режим управления элемента

        #region ОБ

        [Category("Привязки данных"), Description("ID тега готовности"), PropertyGridFilterAttribute,
         DisplayName("ID тега готовности"), Browsable(true)]
        public string TagIDBlockState
        {
            get => (string)GetValue(TagIDBlockStateProperty);
            set
            {
                SetValue(TagIDBlockStateProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<string> TagIDBlockStateProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, string>(nameof(TagIDBlockState), "-1");

        [Browsable(false)]
        public TagDataItem TagDataBlock
        {
            get => (TagDataItem)GetValue(TagDataBlockProperty);
            set
            {
                TagDataItem oldValue = (TagDataItem)GetValue(TagDataBlockProperty);
                if (oldValue != value)
                {
                    if (oldValue != null)
                        oldValue.PropertyChanged -= TdiBlockState_PropertyChanged;
                    if (value != null)
                    {
                        value.PropertyChanged += TdiBlockState_PropertyChanged;
                    }

                    SetValue(TagDataBlockProperty, value);
                }
            }
        }

        public static StyledProperty<TagDataItem> TagDataBlockProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, TagDataItem>(nameof(TagDataBlock), null);

        private void TdiBlockState_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(TagDataItem.TagValueString)) ||
                e.PropertyName.Equals(nameof(TagDataItem.Quality)))
            {
                DrawBlock();
             
            }

        }

        [Category("Свойства элемента мнемосхемы"),
         Description(
             "Отступ блокировки (замочка) в формате (0,0,0,0). Через запятую отступ слева, сверху, справа, снизу. Отступ может быть отрицательным (-10). Имеет значение только отступ слева и сверху."),
         PropertyGridFilterAttribute, DisplayName("Блокировка отступ"), Browsable(true)]
        public Thickness MarginBlock
        {
            get => (Thickness)GetValue(MarginBlockProperty);
            set
            {
                SetValue(MarginBlockProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<Thickness> MarginBlockProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, Thickness>(nameof(MarginBlock),
                new Thickness(-20, 0, 0, -10));

        private void OnBlockChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            DrawBlock();
            DrawIsSelected();
            DrawMouseOver();
         
        }

        [Category("Привязки данных"), Description("ID тега состояния реле готовности"), PropertyGridFilterAttribute,
         DisplayName("ID тега состояния готовности"), Browsable(true)]
        public string TagIDRealBlockState
        {
            get => (string)GetValue(TagIDRealBlockStateProperty);
            set
            {
                SetValue(TagIDRealBlockStateProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<string> TagIDRealBlockStateProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, string>(nameof(TagIDRealBlockState), "-1");

        [Browsable(false)]
        public TagDataItem TagDataRealBlock
        {
            get => (TagDataItem)GetValue(TagDataRealBlockProperty);
            set => SetValue(TagDataRealBlockProperty, value);
        }

        public static StyledProperty<TagDataItem> TagDataRealBlockProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, TagDataItem>(nameof(TagDataRealBlock));


        [Category("Привязки данных"), Description("ID тега деблокирования"), PropertyGridFilterAttribute,
         DisplayName("ID тега деблокирования"), Browsable(true)]
        public string TagIDDeblock
        {
            get => (string)GetValue(TagIDDeblockProperty);
            set
            {
                SetValue(TagIDDeblockProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<string> TagIDDeblockProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, string>(nameof(TagIDDeblock), "-1");

        [Browsable(false)]
        public TagDataItem TagDataDeblock
        {
            get => (TagDataItem)GetValue(TagDataDeblockProperty);
            set
            {
                TagDataItem oldValue = (TagDataItem)GetValue(TagDataDeblockProperty);
                if (oldValue != value)
                {
                    if (oldValue != null)
                        oldValue.PropertyChanged -= TdiDeblock_PropertyChanged;
                    if (value != null)
                        value.PropertyChanged += TdiDeblock_PropertyChanged;
                    SetValue(TagDataDeblockProperty, value);
                }
            }
        }

        public static StyledProperty<TagDataItem> TagDataDeblockProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, TagDataItem>(nameof(TagDataDeblock), null);

        private void TdiDeblock_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(TagDataItem.TagValueString)) ||
                e.PropertyName.Equals(nameof(TagDataItem.Quality)))
            {
                DrawDeblock();
             
            }

        }


        [Category("Свойства элемента мнемосхемы"),
         Description(
             "Отступ деблокировки (ключика) в формате (0,0,0,0). Через запятую отступ слева, сверху, справа, снизу. Отступ может быть отрицательным (-10). Имеет значение только отступ слева и сверху."),
         PropertyGridFilterAttribute, DisplayName("Деблокировка отступ"), Browsable(true)]
        public Thickness MarginDeblock
        {
            get => (Thickness)GetValue(MarginDeblockProperty);
            set
            {
                SetValue(MarginDeblockProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<Thickness> MarginDeblockProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, Thickness>(nameof(MarginDeblock),
                new Thickness(-20, 0, 0, -10));

        private void OnDeblockChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            DrawDeblock();
            DrawIsSelected();
            DrawMouseOver();
         
        }


        #endregion ОБ

        #region разное

        [Category("Свойства элемента мнемосхемы"), Description("Уникальный идентификатор КА в системе."),
         PropertyGridFilterAttribute, DisplayName("Идентификатор КА"), Browsable(false)]
        public int CommonKAID
        {
            get => (int)GetValue(CommonKAIDProperty);
            set
            {
                SetValue(CommonKAIDProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<int> CommonKAIDProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, int>(nameof(CommonKAID), -1);

        [Category("Свойства элемента мнемосхемы"), Description("Ручной ввод данных состояния элемента разрешен"),
         PropertyGridFilterAttribute, DisplayName("Ручной ввод"), Browsable(true)]
        public bool CommutationDeviceStateManualSetEnabled
        {
            get => (bool)GetValue(CommutationDeviceStateManualSetEnabledProperty);
            set
            {
                SetValue(CommutationDeviceStateManualSetEnabledProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<bool> CommutationDeviceStateManualSetEnabledProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, bool>(nameof(CommutationDeviceStateManualSetEnabled),
                true);



        #endregion разное

        #region Баннеры

        [Category("Привязки данных"),
         Description(
             "ID тега видимости плакатов. Тег в виртуальном устройстве, значение задается только вручную с мнемосхемы. Видимость всех 5 плакатов. Задается числом до 31, где степени двойки - видимость отдельных плакатов"),
         PropertyGridFilterAttribute, DisplayName("ID тега видимости плакатов"), Browsable(true)]
        public string TagIDBanners
        {
            get => (string)GetValue(TagIDBannersProperty);
            set
            {
                SetValue(TagIDBannersProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<string> TagIDBannersProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, string>(nameof(TagIDBanners), "-1");


        [Browsable(false)]
        public TagDataItem TagDataBanners
        {
            get => (TagDataItem)GetValue(TagDataBannersProperty);
            set
            {
                TagDataItem oldValue = (TagDataItem)GetValue(TagDataBannersProperty);
                if (oldValue != value)
                {
                    if (oldValue != null)
                        oldValue.PropertyChanged -= ValueOnPropertyChanged;
                    if (value != null)
                        value.PropertyChanged += ValueOnPropertyChanged;
                    SetValue(TagDataBannersProperty, value);
                }
            }
        }

        private void ValueOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(TagDataItem.TagValueString)) ||
                e.PropertyName.Equals(nameof(TagDataItem.Quality)))
            {
                DrawBanners();
                DrawIsSelected();
                DrawMouseOver();
             
            }
        }

        public static StyledProperty<TagDataItem> TagDataBannersProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, TagDataItem>(nameof(TagDataBanners), null);



        [Category("Свойства элемента мнемосхемы"),
         Description(
             "Отступ плакатов в формате (0,0,0,0). Через запятую отступ слева, сверху, справа, снизу. Отступ может быть отрицательным (-10). Имеет значение только отступ слева и сверху."),
         PropertyGridFilterAttribute, DisplayName("Плакаты отступ"), Browsable(true)]
        public Thickness MarginBanner
        {
            get => (Thickness)GetValue(MarginBannerProperty);
            set
            {
                SetValue(MarginBannerProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<Thickness> MarginBannerProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, Thickness>(nameof(MarginBanner),
                new Thickness(-30, 0, 0, -40));

        private void OnBannersChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            DrawBanners();
            DrawIsSelected();
            DrawMouseOver();
         
        }


        #endregion Баннеры

        #region Привязки команд


        [Category("Привязки команд"), Description("ID тега команды включения"), PropertyGridFilterAttribute,
         DisplayName("ID включения"), Browsable(true)]
        public string TagIDCommandOn
        {
            get => (string)GetValue(TagIDCommandOnProperty);
            set
            {
                SetValue(TagIDCommandOnProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<string> TagIDCommandOnProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, string>(nameof(TagIDCommandOn), "-1");

        [Category("Привязки команд"), Description("ID тега команды отключения"), PropertyGridFilterAttribute,
         DisplayName("ID отключения"), Browsable(true)]
        public string TagIDCommandOff
        {
            get => (string)GetValue(TagIDCommandOffProperty);
            set
            {
                SetValue(TagIDCommandOffProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<string> TagIDCommandOffProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, string>(nameof(TagIDCommandOff), "-1");

        [Category("Привязки команд"), Description("ID тега команды сброса готовности управления КА"),
         PropertyGridFilterAttribute, DisplayName("ID сброса готовности"), Browsable(true)]
        public string TagIDCommandClearReady
        {
            get => (string)GetValue(TagIDCommandClearReadyProperty);
            set
            {
                SetValue(TagIDCommandClearReadyProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<string> TagIDCommandClearReadyProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, string>(nameof(TagIDCommandClearReady), "-1");

        [Category("Привязки команд"), Description("ID тега команды выдачи готовности управления КА"),
         PropertyGridFilterAttribute, DisplayName("ID готовности"), Browsable(true)]
        public string TagIDCommandReady
        {
            get => (string)GetValue(TagIDCommandReadyProperty);
            set
            {
                SetValue(TagIDCommandReadyProperty, value);
                RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<string> TagIDCommandReadyProperty =
            AvaloniaProperty.Register<BasicCommutationDevice, string>(nameof(TagIDCommandReady), "-1");


        #endregion Привязки команд

        internal protected DrawingGroup DrawingBlock;
        internal protected DrawingGroup DrawingDeblock;
        internal protected DrawingGroup DrawingControlMode;
        internal protected DrawingGroup DrawingBanners;

        public Image DrawingBlockWrapper { get; set; }

        public Image DrawingDeblockWrapper { get; set; }
        public Image DrawingControlModeWrapper { get; set; }

        public Image DrawingBannerWrapper { get; set; }

        internal protected bool IsBlockPressed = false;
        internal protected bool IsDeblockPressed = false;
        internal protected bool IsBannersPressed = false;
        internal protected bool IsControlModePressed = false;

        internal protected Brush BrushControlModeTextColor;
        internal protected Brush BrushBlue;
        internal protected Brush BrushBlockBody;

        internal protected Pen PenBlue;
        internal protected Pen PenWhite;
        internal protected Pen PenRed;
        internal protected Pen PenBlockBody;
        internal protected Pen PenDeblock;

        internal protected Pen PenNormalState;

        static BasicCommutationDevice()
        {
            AffectsRender<BasicCommutationDevice>(IsConnectorExistLeftProperty);
            AffectsRender<BasicCommutationDevice>(IsConnectorExistRightProperty);
            AffectsRender<BasicCommutationDevice>(NormalStateProperty);
            AffectsRender<BasicCommutationDevice>(ShowNormalStateProperty);
        }
        public BasicCommutationDevice() : base()
        {
            

            #region subscriptions

            TagIDControlModeProperty.Changed.Subscribe(OnControlModeChanged);
            TagDataControlModeProperty.Changed.Subscribe(OnControlModeChanged);
            ControlModeTextDistanceProperty.Changed.Subscribe(OnControlModeChanged);
            ControlModeTextLocalProperty.Changed.Subscribe(OnControlModeChanged);
            MarginControlModeProperty.Changed.Subscribe(OnControlModeChanged);

            ControlModeTextColorProperty.Changed.Subscribe(OnControlModeColorChanged);

            TagIDBlockStateProperty.Changed.Subscribe(OnBlockChanged);
            TagDataBlockProperty.Changed.Subscribe(OnBlockChanged);
            MarginBlockProperty.Changed.Subscribe(OnBlockChanged);

            TagIDDeblockProperty.Changed.Subscribe(OnDeblockChanged);
            TagDataDeblockProperty.Changed.Subscribe(OnDeblockChanged);
            MarginDeblockProperty.Changed.Subscribe(OnDeblockChanged);

            TagIDBannersProperty.Changed.Subscribe(OnBannersChanged);
            TagDataBannersProperty.Changed.Subscribe(OnBannersChanged);
            MarginBannerProperty.Changed.Subscribe(OnBannersChanged);

            ContentColorProperty.Changed.Subscribe(OnBannersChanged);
            ContentColorAlternateProperty.Changed.Subscribe(OnBannersChanged);

            #endregion

            BrushControlModeTextColor = new SolidColorBrush(ControlModeTextColor);
            BrushControlModeTextColor.ToImmutable();

            BrushBlockBody = new SolidColorBrush(Color.Parse("#FFF9CD00"));
            BrushBlockBody.ToImmutable();

            BrushBlue = new SolidColorBrush(Color.Parse("#FF3475CD"));
            BrushBlue.ToImmutable();
            var stops = new GradientStops();
            stops.Add(new GradientStop() { Color = Color.Parse("#FFFBF205"), Offset = 0.2 });
            stops.Add(new GradientStop() { Color = Color.Parse("#FF413D33") });
            stops.Add(new GradientStop() { Color = Color.Parse("#FF606A26"), Offset = 0.99 });
            PenBlockBody = new Pen(new LinearGradientBrush
            {
                GradientStops = stops,
                StartPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative),
                EndPoint = new RelativePoint(0.5, 1, RelativeUnit.Relative),
            }, 3);
            PenBlockBody.ToImmutable();
            PenRed = new Pen(Brushes.Red, 3);
            PenRed.ToImmutable();
            PenDeblock = new Pen(Brushes.Green, 1);
            PenDeblock.ToImmutable();
            PenNormalState = new Pen(Brushes.Yellow, 1);
            PenNormalState.ToImmutable();
            DrawingBlock = new DrawingGroup();
            DrawingBanners = new DrawingGroup();
            DrawingControlMode = new DrawingGroup();
            DrawingDeblock = new DrawingGroup();
            DrawingBlockWrapper = new Image();
            DrawingDeblockWrapper = new Image();
            DrawingBannerWrapper = new Image();
            DrawingControlModeWrapper = new Image();
            if (this.Content is null)
            {
                this.Content = new Canvas();
            }
            (this.Content as Canvas).Children.AddRange(new[]
                { DrawingBannerWrapper, DrawingBlockWrapper, DrawingDeblockWrapper, DrawingControlModeWrapper });
            Loaded += OnLoaded;
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            DrawBlock();
            DrawDeblock();
            DrawControlMode();
            DrawBanners();
        }

        private static StreamGeometry LockGeometry()
        {
            StreamGeometry geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(3, 1), false);
                context.LineTo(new Point(3, -10));
                context.ArcTo(new Point(5, -12), new Size(2, 2), 0, false, SweepDirection.Clockwise);
                context.LineTo(new Point(15, -12));
                context.ArcTo(new Point(17, -10), new Size(2, 2), 0, false, SweepDirection.Clockwise);
                context.LineTo(new Point(17, 1));
                // context.BezierTo(new Point(100, 0), new Point(200, 200), new Point(300, 100), true, true);
            }

            return geometry;
        }

        private static StreamGeometry DeblockGeometry()
        {
            StreamGeometry geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(17, 22), true);
                context.LineTo(new Point(17, 10));
                context.ArcTo(new Point(14, 10), new Size(5, 4), 0, true, SweepDirection.CounterClockwise);
                context.LineTo(new Point(14, 18));
                context.LineTo(new Point(11, 18));
                context.LineTo(new Point(11, 20));
                context.LineTo(new Point(14, 22));
            }

            return geometry;
        }

        protected virtual void DrawBlock()
        {
            var transform = new TranslateTransform(MarginBlock.Left, MarginBlock.Top);
            if (string.IsNullOrEmpty(TagIDBlockState) == false && TagIDBlockState != "-1")
            {
                bool isBlocked = true;
                if (TagDataBlock?.TagValueString != null) // В работе если что-то привязано и там "1"
                {
                    if (TagDataBlock.TagValueString.Equals("1") && (TagDataBlock.Quality == TagValueQuality.Good))
                        isBlocked = false;
                }

                

                GeometryDrawing lockGeometryDrawing = new GeometryDrawing();
                lockGeometryDrawing.Geometry = LockGeometry();
                lockGeometryDrawing.Brush = Brushes.Transparent;
                lockGeometryDrawing.Pen = PenBlockBody;
                
                if (isBlocked == false)
                {
                    lockGeometryDrawing.Geometry.Transform = new TranslateTransform(13, 0);
                }

                var rect = new GeometryDrawing
                {
                    Geometry = new RectangleGeometry(new Rect(0, 0, 20, 16)),
                    Brush = BrushBlockBody,
                    Pen = PenBlockBody
                };

                var ellipse = new GeometryDrawing
                {
                    Geometry = new EllipseGeometry
                    {
                        Center = new Point(10, 8),
                        RadiusX = 2,
                        RadiusY = 2
                    },
                    Brush = Brushes.Black,
                    Pen = PenBlack
                };
                var line = new GeometryDrawing
                {
                    Geometry = new LineGeometry
                    {
                        StartPoint = new Point(10, 8),
                        EndPoint = new Point(10, 14),
                    },
                    Pen = PenBlack
                };
                var quality = new GeometryDrawing();
                // Качество
                if (TagDataBlock?.Quality == TagValueQuality.Handled)
                {
                    quality = new GeometryDrawing
                    {
                        Geometry = HandGeometry(),
                        Brush = BrushBlockBody,
                        Pen = PenHand
                    };
                }
                else if (TagDataBlock?.Quality == TagValueQuality.Invalid)
                {
                    FormattedText ft = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                        new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal,
                            FontStretch.Normal),
                        12, BrushBlockBody);
                    quality = new GeometryDrawing
                    {
                        Geometry = ft.BuildGeometry(new Point(-8, -12)),
                        Brush = BrushBlockBody
                    };
                }

                DrawingBlock = new DrawingGroup
                {
                    Children = new DrawingCollection(new[] { rect, ellipse, line, quality })
                };
                DrawingBlock.Transform = transform;
            }

            DrawingBlockWrapper.Source = new DrawingImage(DrawingBlock);
            DrawingBlockWrapper.RenderTransform = transform;
        }

        internal protected virtual void DrawDeblock()
        {
            var transform = new TranslateTransform(MarginDeblock.Left, MarginDeblock.Top);

            if (string.IsNullOrEmpty(TagIDDeblock) == false && TagIDDeblock != "-1")
            {
                bool isDeblocked = true;
                if (TagDataDeblock?.TagValueString != null) // В работе если что-то привязано и там "1"
                {
                    if (TagDataDeblock.TagValueString.Equals("1") && (TagDataDeblock.Quality == TagValueQuality.Good))
                        isDeblocked = true;
                    else
                        isDeblocked = false;
                }
                var deblock = new GeometryDrawing();
                deblock.Geometry = DeblockGeometry();
                deblock.Pen = PenDeblock;
                if (isDeblocked)
                    deblock.Brush = Brushes.Green;
                else
                    deblock.Brush = Brushes.Transparent;
                // Качество
                var quality = new GeometryDrawing
                {
                    Brush = Brushes.Green
                };
                if (TagDataDeblock?.Quality == TagValueQuality.Handled)
                {
                    quality.Geometry = HandGeometry();
                    quality.Geometry.Transform = new TranslateTransform(-5, -10);
                    quality.Pen = PenHand;
                }
                else if (TagDataDeblock?.Quality == TagValueQuality.Invalid)
                {
                    FormattedText ft = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                        new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal,
                            FontStretch.Normal),
                        12, Brushes.Green);
                    quality.Geometry = ft.BuildGeometry(new Point(3, -10));
                }

                var rect = new GeometryDrawing
                {
                    Geometry = new RectangleGeometry(new Rect(0,0,10,10)),
                    Pen = new Pen(Brushes.Transparent),
                    Brush = Brushes.Transparent
                };

                DrawingDeblock = new DrawingGroup
                {
                    Children = new DrawingCollection(new[] { quality, deblock,rect })
                };
                DrawingDeblock.Transform = transform;

            }

            DrawingDeblockWrapper.Source = new DrawingImage(DrawingDeblock);
            DrawingDeblockWrapper.RenderTransform = transform;
            
        }

        protected virtual void DrawControlMode()
        {
            var transform = new TranslateTransform(MarginControlMode.Left, MarginControlMode.Top);
            if (string.IsNullOrEmpty(TagIDControlMode) == false && TagIDControlMode != "-1")
            {
                bool isDistance = true;
                if (TagDataControlMode?.TagValueString != null) // В работе если что-то привязано и там "1"
                {
                    if (TagDataControlMode.TagValueString.Equals("1") &&
                        (TagDataControlMode.Quality == TagValueQuality.Good))
                        isDistance = true;
                    else
                        isDistance = false;
                }

                //drawingContext.PushTransform(new TranslateTransform(MarginBlock.Left, MarginBlock.Top));
                FormattedText ft = new FormattedText(isDistance ? ControlModeTextDistance : ControlModeTextLocal,
                    CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Black, FontStretch.Normal),
                    12, BrushControlModeTextColor);
                ft.TextAlignment = TextAlignment.Center;

                var text = new GeometryDrawing
                {
                    Geometry = ft.BuildGeometry(new Point(0, 0)),
                    Brush = BrushControlModeTextColor
                };
                var quality = new GeometryDrawing();
                // Качество
                if (TagDataControlMode?.Quality == TagValueQuality.Handled)
                {
                    quality = new GeometryDrawing
                    {
                        Geometry = HandGeometry(),
                        Brush = BrushControlModeTextColor,
                        Pen = PenHand
                    };
                    quality.Geometry.Transform = new TranslateTransform(-20, -13);
                }
                else if (TagDataControlMode?.Quality == TagValueQuality.Invalid)
                {
                    FormattedText ftQ = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                        new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal,
                            FontStretch.Normal),
                        12, BrushControlModeTextColor);
                    quality = new GeometryDrawing
                    {
                        Geometry = ftQ.BuildGeometry(new Point(-15, -9)),
                        Brush = BrushControlModeTextColor
                    };
                }

                DrawingControlMode = new DrawingGroup
                {
                    Children = new DrawingCollection(new[] { quality, text }),
                    Transform = transform
                };
                
            }

            DrawingControlModeWrapper.Source = new DrawingImage(DrawingControlMode);
            DrawingControlModeWrapper.RenderTransform = transform;
        }

        protected virtual void DrawBanners()
        {
            DrawingBanners = new DrawingGroup();
            
            var transform = new TranslateTransform(MarginBanner.Left, MarginBanner.Top);
            
            if (TagDataBanners == null) //На время настройки
            {
                #region На время настройки

                var geometry = new GeometryGroup
                {
                    Children = new GeometryCollection(new[]
                    {
                        new RectangleGeometry(new Rect(0, 0, 60, 30)),
                        new RectangleGeometry(new Rect(2, 2, 60, 30)),
                        new RectangleGeometry(new Rect(4, 4, 60, 30)),
                        new RectangleGeometry(new Rect(6, 6, 60, 30)),
                        new RectangleGeometry(new Rect(8, 8, 60, 30))
                    })
                };
                var rects = new GeometryDrawing
                {
                    Geometry = geometry,
                    Brush = BrushContentColorAlternate,
                    Pen = PenContentColorThin
                };
                FormattedText ft = new FormattedText("Плакаты", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Black,
                        FontStretch.Normal),
                    12, BrushContentColor);
                ft.TextAlignment = TextAlignment.Left;
                var text = new GeometryDrawing
                {
                    Geometry = ft.BuildGeometry(new Point(12, 13)),
                    Brush = BrushContentColor
                };
               
                DrawingBanners = new DrawingGroup
                {
                    Children = new DrawingCollection(new[] { text, rects }),
                    Transform = transform
                };

                #endregion На время настройки
            }
            else
            {
                #region В работе
                if (TagDataBanners?.TagValueString != null)
                {
                    int bannersState = 0;
                    
                    if (int.TryParse(TagDataBanners.TagValueString, out bannersState))
                    {
                        if (bannersState > 0)
                        {
                            if (Convert.ToBoolean(bannersState & 1))
                            {
                                #region 1. Заземлено
                                var banner = new GeometryDrawing
                                {
                                    Geometry = new RectangleGeometry(new Rect(0, 0, 60, 30)),
                                    Brush = BrushBlue,
                                    Pen = PenBlack
                                };
                                
                                FormattedText ft = new FormattedText("Заземлено", CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                        FontStretch.Normal),
                                    10, Brushes.WhiteSmoke);
                                
                                ft.MaxTextWidth = 60;
                                ft.TextAlignment = TextAlignment.Left;
                                var text = new GeometryDrawing
                                {
                                    Geometry = ft.BuildGeometry(new Point(4, 7)),
                                    Brush = Brushes.WhiteSmoke
                                };
                               
                                DrawingBanners.Children.Add(banner);
                                DrawingBanners.Children.Add(text);

                                #endregion 1. Заземлено
                            }

                            if (Convert.ToBoolean(bannersState & 2))
                            {
                                #region 2. ИСПЫТАНИЕ

                                var rect = new GeometryDrawing
                                {
                                    Geometry = new RectangleGeometry
                                    {
                                        Rect = new Rect(2, 2, 60, 30),
                                        
                                    },
                                    Brush = Brushes.Red,
                                    Pen = PenBlack
                                };

                                FormattedText ft = new FormattedText("ИСПЫТАНИЕ опасно для жизни",
                                    CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                        FontStretch.Normal),
                                    6.2, Brushes.WhiteSmoke);

                                ft.MaxTextWidth = 60;
                                ft.TextAlignment = TextAlignment.Center;
                                var text = new GeometryDrawing
                                {
                                    Geometry = ft.BuildGeometry(new Point(2, 7)),
                                    Brush = Brushes.WhiteSmoke
                                };
                                DrawingBanners.Children.Add(rect);
                                DrawingBanners.Children.Add(text);
                                #endregion 2. ИСПЫТАНИЕ
                            }

                            if (Convert.ToBoolean(bannersState & 4))
                            {
                                #region 3. Транзит разомкнут
                                var rect = new GeometryDrawing
                                {
                                    Geometry = new RectangleGeometry
                                    {
                                        Rect = new Rect(4, 4, 60, 30),
                                        
                                    },
                                    Brush = BrushBlue,
                                    Pen = PenBlack
                                };
                                var rect1 = new GeometryDrawing
                                {
                                    Geometry = new RectangleGeometry
                                    {
                                        Rect = new Rect(8, 8, 52, 22),
                                        
                                    },
                                    Brush = Brushes.WhiteSmoke,
                                    Pen = PenBlack
                                };
                                

                                FormattedText ft = new FormattedText("Транзит разомкнут", CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                        FontStretch.Normal),
                                    7, Brushes.Black);

                                ft.MaxTextWidth = 50;
                                ft.TextAlignment = TextAlignment.Center;
                                var text = new GeometryDrawing
                                {
                                    Geometry = ft.BuildGeometry(new Point(9, 8.5)),
                                    Brush = Brushes.Black
                                };
                                DrawingBanners.Children.Add(rect);
                                DrawingBanners.Children.Add(rect1);
                                DrawingBanners.Children.Add(text);
                                #endregion 3. Транзит разомкнут
                            }

                            if (Convert.ToBoolean(bannersState & 8))
                            {
                                #region 4. Работа под напряжением

                                var rect = new GeometryDrawing
                                {
                                    Geometry = new RectangleGeometry
                                    {
                                        Rect = new Rect(6, 6, 60, 30),
                                    },
                                    Brush = Brushes.Red,
                                    Pen = PenBlack
                                };
                                var rect1 = new GeometryDrawing
                                {
                                    Geometry = new RectangleGeometry
                                    {
                                        Rect = new Rect(10, 10, 52, 22),
                                    },
                                    Brush = Brushes.WhiteSmoke,
                                    Pen = PenBlack
                                };
                                FormattedText ft = new FormattedText("Работа под напряжением \nповторно не включать",
                                    CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                        FontStretch.Normal),
                                    4.5, Brushes.Red);

                                ft.MaxTextWidth = 56;
                                ft.TextAlignment = TextAlignment.Center;
                                var text = new GeometryDrawing
                                {
                                    Geometry = ft.BuildGeometry(new Point(10, 11)),
                                    Brush = Brushes.Red
                                };
                                DrawingBanners.Children.Add(rect);
                                DrawingBanners.Children.Add(rect1);
                                DrawingBanners.Children.Add(text);
                                #endregion 4. Работа под напряжением
                            }

                            if (Convert.ToBoolean(bannersState & 16))
                            {
                                #region 5. НЕ ВКЛЮЧАТЬ! Работают люди
                                
                                var rect = new GeometryDrawing
                                {
                                    Geometry = new RectangleGeometry
                                    {
                                        Rect = new Rect(8, 8, 60, 30),
                                    },
                                    Brush = Brushes.Red,
                                    Pen = PenBlack
                                };
                                var rect1 = new GeometryDrawing
                                {
                                    Geometry = new RectangleGeometry
                                    {
                                        Rect = new Rect(12, 12, 52, 22),
                                    },
                                    Brush = Brushes.WhiteSmoke,
                                    Pen = PenBlack
                                };

                                FormattedText ft = new FormattedText("НЕ ВКЛЮЧАТЬ!\nРаботают люди",
                                    CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                        FontStretch.Normal),
                                    6, Brushes.Red);

                                ft.MaxTextWidth = 56;
                                ft.TextAlignment = TextAlignment.Center;
                                var text = new GeometryDrawing
                                {
                                    Geometry = ft.BuildGeometry(new Point(12, 15)),
                                    Brush = Brushes.Red
                                };

                                DrawingBanners.Children.Add(rect);
                                DrawingBanners.Children.Add(rect1);
                                DrawingBanners.Children.Add(text);
                                #endregion 5. НЕ ВКЛЮЧАТЬ! Работают люди
                            }

                            if (Convert.ToBoolean(bannersState & 32))
                            {
                                #region 6. НЕ ВКЛЮЧАТЬ! Работа на линии
                                
                                var rect = new GeometryDrawing
                                {
                                    Geometry = new RectangleGeometry
                                    {
                                        Rect = new Rect(10, 10, 60, 30),
                                    },
                                    Brush = Brushes.Red,
                                    Pen = PenBlack
                                };
                                
                                FormattedText ft = new FormattedText("НЕ ВКЛЮЧАТЬ!\nРабота на линии",
                                    CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                        FontStretch.Normal),
                                    6, Brushes.WhiteSmoke);

                                ft.MaxTextWidth = 56;
                                ft.TextAlignment = TextAlignment.Center;

                                var text = new GeometryDrawing
                                {
                                    Geometry = ft.BuildGeometry(new Point(12, 17)),
                                    Brush = Brushes.WhiteSmoke
                                };
                                
                                DrawingBanners.Children.Add(rect);
                                DrawingBanners.Children.Add(text);
                                DrawingBanners.Transform = transform;

                                #endregion 6. НЕ ВКЛЮЧАТЬ! Работа на линии
                            }
                        }
                        /*

                              

                              <Border Grid.Row="2" Grid.Column="2" Width="85" Height="40" Visibility="{TemplateBinding ASUBanner2Visibility}" Margin="5"  BorderBrush="Black" BorderThickness="1" CornerRadius="1">
                                  <Border BorderBrush="Red" BorderThickness="3" CornerRadius="1">
                                      <Border BorderBrush="Black" BorderThickness="1" CornerRadius="1" Background="White">
                                          <StackPanel VerticalAlignment="Center" HorizontalAlignment="Center">
                                              <TextBlock FontSize="8" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="Red" Text="НЕ ВКЛЮЧАТЬ!" TextAlignment="Center" TextWrapping="NoWrap" FontWeight="Bold"/>
                                              <TextBlock FontSize="7" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="Red" Text="Работают люди" TextAlignment="Center" TextWrapping="NoWrap" FontWeight="Bold"/>
                                          </StackPanel>
                                      </Border>
                                  </Border>
                              </Border>

                              <Border Grid.Row="2" Grid.Column="2" Width="85" Height="40" Visibility="{TemplateBinding ASUBanner1Visibility}" Margin="5"  BorderBrush="Black" BorderThickness="1" CornerRadius="1">
                                  <Border Background="Red" BorderBrush="Red" BorderThickness="1" CornerRadius="1">
                                      <StackPanel VerticalAlignment="Center" HorizontalAlignment="Center">
                                          <TextBlock FontSize="8" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White" Text="НЕ ВКЛЮЧАТЬ!" TextAlignment="Center" TextWrapping="NoWrap" FontWeight="Bold"/>
                                          <TextBlock FontSize="7" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White" Text="Работа на линии" TextAlignment="Center" TextWrapping="NoWrap" FontWeight="Bold"/>
                                      </StackPanel>
                                  </Border>
                              </Border>
                       
                        
                         */
                    }
                }

                #endregion В работе
            }

            DrawingBanners.Transform = transform;
            DrawingBannerWrapper.Source = new DrawingImage(DrawingBanners);
            DrawingBannerWrapper.RenderTransform = transform;
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            e.Pointer.Capture(this);
            ModifyStartPoint = e.GetPosition(this);
            e.Handled = true;
            if (DrawingVisualText.Bounds.Contains(ModifyStartPoint))
            {
                IsTextPressed = IsModifyPressed = true;
                IsBlockPressed = IsDeblockPressed = IsBannersPressed = IsControlModePressed = false;
            }
            else if (DrawingBlock.GetBounds().Contains(ModifyStartPoint))
            {
                IsBlockPressed = IsModifyPressed = true;
                IsTextPressed = IsDeblockPressed = IsBannersPressed = IsControlModePressed = false;
            }
            else if (DrawingDeblock.GetBounds().Contains(ModifyStartPoint))
            {
                IsDeblockPressed = IsModifyPressed = true;
                IsTextPressed = IsBlockPressed = IsBannersPressed = IsControlModePressed = false;
            }
            else if (DrawingControlMode.GetBounds().Contains(ModifyStartPoint))
            {
                IsControlModePressed = IsModifyPressed = true; 
                IsTextPressed = IsBlockPressed = IsDeblockPressed = IsBannersPressed = false;
            }else if (DrawingBanners.GetBounds().Contains(ModifyStartPoint))
            {
                IsBannersPressed = IsModifyPressed = true;
                IsTextPressed = IsBlockPressed = IsDeblockPressed = IsControlModePressed = false;
            }
            else
            {
                e.Handled = false;
            }

            
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (IsModifyPressed && ControlISSelected)
            {
                Point currentPoint = e.GetPosition(this);
                double dX = currentPoint.X - ModifyStartPoint.X;
                double dY = currentPoint.Y - ModifyStartPoint.Y;
                ModifyStartPoint = currentPoint;

                if (IsTextPressed)
                {
                    MarginTextName = new Thickness(MarginTextName.Left + dX, MarginTextName.Top + dY, 0, 0);
                    RiseMnemoMarginChanged(nameof(MarginTextName));
                }
                if (IsBlockPressed)
                {
                    MarginBlock = new Thickness(MarginBlock.Left + dX, MarginBlock.Top + dY, 0, 0);
                    RiseMnemoMarginChanged(nameof(MarginBlock));
                }
                if (IsDeblockPressed)
                {
                    MarginDeblock = new Thickness(MarginDeblock.Left + dX, MarginDeblock.Top + dY, 0, 0);
                    RiseMnemoMarginChanged(nameof(MarginDeblock));
                }
                if (IsBannersPressed)
                {
                    MarginBanner = new Thickness(MarginBanner.Left + dX, MarginBanner.Top + dY, 0, 0);
                    RiseMnemoMarginChanged(nameof(MarginBanner));
                }
                if (IsControlModePressed)
                {
                    MarginControlMode = new Thickness(MarginControlMode.Left + dX, MarginControlMode.Top + dY, 0, 0);
                    RiseMnemoMarginChanged(nameof(MarginControlMode));
                }
            }
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            if (IsModifyPressed)
            {
                IsTextPressed = IsModifyPressed = IsBannersPressed = IsBlockPressed = IsDeblockPressed = IsControlModePressed = false;
            }
        }
    }
    
}