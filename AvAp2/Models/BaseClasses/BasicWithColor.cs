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
        private void ContentColorAlternateChanged(AvaloniaPropertyChangedEventArgs<Color> obj)
        {
            BasicWithColor sender = obj.Sender as BasicWithColor;
            sender.BrushContentColorAlternate = new SolidColorBrush(obj.NewValue.Value);
            sender.BrushContentColorAlternate.ToImmutable();
            
            sender.PenContentColorAlternate = new Pen(BrushContentColorAlternate, (sender as IGeometry)?.LineThickness ?? 3);
            if (sender is IGeometry && ((IGeometry)sender).IsDash)
                sender.PenContentColorAlternate.DashStyle = DashStyle.Dash;
            sender.PenContentColorAlternate.ToImmutable();
            sender.PenContentColorThinAlternate = new Pen(sender.BrushContentColorAlternate, 1);
            sender.PenContentColorThinAlternate.ToImmutable();
        }

        static BasicWithColor()
        {
            AffectsRender<BasicWithColor>(ContentColorProperty, ContentColorAlternateProperty);
        }

        
        public BasicWithColor() : base()
        {
            DrawingMouseOverWrapper.RenderTransform = new RotateTransform(Angle);
            DrawingIsSelectedWrapper.RenderTransform = new RotateTransform(Angle);
            ContentColor = Colors.Green;
            ContentColorAlternate = Colors.Red;
            ContentColorProperty.Changed.Subscribe(ContentColorChanged);
            ContentColorAlternateProperty.Changed.Subscribe(ContentColorAlternateChanged);
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