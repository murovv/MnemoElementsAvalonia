using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using AvAp2.Interfaces;

namespace AvAp2.Models
{
    public abstract class BasicWithColor:BasicWithTextName
    {
        public static Pen TestPen1 = new Pen(Brushes.Blue);
        public static ISolidColorBrush TestBrush1 = Brushes.Black;
        [Category("Свойства элемента мнемосхемы"), Description("Цвет содержимого элемента"), DisplayName("Цвет содержимого"), Browsable(true)]
        public virtual Color ContentColor// У прямоугольника можно просто поменять цвет, а у линии только через класс напряжения, просто цвет спрятан
        {
            get => (Color)GetValue(ContentColorProperty);
            set
            {
                SetValue(ContentColorProperty, value);
                RiseMnemoNeedSave();
                
            }
        }
        public static StyledProperty<Color> ContentColorProperty = AvaloniaProperty.Register<BasicWithColor,Color>(nameof(ContentColor));

        [Category("Свойства элемента мнемосхемы"), Description("Цвет содержимого элемента"),
         DisplayName("Цвет содержимого"), Browsable(true)]
        private void ContentColorChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
        {
            BasicWithColor sender = obj.Sender as BasicWithColor;
            sender.BrushContentColor = TestBrush1;
            
            sender.PenContentColor = TestPen1;
            sender.PenContentColorThin = TestPen1;
        }
        public virtual Color ContentColorAlternate// У прямоугольника можно просто поменять цвет, а у линии только через класс напряжения, просто цвет спрятан
        {
            get => (Color)GetValue(ContentColorAlternateProperty);
            set
            {
                SetValue(ContentColorAlternateProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<Color> ContentColorAlternateProperty = AvaloniaProperty.Register<BasicWithColor,Color>(nameof(ContentColorAlternate));
        private void ContentColorAlternateChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
        {
            BasicWithColor sender = obj.Sender as BasicWithColor;
            sender.BrushContentColorAlternate = TestBrush1;
            sender.PenContentColorAlternate = TestPen1;
            sender.PenContentColorThinAlternate = TestPen1;
        }

        static BasicWithColor()
        {
            AffectsRender<BasicWithColor>(ContentColorProperty, ContentColorAlternateProperty);
        }

        
        public BasicWithColor() : base()
        {
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
        }
        #region Рисование

        internal protected ISolidColorBrush BrushContentColorAlternate;
        internal protected Pen PenContentColorAlternate;
        internal protected Pen PenContentColorThinAlternate;
        #endregion Рисование
        
    }
}