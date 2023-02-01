using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using AvAp2.Models.SubControls;
using Newtonsoft.Json;

namespace AvAp2.Models.BaseClasses
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
        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"), PropertyGridFilter, DisplayName("Сетка"), Browsable(false)]
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
        
        [Category("Свойства элемента мнемосхемы"), Description("Реагировать на действия мыши"), PropertyGridFilterAttribute, DisplayName("Активность"), Browsable(true)]
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
            AvaloniaProperty.Register<BasicMnemoElement,bool>(nameof(ControlISHitTestVisible));
         #region Привязки
        [Category("Привязки данных"), Description("ID привязанных устройств. Для привязки достаточно перетащить устройство на элемент из дерева проекта слева"), PropertyGridFilter, DisplayName("ID привязанных устройств"), Browsable(true)]
        public List<string> DeviceIDs
        {
            get => (List<string>)GetValue(DeviceIDsProperty);
            set
            {
                SetValue(DeviceIDsProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<List<string>> DeviceIDsProperty = AvaloniaProperty.Register<BasicMnemoElement, List<string>>(nameof(DeviceIDs),  new List<string>());

        [Category("Привязки данных"), Description("ID привязанных тегов для всплывающих подсказок. Например, токи фаз. Для привязки достаточно перетащить тег на элемент из дерева проекта слева"), PropertyGridFilter, DisplayName("ID тегов подсказок"), Browsable(true)]
        public List<string> ToolTipsTagIDs
        {
            get => (List<string>)GetValue(ToolTipsTagIDsProperty);
            set
            {
                SetValue(ToolTipsTagIDsProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<List<string>> ToolTipsTagIDsProperty = AvaloniaProperty.Register<BasicMnemoElement, List<string>>(nameof(ToolTipsTagIDs), new List<string>());

        [Category("Привязки данных"), Description("ID привязанных произвольных команд. Например, 'пуск осциллографа' для терминала РЗА или 'РПН больше' для трансформатора. Для привязки достаточно перетащить команду на элемент из дерева проекта слева"), PropertyGridFilter, DisplayName("ID команд"), Browsable(true)]
        public List<string> CommandIDs
        {
            get => (List<string>)GetValue(CommandIDsProperty);
            set
            {
                SetValue(CommandIDsProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<List<string>> CommandIDsProperty = AvaloniaProperty.Register<BasicMnemoElement, List<string>>(nameof(CommandIDs),new List<string>());
        #endregion Привязки
        
        public Control DrawingIsSelected { get; set; }
        public Control DrawingMouseOver { get; set; }

        static BasicMnemoElement()
        {
            ControlISSelectedProperty.Changed.Subscribe(OnControlIsSelectedChanged);
            AngleProperty.Changed.Subscribe(OnAngleChanged);
            AffectsRender<BasicMnemoElement>(AngleProperty,ControlISSelectedProperty);
        }

        public BasicMnemoElement()
        {
            
            DrawingIsSelected = new RenderCaller(DrawIsSelected);
            DrawingMouseOver = new RenderCaller(DrawMouseOver);
            DrawingMouseOver.Opacity = 0;
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
            if (this.Content is null)
            {
                this.Content = new Canvas(){
                    ClipToBounds = false
                };
            }
            (this.Content as Canvas).Children.AddRange(new Control[]{DrawingMouseOver, DrawingIsSelected});
            this.Loaded+= OnLoaded;
            PointerEntered+= OnPointerEntered;
            PointerExited+= OnPointerExited;
            DataContext = this;
        }

        private void OnPointerExited(object? sender, PointerEventArgs e)
        {
            DrawingMouseOver.Opacity = 0;
        }

        private void OnPointerEntered(object? sender, PointerEventArgs e)
        {
            DrawingMouseOver.Opacity = 0.3;
        }
        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            DrawingIsSelected.InvalidateVisual();
            DrawingMouseOver.InvalidateVisual();
        }

        private static void OnControlIsSelectedChanged(AvaloniaPropertyChangedEventArgs<bool> obj)
        {
            //(obj.Sender as BasicMnemoElement).ControlISSelected = obj.NewValue.Value;
            (obj.Sender as BasicMnemoElement).DrawingIsSelected.InvalidateVisual();
        }

        private static void OnAngleChanged(AvaloniaPropertyChangedEventArgs<double> obj)
        {
            (obj.Sender as BasicMnemoElement).DrawingMouseOver.InvalidateVisual();
            (obj.Sender as BasicMnemoElement).DrawingIsSelected.InvalidateVisual();
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

        protected virtual void DrawIsSelected(DrawingContext ctx)
        {
            if (ControlISSelected)
            {
                var transform = ctx.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
                ctx.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(0, 0, 29, 29));
                transform.Dispose();
            }
        }

        protected virtual void DrawMouseOver(DrawingContext ctx)
        {
            var transform = ctx.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
            ctx.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(0, 0, 29, 29));
            transform.Dispose();
        }
    }
}