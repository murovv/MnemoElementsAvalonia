using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia.Helpers;
using AvAp2.Converters;
using AvAp2.Interfaces;
using IProjectModel;

namespace AvAp2.Models
{
    [Description("Обмотка трансформатора")]
    public class CTransformerCoil : CAbstractTransformer, IRegulator
    {
        public override bool ControlIs30Step
        {
            get => false;
        }

        #region IRegulator
        [Category("Свойства элемента мнемосхемы"), Description("Наличие регулировки"), PropertyGridFilterAttribute, DisplayName("Наличие регулировки"), Browsable(true)]
        public bool IsRegulator
        {
            get => (bool)GetValue(IsRegulatorProperty);
            set
            {
                SetValue(IsRegulatorProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> IsRegulatorProperty = AvaloniaProperty.Register<CTransformerCoil,bool>(nameof(IsRegulator), false);

        /*private static void OnBaseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicMnemoElement b = d as BasicMnemoElement;
            b.DrawBase();
        }*/
        #endregion IRegulator

        [Category("Свойства элемента мнемосхемы"), Description("Силовой трансформатор"), PropertyGridFilterAttribute, DisplayName("Силовой трансформатор"), Browsable(true)]
        public bool IsPower
        {
            get => (bool)GetValue(IsPowerProperty);
            set
            {
                SetValue(IsPowerProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> IsPowerProperty = AvaloniaProperty.Register<CTransformerCoil,bool>(nameof(IsPower),false);
        private void OnIsPowerChanged(AvaloniaPropertyChangedEventArgs<bool> obj)
        {
            DrawingIsSelected.InvalidateVisual();
            DrawingMouseOver.InvalidateVisual();
        }

        [Category("Свойства элемента мнемосхемы"), Description("Соединение обмоток трансформатора"), PropertyGridFilterAttribute, DisplayName("Первая обмотка соединение"), Browsable(true)]
        public CoilsConnectionTypes CoilsConnectionType
        {
            get => (CoilsConnectionTypes)GetValue(CoilsConnectionTypeProperty);
            set
            {
                SetValue(CoilsConnectionTypeProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<CoilsConnectionTypes> CoilsConnectionTypeProperty = AvaloniaProperty.Register<CTransformerCoil,CoilsConnectionTypes>(nameof(CoilsConnectionType), CoilsConnectionTypes.StarConnection);

        //====================================================================================================================================
        #region Auto
        [Category("Свойства элемента мнемосхемы"), Description("Автотрансформатор"), PropertyGridFilterAttribute, DisplayName("Автотрансформатор"), Browsable(true)]
        public bool AutoIsExist
        {
            get => (bool)GetValue(AutoIsExistProperty);
            set
            {
                SetValue(AutoIsExistProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> AutoIsExistProperty = AvaloniaProperty.Register<CTransformerCoil,bool>(nameof(AutoIsExist),false);

        internal protected Brush BrushContentColorAutoVoltage;
        internal protected Pen PenContentColorAutoVoltage;

        [Category("Свойства элемента мнемосхемы"), Description("Цвет автотрансформатора в соответствии с классом напряжения"), PropertyGridFilterAttribute, DisplayName("Автотрансформатор цвет"), Browsable(false)]
        private Color AutoVoltageColor
        {
            get => (Color)GetValue(AutoVoltageColorProperty);
            set
            {
                SetValue(AutoVoltageColorProperty, value);
                BrushContentColorAutoVoltage = new SolidColorBrush(AutoVoltageColor);
                BrushContentColorAutoVoltage.ToImmutable();
                PenContentColorAutoVoltage = new Pen(BrushContentColorAutoVoltage, 3);
                PenContentColorAutoVoltage.ToImmutable();
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<Color> AutoVoltageColorProperty = AvaloniaProperty.Register<CTransformerCoil,Color>(nameof(AutoVoltageColor), Color.FromArgb(255, 0, 180, 200));
        /*private static void OnAutoVoltageColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformerCoil b = d as CTransformerCoil;
            b.BrushContentColorAutoVoltage = new SolidColorBrush(b.AutoVoltageColor);
            b.BrushContentColorAutoVoltage.Freeze();
            b.PenContentColorAutoVoltage = new Pen(b.BrushContentColorAutoVoltage, 3);
            b.PenContentColorAutoVoltage.Freeze();
            b.DrawBase();
        }*/

        [Category("Свойства элемента мнемосхемы"), Description("Класс напряжения автотрансформатора"), PropertyGridFilterAttribute, DisplayName("Автотрансформатор напряжение"), Browsable(true)]
        public VoltageClasses AutoVoltage
        {
            get => (VoltageClasses)GetValue(AutoVoltageProperty);
            set
            {
                SetValue(AutoVoltageProperty, value);
                AutoVoltageColor = VoltageEnumColors.VoltageColors[AutoVoltage];
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<VoltageClasses> AutoVoltageProperty = AvaloniaProperty.Register<CTransformerCoil,VoltageClasses>(nameof(AutoVoltage), VoltageClasses.kV110);
        /*private static void OnAutoVoltagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region класс напряжения автотрансформатора
            CTransformerCoil cis = d as CTransformerCoil;
            cis.AutoVoltageColor = VoltageEnumColors.VoltageColors[cis.AutoVoltage];
            #region switch
            //switch (cis.AutoVoltage)
            //{
            //    case VoltageClasses.kV1150:
            //        cis.AutoVoltageColor = Color.FromArgb(255, 205, 138, 255);
            //        break;
            //    case VoltageClasses.kV750:
            //        cis.AutoVoltageColor = Color.FromArgb(255, 0, 0, 200);
            //        break;
            //    case VoltageClasses.kV500:
            //        cis.AutoVoltageColor = Color.FromArgb(255, 165, 15, 10);
            //        break;
            //    case VoltageClasses.kV400:
            //        cis.AutoVoltageColor = Color.FromArgb(255, 240, 150, 30);
            //        break;
            //    case VoltageClasses.kV330:
            //        cis.AutoVoltageColor = Color.FromArgb(255, 0, 140, 0);
            //        break;
            //    case VoltageClasses.kV220:
            //        cis.AutoVoltageColor = Color.FromArgb(255, 200, 200, 0);
            //        break;
            //    case VoltageClasses.kV150:
            //        cis.AutoVoltageColor = Color.FromArgb(255, 170, 150, 0);
            //        break;
            //    case VoltageClasses.kV110:
            //        cis.AutoVoltageColor = Color.FromArgb(255, 0, 180, 200);
            //        break;
            //    case VoltageClasses.kV35:
            //        cis.AutoVoltageColor = Color.FromArgb(255, 130, 100, 50);
            //        break;
            //    case VoltageClasses.kV10:
            //        cis.AutoVoltageColor = Color.FromArgb(255, 100, 0, 100);
            //        break;
            //    case VoltageClasses.kV6:
            //        cis.AutoVoltageColor = Color.FromArgb(255, 200, 150, 100);
            //        break;
            //    case VoltageClasses.kV04:
            //        cis.AutoVoltageColor = Color.FromArgb(255, 190, 190, 190);
            //        break;
            //    case VoltageClasses.kVGenerator:
            //        cis.AutoVoltageColor = Color.FromArgb(255, 230, 70, 230);
            //        break;
            //    case VoltageClasses.kVRepair:
            //        cis.AutoVoltageColor = Color.FromArgb(255, 205, 255, 155);
            //        break;
            //    //case VoltageClasses.kVBlack:
            //    //    cis.AutoVoltageColor = Color.FromArgb(255, 0, 0, 0);
            //    //    break;
            //    default://VoltageClasses.kVEmpty
            //        cis.AutoVoltageColor = Color.FromArgb(255, 255, 255, 255);
            //        break;

            //}
            #endregion switch
            #endregion класс напряжения автотрансформатора
        }*/

        #endregion Auto
        //====================================================================================================================================
        #region выводы обмотки

        [Category("Свойства элемента мнемосхемы"), Description("Видимость левого вывода обмотки"), PropertyGridFilterAttribute, DisplayName("Первая обмотка левый"), Browsable(true)]
        public bool CoilLeftExitIsExist
        {
            get => (bool)GetValue(CoilLeftExitIsExistProperty);
            set
            {
                SetValue(CoilLeftExitIsExistProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> CoilLeftExitIsExistProperty = AvaloniaProperty.Register<CTransformerCoil,bool>(nameof(CoilLeftExitIsExist), false);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость верхнего вывода обмотки"), PropertyGridFilterAttribute, DisplayName("Первая обмотка верхний "), Browsable(true)]
        public bool CoilTopExitIsExist
        {
            get => (bool)GetValue(CoilTopExitIsExistProperty);
            set
            {
                SetValue(CoilTopExitIsExistProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> CoilTopExitIsExistProperty = AvaloniaProperty.Register<CTransformerCoil,bool>(nameof(CoilTopExitIsExist),false);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость правого вывода обмотки"), PropertyGridFilterAttribute, DisplayName("Первая обмотка правый"), Browsable(true)]
        public bool CoilRightExitIsExist
        {
            get => (bool)GetValue(CoilRightExitIsExistProperty);
            set
            {
                SetValue(CoilRightExitIsExistProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> CoilRightExitIsExistProperty = AvaloniaProperty.Register<CTransformerCoil,bool>(nameof(CoilRightExitIsExist), false);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость нижнего вывода обмотки"), PropertyGridFilterAttribute, DisplayName("Первая обмотка нижний"), Browsable(true)]
        public bool CoilBottomExitIsExist
        {
            get => (bool)GetValue(CoilBottomExitIsExistProperty);
            set
            {
                SetValue(CoilBottomExitIsExistProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> CoilBottomExitIsExistProperty = AvaloniaProperty.Register<CTransformerCoil,bool>("CoilBottomExitIsExist",false);

        #endregion выводы обмотки
        //====================================================================================================================================
        #region RPN
        [Category("Привязки команд"), Description("ID тега команды РПН прибавить"), PropertyGridFilterAttribute, DisplayName("Команда РПН прибавить"), Browsable(true)]
        public string TagIDCommandRPNMore
        {
            get => (string)GetValue(TagIDCommandRPNMoreProperty);
            set
            {
                SetValue(TagIDCommandRPNMoreProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> TagIDCommandRPNMoreProperty = AvaloniaProperty.Register<CTransformerCoil,string>(nameof(TagIDCommandRPNMore), "-1");

        [Category("Привязки команд"), Description("Параметр команды РПН прибавить (Для двухпозиционных команд обычно 1, для однопозиционных всегда 1)"), PropertyGridFilterAttribute, DisplayName("Команда РПН прибавить - параметр"), Browsable(true)]
        public byte CommandParameterRPNMore
        {
            get => (byte)GetValue(CommandParameterRPNMoreProperty);
            set
            {
                SetValue(CommandParameterRPNMoreProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<byte> CommandParameterRPNMoreProperty = AvaloniaProperty.Register<CTransformerCoil,byte>(nameof(CommandParameterRPNMore), (byte)1);

        [Category("Привязки команд"), Description("ID тега команды РПН убавить"), PropertyGridFilterAttribute, DisplayName("Команда РПН убавить"), Browsable(true)]
        public string TagIDCommandRPNLess
        {
            get => (string)GetValue(TagIDCommandRPNLessProperty);
            set
            {
                SetValue(TagIDCommandRPNLessProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> TagIDCommandRPNLessProperty = AvaloniaProperty.Register<CTransformerCoil,string>(nameof(TagIDCommandRPNLess),"-1");

        [Category("Привязки команд"), Description("Параметр команды РПН убавить (Для двухпозиционных команд обычно 0, для однопозиционных всегда 1)"), PropertyGridFilterAttribute, DisplayName("Команда РПН убавить - параметр"), Browsable(true)]
        public byte CommandParameterRPNLess
        {
            get => (byte)GetValue(CommandParameterRPNLessProperty);
            set
            {
                SetValue(CommandParameterRPNLessProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<byte> CommandParameterRPNLessProperty = AvaloniaProperty.Register<CTransformerCoil,byte>(nameof(CommandParameterRPNLess), (byte)1);


        //=======================================================================
        [Category("Привязки команд"), Description("ID тега команды РПН режим автоматический"), PropertyGridFilterAttribute, DisplayName("Команда РПН режим автоматический"), Browsable(true)]
        public string TagIDCommandRPNAuto
        {
            get => (string)GetValue(TagIDCommandRPNAutoProperty);
            set
            {
                SetValue(TagIDCommandRPNAutoProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> TagIDCommandRPNAutoProperty = AvaloniaProperty.Register<CTransformerCoil,string>(nameof(TagIDCommandRPNAuto), "-1");

        [Category("Привязки команд"), Description("Параметр команды РПН режим автоматический  (Для двухпозиционных команд обычно 0, для однопозиционных всегда 1)"), PropertyGridFilterAttribute, DisplayName("Команда РПН режим автоматический - параметр"), Browsable(true)]
        public byte CommandParameterRPNAuto
        {
            get => (byte)GetValue(CommandParameterRPNAutoProperty);
            set => SetValue(CommandParameterRPNAutoProperty, value);
        }
        public static StyledProperty<byte> CommandParameterRPNAutoProperty = AvaloniaProperty.Register<CTransformerCoil,byte>(nameof(CommandParameterRPNAuto), (byte)1);

        [Category("Привязки команд"), Description("ID тега команды РПН режим ручной"), PropertyGridFilterAttribute, DisplayName("Команда РПН режим ручной"), Browsable(true)]
        public string TagIDCommandRPNManual
        {
            get => (string)GetValue(TagIDCommandRPNManualProperty);
            set
            {
                SetValue(TagIDCommandRPNManualProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> TagIDCommandRPNManualProperty = AvaloniaProperty.Register<CTransformerCoil,string>(nameof(TagIDCommandRPNManual), "-1");

        [Category("Привязки команд"), Description("Параметр команды РПН режим ручной (Для двухпозиционных команд обычно 1, для однопозиционных всегда 1)"), PropertyGridFilterAttribute, DisplayName("Команда РПН режим ручной - параметр"), Browsable(true)]
        public byte CommandParameterRPNManual
        {
            get => (byte)GetValue(CommandParameterRPNManualProperty);
            set => SetValue(CommandParameterRPNManualProperty, value);
        }
        public static StyledProperty<byte> CommandParameterRPNManualProperty = AvaloniaProperty.Register<CTransformerCoil,byte>(nameof(CommandParameterRPNManual), (byte)1);

        #endregion RPN
        //====================================================================================================================================
        #region ControlMode
        [Category("Привязки данных"), Description("ID дискретного тега режима дистанционного управления РПН. Например, положение 'Телеуправление' ключа режима работы"), PropertyGridFilterAttribute, DisplayName("ID тега 'Телеуправление'"), Browsable(true)]
        public string TagIDControlMode
        {
            get => (string)GetValue(TagIDControlModeProperty);
            set
            {
                SetValue(TagIDControlModeProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> TagIDControlModeProperty = AvaloniaProperty.Register<CTransformerCoil,string>("TagIDControlMode","-1");

        [Category("Привязки данных"), Description("ID дискретного тега ручного управления РПН. Обычно дискретный сигнал терминала, разрешающий 'прибавить/убавить' ступень РПН командами АСУ"), PropertyGridFilterAttribute, DisplayName("ID тега 'Разрешение ручного управления'"), Browsable(true)]
        public string TagIDControlModeManual
        {
            get => (string)GetValue(TagIDControlModeManualProperty);
            set
            {
                SetValue(TagIDControlModeManualProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> TagIDControlModeManualProperty = AvaloniaProperty.Register<CTransformerCoil,string>(nameof(TagIDControlModeManual), "-1");

        #endregion ControlMode

        static CTransformerCoil()
        {
            AffectsRender<CTransformerCoil>(IsRegulatorProperty, CoilsConnectionTypeProperty, AutoIsExistProperty, CoilLeftExitIsExistProperty, CoilRightExitIsExistProperty,CoilTopExitIsExistProperty, CoilBottomExitIsExistProperty, IsPowerProperty, AutoVoltageColorProperty, AutoVoltageProperty);
        }

        private void OnControlISSelectedPropertyChanged(AvaloniaPropertyChangedEventArgs<bool> obj)
        {
            DrawingIsSelected.InvalidateVisual();
        }
        public CTransformerCoil() : base()
        {
            TranslationX = 0;
            TranslationY = 0;
            ControlISSelectedProperty.Changed.Subscribe(OnControlISSelectedPropertyChanged);
            IsPowerProperty.Changed.Subscribe(OnIsPowerChanged);
            ClipToBounds = false;
            DataContext = this;
            BrushContentColorAutoVoltage = new SolidColorBrush(AutoVoltageColor);
            BrushContentColorAutoVoltage.ToImmutable();
            PenContentColorAutoVoltage = new Pen(BrushContentColorAutoVoltage, 3);
            PenContentColorAutoVoltage.ToImmutable();
        }

        public override string ElementTypeFriendlyName
        {
            get => "Обмотка трансформатора";
        }

        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }


        internal static StreamGeometry StarRayGeometry()
        {
            StreamGeometry geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(15, 15), false);
                context.LineTo(new Point(15, 25));
            }
            return geometry;
        }
        internal static StreamGeometry DeltaGeometry()
        {
            //M15,20 33,20 24,5 15,20 32,20
            StreamGeometry geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(6, 20), false);
                context.LineTo(new Point(24, 20));
                context.LineTo(new Point(15, 5));
                context.LineTo(new Point(6, 20));
            }
            return geometry;
        }
        internal static StreamGeometry VGeometry()
        {
            //M30,20 33,20 24,5 15,20 18,20
            StreamGeometry geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(21, 20), false);
                context.LineTo(new Point(24, 20));
                context.LineTo(new Point(15, 5));
                context.LineTo(new Point(6, 20));
                context.LineTo(new Point(9, 20));
            }
            return geometry;
        }
        internal static StreamGeometry ArrowGeometry()
        {
            //Data="M0,28 L35,-7 L33,-8 L38,-10 L37,-5 L35,-7 z"
            StreamGeometry geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(-3, 32), false);
                context.LineTo(new Point(35, -7));
                context.LineTo(new Point(33, -8));
                context.LineTo(new Point(38, -10));
                context.LineTo(new Point(37, -5));
                context.LineTo(new Point(35, -7));
            }
            return geometry;
        }
        internal static StreamGeometry AutoLargeGeometry()
        {
            //Data="M38,12 C40,-5 20,-19 14,-17"
            StreamGeometry geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(35, 15), false);
                context.ArcTo(new Point(14, -16), new Size(25, 25), 0, false, SweepDirection.CounterClockwise);

            }
            return geometry;
        }


        public override void Render(DrawingContext drawingContext)
        {
            #region isActiveState

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

            #endregion isActiveState

            DrawingContext.PushedState rotation;
            using (rotation = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value))
            {
                switch (CoilsConnectionType)
                {
                    case CoilsConnectionTypes.StarConnection:
                        #region StarConnection
                        var geometry1 = StarRayGeometry();
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometry1);
                        var geometry2 = StarRayGeometry();
                        geometry2.Transform = new RotateTransform(120, 15, 15);
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometry2);
                        var geometry3 = StarRayGeometry();
                        geometry3.Transform = new RotateTransform(240, 15, 15);
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometry3);
                        #endregion StarConnection
                        break;
                    case CoilsConnectionTypes.DeltaConnection:
                        #region DeltaConnection
                        var geometryDelta = DeltaGeometry();
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometryDelta);
                        #endregion DeltaConnection
                        break;
                    case CoilsConnectionTypes.VConnection:
                        #region VConnection
                        var geometryV = VGeometry();
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometryV);
                        #endregion VConnection
                        break;
                    case CoilsConnectionTypes.OnePhaseConnection:
                        #region OnePhaseConnection
                        drawingContext.DrawLine(isActiveState ? PenContentColorThin : PenContentColorThinAlternate, new Point(15, 24), new Point(15, 4));
                        #endregion OnePhaseConnection
                        break;
                    default:
                        break;
                }

                if (IsPower)
                {
                    if (CoilLeftExitIsExist)
                        drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(-4, 15));
                    if (CoilTopExitIsExist && !AutoIsExist)
                        drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, -15), new Point(15, -4));
                    if (AutoIsExist)
                    {
                        var geometryAuto = AutoLargeGeometry();
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorAutoVoltage : PenContentColorAlternate, geometryAuto);
                    }
                    if (CoilRightExitIsExist)
                        drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(34, 15), new Point(45, 15));
                    if (CoilBottomExitIsExist)
                        drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 34), new Point(15, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 15), 20, 20);

                    if (IsRegulator)
                    {
                        var geometryArrow = ArrowGeometry();
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, geometryArrow);
                    }
                }
                else
                {
                    if (CoilLeftExitIsExist)
                        drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(4, 15));
                    if (CoilTopExitIsExist)
                        drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, -15), new Point(15, 4));
                    if (CoilRightExitIsExist)
                        drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(26, 15), new Point(45, 15));
                    if (CoilBottomExitIsExist)
                        drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 26), new Point(15, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 15), 10, 10);
                }
            }
            //TODO как сделать нормальный стайлинг
            /*if (ControlISSelected)
            {
                if (IsPower)
                    drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-5, -5, 40, 40));
                else
                    drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(0, 0, 30, 30));
                if (DrawingVisualText.Bounds.Width > 0)
                {
                    Rect selectedRect = DrawingVisualText.Bounds;
                    drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, selectedRect);
                }
            }*/
        }
        protected override void DrawIsSelected(DrawingContext ctx)
        {
            if (ControlISSelected)
            {
                var rotate = ctx.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
                DrawingContext.PushedState translate;
                //Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
                if (IsPower)
                {
                    
                    TranslationX = -5;
                    TranslationY = -5;
                    translate = ctx.PushPostTransform(new TranslateTransform(TranslationX,TranslationY).Value);
                    ctx.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(0, 0, 40, 40));
                }
                else
                {
                    TranslationX = 0;
                    TranslationY = 0;
                    translate = ctx.PushPostTransform(new TranslateTransform(TranslationX,TranslationY).Value);
                    ctx.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(0, 0, 30, 30));
                }
                if (DrawingVisualText != null && DrawingVisualText.Bounds.Width > 0)
                {
                    ctx.DrawRectangle(BrushIsSelected, PenIsSelected, DrawingVisualText.Bounds);
                }
                translate.Dispose();
                rotate.Dispose();
            }

        }

        protected override void DrawMouseOver(DrawingContext ctx)
        {
            var rotate = ctx.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
            DrawingContext.PushedState translate;
            //Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
            if (IsPower)
            {
                    
                TranslationX = -5;
                TranslationY = -5;
                translate = ctx.PushPostTransform(new TranslateTransform(TranslationX,TranslationY).Value);
                ctx.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(0, 0, 40, 40));
            }
            else
            {
                TranslationX = 0;
                TranslationY = 0;
                translate = ctx.PushPostTransform(new TranslateTransform(TranslationX,TranslationY).Value);
                ctx.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(0, 0, 30, 30));
            }
            if (DrawingVisualText != null && DrawingVisualText.Bounds.Width > 0)
            {
                ctx.DrawRectangle(BrushMouseOver, PenMouseOver, DrawingVisualText.Bounds);
            }
            translate.Dispose();
            rotate.Dispose();
        }
        /*internal protected void DrawMouseOver()
        {
            using (var drawingContext = DrawingVisualIsMouseOver.RenderOpen())
            {
                if (IsPower)
                    drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(-5, -5, 40, 40));
                else
                    drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(0, 0, 30, 30));

                if (DrawingVisualText.ContentBounds.Width > 0)
                {
                    Rect selectedRect = DrawingVisualText.ContentBounds;
                    drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, selectedRect);
                }
                drawingContext.Close();
            }
            DrawingVisualIsMouseOver.Opacity = 0;
        }*/
    }
}