using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Rendering;
using AvAp2.Interfaces;

namespace AvAp2.Models
{
     [Description("Прямоугольник")]
    public class CRectangle : BasicWithState, IGeometry
    {        
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
        public static StyledProperty<double> CoordinateX2Property = AvaloniaProperty.Register<CRectangle,double>(nameof(CoordinateX2), 0.0);

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
        public static StyledProperty<double> CoordinateY2Property = AvaloniaProperty.Register<CRectangle,double>(nameof(CoordinateY2), 0.0);

        public Point Coordinate2
        {
            get => new Point(CoordinateX2, CoordinateY2);
        }

        public double ResizerWidth { get; private set; }
        public double ResizerHeight { get; private set; }
        public Rect Border
        {
            get => new Rect(0, 0, CoordinateX2 > 0 ? CoordinateX2 : 1, CoordinateY2 > 0 ? CoordinateY2 : 1);
        }
        [Category("Свойства элемента мнемосхемы"), Description("Толщина линии"), PropertyGridFilterAttribute, DisplayName("Толщина"), Browsable(true)]
        public double LineThickness
        {
            get => (double)GetValue(LineThicknessProperty);
            set
            {
                SetValue(LineThicknessProperty, value);
                ReturnContentPenToDefault();
                
                PenIsSelected = new Pen(Brushes.Red, LineThickness);
                PenIsSelected.ToImmutable();

                PenMouseOver = new Pen(Brushes.Black, LineThickness);
                PenMouseOver.ToImmutable();
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<double> LineThicknessProperty = AvaloniaProperty.Register<CRectangle,double>(nameof(LineThickness), 1.0);

        public void ReturnContentPenToDefault()
        {
            ResizerWidth = 3;
            ResizerHeight = 3;
            PenContentColor = new Pen(BrushContentColor, LineThickness);
            PenContentColor.ToImmutable();
            
            PenContentColorAlternate = new Pen(BrushContentColorAlternate, LineThickness);
            PenContentColorAlternate.ToImmutable();
            
            if (IsDash)
            {
                PenContentColor.DashStyle = DashStyle.Dash;
                PenContentColorAlternate.DashStyle = DashStyle.Dash;
            }
        }

        [Category("Свойства элемента мнемосхемы"), Description("Пунктирная линия"), PropertyGridFilterAttribute, DisplayName("Пунктирная линия"), Browsable(true)]
        public bool IsDash
        {
            get => (bool)GetValue(IsDashProperty);
            set
            {
                SetValue(IsDashProperty, value);
                ReturnContentPenToDefault();
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> IsDashProperty = AvaloniaProperty.Register<CRectangle,bool>(nameof(IsDash), false);
        

        #endregion IGeometry

       
        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"), PropertyGridFilterAttribute, DisplayName("Сетка"), Browsable(false)]
        public override bool ControlIs30Step
        {
            get => false;
        }

        public CRectangle() : base()
        {
            AffectsRender<CRectangle>(IsDashProperty, CoordinateX2Property,CoordinateY2Property, LineThicknessProperty);
            ReturnContentPenToDefault();
            DataContext = this;
            PenIsSelected = new Pen(Brushes.Red, LineThickness);
            PenIsSelected.ToImmutable();

            PenMouseOver = new Pen(Brushes.Black, LineThickness);
            PenMouseOver.ToImmutable();
        }


        public override string ElementTypeFriendlyName
        {
            get => "Прямоугольник";
        }


        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        protected bool IsPointInResizer(Point point)
        {
            return Math.Abs(point.X - CoordinateX2) < ResizerWidth && Math.Abs(point.Y - CoordinateY2) < ResizerHeight;
        }


        public override void Render(DrawingContext drawingContext)
        {
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
            drawingContext.DrawRectangle(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, Border);
        }

        protected override void DrawIsSelected()
        {
            DrawingIsSelected = new GeometryDrawing();
            DrawingResizer = new GeometryDrawing();
            if (ControlISSelected)
            {
                DrawingIsSelected.Geometry = new RectangleGeometry(new Rect(0, 0, CoordinateX2, CoordinateY2));
                var ellipse = new EllipseGeometry();
                
                ellipse.Center = new Point(CoordinateX2, CoordinateY2);
                ellipse.RadiusX = ellipse.RadiusY = 3;
                DrawingResizer.Geometry = ellipse;
            }

            DrawingIsSelected.Brush = BrushIsSelected;
            DrawingIsSelected.Pen = PenIsSelected;
            DrawingResizer.Brush = Brushes.WhiteSmoke;
        }

        protected override void DrawMouseOver()
        {

            DrawingMouseOver.Geometry = new RectangleGeometry(new Rect(0, 0, CoordinateX2, CoordinateY2)); 
            DrawingMouseOver.Brush = BrushMouseOver;
            DrawingMouseOver.Pen = PenMouseOver;
        }

        /*internal protected override void DrawIsSelected()
        {
            using (var drawingContext = DrawingVisualIsSelected.RenderOpen())
            {
                //drawingContext.DrawLine(PenIsSelected, new Point(0, 0), new Point(CoordinateX2, CoordinateY2));
                drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-1, -1, CoordinateX2 > 0 ? CoordinateX2 + 2 : 1, CoordinateY2 > 0 ? CoordinateY2 + 2 : 1));
                drawingContext.Close();
            }
            using (var drawingContext = DrawingVisualResizer.RenderOpen())
            {
                drawingContext.DrawEllipse(Brushes.WhiteSmoke, null, new Point(CoordinateX2, CoordinateY2), 3, 3);
                drawingContext.Close();
            }
            DrawingVisualIsSelected.Opacity = DrawingVisualResizer.Opacity = ControlISSelected ? .3 : 0;
        }

        internal protected override void DrawMouseOver()
        {
            using (var drawingContext = DrawingVisualIsMouseOver.RenderOpen())
            {
                //drawingContext.DrawLine(PenMouseOver, new Point(0, 0), new Point(CoordinateX2, CoordinateY2));
                drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(-1, -1, CoordinateX2 > 0 ? CoordinateX2 + 2 : 1, CoordinateY2 > 0 ? CoordinateY2 + 2 : 1)); 
                drawingContext.Close();
            }
            DrawingVisualIsMouseOver.Opacity = 0;
        }*/


        #region Изменение размеров
        internal protected bool IsResizerPressed = false;
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
                        double resizerDX = 0;
                        if (CoordinateX2 + (deltaX * deltaStep) > 0)
                        {
                            resizerDX = (deltaX * deltaStep);
                            SetValue(CoordinateX2Property, CoordinateX2 + resizerDX); 
                        }
                        double resizerDY = 0;
                        if (CoordinateY2 + (deltaY * deltaStep) > 0)
                        {
                            resizerDY = (deltaY * deltaStep);
                            SetValue(CoordinateY2Property, CoordinateY2 + resizerDY);
                        }

                        ModifyStartPoint = new Point(ModifyStartPoint.X + resizerDX, ModifyStartPoint.Y + resizerDY);
                    }
                    #endregion перетаскивание
                }
                InvalidateStyles();
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (IsPointInResizer(e.GetPosition(this)))
            {
                IsResizerPressed = IsModifyPressed = true;
                IsTextPressed = false;
            }

            if (DrawingVisualText.HitTestCustom(e.GetPosition(null)))
            {
                IsTextPressed = IsModifyPressed = true;
                IsResizerPressed = false;
            }
        }

        #endregion Изменение размеров
    }
}