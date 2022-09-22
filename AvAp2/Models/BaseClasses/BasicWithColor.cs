using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using AvAp2.Interfaces;

namespace AvAp2.Models
{
    public abstract class BasicWithColor:BasicWithTextName
    {
        [Category("Свойства элемента мнемосхемы"), Description("Цвет содержимого элемента"), DisplayName("Цвет содержимого"), Browsable(true)]
        public virtual Color ContentColor// У прямоугольника можно просто поменять цвет, а у линии только через класс напряжения, просто цвет спрятан
        {
            get => (Color)GetValue(ContentColorProperty);
            set
            {
                SetValue(ContentColorProperty, value);
            }
        }
        public static StyledProperty<Color> ContentColorProperty = AvaloniaProperty.Register<BasicWithColor,Color>(nameof(ContentColor));
        [Category("Свойства элемента мнемосхемы"), Description("Цвет содержимого элемента"), DisplayName("Цвет содержимого"), Browsable(true)]
        public virtual Color ContentColorAlternate// У прямоугольника можно просто поменять цвет, а у линии только через класс напряжения, просто цвет спрятан
        {
            get => (Color)GetValue(ContentColorAlternateProperty);
            set
            {
                SetValue(ContentColorAlternateProperty, value);
            }
        }
        public static StyledProperty<Color> ContentColorAlternateProperty = AvaloniaProperty.Register<BasicWithColor,Color>(nameof(ContentColorAlternate));
        public BasicWithColor() : base()
        {
            ContentColor = Colors.Green;
            ContentColorAlternate = Colors.Red;
            BrushContentColor = new SolidColorBrush(ContentColor);
            BrushContentColor.ToImmutable();
            PenContentColor= new Pen(BrushContentColor, 3);
            if (this is IGeometry)
                PenContentColor.DashStyle = ((IGeometry)this).IsDash ? DashStyle.Dash : DashStyle.DashDotDot;
            PenContentColor.ToImmutable();
            PenContentColorThin = new Pen(BrushContentColor, 1);
            PenContentColorThin.ToImmutable();

            BrushContentColorAlternate = new SolidColorBrush(ContentColorAlternate);
            BrushContentColorAlternate.ToImmutable();            
            PenContentColorAlternate = new Pen(BrushContentColorAlternate, (this is IGeometry) ? ((IGeometry)this).LineThickness : 3);
            if (this is IGeometry)
                PenContentColorAlternate.DashStyle = ((IGeometry)this).IsDash ? DashStyle.Dash : DashStyle.DashDotDot;
            PenContentColorAlternate.ToImmutable();
            PenContentColorThinAlternate = new Pen(BrushContentColorAlternate, 1) ;
            PenContentColorThinAlternate.ToImmutable();
        }
        #region Рисование

        internal protected Brush BrushContentColorAlternate;
        internal protected Pen PenContentColorAlternate;
        internal protected Pen PenContentColorThinAlternate;
        #endregion Рисование
        
    }
}