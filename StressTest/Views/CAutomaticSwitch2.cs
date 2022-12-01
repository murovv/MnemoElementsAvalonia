using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.OpenGL;
using AvAp2;
using AvAp2.Converters;
using AvAp2.Interfaces;
using IProjectModel;
using CommutationDeviceStates = AvAp2.CommutationDeviceStates;
using TagValueQuality = AvAp2.TagValueQuality;

namespace StressTest.Views;

public class CAutomaticSwitch2 : Control
{
    #region MnemoChanges

    public static event EventHandler MnemoMarginChanged;

    public class EventArgsMnemoMarginChanged : EventArgs
    {
        #region EventArgsMnemoMarginChanged

        public string PropertyName
        {
            get => propertyName;
        }

        private readonly string propertyName;

        public EventArgsMnemoMarginChanged(string APropertyName)
        {
            propertyName = APropertyName;
        }

        #endregion EventArgsMnemoMarginChanged
    }

    /// <summary>
    /// Запускает событие при изменении отступов содержимого элементов мышью. Можно использовать для группового изменения.
    /// </summary>
    public static event EventHandler MnemoNeedSave;

    /// <summary>
    /// Запускает событие при изменении свойств, требующих сохранения шага в истории изменений
    /// </summary>
    internal protected void RiseMnemoNeedSave()
    {
        MnemoNeedSave?.Invoke(this, new EventArgs());
    }

    internal protected void RiseMnemoMarginChanged(string APropertyName)
    {
        MnemoMarginChanged?.Invoke(this, new EventArgsMnemoMarginChanged(APropertyName));
    }

    #endregion

    #region IConnector

    [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителя левого "),
     DisplayName("Видимость соединителя левого "), Browsable(true)]
    public bool IsConnectorExistLeft
    {
        get => (bool)GetValue(IsConnectorExistLeftProperty);
        set => SetValue(IsConnectorExistLeftProperty, value);
    }

    public static readonly StyledProperty<bool> IsConnectorExistLeftProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, bool>(nameof(IsConnectorExistLeft));

    [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителя правого "),
     DisplayName("Видимость соединителя правого "), Browsable(true)]
    public bool IsConnectorExistRight
    {
        get => (bool)GetValue(IsConnectorExistRightProperty);
        set => SetValue(IsConnectorExistRightProperty, value);
    }

    public static readonly StyledProperty<bool> IsConnectorExistRightProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, bool>(nameof(IsConnectorExistRight));

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
        AvaloniaProperty.Register<CAutomaticSwitch2, CommutationDeviceStates>(nameof(NormalState),
            CommutationDeviceStates.On);

    [Category("Свойства элемента мнемосхемы"), Description("Отображать отклонения от нормального режима"),
     DisplayName("Нормальное состояние отображать "), Browsable(true)]
    public bool ShowNormalState
    {
        get => (bool)GetValue(ShowNormalStateProperty);
        set => SetValue(ShowNormalStateProperty, value);
    }

    public static readonly StyledProperty<bool> ShowNormalStateProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, bool>(nameof(ShowNormalState));

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
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(TagIDControlMode), "-1");

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
        AvaloniaProperty.Register<CAutomaticSwitch2, TagDataItem>(nameof(TagDataControlMode), null);

    private void TdiControlMode_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Equals(nameof(TagDataItem.TagValueString)) || e.PropertyName.Equals(nameof(TagDataItem.Quality)))
            InvalidateVisual();
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
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(ControlModeToolTip), "ДУ в ячейке");

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
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(ControlModeTextDistance), "ДУ");

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
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(ControlModeTextLocal), "МУ");

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
        AvaloniaProperty.Register<CAutomaticSwitch2, Thickness>(nameof(MarginControlMode),
            new Thickness(-20, 0, 0, -10));

    private static void OnControlModeChanged(AvaloniaPropertyChangedEventArgs obj)
    {
        (obj.Sender as CAutomaticSwitch2).InvalidateVisual();
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
        AvaloniaProperty.Register<CAutomaticSwitch2, Color>(nameof(ControlModeTextColor),
            Color.FromArgb(255, 255, 190, 0));


    private static void OnControlModeColorChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
    {
        (obj.Sender as CAutomaticSwitch2).BrushControlModeTextColor = new SolidColorBrush((Color)obj.NewValue.Value);
        (obj.Sender as CAutomaticSwitch2).InvalidateVisual();
    }

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
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(TagIDBlockState), "-1");

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
        AvaloniaProperty.Register<CAutomaticSwitch2, TagDataItem>(nameof(TagDataBlock), null);

    private void TdiBlockState_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Equals(nameof(TagDataItem.TagValueString)) ||
            e.PropertyName.Equals(nameof(TagDataItem.Quality)))
        {
            InvalidateVisual();
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
        AvaloniaProperty.Register<CAutomaticSwitch2, Thickness>(nameof(MarginBlock),
            new Thickness(-20, 0, 0, -10));

    private static void OnBlockChanged(AvaloniaPropertyChangedEventArgs obj)
    {
        (obj.Sender as CAutomaticSwitch2).InvalidateVisual();
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
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(TagIDRealBlockState), "-1");

    [Browsable(false)]
    public TagDataItem TagDataRealBlock
    {
        get => (TagDataItem)GetValue(TagDataRealBlockProperty);
        set => SetValue(TagDataRealBlockProperty, value);
    }

    public static StyledProperty<TagDataItem> TagDataRealBlockProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, TagDataItem>(nameof(TagDataRealBlock));


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
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(TagIDDeblock), "-1");

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
        AvaloniaProperty.Register<CAutomaticSwitch2, TagDataItem>(nameof(TagDataDeblock), null);

    private void TdiDeblock_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Equals(nameof(TagDataItem.TagValueString)) || e.PropertyName.Equals(nameof(TagDataItem.Quality)))
            InvalidateVisual();
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
        AvaloniaProperty.Register<CAutomaticSwitch2, Thickness>(nameof(MarginDeblock),
            new Thickness(-20, 0, 0, -10));

    private static void OnDeblockChanged(AvaloniaPropertyChangedEventArgs obj)
    {
        (obj.Sender as CAutomaticSwitch2).InvalidateVisual();
    }

    #endregion ОБ

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
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(TagIDBanners), "-1");


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

    

    public static StyledProperty<TagDataItem> TagDataBannersProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, TagDataItem>(nameof(TagDataBanners), null);

    private void ValueOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Equals(nameof(TagDataItem.TagValueString)) ||
            e.PropertyName.Equals(nameof(TagDataItem.Quality)))
        {
            InvalidateVisual();
        }
    }
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
        AvaloniaProperty.Register<CAutomaticSwitch2, Thickness>(nameof(MarginBanner),
            new Thickness(-30, 0, 0, -40));

    private static void OnBannersChanged(AvaloniaPropertyChangedEventArgs obj)
    {
        (obj.Sender as CAutomaticSwitch2).InvalidateVisual();
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
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(TagIDCommandOn), "-1");

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
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(TagIDCommandOff), "-1");

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
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(TagIDCommandClearReady), "-1");

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
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(TagIDCommandReady), "-1");

    #endregion Привязки команд

    #region Geometry

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

    #endregion

    #region BasicEquipment

    #region У прямоугольника можно просто поменять цвет, а у линии только через класс напряжения, просто цвет спрятан

    [Category("Свойства элемента мнемосхемы"), Description("Цвет содержимого элемента"), PropertyGridFilterAttribute,
     DisplayName("Цвет содержимого"), Browsable(false)]
    public Color
        ContentColor // У прямоугольника можно просто поменять цвет, а у линии только через класс напряжения, просто цвет спрятан
    {
        get => (Color)GetValue(ContentColorProperty);
        set
        {
            SetValue(ContentColorProperty, value);
            RiseMnemoNeedSave();
        }
    }

    [Category("Свойства элемента мнемосхемы"), Description("Цвет содержимого элемента альтернативный"),
     PropertyGridFilterAttribute, DisplayName("Цвет содержимого альтернативный"), Browsable(false)]
    public Color ContentColorAlternate
    {
        get => (Color)GetValue(ContentColorAlternateProperty);
        set
        {
            SetValue(ContentColorAlternateProperty, value);
            RiseMnemoNeedSave();
        }
    }

    #endregion У прямоугольника можно просто поменять цвет, а у линии только через класс напряжения, просто цвет спрятан

    #region IVideo

    [Category("Свойства элемента мнемосхемы"), Description("Логин видеонаблюдения"), PropertyGridFilterAttribute,
     DisplayName("Видеонаблюдение логин"), Browsable(true)]
    public string VideoLogin
    {
        get => (string)GetValue(VideoLoginProperty);
        set
        {
            SetValue(VideoLoginProperty, value);
            RiseMnemoNeedSave();
        }
    }

    public static StyledProperty<string> VideoLoginProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(VideoLogin), "admin");

    [Category("Свойства элемента мнемосхемы"), Description("Пароль видеонаблюдения"), PropertyGridFilterAttribute,
     DisplayName("Видеонаблюдение пароль"), Browsable(true)]
    public string VideoPassword
    {
        get => (string)GetValue(VideoPasswordProperty);
        set
        {
            SetValue(VideoPasswordProperty, value);
            RiseMnemoNeedSave();
        }
    }

    public static StyledProperty<string> VideoPasswordProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(VideoPassword), "");

    [Category("Свойства элемента мнемосхемы"), Description("Канал видеонаблюдения"), PropertyGridFilterAttribute,
     DisplayName("Видеонаблюдение канал"), Browsable(true)]
    public string VideoChannelID
    {
        get => (string)GetValue(VideoChannelIDProperty);
        set
        {
            SetValue(VideoChannelIDProperty, value);
            RiseMnemoNeedSave();
        }
    }

    public static StyledProperty<string> VideoChannelIDProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(VideoChannelID), "");

    [Category("Свойства элемента мнемосхемы"), Description("Положение камеры видеонаблюдения"),
     PropertyGridFilterAttribute, DisplayName("Видеонаблюдение положение"), Browsable(true)]
    public string VideoChannelPTZ
    {
        get => (string)GetValue(VideoChannelPTZProperty);
        set
        {
            SetValue(VideoChannelPTZProperty, value);
            RiseMnemoNeedSave();
        }
    }

    public static StyledProperty<string> VideoChannelPTZProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, string>("VideoChannelPTZ", "");

    #endregion IVideo


    [Category("Свойства элемента мнемосхемы"), Description("Класс напряжения элемента"), PropertyGridFilterAttribute,
     DisplayName("Напряжение"), Browsable(true)]
    public VoltageClasses VoltageEnum
    {
        get => (VoltageClasses)GetValue(VoltageEnumProperty);
        set
        {
            SetValue(VoltageEnumProperty, value);
            RiseMnemoNeedSave();
        }
    }

    public static StyledProperty<VoltageClasses> VoltageEnumProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, VoltageClasses>(nameof(VoltageEnum), VoltageClasses.kV110);

    private static void OnVoltageChanged(AvaloniaPropertyChangedEventArgs<VoltageClasses> obj)
    {
        #region Смена цвета при изменении класса напряжения

        ((obj.Sender as CAutomaticSwitch2)!).ContentColor = VoltageEnumColors.VoltageColors[obj.NewValue.Value];

        #region switch

        //switch ((VoltageClasses)e.NewValue)
        //{
        //    case VoltageClasses.kV1150:
        //        b.ContentColor = Color.FromArgb(255, 205, 138, 255);
        //        break;
        //    case VoltageClasses.kV750:
        //        b.ContentColor = Color.FromArgb(255, 0, 0, 200);
        //        break;
        //    case VoltageClasses.kV500:
        //        b.ContentColor = Color.FromArgb(255, 165, 15, 10);
        //        break;
        //    case VoltageClasses.kV400:
        //        b.ContentColor = Color.FromArgb(255, 240, 150, 30);
        //        break;
        //    case VoltageClasses.kV330:
        //        b.ContentColor = Color.FromArgb(255, 0, 140, 0);
        //        break;
        //    case VoltageClasses.kV220:
        //        b.ContentColor = Color.FromArgb(255, 200, 200, 0);
        //        break;
        //    case VoltageClasses.kV150:
        //        b.ContentColor = Color.FromArgb(255, 170, 150, 0);
        //        break;
        //    case VoltageClasses.kV110:
        //        b.ContentColor = Color.FromArgb(255, 0, 180, 200);
        //        break;
        //    case VoltageClasses.kV35:
        //        b.ContentColor = Color.FromArgb(255, 130, 100, 50);
        //        break;
        //    case VoltageClasses.kV10:
        //        b.ContentColor = Color.FromArgb(255, 100, 0, 100);
        //        break;
        //    case VoltageClasses.kV6:
        //        b.ContentColor = Color.FromArgb(255, 200, 150, 100);
        //        break;
        //    case VoltageClasses.kV04:
        //        b.ContentColor = Color.FromArgb(255, 190, 190, 190);
        //        break;
        //    case VoltageClasses.kVGenerator:
        //        b.ContentColor = Color.FromArgb(255, 230, 70, 230);
        //        break;
        //    case VoltageClasses.kVRepair:
        //        b.ContentColor = Color.FromArgb(255, 205, 255, 155);
        //        break;
        //    default://VoltageClasses.kVEmpty
        //        b.ContentColor = Color.FromArgb(255, 255, 255, 255);
        //        break;
        //} 

        #endregion

        #endregion
    }

    #endregion

    #region BasicWithState

    public string TdiStateString
    {
        get => GetValue(TdiStateStringProperty);
        set => SetValue(TdiStateStringProperty, value);
    }

    public static readonly StyledProperty<string> TdiStateStringProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(TdiStateString));


    [Category("Привязки данных"),
     Description(
         "Идентификатор тега состояния элемента. Для текущих данных - ток или дискрет, для оборудования - наличие напряжения для цвета"),
     DisplayName("ID тега состояния"), Browsable(true)]
    public string TagIDMainState
    {
        get => (string)GetValue(TagIDMainStateProperty);
        set => SetValue(TagIDMainStateProperty, value);
    }

    public static readonly StyledProperty<string> TagIDMainStateProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(TagIDMainState));

    public TagDataItem TagDataMainState
    {
        get => (TagDataItem)GetValue(TagDataMainStateProperty);
        set
        {
            SetValue(TagDataMainStateProperty, value);
        }
    }
    
    

    public static StyledProperty<TagDataItem> TagDataMainStateProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, TagDataItem>(nameof(TagDataMainState));


    private static void OnTagDataMainStateChanged(AvaloniaPropertyChangedEventArgs<TagDataItem> obj)
    {
        (obj.Sender as CAutomaticSwitch2).InvalidateVisual();
    }

    #endregion

    #region BasicWithColor

    public static StyledProperty<Color> ContentColorProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, Color>(nameof(ContentColor));

    public static StyledProperty<Color> ContentColorAlternateProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, Color>(nameof(ContentColorAlternate));

    public static Pen TestPen1 = new Pen(Brushes.Blue);
    public static ISolidColorBrush TestBrush1 = Brushes.Black;

    internal protected ISolidColorBrush BrushContentColorAlternate;
    internal protected Pen PenContentColorAlternate;
    internal protected Pen PenContentColorThinAlternate;

    private void ContentColorChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
    {
        CAutomaticSwitch2 sender = obj.Sender as CAutomaticSwitch2;
        sender.BrushContentColor = TestBrush1;

        sender.PenContentColor = TestPen1;
        sender.PenContentColorThin = TestPen1;
    }

    private void ContentColorAlternateChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
    {
        CAutomaticSwitch2 sender = obj.Sender as CAutomaticSwitch2;
        sender.BrushContentColorAlternate = TestBrush1;
        sender.PenContentColorAlternate = TestPen1;
        sender.PenContentColorThinAlternate = TestPen1;
    }

    #endregion

    #region BasicWithTextName

    internal protected bool IsModifyPressed = false;
    internal protected bool IsTextPressed = false;
    internal protected Point ModifyStartPoint = new Point(0, 0);
    internal protected bool IsResizerPressed = false;
    internal protected Brush BrushTextNameColor;
    internal protected Pen PenBlack;
    internal protected Pen PenBlack1;
    internal protected Pen PenWhite1;

    [Category("Свойства элемента мнемосхемы"), Description("Диспетчерское наименование элемента"),
     PropertyGridFilterAttribute, DisplayName("Диспетчерское наименование"), Browsable(true)]
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
        AvaloniaProperty.Register<CAutomaticSwitch2, string>(nameof(TextName), defaultValue: "");

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
    //public static  DependencyProperty TextNameToolTipProperty = DependencyProperty.Register("TextNameToolTip", typeof(string), typeof(CAutomaticSwitch2), new PropertyMetadata(""));

    [Category("Свойства элемента мнемосхемы"), Description("Отображать на схеме диспетчерское наименование"),
     PropertyGridFilterAttribute, DisplayName("Диспетчерское наименование видимость"), Browsable(true)]
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
        AvaloniaProperty.Register<CAutomaticSwitch2, bool>(nameof(TextNameISVisible), defaultValue: true);


    [Category("Свойства элемента мнемосхемы"),
     Description(
         "Цвет текста диспетчерского наименования в 16-ричном представлении 0x FF(прозрачность) FF(красный) FF(зелёный) FF(синий)"),
     PropertyGridFilterAttribute, DisplayName("Текст цвет (hex)"), Browsable(true)]
    public Color TextNameColor
    {
        get => GetValue(TextNameColorProperty);
        set
        {
            SetValue(TextNameColorProperty, value);
            RiseMnemoNeedSave();
        }
    }

    public static StyledProperty<Color> TextNameColorProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, Color>(nameof(TextNameColor), Color.FromArgb(255, 0, 0, 0));

    public static void OnColorChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
    {
        (obj.Sender as CAutomaticSwitch2).BrushTextNameColor = new SolidColorBrush(obj.NewValue.Value);
        (obj.Sender as CAutomaticSwitch2).InvalidateVisual();
    }

    public static void OnTextChanged(AvaloniaPropertyChangedEventArgs obj)
    {
        (obj.Sender as CAutomaticSwitch2).InvalidateVisual();
    }

    [Category("Свойства элемента мнемосхемы"),
     Description(
         "Ширина текстового поля диспетчерского наименования. По ширине будет происходить перенос по словам. Если не влезет слово - оно будет обрезано."),
     PropertyGridFilterAttribute, DisplayName("Текст ширина "), Browsable(true)]
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

    public static StyledProperty<double> TextNameFontSizeProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, double>(nameof(TextNameFontSize), 18);


    public static StyledProperty<double> TextNameWidthProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, double>(nameof(TextNameWidth), defaultValue: 90);

    [Category("Свойства элемента мнемосхемы"),
     Description(
         "Отступ диспетчерского наименования в формате (0,0,0,0). Через запятую отступ слева, сверху, справа, снизу. Отступ может быть отрицательным (-10). Имеет значение только отступ слева и сверху"),
     PropertyGridFilterAttribute, DisplayName("Текст отступ (дисп. наименование)"), Browsable(true)]
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
        AvaloniaProperty.Register<CAutomaticSwitch2, Thickness>(nameof(MarginTextName), new Thickness(0, 0, 0, 0));

    public double AngleTextName
    {
        get => (double)GetValue(AngleTextNameProperty);
        set
        {
            SetValue(AngleTextNameProperty, value);
            RiseMnemoNeedSave();
        }
    }

    public static StyledProperty<double> AngleTextNameProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, double>(nameof(AngleTextName), 0.0);

    #endregion

    #region BasicMnemoElement

    internal protected static StreamGeometry HandGeometry()
    {
        StreamGeometry geometry = new StreamGeometry();
        using (var context = geometry.Open())
        {
            //context.ArcTo(new Point(rect.TopLeft.X + cornerRadius.TopLeft, rect.TopLeft.Y),
            //    new Size(cornerRadius.TopLeft, cornerRadius.TopLeft),
            //    90, false, SweepDirection.Clockwise, true, isSmoothJoin);
            context.BeginFigure(new Point(1.5, 9), true);
            context.LineTo(new Point(2, 8));
            context.LineTo(new Point(3, 8));
            context.LineTo(new Point(6.5, 12));
            context.LineTo(new Point(6, 2));
            context.LineTo(new Point(7, 1.5));
            context.LineTo(new Point(8, 2));
            context.LineTo(new Point(9, 8));
            context.LineTo(new Point(9, 1));
            context.LineTo(new Point(10, 0.5));
            context.LineTo(new Point(11, 1));
            context.LineTo(new Point(11, 8));
            context.LineTo(new Point(11.5, 8.5));
            context.LineTo(new Point(12, 2));
            context.LineTo(new Point(13, 1.5));
            context.LineTo(new Point(14, 2));
            context.LineTo(new Point(13.5, 9));
            context.LineTo(new Point(14, 9));
            context.LineTo(new Point(15, 4));
            context.LineTo(new Point(16, 3.5));
            context.LineTo(new Point(17, 4));
            context.LineTo(new Point(16, 11));
            context.LineTo(new Point(16, 16));
            context.LineTo(new Point(15, 18));
            context.LineTo(new Point(8, 18));
        }
            
        //geometry.Freeze();
        return geometry;
    }
    public double Angle
    {
        get => (double)GetValue(AngleProperty);
        set => SetValue(AngleProperty, value);
    }

    /// <summary>
    /// Человеческое название мнемоэлемента для подсказок
    /// </summary>
    public string ElementTypeFriendlyName { get; }

    [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"),
     PropertyGridFilterAttribute, DisplayName("Сетка"), Browsable(false)]
    public virtual bool ControlIs30Step
    {
        get => true;
    }

    public bool ControlISSelected
    {
        get => (bool)GetValue(ControlISSelectedProperty);
        set => SetValue(ControlISSelectedProperty, value);
    }

    public static StyledProperty<bool> ControlISSelectedProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, bool>(nameof(ControlISSelected), false);

    public bool ControlISHitTestVisible
    {
        get => (bool)GetValue(ControlISHitTestVisibleProperty);
        set
        {
            SetValue(ControlISHitTestVisibleProperty, value);
            RiseMnemoNeedSave();
        }
    }

    /// <summary>
    /// Используется для реализации клика мыши касания на тачскрине и перетаскивания мнемосхемы.
    /// </summary>
    public bool IsBeginPressed { get; set; }

    public static StyledProperty<bool> ControlISHitTestVisibleProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, bool>(nameof(ControlISHitTestVisible));

    #region Привязки

    [Category("Привязки данных"),
     Description(
         "ID привязанных устройств. Для привязки достаточно перетащить устройство на элемент из дерева проекта слева"),
     PropertyGridFilterAttribute, DisplayName("ID привязанных устройств"), Browsable(true)]
    public List<string> DeviceIDs
    {
        get => (List<string>)GetValue(DeviceIDsProperty);
        set
        {
            SetValue(DeviceIDsProperty, value);
            RiseMnemoNeedSave();
        }
    }

    public static StyledProperty<List<string>> DeviceIDsProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, List<string>>(nameof(DeviceIDs), new List<string>());

    [Category("Привязки данных"),
     Description(
         "ID привязанных тегов для всплывающих подсказок. Например, токи фаз. Для привязки достаточно перетащить тег на элемент из дерева проекта слева"),
     PropertyGridFilterAttribute, DisplayName("ID тегов подсказок"), Browsable(true)]
    public List<string> ToolTipsTagIDs
    {
        get => (List<string>)GetValue(ToolTipsTagIDsProperty);
        set
        {
            SetValue(ToolTipsTagIDsProperty, value);
            RiseMnemoNeedSave();
        }
    }

    public static StyledProperty<List<string>> ToolTipsTagIDsProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, List<string>>(nameof(ToolTipsTagIDs), new List<string>());

    [Category("Привязки данных"),
     Description(
         "ID привязанных произвольных команд. Например, 'пуск осциллографа' для терминала РЗА или 'РПН больше' для трансформатора. Для привязки достаточно перетащить команду на элемент из дерева проекта слева"),
     PropertyGridFilterAttribute, DisplayName("ID команд"), Browsable(true)]
    public List<string> CommandIDs
    {
        get => (List<string>)GetValue(CommandIDsProperty);
        set
        {
            SetValue(CommandIDsProperty, value);
            RiseMnemoNeedSave();
        }
    }

    public static StyledProperty<List<string>> CommandIDsProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, List<string>>(nameof(CommandIDs), new List<string>());

    #endregion Привязки


    private static void OnControlIsSelectedChanged(AvaloniaPropertyChangedEventArgs<bool> obj)
    {
        (obj.Sender as CAutomaticSwitch2).ControlISSelected = obj.NewValue.Value;
        (obj.Sender as CAutomaticSwitch2).InvalidateVisual();
    }

    private static void OnAngleChanged(AvaloniaPropertyChangedEventArgs<double> obj)
    {
        (obj.Sender as CAutomaticSwitch2).InvalidateVisual();
    }

    public static StyledProperty<double> AngleProperty =
        AvaloniaProperty.Register<CAutomaticSwitch2, double>(nameof(Angle), 0);

    #region Рисование

    internal protected ISolidColorBrush BrushContentColor;
    internal protected Pen PenContentColor;
    internal protected Pen PenContentColorThin;
    public ISolidColorBrush BrushIsSelected { get; protected internal set; }
    public Pen PenIsSelected { get; protected internal set; }
    public ISolidColorBrush BrushMouseOver { get; protected internal set; }
    public Pen PenMouseOver { get; protected set; }
    public ISolidColorBrush BrushHand { get; protected set; }
    public Pen PenHand { get; protected set; }

    #endregion

    #endregion
    
    static CAutomaticSwitch2()
    {
        AffectsRender<CAutomaticSwitch2>(IsConnectorExistLeftProperty);
        AffectsRender<CAutomaticSwitch2>(IsConnectorExistRightProperty);
        AffectsRender<CAutomaticSwitch2>(NormalStateProperty);
        AffectsRender<CAutomaticSwitch2>(ShowNormalStateProperty);
        AffectsRender<CAutomaticSwitch2>(VoltageEnumProperty);
        AffectsRender<CAutomaticSwitch2>(TagDataMainStateProperty);
        AffectsRender<CAutomaticSwitch2>(TdiStateStringProperty);
        AffectsRender<CAutomaticSwitch2>(TagIDMainStateProperty);
        AffectsRender<CAutomaticSwitch2>(ContentColorProperty, ContentColorAlternateProperty);
        AffectsRender<CAutomaticSwitch2>(MarginTextNameProperty);
        AffectsRender<CAutomaticSwitch2>(TextNameISVisibleProperty);
        AffectsRender<CAutomaticSwitch2>(TextNameProperty);
        AffectsRender<CAutomaticSwitch2>(AngleProperty,ControlISSelectedProperty);
        ControlISSelectedProperty.Changed.Subscribe(OnControlIsSelectedChanged);
        AngleProperty.Changed.Subscribe(OnAngleChanged);
        TextNameColorProperty.Changed.Subscribe(OnColorChanged);
        TextNameProperty.Changed.Subscribe(OnTextChanged);
        TextNameISVisibleProperty.Changed.Subscribe(OnTextChanged);
        TextNameWidthProperty.Changed.Subscribe(OnTextChanged);
        TextNameFontSizeProperty.Changed.Subscribe(OnTextChanged);
        MarginTextNameProperty.Changed.Subscribe(OnTextChanged);
        AngleTextNameProperty.Changed.Subscribe(OnTextChanged);
        VoltageEnumProperty.Changed.Subscribe(OnVoltageChanged);
        TagDataMainStateProperty.Changed.Subscribe(OnTagDataMainStateChanged);
    }

    public CAutomaticSwitch2()
    {
        BrushContentColor = Brushes.Black;
        BrushContentColor.ToImmutable();
        PenContentColor = new Pen(Brushes.Black,3);
        PenContentColor.ToImmutable();
        BrushIsSelected = new SolidColorBrush(Colors.Pink, 0.3);
        BrushIsSelected.ToImmutable();
        PenIsSelected = new Pen(new SolidColorBrush(Colors.Red, 0.3), 1);
        //PenIsSelected.Freeze();
        BrushMouseOver = new SolidColorBrush(Colors.Gray, 0.5);
        BrushMouseOver.ToImmutable();
        PenMouseOver = new Pen(Brushes.Black, 1);
        PenMouseOver.ToImmutable();
        BrushHand = Brushes.Green;
        //BrushHand.Freeze();
        PenHand = new Pen(Brushes.DarkGreen, 1);
        PenHand.ToImmutable();
        

        ClipToBounds = false;
        #region subscribtions
        
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
        ContentColor = Colors.Green;
        ContentColorAlternate = Colors.Red;
        ContentColorProperty.Changed.Subscribe(ContentColorChanged);
        ContentColorAlternateProperty.Changed.Subscribe(ContentColorAlternateChanged);
        BrushContentColor = new SolidColorBrush(ContentColor);
        PenContentColor= new Pen(BrushContentColor, 3);
        if (this is IGeometry)
            PenContentColor.DashStyle = ((IGeometry)this).IsDash ? DashStyle.Dash : DashStyle.DashDotDot;
        PenContentColorThin = new Pen(BrushContentColor, 1);

        BrushContentColorAlternate = new SolidColorBrush(ContentColorAlternate);
        PenContentColorAlternate = new Pen(BrushContentColorAlternate, (this is IGeometry) ? ((IGeometry)this).LineThickness : 3);
        if (this is IGeometry)
            PenContentColorAlternate.DashStyle = ((IGeometry)this).IsDash ? DashStyle.Dash : DashStyle.DashDotDot;
        PenContentColorThinAlternate = new Pen(BrushContentColorAlternate, 1) ;

        
            
        ContentColor = VoltageEnumColors.VoltageColors[VoltageClasses.kV110];// Для состояния под напряжением
        ContentColorAlternate = VoltageEnumColors.VoltageColors[VoltageClasses.kVEmpty];// Для состояния без напряжения

        this.TextNameWidth = (double)60;
        this.MarginTextName = new Thickness(-30, -7, 0, 0);
        this.TextNameISVisible = true;
        this.ControlISHitTestVisible = true;
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
        
        
    }

    protected void DrawBlock(DrawingContext drawingContext)
    {
        if (string.IsNullOrEmpty(TagIDBlockState) == false && TagIDBlockState != "-1")
        {
            bool isBlocked = true;
            if (TagDataBlock?.TagValueString != null) // В работе если что-то привязано и там "1"
            {
                if (TagDataBlock.TagValueString.Equals("1") && (TagDataBlock.Quality == TagValueQuality.Good))
                    isBlocked = false;
            }

            var transform1 =
                drawingContext.PushPostTransform(Matrix.CreateTranslation(MarginBlock.Left, MarginBlock.Top));
            DrawingContext.PushedState transform2 = default;
            if (isBlocked == false)
                transform2 = drawingContext.PushPostTransform(Matrix.CreateTranslation(13, 0));
            drawingContext.DrawGeometry(Brushes.Transparent, PenBlockBody, LockGeometry());
            if (isBlocked == false)
                transform2.Dispose();
            drawingContext.DrawRectangle(BrushBlockBody, PenBlockBody, new Rect(0, 0, 20, 16), 3, 3);
            drawingContext.DrawEllipse(Brushes.Black, PenBlack, new Point(10, 8), 2, 2);
            drawingContext.DrawLine(PenBlack, new Point(10, 8), new Point(10, 14));


            // Качество
            if (TagDataBlock?.Quality == TagValueQuality.Handled)
            {
                StreamGeometry geometry = HandGeometry();
                geometry.Transform = new TranslateTransform(-15, -10);
                drawingContext.DrawGeometry(BrushBlockBody, PenHand, geometry);
            }
            else if (TagDataBlock?.Quality == TagValueQuality.Invalid)
            {
                FormattedText ft = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Normal),
                    12, BrushBlockBody);
                drawingContext.DrawText(ft, new Point(-8, -12));
            }

            transform1.Dispose();
        }
    }

    protected void DrawDeblock(DrawingContext drawingContext)
    {
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

            var translate =
                drawingContext.PushPostTransform(Matrix.CreateTranslation(MarginDeblock.Left, MarginDeblock.Top));
            StreamGeometry deblockGeometry = DeblockGeometry();
            if (isDeblocked)
                drawingContext.DrawGeometry(Brushes.Green, PenDeblock, deblockGeometry);
            else
                drawingContext.DrawGeometry(Brushes.Transparent, PenDeblock, deblockGeometry);
            // Качество
            if (TagDataDeblock?.Quality == TagValueQuality.Handled)
            {
                StreamGeometry geometry = HandGeometry();
                geometry.Transform = new TranslateTransform(-5, -10);
                drawingContext.DrawGeometry(Brushes.Green, PenHand, geometry);
            }
            else if (TagDataDeblock?.Quality == TagValueQuality.Invalid)
            {
                FormattedText ft = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Normal),
                    12, Brushes.Green);
                drawingContext.DrawText(ft, new Point(3, -10));
            }

            translate.Dispose();
        }
    }

    protected void DrawControlMode(DrawingContext drawingContext)
    {
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
            var translation =
                drawingContext.PushPostTransform(
                    Matrix.CreateTranslation(MarginControlMode.Left, MarginControlMode.Top));
            drawingContext.DrawText(ft, new Point(0, 0));
            // Качество
            if (TagDataControlMode?.Quality == TagValueQuality.Handled)
            {
                StreamGeometry geometry = HandGeometry();
                geometry.Transform = new TranslateTransform(-20, -13);
                drawingContext.DrawGeometry(BrushControlModeTextColor, PenHand, geometry);
            }
            else if (TagDataControlMode?.Quality == TagValueQuality.Invalid)
            {
                FormattedText ftQ = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Normal),
                    12, BrushControlModeTextColor);
                drawingContext.DrawText(ftQ, new Point(-15, -9));
            }

            translation.Dispose();
        }
    }

    protected void DrawBanners(DrawingContext ctx)
    {
        var transform = ctx.PushPostTransform(new TranslateTransform(MarginBanner.Left, MarginBanner.Top).Value);
        if (TagDataBanners == null) //На время настройки
        {
            #region На время настройки

            FormattedText ft = new FormattedText("Плакаты", CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Black,
                    FontStretch.Normal),
                12, BrushContentColor);
            ft.TextAlignment = TextAlignment.Left;
            ctx.DrawRectangle(BrushContentColorAlternate, PenContentColorThin, new Rect(0, 0, 60, 30));
            ctx.DrawRectangle(BrushContentColorAlternate, PenContentColorThin, new Rect(2, 2, 60, 30));
            ctx.DrawRectangle(BrushContentColorAlternate, PenContentColorThin, new Rect(0, 0, 60, 30));
            ctx.DrawRectangle(BrushContentColorAlternate, PenContentColorThin, new Rect(4, 4, 60, 30));
            ctx.DrawRectangle(BrushContentColorAlternate, PenContentColorThin, new Rect(6, 6, 60, 30));
            ctx.DrawRectangle(BrushContentColorAlternate, PenContentColorThin, new Rect(8, 8, 60, 30));
            ctx.DrawText(ft, new Point(12, 13));

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

                            FormattedText ft = new FormattedText("Заземлено", CultureInfo.CurrentCulture,
                                FlowDirection.LeftToRight,
                                new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                    FontStretch.Normal),
                                10, Brushes.WhiteSmoke);

                            ft.MaxTextWidth = 60;
                            ft.TextAlignment = TextAlignment.Left;
                            ctx.DrawRectangle(BrushBlue, PenBlack, new Rect(0, 0, 60, 30));
                            ctx.DrawText(ft, new Point(4, 7));

                            #endregion 1. Заземлено
                        }

                        if (Convert.ToBoolean(bannersState & 2))
                        {
                            #region 2. ИСПЫТАНИЕ

                            FormattedText ft = new FormattedText("ИСПЫТАНИЕ опасно для жизни",
                                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                    FontStretch.Normal),
                                6.2, Brushes.WhiteSmoke);

                            ft.MaxTextWidth = 60;
                            ft.TextAlignment = TextAlignment.Center;
                            ctx.DrawRectangle(Brushes.Red, PenBlack, new Rect(2, 2, 60, 30));
                            ctx.DrawText(ft, new Point(2, 7));

                            #endregion 2. ИСПЫТАНИЕ
                        }

                        if (Convert.ToBoolean(bannersState & 4))
                        {
                            #region 3. Транзит разомкнут

                            FormattedText ft = new FormattedText("Транзит разомкнут",
                                CultureInfo.CurrentCulture,
                                FlowDirection.LeftToRight,
                                new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                    FontStretch.Normal),
                                7, Brushes.Black);

                            ft.MaxTextWidth = 50;
                            ft.TextAlignment = TextAlignment.Center;
                            ctx.DrawRectangle(BrushBlue, PenBlack, new Rect(4, 4, 60, 30));
                            ctx.DrawRectangle(Brushes.WhiteSmoke, PenBlack, new Rect(8, 8, 52, 22));
                            ctx.DrawText(ft, new Point(9, 8.5));

                            #endregion 3. Транзит разомкнут
                        }

                        if (Convert.ToBoolean(bannersState & 8))
                        {
                            #region 4. Работа под напряжением

                            FormattedText ft = new FormattedText(
                                "Работа под напряжением \nповторно не включать",
                                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                    FontStretch.Normal),
                                4.5, Brushes.Red);

                            ft.MaxTextWidth = 56;
                            ft.TextAlignment = TextAlignment.Center;
                            ctx.DrawRectangle(Brushes.Red, PenBlack, new Rect(6, 6, 60, 30));
                            ctx.DrawRectangle(Brushes.WhiteSmoke, PenBlack, new Rect(10, 10, 52, 22));
                            ctx.DrawText(ft, new Point(10, 11));

                            #endregion 4. Работа под напряжением
                        }

                        if (Convert.ToBoolean(bannersState & 16))
                        {
                            #region 5. НЕ ВКЛЮЧАТЬ! Работают люди

                            FormattedText ft = new FormattedText("НЕ ВКЛЮЧАТЬ!\nРаботают люди",
                                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                    FontStretch.Normal),
                                6, Brushes.Red);

                            ft.MaxTextWidth = 56;
                            ft.TextAlignment = TextAlignment.Center;
                            ctx.DrawRectangle(Brushes.Red, PenBlack, new Rect(8, 8, 60, 30));
                            ctx.DrawRectangle(Brushes.WhiteSmoke, PenBlack, new Rect(12, 12, 52, 22));
                            ctx.DrawText(ft, new Point(12, 15));

                            #endregion 5. НЕ ВКЛЮЧАТЬ! Работают люди
                        }

                        if (Convert.ToBoolean(bannersState & 32))
                        {
                            #region 6. НЕ ВКЛЮЧАТЬ! Работа на линии

                            FormattedText ft = new FormattedText("НЕ ВКЛЮЧАТЬ!\nРабота на линии",
                                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                    FontStretch.Normal),
                                6, Brushes.WhiteSmoke);

                            ft.MaxTextWidth = 56;
                            ft.TextAlignment = TextAlignment.Center;

                            ctx.DrawRectangle(Brushes.Red, PenBlack, new Rect(10, 10, 60, 30));
                            ctx.DrawText(ft, new Point(12, 17));

                            #endregion 6. НЕ ВКЛЮЧАТЬ! Работа на линии
                        }
                    }
                }
            }

            #endregion В работе
        }

        transform.Dispose();
    }

    protected void DrawQuality(DrawingContext ctx)
    {
        if (TagDataMainState != null)
        {
            if (TagDataMainState.Quality == TagValueQuality.Handled)
            {
                ctx.DrawGeometry(BrushContentColor, PenHand, HandGeometry());
            }
            else if (TagDataMainState.Quality == TagValueQuality.Invalid)
            {
                FormattedText ft = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Normal),
                    12, BrushContentColor);
                ctx.DrawText(ft, new Point(0, 0));
            }
        }
    }

    protected void DrawText(DrawingContext ctx)
    {
        if (TextNameISVisible && !String.IsNullOrEmpty(TextName))
        {
            FormattedText ft = new FormattedText(TextName, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Expanded),
                TextNameFontSize, BrushTextNameColor);

            ft.MaxTextWidth = TextNameWidth > 10 ? TextNameWidth : 10;
            ft.TextAlignment = TextAlignment.Center;
            ft.Trimming = TextTrimming.None;
            var translate =
                ctx.PushPostTransform(new TranslateTransform(MarginTextName.Left, MarginTextName.Top).Value);
            var rotate = ctx.PushPostTransform(new RotateTransform(AngleTextName).Value);
            ctx.DrawText(ft, new Point(0, 0));
            rotate.Dispose();
            translate.Dispose();
        }
    }

    protected void DrawIsSelected(DrawingContext ctx)
    {
        if (ControlISSelected)
        {
            var transform = ctx.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
            ctx.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(0, 0, 29, 29));
            transform.Dispose();
        }
    }

    protected void DrawMouseOver(DrawingContext ctx)
    {
        var transform = ctx.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
        ctx.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(0, 0, 29, 29));
        transform.Dispose();
    }

    public void DrawBase(DrawingContext drawingContext)
    {
        DrawingContext.PushedState rotate =
            drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
        var lineLength = Math.Sqrt((100 * 100) + (100 * 100));
        var diffX = LineBoundsHelper.CalculateAdjSide(Angle, lineLength);
        var diffY = LineBoundsHelper.CalculateOppSide(Angle, lineLength);
        var p1 = new Point(200, 200);
        var p2 = new Point(p1.X + diffX, p1.Y + diffY);
        //drawingContext.DrawLine(PenContentColor, p1, p2);
        //drawingContext.DrawRectangle(PenBlack, LineBoundsHelper.CalculateBounds(p1, p2, PenContentColor));
        //----------------------------------------------------------------------------------------------------------------

        bool isActiveState = true; // При настройке, пока ничего не привязано, рисуем цветом класса напряжения
        CommutationDeviceStates state = CommutationDeviceStates.UnDefined;

        if (TagDataMainState?.TagValueString != null) // В работе если что-то привязано и там "1"
        {
            switch (TagDataMainState.TagValueString)
            {
                case "1":
                    state = CommutationDeviceStates.Off;
                    isActiveState = false;
                    break;
                case "2":
                    state = CommutationDeviceStates.On;
                    break;
                case "3":
                    state = CommutationDeviceStates.Broken;
                    break;
            }
        }


        if (IsConnectorExistLeft)
            drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate,
                new Point(-15, 15), new Point(2, 15));
        if (IsConnectorExistRight)
            drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate,
                new Point(28, 15), new Point(45, 15));

        /*drawingContext.DrawRectangle(Brushes.Transparent, PenContentColor, new Rect(1, 1, 28, 28));*/

        if (state == CommutationDeviceStates.On)
        {
            drawingContext.DrawRectangle(Brushes.Red, null, new Rect(2.5, 2.5, 25, 25));
            drawingContext.DrawLine(PenBlack, new Point(5, 15), new Point(25, 15));
        }
        else if (state == CommutationDeviceStates.Off)
        {
            drawingContext.DrawRectangle(Brushes.Green, null, new Rect(2.5, 2.5, 25, 25));
            drawingContext.DrawLine(PenBlack, new Point(15, 5), new Point(15, 25));
        }
        else if (state == CommutationDeviceStates.UnDefined)
        {
            drawingContext.DrawRectangle(Brushes.WhiteSmoke, null, new Rect(2.5, 2.5, 25, 25));
            rotate.Dispose(); //Разворот вопросика в нормальное положение даже если КА повёрнут
            var ft = new FormattedText("?", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Black,
                    FontStretch.Normal),
                16, BrushContentColor);
            drawingContext.DrawText(ft, new Point(10, 7));
        }
        else if (state == CommutationDeviceStates.Broken)
        {
            drawingContext.DrawRectangle(Brushes.WhiteSmoke, null, new Rect(2.5, 2.5, 25, 25));
            drawingContext.DrawLine(PenRed, new Point(-5, 35), new Point(35, -5));
        }

        if (ShowNormalState)
        {
            if (state != NormalState)
                drawingContext.DrawRectangle(Brushes.Transparent, PenNormalState, new Rect(-1, -1, 32, 32));
        }

        if (state != CommutationDeviceStates.UnDefined)
        {
            rotate.Dispose();
        }
    }

    public override void Render(DrawingContext context)
    {
        DrawBase(context);
        DrawIsSelected(context);
        DrawText(context);
        DrawBlock(context);
        DrawDeblock(context);
        DrawQuality(context);
        DrawBanners(context);
        DrawControlMode(context);
    }
}