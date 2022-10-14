using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace AvAp2.Models
{
    public abstract class BasicMnemoElement:UserControl,ICloneable
    {
        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }
        /// <summary>
        /// Человеческое название мнемоэлемента для подсказок
        /// </summary>
        public abstract string ElementTypeFriendlyName { get; }
        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"), PropertyGridFilterAttribute, DisplayName("Сетка"), Browsable(false)]
        public virtual bool ControlIs30Step
        {
            get => true;
        }
        public bool ControlISSelected
        {
            get => (bool)GetValue(ControlISSelectedProperty);
            set => SetValue(ControlISSelectedProperty, value);
        }
        public static StyledProperty<bool> ControlISSelectedProperty = AvaloniaProperty.Register<BasicMnemoElement,bool>(nameof(ControlISSelected), false);

        /*private static void OnSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicMnemoElement b = d as BasicMnemoElement;
            b.DrawingVisualIsSelected.Opacity = b.DrawingVisualResizer.Opacity = (bool)e.NewValue ? .3:0;
        }*/
        public bool ControlISHitTestVisible
        {
            get => (bool)GetValue(ControlISHitTestVisibleProperty);
            set
            {
                SetValue(ControlISHitTestVisibleProperty, value);
                //RiseMnemoNeedSave();
            }
        }

        public static StyledProperty<bool> ControlISHitTestVisibleProperty =
            AvaloniaProperty.Register<BasicMnemoElement,bool>(nameof(ControlISHitTestVisible));
        public GeometryDrawing DrawingIsSelected { get; set; }
        public GeometryDrawing DrawingMouseOver { get; set; }
        
        public BasicMnemoElement()
        {
            DrawingIsSelected = new GeometryDrawing();
            DrawingMouseOver = new GeometryDrawing();
            AffectsRender<BasicMnemoElement>(AngleProperty,ControlISSelectedProperty);
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
            ControlISSelectedProperty.Changed.Subscribe(OnControlIsSelectedChanged);
            this.Loaded+= OnLoaded;
            DataContext = this;
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            DrawIsSelected();
            DrawMouseOver();
        }

        private void OnControlIsSelectedChanged(AvaloniaPropertyChangedEventArgs<bool> obj)
        {
            DrawIsSelected();
            InvalidateStyles();
        }
        public static StyledProperty<double> AngleProperty = AvaloniaProperty.Register<BasicMnemoElement, double>(nameof(Angle),0);

        #region Рисование
        internal protected ISolidColorBrush BrushContentColor;
        internal protected Pen PenContentColor;
        internal protected Pen PenContentColorThin;
        public ISolidColorBrush BrushIsSelected { get; protected internal set; }
        public Pen PenIsSelected { get; protected internal set; }
        public ISolidColorBrush BrushMouseOver { get; protected internal set; }
        public Pen PenMouseOver{ get; protected set; }
        public ISolidColorBrush BrushHand { get; protected set; }
        public Pen PenHand { get; protected set; }
        

        #endregion

        #region ICloneable
        /// <summary>
        /// Клонирование элемента
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();
        #endregion
        /// <summary>
        /// Событие возникает при изменении отступов содержимого элементов мышью. Можно использовать для группового изменения.
        /// </summary>
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
        internal protected void RiseMnemoMarginChanged(string APropertyName)
        {
            MnemoMarginChanged?.Invoke(this, new EventArgsMnemoMarginChanged(APropertyName));
        }
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

        protected virtual void DrawIsSelected()
        {
            if (ControlISSelected)
            {
                DrawingIsSelected.Geometry = new RectangleGeometry(new Rect(0, 0, 29, 29));
            }
            else
            {
                DrawingIsSelected.Geometry = new GeometryGroup();
            }

            DrawingIsSelected.Brush = BrushIsSelected;
            DrawingIsSelected.Pen = PenIsSelected;
        }

        protected virtual void DrawMouseOver()
        {
            DrawingMouseOver.Geometry = new RectangleGeometry(new Rect(0, 0, 29, 29));
            DrawingMouseOver.Brush = BrushMouseOver;
            DrawingMouseOver.Pen = PenMouseOver;
        }
        
    }
}