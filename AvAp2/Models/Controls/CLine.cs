using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Rendering;
using AvAp2.Interfaces;

namespace AvAp2.Models
{
    [Description("Линия")]
    public class CLine : BasicEquipment, IGeometry
    {
        #region  У прямоугольника можно просто поменять цвет, а у линии только через класс напряжения, просто цвет спрятан
        [Category("Свойства элемента мнемосхемы"), Description("Цвет содержимого элемента"), PropertyGridFilterAttribute, DisplayName("Цвет содержимого"), Browsable(true)]
        public override Color ContentColor// У прямоугольника можно просто поменять цвет, а у линии только через класс напряжения, просто цвет спрятан
        {
            get => (Color)GetValue(ContentColorProperty);
            set
            {
                SetValue(ContentColorProperty, value);
                RiseMnemoNeedSave();
            }
        }
        [Category("Свойства элемента мнемосхемы"), Description("Цвет содержимого элемента альтернативный"), PropertyGridFilterAttribute, DisplayName("Цвет содержимого альтернативный"), Browsable(true)]
        public override Color ContentColorAlternate
        {
            get => (Color)GetValue(ContentColorAlternateProperty);
            set
            {
                SetValue(ContentColorAlternateProperty, value);
                RiseMnemoNeedSave();
            }
        }
        #endregion  У прямоугольника можно просто поменять цвет, а у линии только через класс напряжения, просто цвет спрятан

        #region IGeometry
        [Category("Свойства элемента мнемосхемы"), Description("X-координата дальнего конца линии"), PropertyGridFilterAttribute, DisplayName("Координата-X конца"), Browsable(true)]
        public double CoordinateX2
        {
            get => (double)GetValue(CoordinateX2Property);
            set
            {
                SetValue(CoordinateX2Property, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<double> CoordinateX2Property = AvaloniaProperty.Register<CLine, double>(nameof(CoordinateX2), 0.0);

        [Category("Свойства элемента мнемосхемы"), Description("Y-координата дальнего конца линии"), PropertyGridFilterAttribute, DisplayName("Координата-Y конца"), Browsable(true)]
        public double CoordinateY2
        {
            get => (double)GetValue(CoordinateY2Property);
            set
            {
                SetValue(CoordinateY2Property, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<double> CoordinateY2Property = AvaloniaProperty.Register<CLine, double>(nameof(CoordinateY2), 0.0);

        public Point Coordinate2
        {
            get => new Point(CoordinateX2, CoordinateY2);
        }

        [Category("Свойства элемента мнемосхемы"), Description("Толщина линии"), PropertyGridFilterAttribute, DisplayName("Толщина"), Browsable(true)]
        public double LineThickness
        {
            get => (double)GetValue(LineThicknessProperty);
            set
            {
                SetValue(LineThicknessProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<double> LineThicknessProperty = AvaloniaProperty.Register<CLine, double>(nameof(LineThickness),3.0, notifying: OnLineThicknessChanged);

        private static void OnLineThicknessChanged(IAvaloniaObject arg1, bool arg2)
        {
            CLine? l = arg1 as CLine;
            l.PenContentColor = new Pen(l.BrushContentColor, l.LineThickness);
            if (l.IsDash)
            {
                l.PenContentColor.DashStyle = DashStyle.Dash;
                l.PenContentColorAlternate.DashStyle = DashStyle.Dash;
            }
            
            l.PenContentColor.ToImmutable();
            l.PenContentColorAlternate = new Pen(l.BrushContentColorAlternate, l.LineThickness);
            
            l.PenContentColorAlternate.ToImmutable();
            l.PenIsSelected = new Pen(Brushes.Red, l.LineThickness+2);
            l.PenIsSelected.ToImmutable();

            l.PenMouseOver = new Pen(Brushes.Black, l.LineThickness+2);
            l.PenMouseOver.ToImmutable();

            /*l.DrawBase();
            l.DrawIsSelected();
            l.DrawMouseOver();*/
        }
        

        [Category("Свойства элемента мнемосхемы"), Description("Пунктирная линия"), PropertyGridFilterAttribute, DisplayName("Пунктирная линия"), Browsable(true)]
        public bool IsDash
        {
            get => (bool)GetValue(IsDashProperty);
            set
            {
                SetValue(IsDashProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> IsDashProperty = AvaloniaProperty.Register<CLine, bool>(nameof(IsDash), false, notifying:OnIsDashChanged);
        

        private static void OnIsDashChanged(IAvaloniaObject arg1, bool arg2)
        {
            
            CLine l = arg1 as CLine;
            l.PenContentColor = new Pen(l.BrushContentColor, l.LineThickness);
            if (arg2 && l.IsDash)
            {
                l.PenContentColor.DashStyle = DashStyle.Dash;
                l.PenContentColorAlternate.DashStyle = DashStyle.Dash;
            }

            l.PenContentColor.ToImmutable();

            l.PenContentColorAlternate = new Pen(l.BrushContentColorAlternate, l.LineThickness);
            l.PenContentColorAlternate.ToImmutable();
        }
        

         #endregion IGeometry

        #region IConnection
        [Category("Свойства элемента мнемосхемы"), Description("Значение привязанного тега состояния связи (м.б. 0-нет, 1-есть, 2-ошибка; м.б. 1-Down, 2-Up, 3-Test)"), PropertyGridFilterAttribute, DisplayName("Значение состояние - на связи"), Browsable(true)]
        public string StringStateIsConnected
        {
            get => (string)GetValue(StringStateIsConnectedProperty);
            set
            {
                SetValue(StringStateIsConnectedProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> StringStateIsConnectedProperty = AvaloniaProperty.Register<CLine, string>(nameof(StringStateIsConnected), "1" );

        #endregion IConnection

        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"), PropertyGridFilterAttribute, DisplayName("Сетка"), Browsable(false)]
        public override bool ControlIs30Step
        {
            get => false;
        }

        static CLine()
        {
            AffectsRender<CLine>(CoordinateX2Property, CoordinateY2Property, IsDashProperty, LineThicknessProperty);
        }
        public CLine() : base()
        {
            DataContext = this;
            PenContentColor = new Pen(BrushContentColor, LineThickness);
            PenContentColorAlternate = new Pen(BrushContentColorAlternate, LineThickness);
            
            if (IsDash)
            {
                PenContentColor.DashStyle = DashStyle.Dash;
                PenContentColorAlternate.DashStyle = DashStyle.Dash;
            }
            PenContentColor.ToImmutable();
            PenContentColorAlternate.ToImmutable();
            PenIsSelected = new Pen(Brushes.Red, LineThickness + 2);
            PenIsSelected.ToImmutable();

            PenMouseOver = new Pen(Brushes.Black, LineThickness + 2);
            PenMouseOver.ToImmutable();

            ResizerWidth = 3;
            ResizerHeight = 3;

        }
        public double ResizerWidth { get; private set; }
        public double ResizerHeight { get; private set; }
        
        private bool IsPointInResizer(Point point)
        {
            return Math.Abs(point.X - CoordinateX2) < ResizerWidth && Math.Abs(point.Y - CoordinateY2) < ResizerHeight;
        }


        public override string ElementTypeFriendlyName
        {
            get => "Линия";
        }


        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }
        
        protected override void DrawIsSelected()
        {
            DrawingIsSelected = new GeometryDrawing();
            DrawingResizer = new GeometryDrawing();
            if (ControlISSelected)
            {
                DrawingIsSelected.Geometry = new LineGeometry(new Point(0, 0),new Point( CoordinateX2, CoordinateY2));
                var ellipse = new EllipseGeometry();
                
                ellipse.Center = new Point(CoordinateX2, CoordinateY2);
                ellipse.RadiusX = ellipse.RadiusY = 3;
                DrawingResizer.Geometry = ellipse;
            }

            DrawingIsSelected.Brush = BrushIsSelected;
            DrawingIsSelected.Pen = PenIsSelected;
            DrawingResizer.Brush = Brushes.WhiteSmoke;
            DrawingIsSelectedWrapper.Source = new DrawingImage(new DrawingGroup
                {
                    Children = new DrawingCollection(new []{DrawingIsSelected, DrawingResizer})
                }
            );
            DrawingIsSelectedWrapper.RenderTransform = new RotateTransform(Angle, 15, 15);
        }

        protected override void DrawMouseOver()
        {
            DrawingMouseOver.Geometry = new LineGeometry(new Point(0,0), new Point(CoordinateX2, CoordinateY2));
            DrawingMouseOver.Brush = BrushMouseOver;
            DrawingMouseOver.Pen = PenMouseOver;
            DrawingMouseOverWrapper.Source = new DrawingImage(DrawingMouseOver);
            DrawingMouseOverWrapper.RenderTransform = new RotateTransform(Angle);
        }


        public override void Render(DrawingContext drawingContext)
        {
            base.Render(drawingContext);
#warning Не лучший вариант, но по 104 Сашина библиотека выдаёт строки "0" и "1"
            bool isActiveState = false;// При настройке, пока ничего не привязано, рисуем цветом класса напряжения
            if (TagDataMainState == null)
                isActiveState = true;
            else
            {
                if (TagDataMainState.TagValueString != null)
                    if (TagDataMainState.TagValueString.Equals(StringStateIsConnected))// В работе если что-то привязано и там связь есть
                        isActiveState = true;
            }
            drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(0, 0), new Point(CoordinateX2, CoordinateY2));
        }
        #region Изменение размеров
        internal protected bool IsResizerPressed = false;

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (IsPointInResizer(e.GetPosition(this)))
            {
                IsResizerPressed = IsModifyPressed = true;
                IsTextPressed = false;
                e.Handled = true;
            }

            var t = e.GetPosition(this);
            #warning в авалонии 11 хит тест начал работать по  другому, так что пока так
            if (DrawingVisualText.Bounds.Contains(t))
            {
                IsTextPressed = IsModifyPressed = true;
                IsResizerPressed = false;
                e.Handled = true;
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
                if (IsResizerPressed)
                {
                    #region перетаскивание

                    int deltaStep = 30;

                    int deltaX = (int)(currentPoint.X - ModifyStartPoint.X) / deltaStep;
                    int deltaY = (int)(currentPoint.Y - ModifyStartPoint.Y) / deltaStep;

                    if ((Math.Abs(deltaX) > 0) || ((Math.Abs(deltaY) > 0)))
                    {
                        
                        SetValue(CoordinateX2Property, CoordinateX2 + (deltaX * deltaStep));
                            SetValue(CoordinateY2Property, CoordinateY2 + (deltaY * deltaStep));

                            ModifyStartPoint = new Point(ModifyStartPoint.X + (deltaX * deltaStep),
                                ModifyStartPoint.Y + (deltaY * deltaStep));
                            
                    }
                    

                    #endregion перетаскивание
                }
            }

        }

        #endregion

        
       
    }
}