﻿using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using AvAp2.Converters;
using IProjectModel;

namespace AvAp2.Models.Controls
{
    [Description("Четырехобмоточный трансформатор")]
    public class CTransformer4Coils : CTransformer3CoilsV2
    {
        [Category("Свойства элемента мнемосхемы"), Description("Соединение четвёртых обмоток трансформатора"), PropertyGridFilter, DisplayName("Четвертая обмотка соединение"), Browsable(true)]
        public CoilsConnectionTypes CoilsConnectionType4
        {
            get => (CoilsConnectionTypes)GetValue(CoilsConnectionType4Property);
            set
            {
                SetValue(CoilsConnectionType4Property, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<CoilsConnectionTypes> CoilsConnectionType4Property = AvaloniaProperty.Register<CTransformer4Coils, CoilsConnectionTypes >(nameof(CoilsConnectionType4), CoilsConnectionTypes.DeltaConnection);

        internal protected Brush BrushContentColorVoltage4;
        internal protected Pen PenContentColorVoltage4;
        internal protected Pen PenContentColorVoltage4Thin;

        [Category("Свойства элемента мнемосхемы"), Description("Цвет четвёртой обмотки в соответствии с классом напряжения"), PropertyGridFilter, DisplayName("Четвертая обмотка цвет"), Browsable(false)]
        private Color Voltage4Color
        {
            get => (Color)GetValue(Voltage4ColorProperty);
            set
            {
                SetValue(Voltage4ColorProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<Color> Voltage4ColorProperty = AvaloniaProperty.Register<CTransformer4Coils, Color>(nameof(Voltage4Color), Color.FromArgb(255, 0, 180, 200));
        private static void OnVoltage4ColorChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
        {
            var sender = obj.Sender as CTransformer4Coils;
            sender.BrushContentColorVoltage4 = new SolidColorBrush(sender.Voltage4Color);
            sender.BrushContentColorVoltage4.ToImmutable();
            sender.PenContentColorVoltage4 = new Pen(sender.BrushContentColorVoltage4, 3);
            sender.PenContentColorVoltage4.ToImmutable();
            sender.PenContentColorVoltage4Thin = new Pen(sender.BrushContentColorVoltage4, 1);
            sender.PenContentColorVoltage4Thin.ToImmutable();
        }

        [Category("Свойства элемента мнемосхемы"), Description("Класс напряжения четвёртой обмотки"), PropertyGridFilter, DisplayName("Четвертая обмотка напряжение"), Browsable(true)]
        public VoltageClasses Voltage4
        {
            get => (VoltageClasses)GetValue(Voltage4Property);
            set
            {
                SetValue(Voltage4Property, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<VoltageClasses> Voltage4Property = AvaloniaProperty.Register<CTransformer4Coils, VoltageClasses>(nameof(Voltage4), VoltageClasses.kV110);

        private static void OnVoltage4Changed(AvaloniaPropertyChangedEventArgs<VoltageClasses> obj)
        {
            #region класс напряжения 2 обмотки
            
            (obj.Sender as CTransformer4Coils).Voltage4Color = VoltageEnumColors.VoltageColors[(obj.Sender as CTransformer4Coils).Voltage4];

            #endregion класс напряжения 2 обмотки
        }

        #region выводы обмотки
        [Category("Свойства элемента мнемосхемы"), Description("Видимость левого вывода четвёртой обмотки"), PropertyGridFilter, DisplayName("Четвертая обмотка левый"), Browsable(true)]
        public bool CoilLeftExitIsExist4
        {
            get => (bool)GetValue(CoilLeftExitIsExist4Property);
            set
            {
                SetValue(CoilLeftExitIsExist4Property, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> CoilLeftExitIsExist4Property = AvaloniaProperty.Register<CTransformer4Coils,bool >(nameof(CoilLeftExitIsExist4), false);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость верхнего вывода четвёртой обмотки"), PropertyGridFilter, DisplayName("Четвертая обмотка верхний"), Browsable(true)]
        public bool CoilTopExitIsExist4
        {
            get => (bool)GetValue(CoilTopExitIsExist4Property);
            set
            {
                SetValue(CoilTopExitIsExist4Property, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> CoilTopExitIsExist4Property = AvaloniaProperty.Register<CTransformer4Coils, bool>(nameof(CoilTopExitIsExist4), false);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость правого вывода четвёртой обмотки"), PropertyGridFilter, DisplayName("Четвертая обмотка правый"), Browsable(true)]
        public bool CoilRightExitIsExist4
        {
            get => (bool)GetValue(CoilRightExitIsExist4Property);
            set
            {
                SetValue(CoilRightExitIsExist4Property, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> CoilRightExitIsExist4Property = AvaloniaProperty.Register<CTransformer4Coils, bool>(nameof(CoilRightExitIsExist4), false);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость нижнего вывода четвёртой обмотки"), PropertyGridFilter, DisplayName("Четвертая обмотка нижний"), Browsable(true)]
        public bool CoilBottomExitIsExist4
        {
            get => (bool)GetValue(CoilBottomExitIsExist4Property);
            set
            {
                SetValue(CoilBottomExitIsExist4Property, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> CoilBottomExitIsExist4Property = AvaloniaProperty.Register<CTransformer4Coils, bool>(nameof(CoilBottomExitIsExist4),false);
        #endregion выводы обмотки

        static CTransformer4Coils()
        {
            AffectsRender<CTransformer4Coils>(CoilBottomExitIsExist4Property, CoilsConnectionType4Property, CoilLeftExitIsExist4Property, CoilRightExitIsExist4Property, CoilTopExitIsExist4Property, Voltage4Property, Voltage4ColorProperty);
            Voltage4ColorProperty.Changed.Subscribe(OnVoltage4ColorChanged);
            Voltage4Property.Changed.Subscribe(OnVoltage4Changed);
        }
        
        public CTransformer4Coils() : base()
        {
            BrushContentColorVoltage4 = new SolidColorBrush(Voltage4Color);
            BrushContentColorVoltage4.ToImmutable();
            PenContentColorVoltage4 = new Pen(BrushContentColorVoltage4, 3);
            PenContentColorVoltage4.ToImmutable();
            PenContentColorVoltage4Thin = new Pen(BrushContentColorVoltage4, 1);
            PenContentColorVoltage4Thin.ToImmutable();
            
        }
        public override string ElementTypeFriendlyName
        {
            get => "Четырехобмоточный трансформатор";
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
                rotate = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
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
                // Вторичная обмотка (слева)
                DrawingContext.PushedState translate;
                if (IsPower)
                    translate = drawingContext.PushPostTransform(new TranslateTransform(-18, 30).Value);
                else
                    translate = drawingContext.PushPostTransform(new TranslateTransform(-11, 19).Value);
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
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(-27, 15), new Point(-4, 15));
                    //if (CoilTopExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, -15), new Point(15, -4));                    
                    //if (CoilRightExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(34, 15), new Point(45, 15));
                    //if (CoilBottomExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(8, 34), new Point(2, 45));
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

                //====================================================================================================================================
                // Третья обмотка (справа)
                translate.Dispose();
                DrawingContext.PushedState translate1;
                if (IsPower)
                    translate1 = drawingContext.PushPostTransform(new TranslateTransform(18, 30).Value);
                else
                    translate1 = drawingContext.PushPostTransform(new TranslateTransform(11, 19).Value);
                switch (CoilsConnectionType3)
                {
                    case CoilsConnectionTypes.StarConnection:
                        #region StarConnection
                        var geometry1 = StarRayGeometry();
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometry1);
                        var geometry2 = StarRayGeometry();
                        geometry2.Transform = new RotateTransform(120, 15, 15);
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometry2);
                        var geometry3 = StarRayGeometry();
                        geometry3.Transform = new RotateTransform(240, 15, 15);
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometry3);
                        #endregion StarConnection
                        break;
                    case CoilsConnectionTypes.DeltaConnection:
                        #region DeltaConnection
                        var geometryDelta = DeltaGeometry();
                        
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometryDelta);
                        #endregion DeltaConnection
                        break;
                    case CoilsConnectionTypes.VConnection:
                        #region VConnection
                        var geometryV = VGeometry();
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometryV);
                        #endregion VConnection
                        break;
                    case CoilsConnectionTypes.OnePhaseConnection:
                        #region OnePhaseConnection
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, new Point(15, 24), new Point(15, 4));
                        #endregion OnePhaseConnection
                        break;
                    default:
                        break;
                }

                if (IsPower)
                {
                    //if (CoilLeftExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(-16, 15), new Point(-4, 15));
                    //if (CoilTopExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(13, -30), new Point(13, -4));
                    if (CoilRightExitIsExist3)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(34, 15), new Point(57, 15));
                    //if (CoilBottomExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(22, 34), new Point(28, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(14, 15), 20, 20);

                }
                else
                {
                    //if (CoilLeftExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(-15, 15), new Point(4, 15));
                    //if (CoilTopExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(15, -15), new Point(15, 4));
                    //if (CoilRightExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(26, 15), new Point(45, 15));
                    //if (CoilBottomExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(15, 26), new Point(15, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(15, 15), 12, 12);
                }
                //====================================================================================================================================
                // Четвёртая обмотка (снизу)
                translate1.Dispose();
                DrawingContext.PushedState translate2;
                if (IsPower)
                    translate2 = drawingContext.PushPostTransform(new TranslateTransform(0, 60).Value);
                else
                    translate2 = drawingContext.PushPostTransform(new TranslateTransform(0, 38).Value);
                switch (CoilsConnectionType4)
                {
                    case CoilsConnectionTypes.StarConnection:
                        #region StarConnection
                        var geometry1 = StarRayGeometry();
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage4Thin : PenContentColorThinAlternate, geometry1);
                        var geometry2 = StarRayGeometry();
                        geometry2.Transform = new RotateTransform(120, 15, 15);
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage4Thin : PenContentColorThinAlternate, geometry2);
                        var geometry3 = StarRayGeometry();
                        geometry3.Transform = new RotateTransform(240, 15, 15);
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage4Thin : PenContentColorThinAlternate, geometry3);
                        #endregion StarConnection
                        break;
                    case CoilsConnectionTypes.DeltaConnection:
                        #region DeltaConnection
                        var geometryDelta = DeltaGeometry();
                        
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage4Thin : PenContentColorThinAlternate, geometryDelta);
                        #endregion DeltaConnection
                        break;
                    case CoilsConnectionTypes.VConnection:
                        #region VConnection
                        var geometryV = VGeometry();
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage4Thin : PenContentColorThinAlternate, geometryV);
                        #endregion VConnection
                        break;
                    case CoilsConnectionTypes.OnePhaseConnection:
                        #region OnePhaseConnection
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage4Thin : PenContentColorThinAlternate, new Point(15, 24), new Point(15, 4));
                        #endregion OnePhaseConnection
                        break;
                    default:
                        break;
                }

                if (IsPower)
                {
                    if (CoilLeftExitIsExist4)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage4 : PenContentColorAlternate, new Point(-16, 15), new Point(-4, 15));
                    //if (CoilTopExitIsExist4)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage4 : PenContentColorAlternate, new Point(13, -30), new Point(13, -4));
                    if (CoilRightExitIsExist4)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage4 : PenContentColorAlternate, new Point(34, 15), new Point(45, 15));
                    if (CoilBottomExitIsExist4)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage4 : PenContentColorAlternate, new Point(15, 34), new Point(15, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColorVoltage4 : PenContentColorAlternate, new Point(15, 15), 20, 20);

                    
                }
                else
                {
                    //if (CoilLeftExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage4 : PenContentColorAlternate, new Point(-15, 15), new Point(4, 15));
                    //if (CoilTopExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage4 : PenContentColorAlternate, new Point(15, -15), new Point(15, 4));
                    //if (CoilRightExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage4 : PenContentColorAlternate, new Point(26, 15), new Point(45, 15));
                    //if (CoilBottomExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage4 : PenContentColorAlternate, new Point(15, 26), new Point(15, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColorVoltage4 : PenContentColorAlternate, new Point(15, 14), 12, 12);
                }
                translate2.Dispose();
                rotate.Dispose();
        }

        protected override void DrawIsSelected(DrawingContext ctx)
        {
            if (ControlISSelected)
            {
                var rotate = ctx.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
                //Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
                if (IsPower)
                {
                    ctx.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-27, -45, 79, 100));
                }
                else
                { 
                    ctx.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-10, -22, 54, 67));
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
                ctx.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(-25, -5, 79, 100));
            }
            else
            {
                ctx.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(-12, 0, 54, 67));
            }
            if (DrawingVisualText != null && DrawingVisualText.Bounds.Width > 0)
            {
                ctx.DrawRectangle(BrushMouseOver, PenMouseOver, DrawingVisualText.Bounds);
            }
            
            rotate.Dispose();
        }
    }
        
        /*internal protected override void DrawIsSelected()
        {
            using (var drawingContext = DrawingVisualIsSelected.RenderOpen())
            {
                drawingContext.PushTransform(new RotateTransform(Angle, 15, 15));//Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
                if (IsPower)
                    drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-25, -7, 79, 100));
                else
                    drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-12, 0, 54, 67));
                drawingContext.Pop();
                if (DrawingVisualText.ContentBounds.Width > 0)
                {
                    Rect selectedRect = DrawingVisualText.ContentBounds;
                    drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, selectedRect);
                }
                drawingContext.Close();
            }
            DrawingVisualIsSelected.Opacity = ControlISSelected ? .3 : 0;
        }

        internal protected override void DrawMouseOver()
        {
            using (var drawingContext = DrawingVisualIsMouseOver.RenderOpen())
            {
                drawingContext.PushTransform(new RotateTransform(Angle, 15, 15));//Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
                if (IsPower)
                    drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(-25, -7, 79, 100));
                else
                    drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(-12, 0, 54, 67));
                drawingContext.Pop();
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