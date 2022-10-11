using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using AvAp2.Converters;
using IProjectModel;

namespace AvAp2.Models
{
    [Description("Трехобмоточный трансформатор Версия 1 - Правый")]
    public class CTransformer3CoilsV1 : CTransformer2Coils
    {
        [Category("Свойства элемента мнемосхемы"), Description("Соединение третьих обмоток трансформатора"),
         PropertyGridFilterAttribute, DisplayName("Третья обмотка соединение"), Browsable(true)]
        public CoilsConnectionTypes CoilsConnectionType3
        {
            get => (CoilsConnectionTypes)GetValue(CoilsConnectionType3Property);
            set
            {
                SetValue(CoilsConnectionType3Property, value);
                //RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<CoilsConnectionTypes> CoilsConnectionType3Property =
            AvaloniaProperty.Register<CTransformer2Coils, CoilsConnectionTypes>(nameof(CoilsConnectionType3),
                CoilsConnectionTypes.DeltaConnection);

        internal protected Brush BrushContentColorVoltage3;
        internal protected Pen PenContentColorVoltage3;
        internal protected Pen PenContentColorVoltage3Thin;


        [Category("Свойства элемента мнемосхемы"),
         Description("Цвет третьей обмотки в соответствии с классом напряжения"), PropertyGridFilterAttribute,
         DisplayName("Третья обмотка цвет"), Browsable(false)]
        private Color Voltage3Color
        {
            get => (Color)GetValue(Voltage3ColorProperty);
            set
            {
                SetValue(Voltage3ColorProperty, value);
                //RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<Color> Voltage3ColorProperty =
            AvaloniaProperty.Register<CTransformer2Coils, Color>(nameof(Voltage3Color),
                Color.FromArgb(255, 0, 180, 200));

        private void OnVoltage3ColorChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
        {
            BrushContentColorVoltage3 = new SolidColorBrush(Voltage3Color);
            BrushContentColorVoltage3.ToImmutable();
            PenContentColorVoltage3 = new Pen(BrushContentColorVoltage3, 3);
            PenContentColorVoltage3.ToImmutable();
            PenContentColorVoltage3Thin = new Pen(BrushContentColorVoltage3, 1);
            PenContentColorVoltage3Thin.ToImmutable();
        }

        [Category("Свойства элемента мнемосхемы"), Description("Класс напряжения третьей обмотки"),
         PropertyGridFilterAttribute, DisplayName("Третья обмотка напряжение"), Browsable(true)]
        public VoltageClasses Voltage3
        {
            get => (VoltageClasses)GetValue(Voltage3Property);
            set
            {
                SetValue(Voltage3Property, value);
                //RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<VoltageClasses> Voltage3Property =
            AvaloniaProperty.Register<CTransformer2Coils, VoltageClasses>(nameof(Voltage3), VoltageClasses.kV110);

        private void OnVoltage3Changed(AvaloniaPropertyChangedEventArgs<VoltageClasses> obj)
        {
            #region класс напряжения 2 обмотки

            Voltage3Color = VoltageEnumColors.VoltageColors[Voltage3];

            #endregion класс напряжения 2 обмотки
        }

        #region выводы обмотки

        [Category("Свойства элемента мнемосхемы"), Description("Видимость левого вывода третьей обмотки"),
         PropertyGridFilterAttribute, DisplayName("Третья обмотка левый"), Browsable(true)]
        public bool CoilLeftExitIsExist3
        {
            get => (bool)GetValue(CoilLeftExitIsExist3Property);
            set
            {
                SetValue(CoilLeftExitIsExist3Property, value);
                //RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<bool> CoilLeftExitIsExist3Property =
            AvaloniaProperty.Register<CTransformer2Coils, bool>(nameof(CoilLeftExitIsExist3), false);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость верхнего вывода третьей обмотки"),
         PropertyGridFilterAttribute, DisplayName("Третья обмотка верхний"), Browsable(true)]
        public bool CoilTopExitIsExist3
        {
            get => (bool)GetValue(CoilTopExitIsExist3Property);
            set
            {
                SetValue(CoilTopExitIsExist3Property, value);
                //RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<bool> CoilTopExitIsExist3Property =
            AvaloniaProperty.Register<CTransformer2Coils, bool>(nameof(CoilTopExitIsExist3), false);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость правого вывода третьей обмотки"),
         PropertyGridFilterAttribute, DisplayName("Третья обмотка правый"), Browsable(true)]
        public bool CoilRightExitIsExist3
        {
            get => (bool)GetValue(CoilRightExitIsExist3Property);
            set
            {
                SetValue(CoilRightExitIsExist3Property, value);
                //RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<bool> CoilRightExitIsExist3Property =
            AvaloniaProperty.Register<CTransformer2Coils, bool>(nameof(CoilRightExitIsExist3), false);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость нижнего вывода третьей обмотки"),
         PropertyGridFilterAttribute, DisplayName("Третья обмотка нижний"), Browsable(true)]
        public bool CoilBottomExitIsExist3
        {
            get => (bool)GetValue(CoilBottomExitIsExist3Property);
            set
            {
                SetValue(CoilBottomExitIsExist3Property, value);
                //RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<bool> CoilBottomExitIsExist3Property =
            AvaloniaProperty.Register<CTransformer2Coils, bool>(nameof(CoilBottomExitIsExist3), false);

        #endregion выводы обмотки

        public CTransformer3CoilsV1() : base()
        {
            BrushContentColorVoltage3 = new SolidColorBrush(Voltage3Color);
            BrushContentColorVoltage3.ToImmutable();
            PenContentColorVoltage3 = new Pen(BrushContentColorVoltage3, 3);
            PenContentColorVoltage3.ToImmutable();
            PenContentColorVoltage3Thin = new Pen(BrushContentColorVoltage3, 1);
            PenContentColorVoltage3Thin.ToImmutable();
        }

        public override string ElementTypeFriendlyName
        {
            get => "Трехобмоточный трансформатор";
        }

        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        public override void Render(DrawingContext drawingContext)
        {
            #region isActiveState

#warning Не лучший вариант, но по 104 Сашина библиотека выдаёт строки "0" и "1"
            bool isActiveState = false; // При настройке, пока ничего не привязано, рисуем цветом класса напряжения
            if (TagDataMainState == null)
                isActiveState = true;
            else
            {
                if (TagDataMainState.TagValueString != null)
                    if (TagDataMainState.TagValueString.Equals("1")) // В работе если что-то привязано и там "1"
                        isActiveState = true;
            }

            #endregion isActiveState

            DrawingContext.PushedState rotation =
                drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
            switch (CoilsConnectionType)
            {
                case CoilsConnectionTypes.StarConnection:

                    #region StarConnection

                    var geometry1 = StarRayGeometry();
                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometry1);
                    var geometry2 = StarRayGeometry();
                    geometry2.Transform = new RotateTransform(120, 15, 15);
                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometry2);
                    var geometry3 = StarRayGeometry();
                    geometry3.Transform = new RotateTransform(240, 15, 15);
                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometry3);

                    #endregion StarConnection

                    break;
                case CoilsConnectionTypes.DeltaConnection:

                    #region DeltaConnection

                    var geometryDelta = DeltaGeometry();

                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometryDelta);

                    #endregion DeltaConnection

                    break;
                case CoilsConnectionTypes.VConnection:

                    #region VConnection

                    var geometryV = VGeometry();
                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometryV);

                    #endregion VConnection

                    break;
                case CoilsConnectionTypes.OnePhaseConnection:

                    #region OnePhaseConnection

                    drawingContext.DrawLine(isActiveState ? PenContentColorThin : PenContentColorThinAlternate,
                        new Point(15, 24), new Point(15, 4));

                    #endregion OnePhaseConnection

                    break;
                default:
                    break;
            }

            if (IsPower)
            {
                if (CoilLeftExitIsExist)
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate,
                        new Point(-15, 15), new Point(-4, 15));
                if (CoilTopExitIsExist && !AutoIsExist)
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate,
                        new Point(15, -15), new Point(15, -4));
                if (AutoIsExist)
                {
                    var geometryAuto = AutoLargeGeometry();

                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorAutoVoltage : PenContentColorAlternate, geometryAuto);
                }

                //if (CoilRightExitIsExist)
                //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(34, 15), new Point(45, 15));
                //if (CoilBottomExitIsExist)
                //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 34), new Point(15, 45));
                drawingContext.DrawEllipse(Brushes.Transparent,
                    isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 15), 20, 20);

                if (IsRegulator)
                {
                    var geometryArrow = ArrowGeometry();

                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColor : PenContentColorAlternate, geometryArrow);
                }
            }
            else
            {
                //if (CoilLeftExitIsExist)
                //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(4, 15));
                //if (CoilTopExitIsExist)
                drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, -15),
                    new Point(15, 4));
                //if (CoilRightExitIsExist)
                //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(26, 15), new Point(45, 15));
                //if (CoilBottomExitIsExist)
                //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 26), new Point(15, 45));
                drawingContext.DrawEllipse(Brushes.Transparent,
                    isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 15), 12, 12);
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
                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometry1);
                    var geometry2 = StarRayGeometry();
                    geometry2.Transform = new RotateTransform(120, 15, 15);
                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometry2);
                    var geometry3 = StarRayGeometry();
                    geometry3.Transform = new RotateTransform(240, 15, 15);
                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometry3);

                    #endregion StarConnection

                    break;
                case CoilsConnectionTypes.DeltaConnection:

                    #region DeltaConnection

                    var geometryDelta = DeltaGeometry();

                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometryDelta);

                    #endregion DeltaConnection

                    break;
                case CoilsConnectionTypes.VConnection:

                    #region VConnection

                    var geometryV = VGeometry();
                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometryV);

                    #endregion VConnection

                    break;
                case CoilsConnectionTypes.OnePhaseConnection:

                    #region OnePhaseConnection

                    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate,
                        new Point(15, 24), new Point(15, 4));

                    #endregion OnePhaseConnection

                    break;
                default:
                    break;
            }

            if (IsPower)
            {
                if (CoilLeftExitIsExist2)
                    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate,
                        new Point(-15, 15), new Point(-4, 15));
                //if (CoilTopExitIsExist2)
                //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, -15), new Point(15, -4));                    
                //if (CoilRightExitIsExist2)
                //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(34, 15), new Point(45, 15));
                if (CoilBottomExitIsExist2)
                    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate,
                        new Point(15, 34), new Point(15, 45));
                drawingContext.DrawEllipse(Brushes.Transparent,
                    isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, 15), 20, 20);

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
                drawingContext.DrawEllipse(Brushes.Transparent,
                    isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, 15), 12, 12);
            }

            //====================================================================================================================================
            // Третья обмотка
            translate.Dispose();
            DrawingContext.PushedState translate1;
            if (IsPower)
                translate1 = drawingContext.PushPostTransform(new TranslateTransform(30, 15).Value);
            else
                translate1 = drawingContext.PushPostTransform(new TranslateTransform(18, 9.5).Value);
            switch (CoilsConnectionType3)
            {
                case CoilsConnectionTypes.StarConnection:

                    #region StarConnection

                    var geometry1 = StarRayGeometry();
                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometry1);
                    var geometry2 = StarRayGeometry();
                    geometry2.Transform = new RotateTransform(120, 15, 15);
                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometry2);
                    var geometry3 = StarRayGeometry();
                    geometry3.Transform = new RotateTransform(240, 15, 15);
                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometry3);

                    #endregion StarConnection

                    break;
                case CoilsConnectionTypes.DeltaConnection:

                    #region DeltaConnection

                    var geometryDelta = DeltaGeometry();

                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometryDelta);

                    #endregion DeltaConnection

                    break;
                case CoilsConnectionTypes.VConnection:

                    #region VConnection

                    var geometryV = VGeometry();
                    drawingContext.DrawGeometry(Brushes.Transparent,
                        isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometryV);

                    #endregion VConnection

                    break;
                case CoilsConnectionTypes.OnePhaseConnection:

                    #region OnePhaseConnection

                    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate,
                        new Point(15, 24), new Point(15, 4));

                    #endregion OnePhaseConnection

                    break;
                default:
                    break;
            }

            if (IsPower)
            {
                //if (CoilLeftExitIsExist3)
                //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(-15, 15), new Point(-4, 15));
                if (CoilTopExitIsExist3)
                    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate,
                        new Point(15, -30), new Point(15, -4));
                if (CoilRightExitIsExist3)
                    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate,
                        new Point(34, 15), new Point(45, 15));
                if (CoilBottomExitIsExist3)
                    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate,
                        new Point(15, 34), new Point(15, 60));
                drawingContext.DrawEllipse(Brushes.Transparent,
                    isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(14, 15), 20, 20);

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
                drawingContext.DrawEllipse(Brushes.Transparent,
                    isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(15, 15), 12, 12);
            }

            translate1.Dispose();
            rotation.Dispose();
        }

        protected override void InitIsSelected()
        {
            Geometry geometry1 = new RectangleGeometry();
            geometry1.Transform = new RotateTransform(Angle, 15, 15);
            //Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
            if (IsPower)
                geometry1 = new RectangleGeometry(new Rect(-7, -7, 75, 74));
            else
                geometry1 = new RectangleGeometry(new Rect(0, 0, 49, 49));

            GeometryGroup geometry = new GeometryGroup();
            geometry.Children.Add(geometry1);
            if (DrawingVisualText.Bounds.Width > 0)
            {
                Rect selectedRect = DrawingVisualText.Bounds;
                geometry.Children.Add(new RectangleGeometry(selectedRect));
            }
            DrawingIsSelected = new GeometryDrawing();
            DrawingIsSelected.Geometry = geometry;
            DrawingIsSelected.Brush = BrushIsSelected;
            DrawingIsSelected.Pen = PenIsSelected;
        }
        
        protected override void InitMouseOver()
        {
            Geometry geometry1 = new RectangleGeometry();
            geometry1.Transform = new RotateTransform(Angle, 15, 15);
            //Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
            if (IsPower)
                geometry1 = new RectangleGeometry(new Rect(-7, -7, 75, 74));
            else
                geometry1 = new RectangleGeometry(new Rect(0, 0, 49, 49));

            GeometryGroup geometry = new GeometryGroup();
            geometry.Children.Add(geometry1);
            if (DrawingVisualText.Bounds.Width > 0)
            {
                Rect selectedRect = DrawingVisualText.Bounds;
                geometry.Children.Add(new RectangleGeometry(selectedRect));
            }
            DrawingMouseOver = new GeometryDrawing();
            DrawingMouseOver.Geometry = geometry;
            DrawingMouseOver.Brush = BrushIsSelected;
            DrawingMouseOver.Pen = PenIsSelected;
        }
    }
    /*internal protected override void DrawIsSelected()
    {
        using (var drawingContext = DrawingVisualIsSelected.RenderOpen())
        {
            drawingContext.PushTransform(new RotateTransform(Angle, 15, 15));//Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
            if (IsPower)
                drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-7, -7, 75, 74));
            else
                drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(0, 0, 49, 49));
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
                drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(-7, -7, 75, 74));
            else
                drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(0, 0, 49, 49));
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