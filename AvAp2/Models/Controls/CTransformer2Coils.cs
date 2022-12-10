using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using AvAp2.Converters;
using IProjectModel;

namespace AvAp2.Models.Controls
{
    [Description("Двухобмоточный трансформатор")]
    public class CTransformer2Coils : CTransformerCoil
    {
        
        [Category("Свойства элемента мнемосхемы"), Description("Соединение вторых обмоток трансформатора"), PropertyGridFilter, DisplayName("Вторая обмотка соединение"), Browsable(true)]
        public CoilsConnectionTypes CoilsConnectionType2
        {
            get => (CoilsConnectionTypes)GetValue(CoilsConnectionType2Property);
            set
            {
                SetValue(CoilsConnectionType2Property, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<CoilsConnectionTypes> CoilsConnectionType2Property = AvaloniaProperty.Register<CTransformer2Coils, CoilsConnectionTypes>(nameof(CoilsConnectionType2), CoilsConnectionTypes.DeltaConnection);

        internal protected Brush BrushContentColorVoltage2;
        internal protected Pen PenContentColorVoltage2;
        internal protected Pen PenContentColorVoltage2Thin;

        [Category("Свойства элемента мнемосхемы"), Description("Цвет вторичной обмотки в соответствии с классом напряжения"), PropertyGridFilter, DisplayName("Вторая обмотка цвет"), Browsable(false)]
        private Color Voltage2Color
        {
            get => (Color)GetValue(Voltage2ColorProperty);
            set
            {
                SetValue(Voltage2ColorProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<Color> Voltage2ColorProperty = AvaloniaProperty.Register<CTransformer2Coils, Color>(nameof(Voltage2Color), Color.FromArgb(255, 0, 180, 200));
        private static void OnVoltage2ColorChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
        {
            CTransformer2Coils sender = obj.Sender as CTransformer2Coils; 
            sender.BrushContentColorVoltage2 = new SolidColorBrush(sender.Voltage2Color);
            sender.PenContentColorVoltage2 = new Pen(sender.BrushContentColorVoltage2, 3);
            sender.PenContentColorVoltage2Thin = new Pen(sender.BrushContentColorVoltage2, 1);
        }

        [Category("Свойства элемента мнемосхемы"), Description("Класс напряжения вторичной обмотки"), PropertyGridFilter, DisplayName("Вторая обмотка напряжение"), Browsable(true)]
        public VoltageClasses Voltage2
        {
            get => (VoltageClasses)GetValue(Voltage2Property);
            set
            {
                SetValue(Voltage2Property, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<VoltageClasses> Voltage2Property = AvaloniaProperty.Register<CTransformer2Coils, VoltageClasses>(nameof(Voltage2), VoltageClasses.kV110);

        private static void OnVoltage2Changed(AvaloniaPropertyChangedEventArgs<VoltageClasses> obj)
        {
            #region класс напряжения 2 обмотки
            
            (obj.Sender as CTransformer2Coils).Voltage2Color = VoltageEnumColors.VoltageColors[(obj.Sender as CTransformer2Coils).Voltage2];

            #endregion класс напряжения 2 обмотки
        }

        #region выводы обмотки
        [Category("Свойства элемента мнемосхемы"), Description("Видимость левого вывода вторичной обмотки"), PropertyGridFilter, DisplayName("Вторая обмотка левый"), Browsable(true)]
        public bool CoilLeftExitIsExist2
        {
            get => (bool)GetValue(CoilLeftExitIsExist2Property);
            set
            {
                SetValue(CoilLeftExitIsExist2Property, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> CoilLeftExitIsExist2Property = AvaloniaProperty.Register<CTransformer2Coils, bool>(nameof(CoilLeftExitIsExist2), false);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость верхнего вывода вторичной обмотки"), PropertyGridFilter, DisplayName("Вторая обмотка верхний"), Browsable(true)]
        public bool CoilTopExitIsExist2
        {
            get => (bool)GetValue(CoilTopExitIsExist2Property);
            set
            {
                SetValue(CoilTopExitIsExist2Property, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> CoilTopExitIsExist2Property = AvaloniaProperty.Register<CTransformer2Coils, bool>(nameof(CoilTopExitIsExist2), false);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость правого вывода вторичной обмотки"), PropertyGridFilter, DisplayName("Вторая обмотка правый"), Browsable(true)]
        public bool CoilRightExitIsExist2
        {
            get => (bool)GetValue(CoilRightExitIsExist2Property);
            set
            {
                SetValue(CoilRightExitIsExist2Property, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> CoilRightExitIsExist2Property = AvaloniaProperty.Register<CTransformer2Coils, bool>(nameof(CoilRightExitIsExist2), false);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость нижнего вывода вторичной обмотки"), PropertyGridFilter, DisplayName("Вторая обмотка нижний"), Browsable(true)]
        public bool CoilBottomExitIsExist2
        {
            get => (bool)GetValue(CoilBottomExitIsExist2Property);
            set
            {
                SetValue(CoilBottomExitIsExist2Property, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> CoilBottomExitIsExist2Property = AvaloniaProperty.Register<CTransformer2Coils, bool>(nameof(CoilBottomExitIsExist2), false);
        #endregion выводы обмотки

        static CTransformer2Coils()
        {
            AffectsRender<CTransformer2Coils>(CoilsConnectionType2Property, CoilLeftExitIsExist2Property, CoilRightExitIsExist2Property, CoilTopExitIsExist2Property, CoilBottomExitIsExist2Property);
            Voltage2Property.Changed.Subscribe(OnVoltage2Changed);
            Voltage2ColorProperty.Changed.Subscribe(OnVoltage2ColorChanged);
        }
        
        public CTransformer2Coils() : base()
        {
            BrushContentColorVoltage2 = new SolidColorBrush(Voltage2Color);
            PenContentColorVoltage2 = new Pen(BrushContentColorVoltage2, 3);
            PenContentColorVoltage2Thin = new Pen(BrushContentColorVoltage2, 1);
        }
      
        public override string ElementTypeFriendlyName
        {
            get => "Двухобмоточный трансформатор";
        }
      
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
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

            DrawingContext.PushedState rotate;

            drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
            
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
                    //if (CoilBottomExitIsExist)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 34), new Point(15, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 15), 20, 20);

                    if (IsRegulator)
                    {
                        var geometryArrow = ArrowGeometry();
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, geometryArrow);
                    }
                }
                else
                {
                    //if (CoilLeftExitIsExist)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(4, 15));
                    //if (CoilTopExitIsExist)
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, -15), new Point(15, 4));
                    //if (CoilRightExitIsExist)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(26, 15), new Point(45, 15));
                    //if (CoilBottomExitIsExist)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 26), new Point(15, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 15), 12, 12);
                }
                //====================================================================================================================================
                // Вторичная обмотка
                DrawingContext.PushedState translate;
                if (IsPower)
                    translate = drawingContext.PushPostTransform(new TranslateTransform(0, 30).Value);
                else
                    translate = drawingContext.PushPostTransform(new TranslateTransform(0, 19).Value);
                switch (CoilsConnectionType2)
                {
                    case CoilsConnectionTypes.StarConnection:
                        #region StarConnection
                        var geometry1 = StarRayGeometry();
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometry1);
                        var geometry2 = StarRayGeometry();
                        geometry2.Transform = new RotateTransform(120, 15, 15);
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometry2);
                        var geometry3 = StarRayGeometry();
                        geometry3.Transform = new RotateTransform(240, 15, 15);
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometry3);
                        #endregion StarConnection
                        break;
                    case CoilsConnectionTypes.DeltaConnection:
                        #region DeltaConnection
                        var geometryDelta = DeltaGeometry();
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometryDelta);
                        #endregion DeltaConnection
                        break;
                    case CoilsConnectionTypes.VConnection:
                        #region VConnection
                        var geometryV = VGeometry();
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometryV);
                        #endregion VConnection
                        break;
                    case CoilsConnectionTypes.OnePhaseConnection:
                        #region OnePhaseConnection
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, new Point(15, 24), new Point(15, 4));
                        #endregion OnePhaseConnection
                        break;
                    default:
                        break;
                }

                if (IsPower)
                {
                    if (CoilLeftExitIsExist2)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(-15, 15), new Point(-4, 15));
                    //if (CoilTopExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, -15), new Point(15, -4));                    
                    if (CoilRightExitIsExist2)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(34, 15), new Point(45, 15));
                    if (CoilBottomExitIsExist2)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, 34), new Point(15, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, 15), 20, 20);

                }
                else
                {
                    //if (CoilLeftExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(-15, 15), new Point(4, 15));
                    //if (CoilTopExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, -15), new Point(15, 4));
                    //if (CoilRightExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(26, 15), new Point(45, 15));
                    //if (CoilBottomExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, 26), new Point(15, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, 15), 12, 12); 
                }
                translate.Dispose();
           
        }
        //TODO
        protected override void DrawIsSelected(DrawingContext ctx)
        {
            if (ControlISSelected)
            {
                var rotate = ctx.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
                //Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
                if (IsPower)
                {
                    ctx.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-5, -5, 40, 70));
                }
                else
                {
                    ctx.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(0, 0, 30, 49));
                }
                if (DrawingVisualText != null && DrawingVisualText.Bounds.Width > 0)
                {
                    ctx.DrawRectangle(BrushIsSelected, PenIsSelected, DrawingVisualText.Bounds);
                }
                rotate.Dispose();
            }
        }

        protected override void DrawMouseOver(DrawingContext ctx)
        {
            var rotate = ctx.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
            //Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
            if (IsPower)
            {
                ctx.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(-7, -7, 40, 70));
            }
            else
            {
                ctx.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(0, 0, 30, 49));
            }
            if (DrawingVisualText != null && DrawingVisualText.Bounds.Width > 0)
            {
                ctx.DrawRectangle(BrushMouseOver, PenMouseOver, DrawingVisualText.Bounds);
            }
            rotate.Dispose();
        }
    }
}