using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using AvAp2.Interfaces;

namespace AvAp2.Models.BaseClasses
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
                RiseMnemoNeedSave();
                
            }
        }
        public static StyledProperty<Color> ContentColorProperty = AvaloniaProperty.Register<BasicWithColor,Color>(nameof(ContentColor));

        [Category("Свойства элемента мнемосхемы"), Description("Цвет содержимого элемента"),
         DisplayName("Цвет содержимого"), Browsable(true)]
        private static void ContentColorChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
        {
            BasicWithColor sender = obj.Sender as BasicWithColor;
            sender.BrushContentColor = new SolidColorBrush(obj.NewValue.Value);
            sender.BrushContentColor.ToImmutable();
            
            sender.PenContentColor = new Pen(sender.BrushContentColor, (sender as IGeometry)?.LineThickness ?? 3);
            if (sender is IGeometry && ((IGeometry)sender).IsDash)
                sender.PenContentColor.DashStyle = DashStyle.Dash;
            sender.PenContentColor.ToImmutable();
            sender.PenContentColorThin = new Pen(sender.BrushContentColor, 1);
            sender.PenContentColorThin.ToImmutable();
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
        private static void ContentColorAlternateChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
        {
            BasicWithColor sender = obj.Sender as BasicWithColor;
            sender.BrushContentColorAlternate = new SolidColorBrush(sender.ContentColorAlternate);
            sender.BrushContentColorAlternate.ToImmutable();
            
            sender.PenContentColorAlternate = new Pen(sender.BrushContentColorAlternate, (sender as IGeometry)?.LineThickness ?? 3);
            if (sender is IGeometry && ((IGeometry)sender).IsDash)
                sender.PenContentColorAlternate.DashStyle = DashStyle.Dash;
            sender.PenContentColorAlternate.ToImmutable();
            sender.PenContentColorThinAlternate = new Pen(sender.BrushContentColorAlternate, 1);
            sender.PenContentColorThinAlternate.ToImmutable();
        }

        static BasicWithColor()
        {
            AffectsRender<BasicWithColor>(ContentColorProperty, ContentColorAlternateProperty);
            ContentColorProperty.Changed.Subscribe(ContentColorChanged);
            ContentColorAlternateProperty.Changed.Subscribe(ContentColorAlternateChanged);
        }

        
        public BasicWithColor() : base()
        {
            ContentColor = Colors.Green;
            ContentColorAlternate = Colors.Red;
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