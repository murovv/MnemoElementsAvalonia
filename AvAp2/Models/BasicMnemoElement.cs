using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace AvAp2.Models
{
    public abstract class BasicMnemoElement:Control
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
        public BasicMnemoElement()
        {
            BrushContentColor = Brushes.Black;
            BrushContentColor.ToImmutable();
            PenContentColor = new Pen(Brushes.Black,3);
            PenContentColor.ToImmutable();
            BrushIsSelected = Brushes.Pink;
            //BrushIsSelected.Freeze();
            PenIsSelected = new Pen(Brushes.Red, 1);
            //PenIsSelected.Freeze();
            BrushMouseOver = Brushes.Gray;
            //BrushMouseOver.Freeze();
            PenMouseOver = new Pen(Brushes.Black, 1);
            PenMouseOver.ToImmutable();
            BrushHand = Brushes.Green;
            //BrushHand.Freeze();
            PenHand = new Pen(Brushes.DarkGreen, 1);
            PenHand.ToImmutable();
        }
        public static StyledProperty<double> AngleProperty = AvaloniaProperty.Register<BasicMnemoElement, double>(nameof(Angle));

        #region Рисование
        internal protected ISolidColorBrush BrushContentColor;
        internal protected Pen PenContentColor;
        internal protected Pen PenContentColorThin;
        internal protected ISolidColorBrush BrushIsSelected;
        internal protected Pen PenIsSelected;
        internal protected ISolidColorBrush BrushMouseOver;
        internal protected Pen PenMouseOver;
        internal protected ISolidColorBrush BrushHand;
        internal protected Pen PenHand;
        

        #endregion
    }
}