using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
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
        public BasicMnemoElement()
        {
            AffectsRender<BasicMnemoElement>(AngleProperty,ControlISSelectedProperty);
            BrushContentColor = Brushes.Black;
            BrushContentColor.ToImmutable();
            PenContentColor = new Pen(Brushes.Black,3);
            PenContentColor.ToImmutable();
            BrushIsSelected = Brushes.Pink;
            //BrushIsSelected.Freeze();
            PenIsSelected = new Pen(Brushes.Red, 1);
            //PenIsSelected.Freeze();
            BrushMouseOver = Brushes.Gray;
            BrushMouseOver.ToImmutable();
            PenMouseOver = new Pen(Brushes.Black, 1);
            PenMouseOver.ToImmutable();
            BrushHand = Brushes.Green;
            //BrushHand.Freeze();
            PenHand = new Pen(Brushes.DarkGreen, 1);
            PenHand.ToImmutable();
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
        internal protected ISolidColorBrush BrushHand;
        internal protected Pen PenHand;
        

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
        
    }
}